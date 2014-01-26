namespace Plugin.Main
{
    partial class PluginDNSRequestsUC
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
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
      this.DGV_DNSRequests = new System.Windows.Forms.DataGridView();
      this.BT_Set = new System.Windows.Forms.Button();
      this.TB_Filter = new System.Windows.Forms.TextBox();
      this.L_RequestFilter = new System.Windows.Forms.Label();
      this.CMS_DNSRequests = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.TSMI_Clear = new System.Windows.Forms.ToolStripMenuItem();
      this.deleteEntryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.copyHostNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.T_GUIUpdate = new System.Windows.Forms.Timer(this.components);
      ((System.ComponentModel.ISupportInitialize)(this.DGV_DNSRequests)).BeginInit();
      this.CMS_DNSRequests.SuspendLayout();
      this.SuspendLayout();
      // 
      // DGV_DNSRequests
      // 
      this.DGV_DNSRequests.AllowUserToAddRows = false;
      this.DGV_DNSRequests.AllowUserToDeleteRows = false;
      this.DGV_DNSRequests.AllowUserToResizeColumns = false;
      this.DGV_DNSRequests.AllowUserToResizeRows = false;
      dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.DGV_DNSRequests.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
      this.DGV_DNSRequests.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.DGV_DNSRequests.Location = new System.Drawing.Point(17, 44);
      this.DGV_DNSRequests.MultiSelect = false;
      this.DGV_DNSRequests.Name = "DGV_DNSRequests";
      this.DGV_DNSRequests.ReadOnly = true;
      this.DGV_DNSRequests.RowHeadersVisible = false;
      this.DGV_DNSRequests.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.DGV_DNSRequests.RowTemplate.Height = 20;
      this.DGV_DNSRequests.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.DGV_DNSRequests.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.DGV_DNSRequests.Size = new System.Drawing.Size(830, 309);
      this.DGV_DNSRequests.TabIndex = 0;
      this.DGV_DNSRequests.TabStop = false;
      this.DGV_DNSRequests.VirtualMode = true;
      this.DGV_DNSRequests.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_DNSRequests_CellContentClick);
      this.DGV_DNSRequests.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DGV_DNSRequests_MouseDown);
      this.DGV_DNSRequests.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DGV_DNSRequests_MouseUp);
      // 
      // BT_Set
      // 
      this.BT_Set.Location = new System.Drawing.Point(359, 16);
      this.BT_Set.Name = "BT_Set";
      this.BT_Set.Size = new System.Drawing.Size(33, 20);
      this.BT_Set.TabIndex = 2;
      this.BT_Set.Text = "Set";
      this.BT_Set.UseVisualStyleBackColor = true;
      this.BT_Set.Click += new System.EventHandler(this.BT_Set_Click);
      // 
      // TB_Filter
      // 
      this.TB_Filter.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.TB_Filter.Location = new System.Drawing.Point(113, 16);
      this.TB_Filter.Name = "TB_Filter";
      this.TB_Filter.Size = new System.Drawing.Size(231, 20);
      this.TB_Filter.TabIndex = 1;
      this.TB_Filter.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TB_Filter_KeyUp);
      // 
      // L_RequestFilter
      // 
      this.L_RequestFilter.AutoSize = true;
      this.L_RequestFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.L_RequestFilter.Location = new System.Drawing.Point(23, 19);
      this.L_RequestFilter.Name = "L_RequestFilter";
      this.L_RequestFilter.Size = new System.Drawing.Size(83, 13);
      this.L_RequestFilter.TabIndex = 0;
      this.L_RequestFilter.Text = "Request filter";
      // 
      // CMS_DNSRequests
      // 
      this.CMS_DNSRequests.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_Clear,
            this.deleteEntryToolStripMenuItem,
            this.copyHostNameToolStripMenuItem});
      this.CMS_DNSRequests.Name = "CMS_DNSRequests";
      this.CMS_DNSRequests.Size = new System.Drawing.Size(162, 70);
      // 
      // TSMI_Clear
      // 
      this.TSMI_Clear.Name = "TSMI_Clear";
      this.TSMI_Clear.Size = new System.Drawing.Size(161, 22);
      this.TSMI_Clear.Text = "Clear";
      this.TSMI_Clear.Click += new System.EventHandler(this.TSMI_Clear_Click);
      // 
      // deleteEntryToolStripMenuItem
      // 
      this.deleteEntryToolStripMenuItem.Name = "deleteEntryToolStripMenuItem";
      this.deleteEntryToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
      this.deleteEntryToolStripMenuItem.Text = "Delete entry";
      this.deleteEntryToolStripMenuItem.Click += new System.EventHandler(this.deleteEntryToolStripMenuItem_Click);
      // 
      // copyHostNameToolStripMenuItem
      // 
      this.copyHostNameToolStripMenuItem.Name = "copyHostNameToolStripMenuItem";
      this.copyHostNameToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
      this.copyHostNameToolStripMenuItem.Text = "Copy host name";
      this.copyHostNameToolStripMenuItem.Click += new System.EventHandler(this.copyHostNameToolStripMenuItem_Click);
      // 
      // T_GUIUpdate
      // 
      this.T_GUIUpdate.Interval = 500;
      this.T_GUIUpdate.Tick += new System.EventHandler(this.T_GUIUpdate_Tick);
      // 
      // PluginDNSRequestsUC
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.Transparent;
      this.Controls.Add(this.BT_Set);
      this.Controls.Add(this.TB_Filter);
      this.Controls.Add(this.L_RequestFilter);
      this.Controls.Add(this.DGV_DNSRequests);
      this.Name = "PluginDNSRequestsUC";
      this.Size = new System.Drawing.Size(996, 368);
      ((System.ComponentModel.ISupportInitialize)(this.DGV_DNSRequests)).EndInit();
      this.CMS_DNSRequests.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView DGV_DNSRequests;
        private System.Windows.Forms.Button BT_Set;
        private System.Windows.Forms.TextBox TB_Filter;
        private System.Windows.Forms.Label L_RequestFilter;
        private System.Windows.Forms.ContextMenuStrip CMS_DNSRequests;
        private System.Windows.Forms.ToolStripMenuItem TSMI_Clear;
        private System.Windows.Forms.ToolStripMenuItem deleteEntryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyHostNameToolStripMenuItem;
        private System.Windows.Forms.Timer T_GUIUpdate;
    }
}
