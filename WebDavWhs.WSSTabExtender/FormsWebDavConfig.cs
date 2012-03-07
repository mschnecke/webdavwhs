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
				string defaultWebSiteName = this.Core.Iis.GetDefaultWebSite();
				webDavStatus = this.Core.Iis.GetWebDavStatus(defaultWebSiteName);
			}
			catch(Exception exception)
			{
				Trace.TraceError(exception.ToString());
			}

			switch(webDavStatus)
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
				if(this.Core.Iis.ExistsVirtualDirectory(defaultWebSiteName, @"/", this.Core.Settings.VirtualDirectoryAlias) == false)
				{
					this.Core.Settings.WebDavEnabled = false;
				}
			}
			catch(Exception exception)
			{
				Trace.TraceError(exception.ToString());
			}

			try
			{
				this.Core.Settings.DomainName = this.Core.GetDomainName();
			}
			catch(Exception exception)
			{
				Trace.TraceError(exception.ToString());
			}

			if (string.IsNullOrEmpty(this.Core.Settings.DomainName))
			{
				try
				{
					this.Core.Settings.DomainName = Environment.MachineName;
				}
				catch(Exception exception)
				{
					Trace.TraceError(exception.ToString());
				}
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

			Cursor.Current = Cursors.WaitCursor;

			this.CollectData();

			try
			{
				if(this.Core.Settings.WebDavEnabled)
				{
					this.Core.EnableWebDav();
				}
				else
				{
					this.Core.DisableWebDav();
				}
			}
			catch(Exception exception)
			{
				Trace.TraceError(exception.ToString());
				e.Cancel = true;
				Cursor.Current = Cursors.Default;
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
			Cursor.Current = Cursors.Default;
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

			this.cbSsl.Checked = this.Core.Settings.UseSsl;

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
			this.Core.Settings.UseSsl = this.cbSsl.Checked;
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
			}

			this.gbVirtDir.Enabled = this.rbEnable.Checked;
			this.cbSsl.Enabled = this.rbEnable.Checked;

			this.SetUrlText();
		}

		/// <summary>
		/// Sets the URL text.
		/// </summary>
		private void SetUrlText()
		{
			string prefix = @"http";

			if(this.cbSsl.Checked)
			{
				prefix = @"https";
			}

			this.tbUrl.Text = string.Format(StringResource.PlaceholderUrl,
			                                prefix,
			                                this.Core.Settings.DomainName,
			                                this.tbVirtDir.Text);
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
			Clipboard.SetDataObject(this.tbUrl.Text);
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

		/// <summary>
		/// Handles the CheckedChanged event of the cbSsl control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void CbSslCheckedChanged(object sender, EventArgs e)
		{
			this.ValidateControls();
		}
	}
}