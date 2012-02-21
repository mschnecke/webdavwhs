using System.Diagnostics;
using Microsoft.Deployment.WindowsInstaller;

namespace WebDavWhs.CustomAction.Common
{
	/// <summary>
	/// MSI log file trace listener.
	/// </summary>
	internal class MsiLogTraceListener : TraceListener
	{
		/// <summary>
		/// Gets or sets the session.
		/// </summary>
		/// <value>
		/// The session.
		/// </value>
		private Session Session
		{
			get;
			set;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MsiLogTraceListener"/> class.
		/// </summary>
		/// <param name="session">The session.</param>
		public MsiLogTraceListener(Session session)
		{
			this.Session = session;
		}

		/// <summary>
		/// When overridden in a derived class, writes the specified message to the listener you create in the derived class.
		/// </summary>
		/// <param name="message">A message to write.</param><filterpriority>2</filterpriority>
		public override void Write(string message)
		{
			this.Session.Log(message);
		}

		/// <summary>
		/// When overridden in a derived class, writes a message to the listener you create in the derived class, followed by a line terminator.
		/// </summary>
		/// <param name="message">A message to write.</param><filterpriority>2</filterpriority>
		public override void WriteLine(string message)
		{
			this.Session.Log(message);
		}
	}
}