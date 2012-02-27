//----------------------------------------------------------------------------------------
// <copyright file="Iis.cs" >
//     Copyright (c) 2012, Michael Schnecke, Göran Watzke. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using Microsoft.Web.Administration;

namespace WebDavWhs
{
	/// <summary>
	/// 	Provides properties and methodes to handle IIS
	/// </summary>
	internal class Iis : IDisposable
	{
		/// <summary>
		/// 	Disposable flag.
		/// </summary>
		private bool isDisposed;

		/// <summary>
		/// 	The server manager object
		/// </summary>
		private ServerManager serverManager;

		/// <summary>
		/// 	The root path.
		/// </summary>
		private const string path = @"/";

		/// <summary>
		/// 	Gets the server manager.
		/// </summary>
		/// <value> The server manager. </value>
		private ServerManager ServerManager
		{
			get
			{
				if(this.serverManager == null)
				{
					this.serverManager = new ServerManager();
				}

				return this.serverManager;
			}
		}

		#region Lifecycle

		/// <summary>
		/// 	Initializes a new instance of the <see cref="Iis" /> class.
		/// </summary>
		public Iis()
		{
			this.isDisposed = false;
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

			if(this.ServerManager != null)
			{
				this.ServerManager.Dispose();
			}

			this.isDisposed = true;
			GC.SuppressFinalize(this);
		}

		#endregion

		/// <summary>
		/// 	Creates the virtual directory.
		/// </summary>
		/// <param name="virtualDirectoryName"> Name of the virtual directory. </param>
		/// <param name="physicalPath"> The physical path. </param>
		public void CreateVirtualDirectory(string virtualDirectoryName, string physicalPath)
		{
			Trace.TraceInformation("CreateVirtualDirectory...");

			string defaultWebSite = this.GetDefaultWebSite();

			if(string.IsNullOrEmpty(defaultWebSite))
			{
				Trace.TraceError("Default web site not found.");
				Trace.TraceInformation("CreateVirtualDirectory...finished.");
				return;
			}

			if (this.ExistsVirtualDirectory(defaultWebSite, virtualDirectoryName))
			{
				Trace.TraceInformation("Virtual directory already exists.");
				Trace.TraceInformation("CreateVirtualDirectory...finished.");
				return;
			}

			Application applicationElement = this.ServerManager.Sites[defaultWebSite].Applications[path];
			VirtualDirectory virtualDirectory = applicationElement.VirtualDirectories.CreateElement();
			virtualDirectory.Path = string.Format(@"/{0}", virtualDirectoryName);
			virtualDirectory.PhysicalPath = physicalPath;
			applicationElement.VirtualDirectories.Add(virtualDirectory);

			this.ServerManager.CommitChanges();
			Trace.TraceInformation("CreateVirtualDirectory...finished.");
		}

		/// <summary>
		/// Existses the virtual directory.
		/// </summary>
		/// <param name="webSiteName">Name of the web site.</param>
		/// <param name="virtualDirectoryName">Name of the virtual directory.</param>
		/// <returns>Validation result.</returns>
		public bool ExistsVirtualDirectory(string webSiteName, string virtualDirectoryName)
		{
			Trace.TraceInformation("ExistsVirtualDirectory...");

			VirtualDirectory virtualDirectory =
					this.ServerManager.Sites[webSiteName].Applications[path].VirtualDirectories[
						string.Format(@"/{0}", virtualDirectoryName)
						];

			if (virtualDirectory == null)
			{
				Trace.TraceInformation("ExistsVirtualDirectory...finished.");
				return false;
			}

			Trace.TraceInformation("ExistsVirtualDirectory...finished.");
			return true;
		}


		/// <summary>
		/// 	Sets the windows authentication.
		/// </summary>
		/// <param name="webSiteName"> Name of the web site. </param>
		/// <param name="enabled"> if set to <c>true</c> [enabled]. </param>
		public void SetWindowsAuthentication(string webSiteName, bool enabled)
		{
			Trace.TraceInformation("SetWindowsAuthentication...");

			Configuration configuration = this.ServerManager.GetApplicationHostConfiguration();

			ConfigurationSection windowsAuthenticationSection =
				configuration.GetSection("system.webServer/security/authentication/windowsAuthentication", webSiteName);
			windowsAuthenticationSection["enabled"] = enabled;

			this.ServerManager.CommitChanges();
			Trace.TraceInformation("SetWindowsAuthentication...finished.");
		}

		/// <summary>
		/// Sets the anonymous authentication.
		/// </summary>
		/// <param name="webSiteName">Name of the web site.</param>
		/// <param name="enabled">if set to <c>true</c> [enabled].</param>
		public void SetAnonymousAuthentication(string webSiteName, bool enabled)
		{
			Trace.TraceInformation("SetAnonymousAuthentication...");
			
			Configuration configuration = this.ServerManager.GetApplicationHostConfiguration();
			
			ConfigurationSection anonymousAuthenticationSection =
				configuration.GetSection("system.webServer/security/authentication/anonymousAuthentication", webSiteName);
			
			anonymousAuthenticationSection["enabled"] = enabled;
			
			this.ServerManager.CommitChanges();

			Trace.TraceInformation("SetAnonymousAuthentication...finished.");
		}

		/// <summary>
		/// Sets the basic authentication.
		/// </summary>
		/// <param name="webSiteName">Name of the web site.</param>
		/// <param name="enabled">if set to <c>true</c> [enabled].</param>
		public void SetBasicAuthentication(string webSiteName, bool enabled)
		{
			Trace.TraceInformation("SetBasicAuthentication...");

			Configuration configuration = this.ServerManager.GetApplicationHostConfiguration();

			ConfigurationSection basicAuthenticationSection =
				configuration.GetSection("system.webServer/security/authentication/basicAuthentication", webSiteName);
				basicAuthenticationSection["enabled"] = enabled;

			this.ServerManager.CommitChanges();
			Trace.TraceInformation("SetBasicAuthentication...finished.");
		}

		/// <summary>
		/// 	Sets the web dav status.
		/// </summary>
		/// <param name="enabled"> if set to <c>true</c> [enabled]. </param>
		/// <param name="requireSsl"> if set to <c>true</c> [require SSL]. </param>
		public void SetWebDavStatus(bool enabled, bool requireSsl)
		{
			Trace.TraceInformation("SetWebDavStatus...");

			string defaultWebSite = this.GetDefaultWebSite();

			if(string.IsNullOrEmpty(defaultWebSite))
			{
				Trace.TraceError("Default web site not found.");
				Trace.TraceInformation("SetWebDavStatus...finished.");
				return;
			}

			Configuration config = this.ServerManager.GetApplicationHostConfiguration();
			ConfigurationSection authoringSection = config.GetSection("system.webServer/webdav/authoring", defaultWebSite);

			authoringSection["enabled"] = enabled;
			authoringSection["requireSsl"] = requireSsl;
			this.ServerManager.CommitChanges();
			Trace.TraceInformation("SetWebDavStatus...finished.");
		}

		/// <summary>
		/// Gets the WebDAV status.
		/// </summary>
		/// <returns>The WebDAV status.</returns>
		public WebDavStatus GetWebDavStatus()
		{
			Trace.TraceInformation("GetWebDavStatus...");

			string defaultWebSite = this.GetDefaultWebSite();

			if (string.IsNullOrEmpty(defaultWebSite))
			{
				Trace.TraceError("Default web site not found.");
				Trace.TraceInformation("GetWebDavStatus...finished.");
				return WebDavStatus.Unknown;
			}

			Configuration config = this.ServerManager.GetApplicationHostConfiguration();
			ConfigurationSection authoringSection = config.GetSection("system.webServer/webdav/authoring", defaultWebSite);

			Trace.TraceInformation("GetWebDavStatus...finished.");
			bool enabled = (bool) authoringSection["enabled"];

			if(enabled)
			{
				Trace.TraceInformation("GetWebDavStatus...finished.");
				return WebDavStatus.Enabled;
			}

			Trace.TraceInformation("GetWebDavStatus...finished.");
			return WebDavStatus.Disabled;
		}

		/// <summary>
		/// 	Gets the default web site.
		/// </summary>
		/// <returns> The default web site. </returns>
		public string GetDefaultWebSite()
		{
			Trace.TraceInformation("GetDefaultWebSite...");
			string siteName = string.Empty;

			Configuration config = this.ServerManager.GetApplicationHostConfiguration();
			ConfigurationSection sitesSection = config.GetSection("system.applicationHost/sites");
			ConfigurationElementCollection sitesCollection = sitesSection.GetCollection();

			foreach(ConfigurationElement site in sitesCollection)
			{
				long id = (long)site.Attributes["id"].Value;

				if(id != 1)
				{
					continue;
				}

				siteName = (string)site.Attributes["name"].Value;
				break;
			}

			Trace.TraceInformation("Site name: '{0}'", siteName);
			Trace.TraceInformation("GetDefaultWebSite...finished.");
			return siteName;
		}
	}
}