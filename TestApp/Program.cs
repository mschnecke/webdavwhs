using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using WebDavWhs;

namespace TestApp
{
	class Program
	{
		static void Main(string[] args)
		{
			Trace.Listeners.Add(new ConsoleTraceListener());
			Trace.AutoFlush = true;


			Iis iis = new Iis();

			try
			{
				iis.SetWebDavStatus(true, true);


				iis.SetWebDavStatus(false, true);



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
