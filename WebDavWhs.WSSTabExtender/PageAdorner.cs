//----------------------------------------------------------------------------------------
// <copyright file="PageAdorner.cs" >
//     Copyright (c) 2012, Michael Schnecke, Göran Watzke. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
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
		/// 	The Enable task.
		/// </summary>
		private SyncUiTask enableTask;

		/// <summary>
		/// 	The Disable task.
		/// </summary>
		private SyncUiTask disableTask;

		/// <summary>
		/// 	Gets or sets the IIS.
		/// </summary>
		/// <value> The IIS. </value>
		private Iis Iis
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
			this.InitializeTasks();
			this.Iis = new Iis();
		}

		/// <summary>
		/// 	Releases unmanaged and - optionally - managed resources
		/// </summary>
		/// <param name="disposing"> <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources. </param>
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			this.Iis.Dispose();
		}

		public override ListColumnCollection<ListObject> CreateColumns()
		{
			ListColumnCollection<ListObject> columns = new ListColumnCollection<ListObject>();

			// Place your column definitions here

			return (columns);
		}

		public override ListProviderAdorner CreateRefreshContext()
		{
			return (new ListAdorner());
		}

		/// <summary>
		/// 	Creates the tasks.
		/// </summary>
		/// <returns> </returns>
		public override TaskCollection CreateTasks()
		{
			TaskCollection tasks = new TaskCollection();
			WebDavStatus webDavStatus = this.GetWebDavStatus();

			switch(webDavStatus)
			{
				case WebDavStatus.Enabled:
					tasks.Add(this.disableTask);
					break;
				default:
					tasks.Add(this.enableTask);
					break;
			}

			return tasks;
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

		/// <summary>
		/// 	Initializes the tasks.
		/// </summary>
		private void InitializeTasks()
		{
			try
			{
				this.enableTask = new SyncUiTask(StringResource.GlobalTask_Enable,
				                                 delegate
				                                 	{
				                                 		this.EnableWebDav();
				                                 		return null;
				                                 	});

				this.disableTask = new SyncUiTask(StringResource.GlobalTask_Disable,
				                                  delegate
				                                  	{
				                                  		this.DisableWebDav();
				                                  		return null;
				                                  	});
			}
			catch(Exception exception)
			{
				Trace.TraceError(exception.ToString());
			}
		}

		/// <summary>
		/// 	Enables the web dav.
		/// </summary>
		private void EnableWebDav()
		{
			try
			{
				this.Iis.SetWebDavStatus(true, true);

				// todo: add virt dir
				// todo: add auth settings
			}
			catch(Exception exception)
			{
				Trace.TraceError(exception.ToString());
			}

			TaskCollection tasks = new TaskCollection();
			tasks.Remove(this.enableTask);
			tasks.Add(this.disableTask);

			this.Refresh();
		}

		/// <summary>
		/// 	Disables the web dav.
		/// </summary>
		private void DisableWebDav()
		{
			try
			{
				// todo: remove virt dir

				this.Iis.SetWebDavStatus(false, true);
			}
			catch(Exception exception)
			{
				Trace.TraceError(exception.ToString());
			}

			TaskCollection tasks = new TaskCollection();
			tasks.Remove(this.disableTask);
			tasks.Add(this.enableTask);

			this.Refresh();
		}

		/// <summary>
		/// 	Gets the web dav status.
		/// </summary>
		/// <returns> </returns>
		private WebDavStatus GetWebDavStatus()
		{
			try
			{
				return this.Iis.GetWebDavStatus();
			}
			catch(Exception exception)
			{
				Trace.TraceError(exception.ToString());
				return WebDavStatus.Unknown;
			}
		}
	}
}