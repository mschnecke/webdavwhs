//----------------------------------------------------------------------------------------
// <copyright file="StoragePropertyEventArgs.cs" >
//     Copyright (c) 2012, Michael Schnecke, Göran Watzke. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace WebDavWhs
{
	/// <summary>
	/// 	The storage change property event arguments.
	/// </summary>
	internal class StoragePropertyEventArgs : EventArgs
	{
		/// <summary>
		/// 	Gets or sets the server folders.
		/// </summary>
		/// <value> The server folders. </value>
		public Dictionary<Guid, ServerFolder> ServerFolders
		{
			get;
			set;
		}

		/// <summary>
		/// 	Initializes a new instance of the <see cref="StoragePropertyEventArgs" /> class.
		/// </summary>
		public StoragePropertyEventArgs()
		{
			this.ServerFolders = new Dictionary<Guid, ServerFolder>();
		}

		/// <summary>
		/// 	Initializes a new instance of the <see cref="StoragePropertyEventArgs" /> class.
		/// </summary>
		/// <param name="serverFolders"> The server folders. </param>
		public StoragePropertyEventArgs(Dictionary<Guid, ServerFolder> serverFolders)
		{
			this.ServerFolders = serverFolders;
		}
	}
}