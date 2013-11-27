using System;
using System.Windows.Forms;
namespace Plugin.Main
{
    public partial class PluginSystemsUC 
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
          this.CMS_Systems = new System.Windows.Forms.ContextMenuStrip(this.components);
          this.TSMI_Clear = new System.Windows.Forms.ToolStripMenuItem();
          this.deleteEntryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
          this.DGV_Systems = new System.Windows.Forms.DataGridView();
          this.CMS_Systems.SuspendLayout();
          ((System.ComponentModel.ISupportInitialize)(this.DGV_Systems)).BeginInit();
          this.SuspendLayout();
          // 
          // CMS_Systems
          // 
          this.CMS_Systems.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_Clear,
            this.deleteEntryToolStripMenuItem});
          this.CMS_Systems.Name = "CMS_Systems";
          this.CMS_Systems.Size = new System.Drawing.Size(138, 48);
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
          // DGV_Systems
          // 
          this.DGV_Systems.AllowUserToAddRows = false;
          this.DGV_Systems.AllowUserToDeleteRows = false;
          this.DGV_Systems.AllowUserToResizeColumns = false;
          this.DGV_Systems.AllowUserToResizeRows = false;
          dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
          dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
          dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
          dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
          dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
          dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
          this.DGV_Systems.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
          this.DGV_Systems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
          this.DGV_Systems.Location = new System.Drawing.Point(17, 15);
          this.DGV_Systems.MultiSelect = false;
          this.DGV_Systems.Name = "DGV_Systems";
          this.DGV_Systems.RowHeadersVisible = false;
          dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          this.DGV_Systems.RowsDefaultCellStyle = dataGridViewCellStyle2;
          this.DGV_Systems.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          this.DGV_Systems.RowTemplate.Height = 20;
          this.DGV_Systems.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
          this.DGV_Systems.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
          this.DGV_Systems.Size = new System.Drawing.Size(830, 338);
          this.DGV_Systems.TabIndex = 1;
          this.DGV_Systems.DoubleClick += new System.EventHandler(this.DGV_Systems_DoubleClick);
          this.DGV_Systems.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DGV_Systems_MouseDown);
          this.DGV_Systems.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DGV_Systems_MouseUp_1);
          // 
          // PluginSystemsUC
          // 
          this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
          this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
          this.BackColor = System.Drawing.Color.Transparent;
          this.Controls.Add(this.DGV_Systems);
          this.Name = "PluginSystemsUC";
          this.Size = new System.Drawing.Size(996, 368);
          this.CMS_Systems.ResumeLayout(false);
          ((System.ComponentModel.ISupportInitialize)(this.DGV_Systems)).EndInit();
          this.ResumeLayout(false);

        }

        #endregion

        private ContextMenuStrip CMS_Systems;
        private ToolStripMenuItem TSMI_Clear;
        private DataGridView DGV_Systems;
        private ToolStripMenuItem deleteEntryToolStripMenuItem;



    }
}
