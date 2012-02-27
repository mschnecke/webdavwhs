//----------------------------------------------------------------------------------------
// <copyright file="CustomAction.cs" >
//     Copyright (c) 2012, Michael Schnecke, Göran Watzke. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------

using System;
using Microsoft.Deployment.WindowsInstaller;

namespace WebDavWhs.CustomAction
{
	/// <summary>
	/// 	Provides Custom Actions for Windows Installer.
	/// </summary>
	public class CustomActions
	{
		/// <summary>
		/// Creates the process.
		/// </summary>
		/// <param name="session">The session.</param>
		/// <returns>Custom Action result.</returns>
		[CustomAction]
		public static ActionResult CreateProcess(Session session)
		{
			string application = session.CustomActionData["App"];
			string arguments = session.CustomActionData["Args"];

			session.Log(@"CreateProcess CustomActionData Argument: {0}: '{1}'", "App", application);
			session.Log(@"CreateProcess CustomActionData Argument: {0}: '{1}'", "Args", arguments);

			string stdoutput = null;

			try
			{
				ProcessHelper.StartProcess(application, arguments, true, true, out stdoutput);
			}
			catch (Exception exception)
			{
				session.Log(@"CreateProcess Exception: {0}", exception.ToString());
				return ActionResult.Failure;
			}
			finally
			{
				session.Log(@"CreateProcess output: {0}", stdoutput);
			}

			return ActionResult.Success;
		}
	}
}