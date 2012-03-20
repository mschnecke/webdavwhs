//----------------------------------------------------------------------------------------
// <copyright file="UpdateInfoEventArguments.cs" >
//     Copyright (c) 2012, Michael Schnecke, Göran Watzke. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------

using System;
using Microsoft.WindowsServerSolutions.AddinInfrastructure;

namespace WebDavWhs
{
	/// <summary>
	/// </summary>
	internal class UpdateInfoEventArguments : EventArgs
	{
		/// <summary>
		/// 	Gets or sets the GUID.
		/// </summary>
		/// <value> The GUID. </value>
		public Guid Guid
		{
			get;
			set;
		}

		/// <summary>
		/// 	Gets or sets the version.
		/// </summary>
		/// <value> The version. </value>
		public Version Version
		{
			get;
			set;
		}

		/// <summary>
		/// 	Gets or sets the address URI.
		/// </summary>
		/// <value> The address URI. </value>
		public Uri AddressUri
		{
			get;
			set;
		}

		/// <summary>
		/// 	Gets or sets the update classification.
		/// </summary>
		/// <value> The update classification. </value>
		public UpdateClassification UpdateClassification
		{
			get;
			set;
		}

		/// <summary>
		/// 	Initializes a new instance of the <see cref="UpdateInfoEventArguments" /> class.
		/// </summary>
		public UpdateInfoEventArguments()
		{
			this.Version = new Version();
			this.UpdateClassification = UpdateClassification.Update;
		}
	}
}