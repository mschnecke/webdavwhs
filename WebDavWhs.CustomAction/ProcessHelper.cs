//----------------------------------------------------------------------------------------
// <copyright file="ProcessHelper.cs" >
//     Copyright (c) 2012, Michael Schnecke, Göran Watzke. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------

using System.Diagnostics;

namespace WebDavWhs.CustomAction
{
	/// <summary>
	/// Provides methods to create processes.
	/// </summary>
	internal class ProcessHelper
	{
		/// <summary>
		/// 	Occurs when StartProcess output data has been received.
		/// </summary>
		public static event DataReceivedEventHandler StartProcessOutputDataReceived;

		/// <summary>
		/// 	Starts process and waits for exit or runs silently optional.
		/// </summary>
		/// <param name = "application">Application to start.</param>
		/// <param name = "commandLine">Command line to pass.</param>
		/// <param name = "wait">Wait for exit.</param>
		/// <param name = "silent">Run silently.</param>
		public static void StartProcess(string application, string commandLine, bool wait, bool silent)
		{
			Process process = new Process();

			try
			{
				process.StartInfo.FileName = application;
				process.StartInfo.Arguments = commandLine;
				process.StartInfo.ErrorDialog = true;
				process.StartInfo.UseShellExecute = false;
				process.StartInfo.RedirectStandardOutput = true;
				process.OutputDataReceived += OnProcessOutputHandler;

				if(silent)
				{
					process.StartInfo.CreateNoWindow = true;
					process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
				}

				process.Start();
				process.BeginOutputReadLine();

				if(wait)
				{
					process.WaitForExit();
				}
			}
			finally
			{
				process.Close();
			}
		}

		/// <summary>
		/// 	Starts process and waits for exit.
		/// </summary>
		/// <param name = "application">Application to start.</param>
		/// <param name = "commandLine">Command line to pass.</param>
		/// <param name = "silent">Start silent.</param>
		/// <param name = "wait">Wait for exit.</param>
		/// <param name = "stdOutput">The standard output.</param>
		public static void StartProcess(string application, string commandLine, bool silent, bool wait, out string stdOutput)
		{
			Process process = new Process();
			stdOutput = string.Empty;

			try
			{
				process.StartInfo.FileName = application;
				process.StartInfo.Arguments = commandLine;
				process.StartInfo.ErrorDialog = true;
				process.StartInfo.UseShellExecute = false;
				process.StartInfo.RedirectStandardOutput = true;

				if(silent)
				{
					process.StartInfo.CreateNoWindow = true;
					process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
				}

				process.Start();
				stdOutput = process.StandardOutput.ReadToEnd();

				if(wait)
				{
					process.WaitForExit();
				}
			}
			finally
			{
				process.Close();
				process.Dispose();
			}
		}

		/// <summary>
		/// 	Processes the output handler.
		/// </summary>
		/// <param name = "sender">The sender.</param>
		/// <param name = "e">The <see cref = "System.Diagnostics.DataReceivedEventArgs" /> instance containing the event data.</param>
		private static void OnProcessOutputHandler(object sender, DataReceivedEventArgs e)
		{
			if(StartProcessOutputDataReceived != null)
			{
				StartProcessOutputDataReceived(null, e);
			}
		}
	}
}