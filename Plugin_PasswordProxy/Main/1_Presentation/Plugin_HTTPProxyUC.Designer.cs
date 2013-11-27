namespace Plugin.Main
{
  partial class PluginHTTPProxyUC
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
          System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
          this.DGV_Accounts = new System.Windows.Forms.DataGridView();
          this.CMS_PasswordProxy = new System.Windows.Forms.ContextMenuStrip(this.components);
          this.TSMI_Clear = new System.Windows.Forms.ToolStripMenuItem();
          this.deleteEntryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
          this.TB_RemoteHost = new System.Windows.Forms.TextBox();
          this.L_RemoteHost = new System.Windows.Forms.Label();
          this.CB_RedirectTo = new System.Windows.Forms.CheckBox();
          this.TB_RedirectURL = new System.Windows.Forms.TextBox();
          ((System.ComponentModel.ISupportInitialize)(this.DGV_Accounts)).BeginInit();
          this.CMS_PasswordProxy.SuspendLayout();
          this.SuspendLayout();
          // 
          // DGV_Accounts
          // 
          this.DGV_Accounts.AllowUserToAddRows = false;
          this.DGV_Accounts.AllowUserToDeleteRows = false;
          this.DGV_Accounts.AllowUserToResizeColumns = false;
          this.DGV_Accounts.AllowUserToResizeRows = false;
          dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
          dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
          dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
          dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
          dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
          dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
          this.DGV_Accounts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
          this.DGV_Accounts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
          this.DGV_Accounts.Location = new System.Drawing.Point(17, 44);
          this.DGV_Accounts.MultiSelect = false;
          this.DGV_Accounts.Name = "DGV_Accounts";
          this.DGV_Accounts.RowHeadersVisible = false;
          this.DGV_Accounts.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          this.DGV_Accounts.RowTemplate.Height = 20;
          this.DGV_Accounts.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
          this.DGV_Accounts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
          this.DGV_Accounts.Size = new System.Drawing.Size(830, 309);
          this.DGV_Accounts.TabIndex = 3;
          this.DGV_Accounts.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_Accounts_CellContentClick);
          this.DGV_Accounts.DoubleClick += new System.EventHandler(this.DGV_Accounts_DoubleClick);
          this.DGV_Accounts.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DGV_Accounts_MouseUp);
          // 
          // CMS_PasswordProxy
          // 
          this.CMS_PasswordProxy.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_Clear,
            this.deleteEntryToolStripMenuItem});
          this.CMS_PasswordProxy.Name = "CMS_PasswordProxy";
          this.CMS_PasswordProxy.Size = new System.Drawing.Size(138, 48);
          // 
          // TSMI_Clear
          // 
          this.TSMI_Clear.Name = "TSMI_Clear";
          this.TSMI_Clear.Size = new System.Drawing.Size(137, 22);
          this.TSMI_Clear.Text = "Clear";
          this.TSMI_Clear.Click += new System.EventHandler(this.TSMI_Clear_Click);
          // 
          // deleteEntryToolStripMenuItem
          // 
          this.deleteEntryToolStripMenuItem.Name = "deleteEntryToolStripMenuItem";
          this.deleteEntryToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
          this.deleteEntryToolStripMenuItem.Text = "Delete entry";
          this.deleteEntryToolStripMenuItem.Click += new System.EventHandler(this.deleteEntryToolStripMenuItem_Click);
          // 
          // TB_RemoteHost
          // 
          this.TB_RemoteHost.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          this.TB_RemoteHost.Location = new System.Drawing.Point(104, 16);
          this.TB_RemoteHost.Name = "TB_RemoteHost";
          this.TB_RemoteHost.Size = new System.Drawing.Size(200, 20);
          this.TB_RemoteHost.TabIndex = 1;
          // 
          // L_RemoteHost
          // 
          this.L_RemoteHost.AutoSize = true;
          this.L_RemoteHost.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          this.L_RemoteHost.Location = new System.Drawing.Point(23, 19);
          this.L_RemoteHost.Name = "L_RemoteHost";
          this.L_RemoteHost.Size = new System.Drawing.Size(67, 13);
          this.L_RemoteHost.TabIndex = 0;
          this.L_RemoteHost.Text = "Host name";
          // 
          // CB_RedirectTo
          // 
          this.CB_RedirectTo.AutoSize = true;
          this.CB_RedirectTo.Location = new System.Drawing.Point(414, 18);
          this.CB_RedirectTo.Name = "CB_RedirectTo";
          this.CB_RedirectTo.Size = new System.Drawing.Size(78, 17);
          this.CB_RedirectTo.TabIndex = 2;
          this.CB_RedirectTo.Text = "Redirect to";
          this.CB_RedirectTo.UseVisualStyleBackColor = true;
          this.CB_RedirectTo.CheckedChanged += new System.EventHandler(this.CB_RedirectTo_CheckedChanged);
          // 
          // TB_RedirectURL
          // 
          this.TB_RedirectURL.Enabled = false;
          this.TB_RedirectURL.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          this.TB_RedirectURL.Location = new System.Drawing.Point(496, 16);
          this.TB_RedirectURL.Name = "TB_RedirectURL";
          this.TB_RedirectURL.Size = new System.Drawing.Size(306, 20);
          this.TB_RedirectURL.TabIndex = 3;
          this.TB_RedirectURL.Text = "www.buglist.io/";
          // 
          // PluginHTTPProxyUC
          // 
          this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
          this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
          this.BackColor = System.Drawing.Color.Transparent;
          this.Controls.Add(this.TB_RedirectURL);
          this.Controls.Add(this.CB_RedirectTo);
          this.Controls.Add(this.L_RemoteHost);
          this.Controls.Add(this.TB_RemoteHost);
          this.Controls.Add(this.DGV_Accounts);
          this.Name = "PluginHTTPProxyUC";
          this.Size = new System.Drawing.Size(996, 368);
          ((System.ComponentModel.ISupportInitialize)(this.DGV_Accounts)).EndInit();
          this.CMS_PasswordProxy.ResumeLayout(false);
          this.ResumeLayout(false);
          this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView DGV_Accounts;
        private System.Windows.Forms.ContextMenuStrip CMS_PasswordProxy;
        private System.Windows.Forms.ToolStripMenuItem TSMI_Clear;
        private System.Windows.Forms.ToolStripMenuItem deleteEntryToolStripMenuItem;
        private System.Windows.Forms.TextBox TB_RemoteHost;
        private System.Windows.Forms.Label L_RemoteHost;
        private System.Windows.Forms.CheckBox CB_RedirectTo;
        private System.Windows.Forms.TextBox TB_RedirectURL;

    }
}
