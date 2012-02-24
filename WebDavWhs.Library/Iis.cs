using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Web.Administration;

namespace WebDavWhs
{
	/// <summary>
	/// Provides properties and methodes to handle IIS
	/// </summary>
	public class Iis : IDisposable
	{
		/// <summary>
		/// Disposable flag.
		/// </summary>
		private bool isDisposed;

		/// <summary>
		/// Gets or sets the server manager.
		/// </summary>
		/// <value>
		/// The server manager.
		/// </value>
		private ServerManager ServerManager
		{
			get;
			set;
		}

		#region Lifecycle

		/// <summary>
		/// Initializes a new instance of the <see cref="Iis"/> class.
		/// </summary>
		public Iis()
		{
			this.isDisposed = false;
			this.ServerManager = new ServerManager();
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
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
		/// Sets the web dav status.
		/// </summary>
		/// <param name="enabled">if set to <c>true</c> [enabled].</param>
		/// <param name="requireSsl">if set to <c>true</c> [require SSL].</param>
		public void SetWebDavStatus(bool enabled, bool requireSsl)
		{
			Trace.TraceInformation("SetWebDavStatus...");

			string defaultWebSite = this.GetDefaultWebSite();

			if (string.IsNullOrEmpty(defaultWebSite))
			{
				Trace.TraceError("Default web site not found.");
				Trace.TraceInformation("SetWebDavStatus...finished.");
				return;
			}
			
			try
			{
				Configuration config = this.ServerManager.GetApplicationHostConfiguration();
				ConfigurationSection authoringSection = config.GetSection("system.webServer/webdav/authoring", defaultWebSite);

				authoringSection["enabled"] = enabled;
				authoringSection["requireSsl"] = requireSsl;
				this.ServerManager.CommitChanges();
			}
			finally
			{
				Trace.TraceInformation("SetWebDavStatus...finished.");
			}
		}

		/// <summary>
		/// Gets the default web site.
		/// </summary>
		/// <returns>The default web site.</returns>
		private string GetDefaultWebSite()
		{
			Trace.TraceInformation("GetDefaultWebSite...");
			string siteName = string.Empty;

			try
			{
				Configuration config = this.ServerManager.GetApplicationHostConfiguration();
				ConfigurationSection sitesSection = config.GetSection("system.applicationHost/sites");
				ConfigurationElementCollection sitesCollection = sitesSection.GetCollection();

				foreach (ConfigurationElement site in sitesCollection)
				{
					long id = (long)site.Attributes["id"].Value;

					if (id != 1)
					{
						continue;
					}

					siteName = (string)site.Attributes["name"].Value;
					break;
				}

				Trace.TraceInformation("Site name: '{0}'", siteName);
				return siteName;
			}
			finally
			{
				Trace.TraceInformation("GetDefaultWebSite...finished.");
			}
		}



	}

	

}
