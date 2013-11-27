namespace Plugin.Main
{
  partial class PluginHTTPInjectUC
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PluginHTTPInjectUC));
      this.DGV_Inject = new System.Windows.Forms.DataGridView();
      this.L_RequestedHost = new System.Windows.Forms.Label();
      this.TB_RequestedHost = new System.Windows.Forms.TextBox();
      this.L_Replacement_Host = new System.Windows.Forms.Label();
      this.RB_Inject = new System.Windows.Forms.RadioButton();
      this.RB_Redirect = new System.Windows.Forms.RadioButton();
      this.TB_ReplacementURL = new System.Windows.Forms.TextBox();
      this.BT_InjectFile = new System.Windows.Forms.Button();
      this.OFD_InjectedFile = new System.Windows.Forms.OpenFileDialog();
      this.BT_Add = new System.Windows.Forms.Button();
      this.TB_RequestedURL = new System.Windows.Forms.TextBox();
      this.L_RequestedURL = new System.Windows.Forms.Label();
      this.CMS_DataGrid_RightMouseButton = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.deleteEntryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.deleteAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.TB_ReplacementHost = new System.Windows.Forms.TextBox();
      this.L_Replacement_URL = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.DGV_Inject)).BeginInit();
      this.CMS_DataGrid_RightMouseButton.SuspendLayout();
      this.SuspendLayout();
      // 
      // DGV_Inject
      // 
      this.DGV_Inject.AllowUserToAddRows = false;
      this.DGV_Inject.AllowUserToDeleteRows = false;
      this.DGV_Inject.AllowUserToResizeColumns = false;
      this.DGV_Inject.AllowUserToResizeRows = false;
      dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.DGV_Inject.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
      this.DGV_Inject.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.DGV_Inject.Location = new System.Drawing.Point(17, 76);
      this.DGV_Inject.MultiSelect = false;
      this.DGV_Inject.Name = "DGV_Inject";
      this.DGV_Inject.RowHeadersVisible = false;
      this.DGV_Inject.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.DGV_Inject.RowTemplate.Height = 20;
      this.DGV_Inject.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.DGV_Inject.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.DGV_Inject.Size = new System.Drawing.Size(830, 277);
      this.DGV_Inject.TabIndex = 0;
      this.DGV_Inject.TabStop = false;
      this.DGV_Inject.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DGV_Inject_MouseUp);
      // 
      // L_RequestedHost
      // 
      this.L_RequestedHost.AutoSize = true;
      this.L_RequestedHost.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.L_RequestedHost.Location = new System.Drawing.Point(118, 15);
      this.L_RequestedHost.Name = "L_RequestedHost";
      this.L_RequestedHost.Size = new System.Drawing.Size(98, 13);
      this.L_RequestedHost.TabIndex = 0;
      this.L_RequestedHost.Text = "Requested Host";
      // 
      // TB_RequestedHost
      // 
      this.TB_RequestedHost.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.TB_RequestedHost.Location = new System.Drawing.Point(222, 11);
      this.TB_RequestedHost.Name = "TB_RequestedHost";
      this.TB_RequestedHost.Size = new System.Drawing.Size(142, 20);
      this.TB_RequestedHost.TabIndex = 3;
      // 
      // L_Replacement_Host
      // 
      this.L_Replacement_Host.AutoSize = true;
      this.L_Replacement_Host.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.L_Replacement_Host.Location = new System.Drawing.Point(118, 42);
      this.L_Replacement_Host.Name = "L_Replacement_Host";
      this.L_Replacement_Host.Size = new System.Drawing.Size(81, 13);
      this.L_Replacement_Host.TabIndex = 0;
      this.L_Replacement_Host.Text = "Injected host";
      // 
      // RB_Inject
      // 
      this.RB_Inject.AutoSize = true;
      this.RB_Inject.Location = new System.Drawing.Point(29, 38);
      this.RB_Inject.Name = "RB_Inject";
      this.RB_Inject.Size = new System.Drawing.Size(51, 17);
      this.RB_Inject.TabIndex = 2;
      this.RB_Inject.Text = "Inject";
      this.RB_Inject.UseVisualStyleBackColor = true;
      this.RB_Inject.CheckedChanged += new System.EventHandler(this.RB_Redirect_CheckedChanged);
      // 
      // RB_Redirect
      // 
      this.RB_Redirect.AutoSize = true;
      this.RB_Redirect.Checked = true;
      this.RB_Redirect.Location = new System.Drawing.Point(29, 12);
      this.RB_Redirect.Name = "RB_Redirect";
      this.RB_Redirect.Size = new System.Drawing.Size(65, 17);
      this.RB_Redirect.TabIndex = 1;
      this.RB_Redirect.TabStop = true;
      this.RB_Redirect.Text = "Redirect";
      this.RB_Redirect.UseVisualStyleBackColor = true;
      this.RB_Redirect.CheckedChanged += new System.EventHandler(this.RB_Redirect_CheckedChanged);
      // 
      // TB_ReplacementURL
      // 
      this.TB_ReplacementURL.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.TB_ReplacementURL.Location = new System.Drawing.Point(503, 39);
      this.TB_ReplacementURL.Name = "TB_ReplacementURL";
      this.TB_ReplacementURL.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
      this.TB_ReplacementURL.Size = new System.Drawing.Size(133, 20);
      this.TB_ReplacementURL.TabIndex = 6;
      // 
      // BT_InjectFile
      // 
      this.BT_InjectFile.Location = new System.Drawing.Point(645, 44);
      this.BT_InjectFile.Margin = new System.Windows.Forms.Padding(0);
      this.BT_InjectFile.Name = "BT_InjectFile";
      this.BT_InjectFile.Size = new System.Drawing.Size(15, 15);
      this.BT_InjectFile.TabIndex = 7;
      this.BT_InjectFile.Text = "+";
      this.BT_InjectFile.UseVisualStyleBackColor = true;
      this.BT_InjectFile.Click += new System.EventHandler(this.BT_InjectFile_Click);
      // 
      // OFD_InjectedFile
      // 
      this.OFD_InjectedFile.Title = "Injected file";
      this.OFD_InjectedFile.FileOk += new System.ComponentModel.CancelEventHandler(this.OFD_InjectedFile_FileOk);
      // 
      // BT_Add
      // 
      this.BT_Add.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BT_Add.BackgroundImage")));
      this.BT_Add.Location = new System.Drawing.Point(682, 5);
      this.BT_Add.Name = "BT_Add";
      this.BT_Add.Size = new System.Drawing.Size(80, 64);
      this.BT_Add.TabIndex = 8;
      this.BT_Add.UseVisualStyleBackColor = true;
      this.BT_Add.Click += new System.EventHandler(this.BT_Add_Click);
      // 
      // TB_RequestedURL
      // 
      this.TB_RequestedURL.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.TB_RequestedURL.Location = new System.Drawing.Point(503, 8);
      this.TB_RequestedURL.Name = "TB_RequestedURL";
      this.TB_RequestedURL.Size = new System.Drawing.Size(133, 20);
      this.TB_RequestedURL.TabIndex = 4;
      // 
      // L_RequestedURL
      // 
      this.L_RequestedURL.AutoSize = true;
      this.L_RequestedURL.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.L_RequestedURL.Location = new System.Drawing.Point(402, 12);
      this.L_RequestedURL.Name = "L_RequestedURL";
      this.L_RequestedURL.Size = new System.Drawing.Size(97, 13);
      this.L_RequestedURL.TabIndex = 0;
      this.L_RequestedURL.Text = "Requested URL";
      // 
      // CMS_DataGrid_RightMouseButton
      // 
      this.CMS_DataGrid_RightMouseButton.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteEntryToolStripMenuItem,
            this.deleteAllToolStripMenuItem});
      this.CMS_DataGrid_RightMouseButton.Name = "CMS_DataGrid_RightMouseButton";
      this.CMS_DataGrid_RightMouseButton.Size = new System.Drawing.Size(138, 48);
      // 
      // deleteEntryToolStripMenuItem
      // 
      this.deleteEntryToolStripMenuItem.Name = "deleteEntryToolStripMenuItem";
      this.deleteEntryToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
      this.deleteEntryToolStripMenuItem.Text = "Delete entry";
      this.deleteEntryToolStripMenuItem.Click += new System.EventHandler(this.deleteEntryToolStripMenuItem_Click);
      // 
      // deleteAllToolStripMenuItem
      // 
      this.deleteAllToolStripMenuItem.Name = "deleteAllToolStripMenuItem";
      this.deleteAllToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
      this.deleteAllToolStripMenuItem.Text = "Delete all";
      this.deleteAllToolStripMenuItem.Click += new System.EventHandler(this.deleteAllToolStripMenuItem_Click);
      // 
      // TB_ReplacementHost
      // 
      this.TB_ReplacementHost.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.TB_ReplacementHost.Location = new System.Drawing.Point(222, 39);
      this.TB_ReplacementHost.Name = "TB_ReplacementHost";
      this.TB_ReplacementHost.Size = new System.Drawing.Size(142, 20);
      this.TB_ReplacementHost.TabIndex = 5;
      // 
      // L_Replacement_URL
      // 
      this.L_Replacement_URL.AutoSize = true;
      this.L_Replacement_URL.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.L_Replacement_URL.Location = new System.Drawing.Point(402, 42);
      this.L_Replacement_URL.Name = "L_Replacement_URL";
      this.L_Replacement_URL.Size = new System.Drawing.Size(82, 13);
      this.L_Replacement_URL.TabIndex = 0;
      this.L_Replacement_URL.Text = "Injected URL";
      // 
      // PluginHTTPInjectUC
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.L_Replacement_URL);
      this.Controls.Add(this.TB_ReplacementHost);
      this.Controls.Add(this.TB_RequestedURL);
      this.Controls.Add(this.L_RequestedURL);
      this.Controls.Add(this.BT_Add);
      this.Controls.Add(this.BT_InjectFile);
      this.Controls.Add(this.TB_ReplacementURL);
      this.Controls.Add(this.RB_Redirect);
      this.Controls.Add(this.RB_Inject);
      this.Controls.Add(this.L_Replacement_Host);
      this.Controls.Add(this.TB_RequestedHost);
      this.Controls.Add(this.L_RequestedHost);
      this.Controls.Add(this.DGV_Inject);
      this.Name = "PluginHTTPInjectUC";
      this.Size = new System.Drawing.Size(996, 368);
      ((System.ComponentModel.ISupportInitialize)(this.DGV_Inject)).EndInit();
      this.CMS_DataGrid_RightMouseButton.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.DataGridView DGV_Inject;
    private System.Windows.Forms.Label L_RequestedHost;
    private System.Windows.Forms.TextBox TB_RequestedHost;
    private System.Windows.Forms.Label L_Replacement_Host;
    private System.Windows.Forms.RadioButton RB_Inject;
    private System.Windows.Forms.RadioButton RB_Redirect;
    private System.Windows.Forms.TextBox TB_ReplacementURL;
    private System.Windows.Forms.Button BT_InjectFile;
    private System.Windows.Forms.OpenFileDialog OFD_InjectedFile;
    private System.Windows.Forms.Button BT_Add;
    private System.Windows.Forms.TextBox TB_RequestedURL;
    private System.Windows.Forms.Label L_RequestedURL;
    private System.Windows.Forms.ContextMenuStrip CMS_DataGrid_RightMouseButton;
    private System.Windows.Forms.ToolStripMenuItem deleteEntryToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem deleteAllToolStripMenuItem;
    private System.Windows.Forms.TextBox TB_ReplacementHost;
    private System.Windows.Forms.Label L_Replacement_URL;
  }
}
