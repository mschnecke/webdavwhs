//----------------------------------------------------------------------------------------
// <copyright file="IMsiSessionLogger.cs" >
//     Copyright (c) 2012, Michael Schnecke, Göran Watzke. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------

namespace WebDavWhs.CustomAction.Common
{
	/// <summary>
	/// Interface for MSI logger.
	/// </summary>
	internal interface IMsiSessionLogger
	{
		/// <summary>
		/// 	Writes the specified log message.
		/// </summary>
		/// <param name = "message">The message.</param>
		void WriteLog(string message);

		/// <summary>
		/// 	Writes the specified log message.
		/// </summary>
		/// <param name = "format">The format.</param>
		/// <param name = "args">The args.</param>
		void WriteLog(string format, params object[] args);
	}
}