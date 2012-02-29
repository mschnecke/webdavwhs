//----------------------------------------------------------------------------------------
// <copyright file="ServerFolder.cs" >
//     Copyright (c) 2012, Michael Schnecke, Göran Watzke. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------

using System;

namespace WebDavWhs
{
	/// <summary>
	/// 	The server folder.
	/// </summary>
	internal class ServerFolder
	{
		/// <summary>
		/// 	Gets or sets the name.
		/// </summary>
		/// <value> The name. </value>
		public string Name
		{
			get;
			set;
		}

		/// <summary>
		/// 	Gets or sets the physical path.
		/// </summary>
		/// <value> The physical path. </value>
		public string Path
		{
			get;
			set;
		}

		/// <summary>
		/// 	Gets or sets the ID.
		/// </summary>
		/// <value> The ID. </value>
		public Guid ID
		{
			get;
			set;
		}

		/// <summary>
		/// 	Initializes a new instance of the <see cref="ServerFolder" /> class.
		/// </summary>
		public ServerFolder()
		{
			this.Name = string.Empty;
			this.Path = string.Empty;
		}
	}
}