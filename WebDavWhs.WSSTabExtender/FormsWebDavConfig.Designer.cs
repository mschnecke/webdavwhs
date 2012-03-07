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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormsWebDavConfig));
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOk = new System.Windows.Forms.Button();
			this.rbDisable = new System.Windows.Forms.RadioButton();
			this.rbEnable = new System.Windows.Forms.RadioButton();
			this.tbVirtDir = new System.Windows.Forms.TextBox();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tpCommon = new System.Windows.Forms.TabPage();
			this.cbSsl = new System.Windows.Forms.CheckBox();
			this.gbVirtDir = new System.Windows.Forms.GroupBox();
			this.tbUrl = new System.Windows.Forms.TextBox();
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
			resources.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnOk
			// 
			resources.ApplyResources(this.btnOk, "btnOk");
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Name = "btnOk";
			this.btnOk.UseVisualStyleBackColor = true;
			// 
			// rbDisable
			// 
			resources.ApplyResources(this.rbDisable, "rbDisable");
			this.rbDisable.Checked = true;
			this.rbDisable.Name = "rbDisable";
			this.rbDisable.TabStop = true;
			this.rbDisable.UseVisualStyleBackColor = true;
			this.rbDisable.CheckedChanged += new System.EventHandler(this.RbDisableCheckedChanged);
			// 
			// rbEnable
			// 
			resources.ApplyResources(this.rbEnable, "rbEnable");
			this.rbEnable.Name = "rbEnable";
			this.rbEnable.UseVisualStyleBackColor = true;
			this.rbEnable.CheckedChanged += new System.EventHandler(this.RbEnableCheckedChanged);
			// 
			// tbVirtDir
			// 
			resources.ApplyResources(this.tbVirtDir, "tbVirtDir");
			this.tbVirtDir.Name = "tbVirtDir";
			this.tbVirtDir.TextChanged += new System.EventHandler(this.TbVirtDirTextChanged);
			// 
			// tabControl
			// 
			resources.ApplyResources(this.tabControl, "tabControl");
			this.tabControl.Controls.Add(this.tpCommon);
			this.tabControl.Controls.Add(this.tpLogging);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			// 
			// tpCommon
			// 
			this.tpCommon.Controls.Add(this.cbSsl);
			this.tpCommon.Controls.Add(this.gbVirtDir);
			this.tpCommon.Controls.Add(this.rbDisable);
			this.tpCommon.Controls.Add(this.rbEnable);
			resources.ApplyResources(this.tpCommon, "tpCommon");
			this.tpCommon.Name = "tpCommon";
			this.tpCommon.UseVisualStyleBackColor = true;
			// 
			// cbSsl
			// 
			resources.ApplyResources(this.cbSsl, "cbSsl");
			this.cbSsl.Name = "cbSsl";
			this.cbSsl.UseVisualStyleBackColor = true;
			this.cbSsl.CheckedChanged += new System.EventHandler(this.CbSslCheckedChanged);
			// 
			// gbVirtDir
			// 
			resources.ApplyResources(this.gbVirtDir, "gbVirtDir");
			this.gbVirtDir.Controls.Add(this.tbUrl);
			this.gbVirtDir.Controls.Add(this.lilaLink);
			this.gbVirtDir.Controls.Add(this.tbVirtDir);
			this.gbVirtDir.Name = "gbVirtDir";
			this.gbVirtDir.TabStop = false;
			// 
			// tbUrl
			// 
			resources.ApplyResources(this.tbUrl, "tbUrl");
			this.tbUrl.Name = "tbUrl";
			this.tbUrl.ReadOnly = true;
			// 
			// lilaLink
			// 
			resources.ApplyResources(this.lilaLink, "lilaLink");
			this.lilaLink.Name = "lilaLink";
			this.lilaLink.TabStop = true;
			this.lilaLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LilaLinkLinkClicked);
			// 
			// tpLogging
			// 
			this.tpLogging.Controls.Add(this.lilaLogFileLocation);
			this.tpLogging.Controls.Add(this.cbLogging);
			resources.ApplyResources(this.tpLogging, "tpLogging");
			this.tpLogging.Name = "tpLogging";
			this.tpLogging.UseVisualStyleBackColor = true;
			// 
			// lilaLogFileLocation
			// 
			resources.ApplyResources(this.lilaLogFileLocation, "lilaLogFileLocation");
			this.lilaLogFileLocation.Name = "lilaLogFileLocation";
			this.lilaLogFileLocation.TabStop = true;
			this.lilaLogFileLocation.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LilaLogFileLocationLinkClicked);
			// 
			// cbLogging
			// 
			resources.ApplyResources(this.cbLogging, "cbLogging");
			this.cbLogging.Name = "cbLogging";
			this.cbLogging.UseVisualStyleBackColor = true;
			// 
			// errorProvider
			// 
			this.errorProvider.ContainerControl = this;
			// 
			// FormsWebDavConfig
			// 
			this.AcceptButton = this.btnOk;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.Controls.Add(this.tabControl);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.btnCancel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormsWebDavConfig";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
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
		private System.Windows.Forms.CheckBox cbSsl;
		private System.Windows.Forms.TextBox tbUrl;
	}
}