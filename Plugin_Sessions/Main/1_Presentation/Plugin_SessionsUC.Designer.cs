namespace Plugin.Main
{
    partial class PluginSessionsUC
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
          System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Sessions");
          System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PluginSessionsUC));
          this.DGV_Sessions = new System.Windows.Forms.DataGridView();
          this.TV_Sessions = new System.Windows.Forms.TreeView();
          this.IL_Sessions = new System.Windows.Forms.ImageList(this.components);
          this.CMS_Sessions = new System.Windows.Forms.ContextMenuStrip(this.components);
          this.TSMI_Clear = new System.Windows.Forms.ToolStripMenuItem();
          this.TSMI_ShowData = new System.Windows.Forms.ToolStripMenuItem();
          this.deleteEntryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
          ((System.ComponentModel.ISupportInitialize)(this.DGV_Sessions)).BeginInit();
          this.CMS_Sessions.SuspendLayout();
          this.SuspendLayout();
          // 
          // DGV_Sessions
          // 
          this.DGV_Sessions.AllowUserToAddRows = false;
          this.DGV_Sessions.AllowUserToDeleteRows = false;
          this.DGV_Sessions.AllowUserToResizeColumns = false;
          this.DGV_Sessions.AllowUserToResizeRows = false;
          dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
          dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
          dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
          dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
          dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
          dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
          this.DGV_Sessions.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
          this.DGV_Sessions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
          this.DGV_Sessions.Location = new System.Drawing.Point(193, 15);
          this.DGV_Sessions.MultiSelect = false;
          this.DGV_Sessions.Name = "DGV_Sessions";
          this.DGV_Sessions.RowHeadersVisible = false;
          dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          this.DGV_Sessions.RowsDefaultCellStyle = dataGridViewCellStyle2;
          this.DGV_Sessions.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          this.DGV_Sessions.RowTemplate.Height = 20;
          this.DGV_Sessions.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
          this.DGV_Sessions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
          this.DGV_Sessions.Size = new System.Drawing.Size(654, 338);
          this.DGV_Sessions.TabIndex = 1;
          this.DGV_Sessions.DoubleClick += new System.EventHandler(this.DGV_Sessions_DoubleClick);
          this.DGV_Sessions.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DGV_Sessions_MouseDown);
          this.DGV_Sessions.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DGV_Sessions_MouseUp);
          // 
          // TV_Sessions
          // 
          this.TV_Sessions.ImageIndex = 0;
          this.TV_Sessions.ImageList = this.IL_Sessions;
          this.TV_Sessions.ItemHeight = 22;
          this.TV_Sessions.Location = new System.Drawing.Point(17, 15);
          this.TV_Sessions.Name = "TV_Sessions";
          treeNode1.Name = "SessionRoot";
          treeNode1.Text = "Sessions";
          this.TV_Sessions.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
          this.TV_Sessions.SelectedImageIndex = 0;
          this.TV_Sessions.Size = new System.Drawing.Size(161, 338);
          this.TV_Sessions.TabIndex = 1;
          this.TV_Sessions.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.TV_Sessions_AfterCollapse);
          this.TV_Sessions.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.TV_Sessions_NodeMouseClick);
          // 
          // IL_Sessions
          // 
          this.IL_Sessions.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("IL_Sessions.ImageStream")));
          this.IL_Sessions.TransparentColor = System.Drawing.Color.Transparent;
          this.IL_Sessions.Images.SetKeyName(0, "chart_organisation.png");
          this.IL_Sessions.Images.SetKeyName(1, "FaceBook.png");
          this.IL_Sessions.Images.SetKeyName(2, "Twitter.png");
          this.IL_Sessions.Images.SetKeyName(3, "yahoo.png");
          this.IL_Sessions.Images.SetKeyName(4, "google.png");
          this.IL_Sessions.Images.SetKeyName(5, "microsoft.png");
          this.IL_Sessions.Images.SetKeyName(6, "wordpress.png");
          this.IL_Sessions.Images.SetKeyName(7, "youtube.png");
          this.IL_Sessions.Images.SetKeyName(8, "hi5.png");
          this.IL_Sessions.Images.SetKeyName(9, "linkedin.png");
          // 
          // CMS_Sessions
          // 
          this.CMS_Sessions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_Clear,
            this.TSMI_ShowData,
            this.deleteEntryToolStripMenuItem});
          this.CMS_Sessions.Name = "CMS_Sessions";
          this.CMS_Sessions.Size = new System.Drawing.Size(138, 70);
          // 
          // TSMI_Clear
          // 
          this.TSMI_Clear.Name = "TSMI_Clear";
          this.TSMI_Clear.Size = new System.Drawing.Size(137, 22);
          this.TSMI_Clear.Text = "Clear";
          this.TSMI_Clear.Click += new System.EventHandler(this.TSMI_Clear_Click);
          // 
          // TSMI_ShowData
          // 
          this.TSMI_ShowData.Name = "TSMI_ShowData";
          this.TSMI_ShowData.Size = new System.Drawing.Size(137, 22);
          this.TSMI_ShowData.Text = "Show data";
          this.TSMI_ShowData.Click += new System.EventHandler(this.TSMI_ShowData_Click);
          // 
          // deleteEntryToolStripMenuItem
          // 
          this.deleteEntryToolStripMenuItem.Name = "deleteEntryToolStripMenuItem";
          this.deleteEntryToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
          this.deleteEntryToolStripMenuItem.Text = "Delete entry";
          this.deleteEntryToolStripMenuItem.Click += new System.EventHandler(this.deleteEntryToolStripMenuItem_Click);
          // 
          // PluginSessionsUC
          // 
          this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
          this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
          this.BackColor = System.Drawing.Color.Transparent;
          this.Controls.Add(this.TV_Sessions);
          this.Controls.Add(this.DGV_Sessions);
          this.Name = "PluginSessionsUC";
          this.Size = new System.Drawing.Size(996, 368);
          ((System.ComponentModel.ISupportInitialize)(this.DGV_Sessions)).EndInit();
          this.CMS_Sessions.ResumeLayout(false);
          this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView DGV_Sessions;
        private System.Windows.Forms.TreeView TV_Sessions;
        private System.Windows.Forms.ImageList IL_Sessions;
        private System.Windows.Forms.ContextMenuStrip CMS_Sessions;
        private System.Windows.Forms.ToolStripMenuItem TSMI_Clear;
        private System.Windows.Forms.ToolStripMenuItem TSMI_ShowData;
        private System.Windows.Forms.ToolStripMenuItem deleteEntryToolStripMenuItem;


    }
}
