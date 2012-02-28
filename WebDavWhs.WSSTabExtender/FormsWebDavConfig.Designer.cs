namespace WebDavWhs
{
	partial class FormsWebDavConfig
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOk = new System.Windows.Forms.Button();
			this.rbDisable = new System.Windows.Forms.RadioButton();
			this.rbEnable = new System.Windows.Forms.RadioButton();
			this.tbVirtDir = new System.Windows.Forms.TextBox();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tpCommon = new System.Windows.Forms.TabPage();
			this.gbVirtDir = new System.Windows.Forms.GroupBox();
			this.lilaLink = new System.Windows.Forms.LinkLabel();
			this.tpLogging = new System.Windows.Forms.TabPage();
			this.lilaLogFileLocation = new System.Windows.Forms.LinkLabel();
			this.cbLogging = new System.Windows.Forms.CheckBox();
			this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
			this.tabControl.SuspendLayout();
			this.tpCommon.SuspendLayout();
			this.gbVirtDir.SuspendLayout();
			this.tpLogging.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
			this.SuspendLayout();
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(334, 314);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnOk
			// 
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(253, 314);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 0;
			this.btnOk.Text = "OK";
			this.btnOk.UseVisualStyleBackColor = true;
			// 
			// rbDisable
			// 
			this.rbDisable.AutoSize = true;
			this.rbDisable.Checked = true;
			this.rbDisable.Location = new System.Drawing.Point(33, 31);
			this.rbDisable.Name = "rbDisable";
			this.rbDisable.Size = new System.Drawing.Size(108, 17);
			this.rbDisable.TabIndex = 0;
			this.rbDisable.TabStop = true;
			this.rbDisable.Text = "&Disable WebDAV";
			this.rbDisable.UseVisualStyleBackColor = true;
			this.rbDisable.CheckedChanged += new System.EventHandler(this.RbDisableCheckedChanged);
			// 
			// rbEnable
			// 
			this.rbEnable.AutoSize = true;
			this.rbEnable.Location = new System.Drawing.Point(33, 54);
			this.rbEnable.Name = "rbEnable";
			this.rbEnable.Size = new System.Drawing.Size(106, 17);
			this.rbEnable.TabIndex = 1;
			this.rbEnable.Text = "&Enable WebDAV";
			this.rbEnable.UseVisualStyleBackColor = true;
			this.rbEnable.CheckedChanged += new System.EventHandler(this.RbEnableCheckedChanged);
			// 
			// tbVirtDir
			// 
			this.tbVirtDir.Location = new System.Drawing.Point(18, 33);
			this.tbVirtDir.Name = "tbVirtDir";
			this.tbVirtDir.Size = new System.Drawing.Size(149, 20);
			this.tbVirtDir.TabIndex = 0;
			this.tbVirtDir.TextChanged += new System.EventHandler(this.TbVirtDirTextChanged);
			// 
			// tabControl
			// 
			this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl.Controls.Add(this.tpCommon);
			this.tabControl.Controls.Add(this.tpLogging);
			this.tabControl.Location = new System.Drawing.Point(12, 12);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(397, 282);
			this.tabControl.TabIndex = 2;
			// 
			// tpCommon
			// 
			this.tpCommon.Controls.Add(this.gbVirtDir);
			this.tpCommon.Controls.Add(this.rbDisable);
			this.tpCommon.Controls.Add(this.rbEnable);
			this.tpCommon.Location = new System.Drawing.Point(4, 22);
			this.tpCommon.Name = "tpCommon";
			this.tpCommon.Padding = new System.Windows.Forms.Padding(3);
			this.tpCommon.Size = new System.Drawing.Size(389, 256);
			this.tpCommon.TabIndex = 0;
			this.tpCommon.Text = "Common";
			this.tpCommon.UseVisualStyleBackColor = true;
			// 
			// gbVirtDir
			// 
			this.gbVirtDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gbVirtDir.Controls.Add(this.lilaLink);
			this.gbVirtDir.Controls.Add(this.tbVirtDir);
			this.gbVirtDir.Location = new System.Drawing.Point(33, 86);
			this.gbVirtDir.Name = "gbVirtDir";
			this.gbVirtDir.Size = new System.Drawing.Size(327, 108);
			this.gbVirtDir.TabIndex = 2;
			this.gbVirtDir.TabStop = false;
			this.gbVirtDir.Text = "&Alias for the virtual webdav folder";
			// 
			// lilaLink
			// 
			this.lilaLink.AutoSize = true;
			this.lilaLink.Location = new System.Drawing.Point(15, 71);
			this.lilaLink.Name = "lilaLink";
			this.lilaLink.Size = new System.Drawing.Size(90, 13);
			this.lilaLink.TabIndex = 1;
			this.lilaLink.TabStop = true;
			this.lilaLink.Text = "https://localhost/";
			this.lilaLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LilaLinkLinkClicked);
			// 
			// tpLogging
			// 
			this.tpLogging.Controls.Add(this.lilaLogFileLocation);
			this.tpLogging.Controls.Add(this.cbLogging);
			this.tpLogging.Location = new System.Drawing.Point(4, 22);
			this.tpLogging.Name = "tpLogging";
			this.tpLogging.Size = new System.Drawing.Size(389, 256);
			this.tpLogging.TabIndex = 1;
			this.tpLogging.Text = "Logging";
			this.tpLogging.UseVisualStyleBackColor = true;
			// 
			// lilaLogFileLocation
			// 
			this.lilaLogFileLocation.AutoSize = true;
			this.lilaLogFileLocation.Location = new System.Drawing.Point(50, 68);
			this.lilaLogFileLocation.Name = "lilaLogFileLocation";
			this.lilaLogFileLocation.Size = new System.Drawing.Size(106, 13);
			this.lilaLogFileLocation.TabIndex = 1;
			this.lilaLogFileLocation.TabStop = true;
			this.lilaLogFileLocation.Text = "Open log file location";
			this.lilaLogFileLocation.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LilaLogFileLocationLinkClicked);
			// 
			// cbLogging
			// 
			this.cbLogging.AutoSize = true;
			this.cbLogging.Location = new System.Drawing.Point(32, 37);
			this.cbLogging.Name = "cbLogging";
			this.cbLogging.Size = new System.Drawing.Size(96, 17);
			this.cbLogging.TabIndex = 0;
			this.cbLogging.Text = "&Enable logging";
			this.cbLogging.UseVisualStyleBackColor = true;
			// 
			// errorProvider
			// 
			this.errorProvider.ContainerControl = this;
			// 
			// FormsWebDavConfig
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(421, 349);
			this.Controls.Add(this.tabControl);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.btnCancel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormsWebDavConfig";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Configure WebDAV";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormsWebDavConfigFormClosing);
			this.Load += new System.EventHandler(this.FormsWebDavConfigLoad);
			this.tabControl.ResumeLayout(false);
			this.tpCommon.ResumeLayout(false);
			this.tpCommon.PerformLayout();
			this.gbVirtDir.ResumeLayout(false);
			this.gbVirtDir.PerformLayout();
			this.tpLogging.ResumeLayout(false);
			this.tpLogging.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.RadioButton rbDisable;
		private System.Windows.Forms.RadioButton rbEnable;
		private System.Windows.Forms.TextBox tbVirtDir;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage tpCommon;
		private System.Windows.Forms.GroupBox gbVirtDir;
		private System.Windows.Forms.LinkLabel lilaLink;
		private System.Windows.Forms.ErrorProvider errorProvider;
		private System.Windows.Forms.TabPage tpLogging;
		private System.Windows.Forms.CheckBox cbLogging;
		private System.Windows.Forms.LinkLabel lilaLogFileLocation;
	}
}