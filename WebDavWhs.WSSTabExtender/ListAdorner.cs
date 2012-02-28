//----------------------------------------------------------------------------------------
// <copyright file="ListAdorner.cs" >
//     Copyright (c) 2012, Michael Schnecke, Göran Watzke. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------

using System.Collections.Generic;
using Microsoft.WindowsServerSolutions.Administration.ObjectModel.Adorners;

namespace WebDavWhs
{
	/// <summary>
	/// The ListAdorner 
	/// </summary>
	public class ListAdorner : ListProviderAdorner
	{
		/// <summary>
		/// Refreshes the and listen for updates.
		/// </summary>
		/// <param name="list">The list.</param>
		public override void RefreshAndListenForUpdates(IList<ListObject> list)
		{
		}

		/// <summary>
		/// Stops the listening for updates.
		/// </summary>
		public override void StopListeningForUpdates()
		{
		}
	}
}
