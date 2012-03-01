//----------------------------------------------------------------------------------------
// <copyright file="Program.cs" >
//     Copyright (c) 2012, Michael Schnecke, Göran Watzke. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------


using System;
using System.Diagnostics;

namespace WebDavWhs
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			Core core = new Core();

			try
			{
				//core.Settings = ApplicationSettings.LoadSettings();

				//core.EnableWebDav();

				//core.DisableWebDav();



			}
			catch(Exception exception)
			{
				Trace.TraceError(exception.ToString());
			}
			finally
			{
				core.Dispose();
			}
		}
	}
}