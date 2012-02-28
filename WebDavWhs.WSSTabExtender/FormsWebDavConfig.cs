//----------------------------------------------------------------------------------------
// <copyright file="FormsWebDavConfig.cs" >
//     Copyright (c) 2012, Michael Schnecke, Göran Watzke. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace WebDavWhs
{
	/// <summary>
	/// 	The webdav configuration form.
	/// </summary>
	public partial class FormsWebDavConfig : Form
	{
		/// <summary>
		/// 	Gets or sets the IIS.
		/// </summary>
		/// <value> The IIS. </value>
		private Iis Iis
		{
			get;
			set;
		}

		/// <summary>
		/// 	Gets or sets the settings.
		/// </summary>
		/// <value> The settings. </value>
		private ApplicationSettings Settings
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the trace listener.
		/// </summary>
		/// <value>
		/// The trace listener.
		/// </value>
		private TextWriterTraceListener TraceListener
		{
			get;
			set;
		}

		/// <summary>
		/// 	Initializes a new instance of the <see cref="FormsWebDavConfig" /> class.
		/// </summary>
		public FormsWebDavConfig()
		{
			this.InitializeComponent();
			this.InitializeComponentEx();
		}

		/// <summary>
		/// 	Releases unmanaged resources and performs other cleanup operations before the <see cref="FormsWebDavConfig" /> is reclaimed by garbage collection.
		/// </summary>
		~FormsWebDavConfig()
		{
			this.Cleanup();
		}

		/// <summary>
		/// 	Initializes aditional components.
		/// </summary>
		private void InitializeComponentEx()
		{
			this.Iis = new Iis();
			this.Settings = new ApplicationSettings();
			this.EnableLogger(this.Settings.EnableLogging);
		}

		/// <summary>
		/// 	Cleanups this instance.
		/// </summary>
		private void Cleanup()
		{
			if(this.Iis != null)
			{
				this.Iis.Dispose();
			}
		}

		/// <summary>
		/// 	Formses the web dav config load.
		/// </summary>
		/// <param name="sender"> The sender. </param>
		/// <param name="e"> The <see cref="System.EventArgs" /> instance containing the event data. </param>
		private void FormsWebDavConfigLoad(object sender, EventArgs e)
		{
			Trace.TraceInformation("FormsWebDavConfigLoad...");

			WebDavStatus webDavStatus = WebDavStatus.Unknown;

			try
			{
				// load settings
				this.Settings = ApplicationSettings.LoadSettings();

				// validate webdav state
				webDavStatus = this.Iis.GetWebDavStatus();
			}
			catch(Exception exception)
			{
				Trace.TraceError(exception.ToString());
			}

			switch(webDavStatus)
			{
				case WebDavStatus.Enabled:
					this.Settings.WebDavEnabled = true;
					break;
				case WebDavStatus.Disabled:
					this.Settings.WebDavEnabled = false;
					break;
				default:
					this.Settings.WebDavEnabled = false;
					break;
			}

			try
			{
				// validate whether the virtual directory already exists or not.
				string defaultWebSiteName = this.Iis.GetDefaultWebSite();
				if(this.Iis.ExistsVirtualDirectory(defaultWebSiteName, this.Settings.VirtualDirectoryAlias) == false)
				{
					this.Settings.WebDavEnabled = false;
				}
			}
			catch(Exception exception)
			{
				Trace.TraceError(exception.ToString());
			}

			this.PopulateData();
		}

		/// <summary>
		/// 	Formses the web dav config form closing.
		/// </summary>
		/// <param name="sender"> The sender. </param>
		/// <param name="e"> The <see cref="System.Windows.Forms.FormClosingEventArgs" /> instance containing the event data. </param>
		private void FormsWebDavConfigFormClosing(object sender, FormClosingEventArgs e)
		{
			if(this.DialogResult != DialogResult.OK)
			{
				return;
			}

			this.CollectData();
			
			try
			{
				string defaultWebSiteName = this.Iis.GetDefaultWebSite();

				if (this.Settings.WebDavEnabled)
				{
					// check whether the virtual directory already exists or not and ask user for proceeding
					if (this.Iis.ExistsVirtualDirectory(defaultWebSiteName, this.Settings.VirtualDirectoryAlias))
					{
						if (MessageBox.Show(string.Format(StringResource.Question_Overwite, this.Settings.VirtualDirectoryAlias), StringResource.AddInName, MessageBoxButtons.YesNo) != DialogResult.Yes)
						{
							e.Cancel = true;
							return;
						}


						// todo: remove the virtual directory

					}

					// enable WebDAV
					if (this.Iis.GetWebDavStatus() != WebDavStatus.Enabled)
					{
						this.Iis.SetWebDavStatus(true, true);
					}

					//// create virtual directory
					//string serverFolderPath = ""; //TODO get server folder root
					//this.Iis.CreateVirtualDirectory(this.Settings.VirtualDirectoryAlias, serverFolderPath);

					//// set authentication
					//this.Iis.SetAnonymousAuthentication(string.Format("{0}/{1}", defaultWebSiteName, this.Settings.VirtualDirectoryAlias), false);
					//this.Iis.SetBasicAuthentication(string.Format("{0}/{1}", defaultWebSiteName, this.Settings.VirtualDirectoryAlias), false);
					//this.Iis.SetWindowsAuthentication(string.Format("{0}/{1}", defaultWebSiteName, this.Settings.VirtualDirectoryAlias), true);
				}
				else
				{
					
				}
			}
			catch(Exception exception)
			{
				MessageBox.Show(exception.Message);
				e.Cancel = true;
				return;
			}

			try
			{
				ApplicationSettings.SaveSettings(this.Settings);
			}
			catch(Exception exception)
			{
				Trace.TraceError(exception.ToString());
			}
		}

		/// <summary>
		/// 	Populates the data.
		/// </summary>
		private void PopulateData()
		{
			if(this.Settings.WebDavEnabled)
			{
				this.rbEnable.Checked = true;
			}
			else
			{
				this.rbDisable.Checked = true;
			}

			if(string.IsNullOrEmpty(this.Settings.VirtualDirectoryAlias))
			{
				this.Settings.VirtualDirectoryAlias = "webdav";
			}

			this.tbVirtDir.Text = this.Settings.VirtualDirectoryAlias;
			this.cbLogging.Checked = this.Settings.EnableLogging;

			this.ValidateControls();
		}

		/// <summary>
		/// 	Collects the data.
		/// </summary>
		private void CollectData()
		{
			this.Settings.WebDavEnabled = this.rbEnable.Checked;
			this.Settings.VirtualDirectoryAlias = this.tbVirtDir.Text;
			this.Settings.EnableLogging = this.cbLogging.Checked;
		}

		/// <summary>
		/// 	Validates the controls.
		/// </summary>
		private void ValidateControls()
		{
			if(string.IsNullOrWhiteSpace(this.tbVirtDir.Text))
			{
				this.btnOk.Enabled = false;
				this.errorProvider.SetError(this.tbVirtDir, StringResource.Error_VirtDirName);
			}
			else
			{
				this.btnOk.Enabled = true;
				this.errorProvider.SetError(this.tbVirtDir, string.Empty);
				this.lilaLink.Text = string.Format(StringResource.PlaceholderUrl, this.tbVirtDir.Text);
			}

			this.gbVirtDir.Enabled = this.rbEnable.Checked;
		}

		/// <summary>
		/// 	Handles the CheckedChanged event of the control.
		/// </summary>
		/// <param name="sender"> The sender. </param>
		/// <param name="e"> The <see cref="System.EventArgs" /> instance containing the event data. </param>
		private void RbDisableCheckedChanged(object sender, EventArgs e)
		{
			this.ValidateControls();
		}

		/// <summary>
		/// 	Handles the CheckedChanged event of the control.
		/// </summary>
		/// <param name="sender"> The sender. </param>
		/// <param name="e"> The <see cref="System.EventArgs" /> instance containing the event data. </param>
		private void RbEnableCheckedChanged(object sender, EventArgs e)
		{
			this.ValidateControls();
		}

		/// <summary>
		/// 	Tbs the virt dir text changed.
		/// </summary>
		/// <param name="sender"> The sender. </param>
		/// <param name="e"> The <see cref="System.EventArgs" /> instance containing the event data. </param>
		private void TbVirtDirTextChanged(object sender, EventArgs e)
		{
			this.ValidateControls();
		}

		/// <summary>
		/// 	Handles the LinkClicked event of the lilaLink control.
		/// </summary>
		/// <param name="sender"> The sender. </param>
		/// <param name="e"> The <see cref="System.Windows.Forms.LinkLabelLinkClickedEventArgs" /> instance containing the event data. </param>
		private void LilaLinkLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start(this.lilaLink.Text);
		}

		/// <summary>
		/// Handles the LinkClicked event of the lilaLink control.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.Windows.Forms.LinkLabelLinkClickedEventArgs"/> instance containing the event data.</param>
		private void LilaLogFileLocationLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start(this.Settings.ApplicationDataFolder);
		}

		/// <summary>
		/// Enables the logger.
		/// </summary>
		/// <param name="enable">if set to <c>true</c> [enable].</param>
		private void EnableLogger(bool enable)
		{
			const string tracelistenerName = "WEBDAVWHSTL";

			try
			{
				if(enable)
				{
					string logfile = Path.Combine(this.Settings.ApplicationDataFolder, "logfile.txt");
					TextWriterTraceListener traceListener = new TextWriterTraceListener(logfile, tracelistenerName);
					Trace.Listeners.Add(traceListener);
					Trace.AutoFlush = true;

					Trace.TraceInformation("EnableLogger...");
				}
				else
				{
					Trace.Listeners.Remove(tracelistenerName);
				}
			}
			catch (Exception exception)
			{
				Trace.TraceError(exception.ToString());
			}
		}
	}
}