using System;
using System.Diagnostics;
using WebDavWhs;

namespace TestApp
{
	class Program
	{
		static void Main(string[] args)
		{
			Trace.Listeners.Add(new ConsoleTraceListener());
			Trace.AutoFlush = true;

			const string virtDir = "webdav";
			const string physPath = @"D:\ServerFolders";

			Iis iis = new Iis();

			try
			{
				iis.SetWebDavStatus(true, true);

				iis.CreateVirtualDirectory(virtDir, physPath);

				string defaultWebSite = iis.GetDefaultWebSite();

				iis.SetAnonymousAuthentication(defaultWebSite + "/" + virtDir, false);
				
				iis.SetBasicAuthentication(defaultWebSite + "/" + virtDir, false);
				
				iis.SetWindowsAuthentication(defaultWebSite + "/" + virtDir, true);
				
			}
			catch (Exception exception)
			{

			}
			finally
			{
				iis.Dispose();
			}
		}
	}
}
