//----------------------------------------------------------------------------------------
// <copyright file="PageAdorner.cs" >
//     Copyright (c) 2012, Michael Schnecke, Göran Watzke. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------

using System;
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
		/// 	Initializes a new instance of the <see cref="PageAdorner" /> class.
		/// </summary>
		public PageAdorner()
			: base(new Guid("{572F7115-00BC-4E0F-B466-23FA6468219C}"), "WebDAV for WHS Add-in", "WebDAV for WHS Add-in")
		{
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
			                         		FormsWebDavConfig formsWebDavConfig = new FormsWebDavConfig();
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