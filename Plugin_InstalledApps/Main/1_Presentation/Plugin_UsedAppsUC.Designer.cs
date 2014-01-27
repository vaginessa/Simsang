namespace Plugin.Main
{
    partial class PluginUsedAppsUC
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
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
      this.DGV_Applications = new System.Windows.Forms.DataGridView();
      this.CMS_Applications = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.TSMI_Clear = new System.Windows.Forms.ToolStripMenuItem();
      this.deleteEntryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.T_GUIUpdate = new System.Windows.Forms.Timer(this.components);
      ((System.ComponentModel.ISupportInitialize)(this.DGV_Applications)).BeginInit();
      this.CMS_Applications.SuspendLayout();
      this.SuspendLayout();
      // 
      // DGV_Applications
      // 
      this.DGV_Applications.AllowUserToAddRows = false;
      this.DGV_Applications.AllowUserToDeleteRows = false;
      this.DGV_Applications.AllowUserToResizeColumns = false;
      this.DGV_Applications.AllowUserToResizeRows = false;
      dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.DGV_Applications.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
      this.DGV_Applications.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.DGV_Applications.Location = new System.Drawing.Point(17, 15);
      this.DGV_Applications.MultiSelect = false;
      this.DGV_Applications.Name = "DGV_Applications";
      this.DGV_Applications.RowHeadersVisible = false;
      dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.DGV_Applications.RowsDefaultCellStyle = dataGridViewCellStyle2;
      this.DGV_Applications.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.DGV_Applications.RowTemplate.Height = 20;
      this.DGV_Applications.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.DGV_Applications.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.DGV_Applications.Size = new System.Drawing.Size(830, 338);
      this.DGV_Applications.TabIndex = 1;
      this.DGV_Applications.DoubleClick += new System.EventHandler(this.DGV_Applications_DoubleClick);
      this.DGV_Applications.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DGV_Applications_MouseDown);
      this.DGV_Applications.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DGV_Applications_MouseUp);
      // 
      // CMS_Applications
      // 
      this.CMS_Applications.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_Clear,
            this.deleteEntryToolStripMenuItem});
      this.CMS_Applications.Name = "CMS_UsedApps";
      this.CMS_Applications.Size = new System.Drawing.Size(138, 48);
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
      // T_GUIUpdate
      // 
      this.T_GUIUpdate.Interval = 500;
      this.T_GUIUpdate.Tick += new System.EventHandler(this.T_GUIUpdate_Tick);
      // 
      // PluginUsedAppsUC
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.Transparent;
      this.Controls.Add(this.DGV_Applications);
      this.Name = "PluginUsedAppsUC";
      this.Size = new System.Drawing.Size(996, 368);
      ((System.ComponentModel.ISupportInitialize)(this.DGV_Applications)).EndInit();
      this.CMS_Applications.ResumeLayout(false);
      this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView DGV_Applications;
        private System.Windows.Forms.ContextMenuStrip CMS_Applications;
        private System.Windows.Forms.ToolStripMenuItem TSMI_Clear;
        private System.Windows.Forms.ToolStripMenuItem deleteEntryToolStripMenuItem;
        private System.Windows.Forms.Timer T_GUIUpdate;
    }
}
