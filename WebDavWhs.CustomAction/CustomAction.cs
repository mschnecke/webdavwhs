using System;
using Microsoft.Deployment.WindowsInstaller;

namespace WebDavWhs.CustomAction
{
	/// <summary>
	/// 	Provides Custom Actions for Windows Installer.
	/// </summary>
	public class CustomActions
	{
		[CustomAction]
		public static ActionResult TestCa(Session session)
		{
			try
			{
				return ActionResult.Success;
			}
			catch(Exception exception)
			{
				session.Log(@"Exception: {0}", exception.ToString());
				return ActionResult.Failure;
			}
		}
	}
}