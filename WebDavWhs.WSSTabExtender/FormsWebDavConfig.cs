//----------------------------------------------------------------------------------------
// <copyright file="FormsWebDavConfig.cs" >
//     Copyright (c) 2012, Michael Schnecke, Göran Watzke. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace WebDavWhs
{
	/// <summary>
	/// 	The webdav configuration form.
	/// </summary>
	internal partial class FormsWebDavConfig : Form
	{
		/// <summary>
		/// 	Gets or sets the core.
		/// </summary>
		/// <value> The core. </value>
		private Core Core
		{
			get;
			set;
		}

		/// <summary>
		/// 	Initializes a new instance of the <see cref="FormsWebDavConfig" /> class.
		/// </summary>
		/// <param name="core"> The core. </param>
		public FormsWebDavConfig(Core core)
		{
			this.Core = core;
			this.InitializeComponent();
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
				// validate webdav state
				webDavStatus = this.Core.Iis.GetWebDavStatus();
			}
			catch(Exception exception)
			{
				Trace.TraceError(exception.ToString());
			}

			switch (webDavStatus)
			{
				case WebDavStatus.Enabled:
					this.Core.Settings.WebDavEnabled = true;
					break;
				case WebDavStatus.Disabled:
					this.Core.Settings.WebDavEnabled = false;
					break;
				default:
					this.Core.Settings.WebDavEnabled = false;
					break;
			}

			try
			{
				// validate whether the virtual directory already exists or not.
				string defaultWebSiteName = this.Core.Iis.GetDefaultWebSite();
				if (this.Core.Iis.ExistsVirtualDirectory(defaultWebSiteName, this.Core.Settings.VirtualDirectoryAlias) == false)
				{
					this.Core.Settings.WebDavEnabled = false;
				}
			}
			catch(Exception exception)
			{
				Trace.TraceError(exception.ToString());
			}

			this.PopulateData();
			Trace.TraceInformation("FormsWebDavConfigLoad...finished.");
		}

		/// <summary>
		/// 	Formses the web dav config form closing.
		/// </summary>
		/// <param name="sender"> The sender. </param>
		/// <param name="e"> The <see cref="System.Windows.Forms.FormClosingEventArgs" /> instance containing the event data. </param>
		private void FormsWebDavConfigFormClosing(object sender, FormClosingEventArgs e)
		{
			Trace.TraceInformation("FormsWebDavConfigFormClosing...");

			if(this.DialogResult != DialogResult.OK)
			{
				Trace.TraceInformation("FormsWebDavConfigFormClosing...finished.");
				return;
			}

			this.CollectData();

			try
			{
				string defaultWebSiteName = this.Core.Iis.GetDefaultWebSite();

				if(this.Core.Settings.WebDavEnabled)
				{
					// enable WebDAV
					if (this.Core.Iis.GetWebDavStatus() != WebDavStatus.Enabled)
					{
						this.Core.Iis.SetWebDavStatus(true, true);
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
				Trace.TraceError(exception.ToString());
				e.Cancel = true;
				return;
			}

			try
			{
				ApplicationSettings.SaveSettings(this.Core.Settings);
			}
			catch(Exception exception)
			{
				Trace.TraceError(exception.ToString());
			}

			Trace.TraceInformation("FormsWebDavConfigFormClosing...finished.");
		}

		/// <summary>
		/// 	Populates the data.
		/// </summary>
		private void PopulateData()
		{
			if(this.Core.Settings.WebDavEnabled)
			{
				this.rbEnable.Checked = true;
			}
			else
			{
				this.rbDisable.Checked = true;
			}

			if(string.IsNullOrEmpty(this.Core.Settings.VirtualDirectoryAlias))
			{
				this.Core.Settings.VirtualDirectoryAlias = "webdav";
			}

			this.tbVirtDir.Text = this.Core.Settings.VirtualDirectoryAlias;
			this.cbLogging.Checked = this.Core.Settings.EnableLogging;

			this.ValidateControls();
		}

		/// <summary>
		/// 	Collects the data.
		/// </summary>
		private void CollectData()
		{
			this.Core.Settings.WebDavEnabled = this.rbEnable.Checked;
			this.Core.Settings.VirtualDirectoryAlias = this.tbVirtDir.Text;
			this.Core.Settings.EnableLogging = this.cbLogging.Checked;
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
		/// 	Handles the LinkClicked event of the lilaLink control.
		/// </summary>
		/// <param name="sender"> The sender. </param>
		/// <param name="e"> The <see cref="System.Windows.Forms.LinkLabelLinkClickedEventArgs" /> instance containing the event data. </param>
		private void LilaLogFileLocationLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start(this.Core.Settings.ApplicationDataFolder);
		}
	}
}