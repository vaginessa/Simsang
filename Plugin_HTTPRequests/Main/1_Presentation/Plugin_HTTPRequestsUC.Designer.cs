namespace Plugin.Main
{
    partial class PluginHTTPRequestsUC
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
          this.components = new System.ComponentModel.Container();
          System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
          this.DGV_HTTPRequests = new System.Windows.Forms.DataGridView();
          this.CMS_HTTPRequests = new System.Windows.Forms.ContextMenuStrip(this.components);
          this.TSMI_Clear = new System.Windows.Forms.ToolStripMenuItem();
          this.deleteEntryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
          this.requestDetailsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
          this.L_URLFilter = new System.Windows.Forms.Label();
          this.TB_Filter = new System.Windows.Forms.TextBox();
          this.BT_Set = new System.Windows.Forms.Button();
          this.T_GUIUpdate = new System.Windows.Forms.Timer(this.components);
          ((System.ComponentModel.ISupportInitialize)(this.DGV_HTTPRequests)).BeginInit();
          this.CMS_HTTPRequests.SuspendLayout();
          this.SuspendLayout();
          // 
          // DGV_HTTPRequests
          // 
          this.DGV_HTTPRequests.AllowUserToAddRows = false;
          this.DGV_HTTPRequests.AllowUserToDeleteRows = false;
          this.DGV_HTTPRequests.AllowUserToResizeColumns = false;
          this.DGV_HTTPRequests.AllowUserToResizeRows = false;
          dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
          dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
          dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
          dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
          dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
          dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
          this.DGV_HTTPRequests.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
          this.DGV_HTTPRequests.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
          this.DGV_HTTPRequests.Location = new System.Drawing.Point(17, 44);
          this.DGV_HTTPRequests.MultiSelect = false;
          this.DGV_HTTPRequests.Name = "DGV_HTTPRequests";
          this.DGV_HTTPRequests.ReadOnly = true;
          this.DGV_HTTPRequests.RowHeadersVisible = false;
          this.DGV_HTTPRequests.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          this.DGV_HTTPRequests.RowTemplate.Height = 20;
          this.DGV_HTTPRequests.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
          this.DGV_HTTPRequests.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
          this.DGV_HTTPRequests.Size = new System.Drawing.Size(830, 309);
          this.DGV_HTTPRequests.TabIndex = 3;
          this.DGV_HTTPRequests.DoubleClick += new System.EventHandler(this.DGV_HTTPRequests_DoubleClick);
          this.DGV_HTTPRequests.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DGV_HTTPRequests_MouseDown);
          this.DGV_HTTPRequests.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DGV_HTTPRequests_MouseUp);
          // 
          // CMS_HTTPRequests
          // 
          this.CMS_HTTPRequests.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_Clear,
            this.deleteEntryToolStripMenuItem,
            this.requestDetailsToolStripMenuItem});
          this.CMS_HTTPRequests.Name = "CMS_Downloads";
          this.CMS_HTTPRequests.Size = new System.Drawing.Size(174, 70);
          // 
          // TSMI_Clear
          // 
          this.TSMI_Clear.Name = "TSMI_Clear";
          this.TSMI_Clear.Size = new System.Drawing.Size(173, 22);
          this.TSMI_Clear.Text = "Clear";
          this.TSMI_Clear.Click += new System.EventHandler(this.TSMI_Clear_Click);
          // 
          // deleteEntryToolStripMenuItem
          // 
          this.deleteEntryToolStripMenuItem.Name = "deleteEntryToolStripMenuItem";
          this.deleteEntryToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
          this.deleteEntryToolStripMenuItem.Text = "Delete entry";
          this.deleteEntryToolStripMenuItem.Click += new System.EventHandler(this.deleteEntryToolStripMenuItem_Click);
          // 
          // requestDetailsToolStripMenuItem
          // 
          this.requestDetailsToolStripMenuItem.Name = "requestDetailsToolStripMenuItem";
          this.requestDetailsToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
          this.requestDetailsToolStripMenuItem.Text = "Open in browser ...";
          this.requestDetailsToolStripMenuItem.Click += new System.EventHandler(this.requestDetailsToolStripMenuItem_Click);
          // 
          // L_URLFilter
          // 
          this.L_URLFilter.AutoSize = true;
          this.L_URLFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          this.L_URLFilter.Location = new System.Drawing.Point(23, 19);
          this.L_URLFilter.Name = "L_URLFilter";
          this.L_URLFilter.Size = new System.Drawing.Size(61, 13);
          this.L_URLFilter.TabIndex = 0;
          this.L_URLFilter.Text = "URL filter";
          // 
          // TB_Filter
          // 
          this.TB_Filter.Location = new System.Drawing.Point(104, 16);
          this.TB_Filter.Name = "TB_Filter";
          this.TB_Filter.Size = new System.Drawing.Size(231, 20);
          this.TB_Filter.TabIndex = 1;
          this.TB_Filter.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TB_Filter_KeyUp);
          // 
          // BT_Set
          // 
          this.BT_Set.Location = new System.Drawing.Point(350, 16);
          this.BT_Set.Name = "BT_Set";
          this.BT_Set.Size = new System.Drawing.Size(33, 20);
          this.BT_Set.TabIndex = 2;
          this.BT_Set.Text = "Set";
          this.BT_Set.UseVisualStyleBackColor = true;
          this.BT_Set.Click += new System.EventHandler(this.BT_Set_Click);
          // 
          // T_GUIUpdate
          // 
          this.T_GUIUpdate.Interval = 500;
          this.T_GUIUpdate.Tick += new System.EventHandler(this.T_GUIUpdate_Tick);
          // 
          // PluginHTTPRequestsUC
          // 
          this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
          this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
          this.BackColor = System.Drawing.Color.Transparent;
          this.Controls.Add(this.BT_Set);
          this.Controls.Add(this.TB_Filter);
          this.Controls.Add(this.L_URLFilter);
          this.Controls.Add(this.DGV_HTTPRequests);
          this.DoubleBuffered = true;
          this.Name = "PluginHTTPRequestsUC";
          this.Size = new System.Drawing.Size(996, 368);
          ((System.ComponentModel.ISupportInitialize)(this.DGV_HTTPRequests)).EndInit();
          this.CMS_HTTPRequests.ResumeLayout(false);
          this.ResumeLayout(false);
          this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView DGV_HTTPRequests;
        private System.Windows.Forms.ContextMenuStrip CMS_HTTPRequests;
        private System.Windows.Forms.ToolStripMenuItem TSMI_Clear;
        private System.Windows.Forms.Label L_URLFilter;
        private System.Windows.Forms.TextBox TB_Filter;
        private System.Windows.Forms.Button BT_Set;
        private System.Windows.Forms.Timer T_GUIUpdate;
        private System.Windows.Forms.ToolStripMenuItem deleteEntryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem requestDetailsToolStripMenuItem;
    }
}
