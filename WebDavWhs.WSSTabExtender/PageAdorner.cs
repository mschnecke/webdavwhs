//----------------------------------------------------------------------------------------
// <copyright file="PageAdorner.cs" >
//     Copyright (c) 2012, Michael Schnecke, Göran Watzke. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Microsoft.WindowsServerSolutions.AddinInfrastructure;
using Microsoft.WindowsServerSolutions.Administration.ObjectModel;
using Microsoft.WindowsServerSolutions.Administration.ObjectModel.Adorners;

namespace WebDavWhs
{
	/// <summary>
	/// 	The PageAdorner.
	/// </summary>
	public class PageAdorner : PageContentAdorner
	{
		/// <summary>
		/// 	Gets or sets the core.
		/// </summary>
		/// <value> The core. </value>
		private Core Core
		{
			get;
			set;
		}

		/// <summary>
		/// 	Initializes a new instance of the <see cref="PageAdorner" /> class.
		/// </summary>
		public PageAdorner()
			: base(new Guid("{572F7115-00BC-4E0F-B466-23FA6468219C}"), "WebDAV for WHS Add-in", "WebDAV for WHS Add-in")
		{
			this.Initialize();
		}

		/// <summary>
		/// 	Releases unmanaged resources and performs other cleanup operations before the <see cref="PageAdorner" /> is reclaimed by garbage collection.
		/// </summary>
		~PageAdorner()
		{
			try
			{
				if(this.Core != null)
				{
					this.Core.Dispose();
				}
			}
			catch(Exception exception)
			{
				Trace.TraceError(exception.ToString());
			}
		}

		/// <summary>
		/// 	Initializes this instance.
		/// </summary>
		private void Initialize()
		{
			try
			{
				this.Core = new Core();
				this.Core.Storage.Connect();
				this.Core.VersionUpdate += this.VersionUpdate;
				this.Core.CheckForUpdate();
			}
			catch(Exception exception)
			{
				Trace.TraceError(exception.ToString());
			}

			try
			{
				this.Core.Settings = ApplicationSettings.LoadSettings();
				this.InitializeLogging();
			}
			catch(Exception exception)
			{
				Trace.TraceError(exception.ToString());
				this.Core.Settings = new ApplicationSettings();
			}


		}


		/// <summary>
		/// Updates version info in Add-In page.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The e.</param>
		private void VersionUpdate(object sender, UpdateInfoEventArguments e)
		{
			AddInManager om = new AddInManager();

			try
			{
				om.NewAddInVersionAvailableAsync(e.Guid, e.Version, e.AddressUri, e.UpdateClassification); 
			}
			catch(Exception exception)
			{
				Trace.TraceError(exception.ToString());
			}
		}

		/// <summary>
		/// Initializes the logging.
		/// </summary>
		private void InitializeLogging()
		{
			if (this.Core.Settings.EnableLogging == false)
			{
				return;
			}

			try
			{
				Trace.Listeners.Add(new TextWriterTraceListener(Path.Combine(this.Core.Settings.ApplicationDataFolder, "logfile.log")));
				Trace.AutoFlush = true;
				Trace.TraceInformation("Start logging...");
			}
			catch (Exception exception)
			{
				Trace.TraceError(exception.ToString());
			}
		}

		/// <summary>
		/// 	Creates the tasks.
		/// </summary>
		/// <returns> </returns>
		public override TaskCollection CreateTasks()
		{
			TaskCollection tasks = new TaskCollection();
			tasks.Add(new SyncUiTask(StringResource.ConfigureWebdav,
			                         delegate
			                         	{
											// check for remote web access
											if (this.Core.CheckRemoteAccess() == false)
											{
												MessageBox.Show(StringResource.Error_RemoteAccess);
												return null;
											}

											// opens settings dialog
			                         		FormsWebDavConfig formsWebDavConfig = new FormsWebDavConfig(this.Core);
			                         		formsWebDavConfig.ShowDialog();
			                         		return null;
			                         	}));

			return tasks;
		}

		/// <summary>
		/// 	Creates the columns.
		/// </summary>
		/// <returns> </returns>
		public override ListColumnCollection<ListObject> CreateColumns()
		{
			return new ListColumnCollection<ListObject>();
		}

		/// <summary>
		/// 	Creates the refresh context.
		/// </summary>
		/// <returns> </returns>
		public override ListProviderAdorner CreateRefreshContext()
		{
			return new ListAdorner();
		}

		/// <summary>
		/// 	Gets the details.
		/// </summary>
		/// <param name="listObject"> The list object. </param>
		/// <returns> </returns>
		public override DetailGroup GetDetails(ListObject listObject)
		{
			return null;
		}
	}
}