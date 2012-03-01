//----------------------------------------------------------------------------------------
// <copyright file="Core.cs" >
//     Copyright (c) 2012, Michael Schnecke, Göran Watzke. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using Microsoft.WindowsServerSolutions.Storage;

namespace WebDavWhs
{
	/// <summary>
	/// 	Provides properties and methods for the central BL.
	/// </summary>
	internal class Core : IDisposable
	{
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
			if (this.Iis.GetWebDavStatus(defaultWebSiteName) != WebDavStatus.Enabled)
			{
				this.Iis.SetWebDavStatus(defaultWebSiteName, true, true);
			}

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
				
				this.Iis.CreateVirtualDirectory(defaultWebSiteName, @"/", string.Format(@"{0}/{1}", this.Settings.VirtualDirectoryAlias, folder.Name), folder.Path);
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