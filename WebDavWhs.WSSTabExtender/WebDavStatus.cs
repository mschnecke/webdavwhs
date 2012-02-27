//----------------------------------------------------------------------------------------
// <copyright file="WebDavStatus.cs" >
//     Copyright (c) 2012, Michael Schnecke, Göran Watzke. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------

namespace WebDavWhs
{
	/// <summary>
	/// 	The WebDAV state.
	/// </summary>
	internal enum WebDavStatus
	{
		/// <summary>
		/// 	Unknown.
		/// </summary>
		Unknown,

		/// <summary>
		/// 	WebDAV enabled.
		/// </summary>
		Enabled,

		/// <summary>
		/// 	WebDAV disabled.
		/// </summary>
		Disabled
	}
}