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

		#region Virtual Directories

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

		/// <summary>
		/// 	Creates the virtual directory.
		/// </summary>
		/// <param name="webSiteName"> Name of the web site. </param>
		/// <param name="path"> The path. </param>
		/// <param name="virtualDirectoryName"> Name of the virtual directory. </param>
		/// <param name="physicalPath"> The physical path. </param>
		public void CreateVirtualDirectory(string webSiteName, string path, string virtualDirectoryName, string physicalPath)
		{
			Trace.TraceInformation("CreateVirtualDirectory...");

			if(this.ExistsVirtualDirectory(webSiteName, path, virtualDirectoryName))
			{
				Trace.TraceInformation("Virtual directory already exists.");
				Trace.TraceInformation("CreateVirtualDirectory...finished.");
				return;
			}

			Application applicationElement = this.ServerManager.Sites[webSiteName].Applications[path];
			VirtualDirectory virtualDirectory = applicationElement.VirtualDirectories.CreateElement();
			virtualDirectory.Path = string.Format(@"/{0}", virtualDirectoryName);
			virtualDirectory.PhysicalPath = physicalPath;
			applicationElement.VirtualDirectories.Add(virtualDirectory);

			this.ServerManager.CommitChanges();
			Trace.TraceInformation("CreateVirtualDirectory...finished.");
		}

		/// <summary>
		/// 	Removes the virtual directory.
		/// </summary>
		/// <param name="webSiteName"> Name of the web site. </param>
		/// <param name="path"> The path. </param>
		/// <param name="virtualDirectoryName"> Name of the virtual directory. </param>
		/// <param name="recursive"> if set to <c>true</c> [recursive]. </param>
		public void RemoveVirtualDirectory(string webSiteName, string path, string virtualDirectoryName, bool recursive)
		{
			Trace.TraceInformation("RemoveVirtualDirectory...");

			Application applicationElement = this.ServerManager.Sites[webSiteName].Applications[path];

			foreach(VirtualDirectory virtualDirectory in applicationElement.VirtualDirectories)
			{
				if(string.Compare(virtualDirectory.Path, @"/", StringComparison.OrdinalIgnoreCase) == 0)
				{
					continue;
				}

				if (string.Compare(virtualDirectory.Path, string.Format(@"/{0}", virtualDirectoryName), StringComparison.OrdinalIgnoreCase) == 0)
				{
					virtualDirectory.Delete();
					continue;
				}

				if(recursive)
				{
					if (virtualDirectory.Path.Contains(string.Format(@"/{0}/", virtualDirectoryName)))
					{
						virtualDirectory.Delete();
					}
				}
			}

			this.ServerManager.CommitChanges();
			Trace.TraceInformation("RemoveVirtualDirectory...finished.");
		}

		/// <summary>
		/// 	Existses the virtual directory.
		/// </summary>
		/// <param name="webSiteName"> Name of the web site. </param>
		/// <param name="path"> The path. </param>
		/// <param name="virtualDirectoryName"> Name of the virtual directory. </param>
		/// <returns> Validation result. </returns>
		public bool ExistsVirtualDirectory(string webSiteName, string path, string virtualDirectoryName)
		{
			Trace.TraceInformation("ExistsVirtualDirectory...");

			VirtualDirectory virtualDirectory =
				this.ServerManager.Sites[webSiteName].Applications[path].VirtualDirectories[
					string.Format(@"/{0}", virtualDirectoryName)
					];

			if(virtualDirectory == null)
			{
				Trace.TraceInformation("ExistsVirtualDirectory...finished.");
				return false;
			}

			Trace.TraceInformation("ExistsVirtualDirectory...finished.");
			return true;
		}

		/// <summary>
		/// 	Sets the virtual directory defaults.
		/// </summary>
		/// <param name="webSiteName"> Name of the web site. </param>
		/// <param name="path"> The path. </param>
		/// <param name="virtualDirectoryName"> Name of the virtual directory. </param>
		public void SetVirtualDirectoryDefaults(string webSiteName, string path, string virtualDirectoryName)
		{
			Trace.TraceInformation("SetVirtualDirectoryDefaults...");

			Application applicationElement = this.ServerManager.Sites[webSiteName].Applications[path];
			VirtualDirectoryDefaults virtualDirectoryDefaults = applicationElement.VirtualDirectoryDefaults;

			virtualDirectoryDefaults["allowSubDirConfig"] = false;

			this.ServerManager.CommitChanges();

			Trace.TraceInformation("SetVirtualDirectoryDefaults...finished.");
		}

		/// <summary>
		/// 	Enables the directory browsing.
		/// </summary>
		/// <param name="webSiteName"> Name of the web site. </param>
		/// <param name="enabled"> if set to <c>true</c> [enabled]. </param>
		public void EnableDirectoryBrowsing(string webSiteName, bool enabled)
		{
			Trace.TraceInformation("EnableDirectoryBrowsing...");

			try
			{
				this.UnlockSectionForSite("system.webServer/directoryBrowse", webSiteName);
				Configuration config = this.ServerManager.GetWebConfiguration(webSiteName);
				ConfigurationSection directoryBrowseSection = config.GetSection("system.webServer/directoryBrowse");
				directoryBrowseSection["enabled"] = true;
				this.ServerManager.CommitChanges();
			}
			finally
			{
				Trace.TraceInformation("EnableDirectoryBrowsing...finished.");
			}
		}

		/// <summary>
		/// 	Unlocks the section for site.
		/// </summary>
		/// <param name="sectionPath"> The section path. </param>
		/// <param name="siteName"> Name of the site. </param>
		private void UnlockSectionForSite(string sectionPath, string siteName)
		{
			Configuration config = this.ServerManager.GetApplicationHostConfiguration();
			ConfigurationSection section = config.GetSection(sectionPath, siteName);
			section.OverrideMode = OverrideMode.Allow;
			this.ServerManager.CommitChanges();
		}

		#endregion

		#region Authentication

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
		/// 	Sets the anonymous authentication.
		/// </summary>
		/// <param name="webSiteName"> Name of the web site. </param>
		/// <param name="enabled"> if set to <c>true</c> [enabled]. </param>
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
		/// 	Sets the basic authentication.
		/// </summary>
		/// <param name="webSiteName"> Name of the web site. </param>
		/// <param name="enabled"> if set to <c>true</c> [enabled]. </param>
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

		#endregion

		#region WebDAV

		/// <summary>
		/// 	Sets the web dav status.
		/// </summary>
		/// <param name="webSiteName"> Name of the web site. </param>
		/// <param name="enabled"> if set to <c>true</c> [enabled]. </param>
		/// <param name="requireSsl"> if set to <c>true</c> [require SSL]. </param>
		public void SetWebDavStatus(string webSiteName, bool enabled, bool requireSsl)
		{
			Trace.TraceInformation("SetWebDavStatus...");

			Configuration config = this.ServerManager.GetApplicationHostConfiguration();
			ConfigurationSection authoringSection = config.GetSection("system.webServer/webdav/authoring", webSiteName);

			authoringSection["enabled"] = enabled;
			authoringSection["requireSsl"] = requireSsl;
			this.ServerManager.CommitChanges();
			Trace.TraceInformation("SetWebDavStatus...finished.");
		}

		/// <summary>
		/// 	Gets the WebDAV status.
		/// </summary>
		/// <param name="webSiteName"> Name of the web site. </param>
		/// <returns> The WebDAV status. </returns>
		public WebDavStatus GetWebDavStatus(string webSiteName)
		{
			Trace.TraceInformation("GetWebDavStatus...");

			Configuration config = this.ServerManager.GetApplicationHostConfiguration();
			ConfigurationSection authoringSection = config.GetSection("system.webServer/webdav/authoring", webSiteName);

			Trace.TraceInformation("GetWebDavStatus...finished.");
			bool enabled = (bool)authoringSection["enabled"];

			if(enabled)
			{
				Trace.TraceInformation("GetWebDavStatus...finished.");
				return WebDavStatus.Enabled;
			}

			Trace.TraceInformation("GetWebDavStatus...finished.");
			return WebDavStatus.Disabled;
		}

		/// <summary>
		/// 	Sets the WebDAV authoring rule.
		/// </summary>
		/// <param name="webSiteName"> Name of the web site. </param>
		public void SetWebDavAuthoringRule(string webSiteName)
		{
			Trace.TraceInformation("AddWebDavAuthoringRule...");

			try
			{
				Configuration configuration = this.ServerManager.GetApplicationHostConfiguration();
				ConfigurationSection authoringRulesSection = configuration.GetSection("system.webServer/webdav/authoringRules",
				                                                                      webSiteName);
				authoringRulesSection["allowNonMimeMapFiles"] = true;

				ConfigurationElementCollection authoringRulesCollection = authoringRulesSection.GetCollection();
				ConfigurationElement addElement = authoringRulesCollection.CreateElement("add");

				addElement["users"] = @"*";
				addElement["path"] = @"*";
				addElement["access"] = @"Read, Write, Source";
				authoringRulesCollection.Add(addElement);

				this.ServerManager.CommitChanges();
			}
			finally
			{
				Trace.TraceInformation("AddWebDavAuthoringRule...finished.");
			}
		}

		/// <summary>
		/// 	Removes all web dav authoring rule.
		/// </summary>
		/// <param name="webSiteName"> Name of the web site. </param>
		public void RemoveAllWebDavAuthoringRules(string webSiteName)
		{
			Trace.TraceInformation("RemoveAllWebDavAuthoringRules...");

			try
			{
				Configuration configuration = this.ServerManager.GetApplicationHostConfiguration();
				ConfigurationSection authoringRulesSection = configuration.GetSection("system.webServer/webdav/authoringRules",
				                                                                      webSiteName);

				ConfigurationElementCollection authoringRulesCollection = authoringRulesSection.GetCollection();
				authoringRulesCollection.Clear();

				this.ServerManager.CommitChanges();
			}
			finally
			{
				Trace.TraceInformation("RemoveAllWebDavAuthoringRules...finished.");
			}
		}

		#endregion
	}
}