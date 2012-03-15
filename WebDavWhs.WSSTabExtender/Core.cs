//----------------------------------------------------------------------------------------
// <copyright file="Core.cs" >
//     Copyright (c) 2012, Michael Schnecke, Göran Watzke. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using Microsoft.Win32;
using Microsoft.WindowsServerSolutions.AddinInfrastructure;
using Microsoft.WindowsServerSolutions.Storage;

namespace WebDavWhs
{
	/// <summary>
	/// 	Provides properties and methods for the central BL.
	/// </summary>
	internal class Core : IDisposable
	{
		/// <summary>
		/// Occurs when version update was detected.
		/// </summary>
		public event EventHandler<UpdateInfoEventArguments> VersionUpdate;

		/// <summary>
		/// The update worker.
		/// </summary>
		private BackgroundWorker updateInfoWorker;

		/// <summary>
		/// 	Disposable flag.
		/// </summary>
		private bool isDisposed;

		/// <summary>
		/// 	Gets or sets the IIS.
		/// </summary>
		/// <value> The IIS. </value>
		internal Iis Iis
		{
			get;
			set;
		}

		/// <summary>
		/// 	Gets or sets the settings.
		/// </summary>
		/// <value> The settings. </value>
		internal ApplicationSettings Settings
		{
			get;
			set;
		}

		/// <summary>
		/// 	Gets or sets the storage.
		/// </summary>
		/// <value> The storage. </value>
		internal Storage Storage
		{
			get;
			set;
		}

		/// <summary>
		/// 	Initializes a new instance of the <see cref="Core" /> class.
		/// </summary>
		public Core()
		{
			this.isDisposed = false;
			this.Settings = new ApplicationSettings();
			this.Storage = new Storage();
			this.Storage.StoragePropertyChanged += this.Storage_StoragePropertyChanged;
			this.Iis = new Iis();
		}

		/// <summary>
		/// 	Releases unmanaged resources and performs other cleanup operations before the <see cref="Core" /> is reclaimed by garbage collection.
		/// </summary>
		~Core()
		{
			this.Storage.StoragePropertyChanged -= this.Storage_StoragePropertyChanged;
			this.Dispose();
		}

		/// <summary>
		/// 	Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			if(this.isDisposed)
			{
				return;
			}

			if (this.updateInfoWorker != null)
			{
				this.updateInfoWorker.Dispose();
			}

			if(this.Storage != null)
			{
				this.Storage.Dispose();
			}

			if(this.Iis != null)
			{
				this.Iis.Dispose();
			}

			this.isDisposed = true;
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Enables the WebDAV feature.
		/// </summary>
		public void EnableWebDav()
		{
			string defaultWebSiteName = this.Iis.GetDefaultWebSite();
			string rootVirtDir = string.Format("{0}/{1}", defaultWebSiteName, this.Settings.VirtualDirectoryAlias);

			// remove existing virtual root directory
			this.Iis.RemoveVirtualDirectory(defaultWebSiteName, @"/", this.Settings.VirtualDirectoryAlias, true);
			
			// create physical path for the virtual root directory
			string directoryPath = Path.Combine(this.Settings.ApplicationDataFolder, this.Settings.VirtualDirectoryAlias);
			this.CreateRootDir(directoryPath);

			// TODO apply access rules

			// create virtual root directory
			this.Iis.CreateVirtualDirectory(defaultWebSiteName, @"/", this.Settings.VirtualDirectoryAlias, directoryPath);
			
			// configure virtual directory
			this.Iis.SetAnonymousAuthentication(rootVirtDir, false);
			this.Iis.SetBasicAuthentication(rootVirtDir, false);
			this.Iis.SetWindowsAuthentication(rootVirtDir, true);
			this.Iis.EnableDirectoryBrowsing(rootVirtDir, true);

			// hide web.config
			File.SetAttributes(string.Format(@"{0}\web.config", directoryPath), FileAttributes.Hidden);

			// enable WebDAV
			this.Iis.SetWebDavStatus(defaultWebSiteName, true, this.Settings.UseSsl);
			this.Iis.RemoveAllWebDavAuthoringRules(rootVirtDir);
			this.Iis.SetWebDavAuthoringRule(rootVirtDir);

			// create sub virtual directories
			foreach (Folder folder in this.Storage.Folders)
			{
				if (folder.Shared == false)
				{
					continue;
				}

				if (folder.Path.Contains("ServerFolders") == false)
				{
					continue;
				}

				string virtDir = string.Format(@"{0}/{1}", this.Settings.VirtualDirectoryAlias, folder.Name);
				this.Iis.CreateVirtualDirectory(defaultWebSiteName, @"/", virtDir, folder.Path);
			}
		}

		/// <summary>
		/// Disables the WebDAV feature.
		/// </summary>
		public void DisableWebDav()
		{
			// remove virtual directories
			string defaultWebSiteName = this.Iis.GetDefaultWebSite();
			this.Iis.RemoveVirtualDirectory(defaultWebSiteName, @"/", this.Settings.VirtualDirectoryAlias, true);

			// disable WebDAV
			if (this.Iis.GetWebDavStatus(defaultWebSiteName) == WebDavStatus.Enabled)
			{
				this.Iis.SetWebDavStatus(defaultWebSiteName, false, true);
			}

			// remove physical root directory
			string directoryPath = Path.Combine(this.Settings.ApplicationDataFolder, this.Settings.VirtualDirectoryAlias);
			this.RemoveRootDir(directoryPath);
		}

		/// <summary>
		/// Gets the name of the WHS domain.
		/// </summary>
		/// <returns></returns>
		public string GetDomainName()
		{
			Trace.TraceInformation("GetDomainName...");

			RegistryKey regKey = null;

			try
			{
				const string subKeyString = @"SOFTWARE\Microsoft\Windows Server\Domain Manager\ActiveConfiguration";
				regKey = Registry.LocalMachine.OpenSubKey(subKeyString, RegistryKeyPermissionCheck.ReadSubTree);

				if (regKey == null)
				{
					Trace.TraceInformation("GetDomainName...finished.");
					return string.Empty;
				}

				return (string)regKey.GetValue("DomainName", string.Empty);
			}
			finally
			{
				if (regKey != null)
				{
					regKey.Close();
				}

				Trace.TraceInformation("GetDomainName...finished.");
			}
		}


		/// <summary>
		/// Checks for an update.
		/// </summary>
		public void CheckForUpdate()
		{
			this.updateInfoWorker = new BackgroundWorker();
			this.updateInfoWorker.RunWorkerCompleted += this.UpdateInfoWorkerRunWorkerCompleted;
			this.updateInfoWorker.DoWork += this.UpdateInfoWorkerDoWork;
			this.updateInfoWorker.RunWorkerAsync();
		}

		/// <summary>
		/// Updates the info worker do work.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.ComponentModel.DoWorkEventArgs"/> instance containing the event data.</param>
		private void UpdateInfoWorkerDoWork(object sender, DoWorkEventArgs e)
		{
			try
			{
				Version version = this.GetUpdateInfo();

				if (version == null)
				{
					e.Result = null;
					return;
				}

				Version assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;

				if (version.CompareTo(assemblyVersion) > 0)
				{
					e.Result = version;
					return;
				}

				e.Result = null;
			}
			catch(Exception exception)
			{
				Trace.TraceError(exception.ToString());
				e.Result = null;
			}
		}

		/// <summary>
		/// Handles the RunWorkerCompleted event of the updateInfoWorker control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.ComponentModel.RunWorkerCompletedEventArgs"/> instance containing the event data.</param>
		private void UpdateInfoWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (e.Result == null)
			{
				return;
			}

			Version version = e.Result as Version;

			UpdateInfoEventArguments updateInfo = new UpdateInfoEventArguments();

			try
			{
				updateInfo.AddressUri = new Uri(StringResource.AdrUri);

				Guid guid;
				if (Guid.TryParse(StringResource.AddInId, out guid) == false)
				{
					return;
				}

				updateInfo.Guid = guid;
				updateInfo.UpdateClassification = UpdateClassification.Update;
				updateInfo.Version = version;
			}
			catch(Exception exception)
			{
				Trace.TraceError(exception.ToString());
				return;
			}

			if (this.VersionUpdate != null)
			{
				this.VersionUpdate(this, updateInfo);
			}
		}

		/// <summary>
		/// Gets the update info.
		/// </summary>
		/// <returns>Version, in error case null.</returns>
		private Version GetUpdateInfo()
		{
			WebClient webClient = new WebClient();
			Stream webContent = null;
			StreamReader streamReader = null;

			try
			{
				webContent = webClient.OpenRead(StringResource.AdrUri);

				if (webContent == null)
				{
					return null;
				}

				streamReader = new StreamReader(webContent);

				string line;
				while ((line = streamReader.ReadLine()) != null)
				{
					if (line.IndexOf("ReleaseName", StringComparison.Ordinal) > 0)
					{
						string[] split = line.Split(new[] { ' ' });
						string version = split[split.Length - 1].Replace("</td>", "");

						Version v;

						if (Version.TryParse(version, out v) == false)
						{
							return null;
						}

						return v;
					}
				}

				return null;
			}
			finally
			{
				if (streamReader != null)
				{
					streamReader.Dispose();
				}
				if (webContent != null)
				{
					webContent.Dispose();
				}

				webClient.Dispose();
			}
		}


		/// <summary>
		/// 	Creates the root dir.
		/// </summary>
		/// <param name="directoryPath"> The directory path. </param>
		private void CreateRootDir(string directoryPath)
		{
			Trace.TraceInformation("CreateRootDir...");

			try
			{
				if (Directory.Exists(directoryPath) == false)
				{
					Directory.CreateDirectory(directoryPath);
				}
			}
			finally
			{
				Trace.TraceInformation("CreateRootDir...finished.");
			}
		}

		/// <summary>
		/// 	Removes the root dir.
		/// </summary>
		private void RemoveRootDir(string directoryPath)
		{
			Trace.TraceInformation("RemoveRootDir...");

			try
			{
				if(Directory.Exists(directoryPath))
				{
					Directory.Delete(directoryPath, true);
				}
			}
			finally
			{
				Trace.TraceInformation("RemoveRootDir...finished.");
			}
		}

		/// <summary>
		/// 	Handles the StoragePropertyChanged event of the Storage control.
		/// </summary>
		/// <param name="sender"> The source of the event. </param>
		/// <param name="e"> The <see cref="WebDavWhs.StoragePropertyEventArgs" /> instance containing the event data. </param>
		private void Storage_StoragePropertyChanged(object sender, StoragePropertyEventArgs e)
		{
		}
	}
}