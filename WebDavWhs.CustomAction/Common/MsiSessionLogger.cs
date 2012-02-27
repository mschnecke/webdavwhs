//----------------------------------------------------------------------------------------
// <copyright file="MsiSessionLogger.cs" >
//     Copyright (c) 2012, Michael Schnecke, Göran Watzke. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------

using Microsoft.Deployment.WindowsInstaller;

namespace WebDavWhs.CustomAction.Common
{
	/// <summary>
	/// 	Provides properties and methods to handle the MSI session log.
	/// </summary>
	internal class MsiSessionLogger : IMsiSessionLogger
	{
		/// <summary>
		/// 	The session.
		/// </summary>
		private readonly Session session;

		/// <summary>
		/// 	Initializes a new instance of the <see cref="MsiSessionLogger" /> class.
		/// </summary>
		/// <param name="session"> The session. </param>
		public MsiSessionLogger(Session session)
		{
			this.session = session;
		}

		/// <summary>
		/// 	Writes the specified log message.
		/// </summary>
		/// <param name="message"> The message. </param>
		public void WriteLog(string message)
		{
			if(string.IsNullOrEmpty(message))
			{
				return;
			}

			if(this.session == null)
			{
				return;
			}

			this.session.Log(message);
		}

		/// <summary>
		/// 	Writes the specified log message.
		/// </summary>
		/// <param name="format"> The format. </param>
		/// <param name="args"> The args. </param>
		public void WriteLog(string format, params object[] args)
		{
			if(string.IsNullOrEmpty(format))
			{
				return;
			}

			if(args == null)
			{
				return;
			}

			if(this.session == null)
			{
				return;
			}

			this.session.Log(format, args);
		}
	}
}