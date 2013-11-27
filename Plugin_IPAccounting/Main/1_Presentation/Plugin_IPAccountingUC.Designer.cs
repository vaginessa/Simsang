namespace Plugin.Main
{
    partial class PluginIPAccountingUC
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
          System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
          this.DGV_TrafficData = new System.Windows.Forms.DataGridView();
          this.CMS_DataGrid_RightMouseButton = new System.Windows.Forms.ContextMenuStrip(this.components);
          this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
          this.RB_Service = new System.Windows.Forms.RadioButton();
          this.RB_LocalIP = new System.Windows.Forms.RadioButton();
          this.RB_RemoteIP = new System.Windows.Forms.RadioButton();
          this.label1 = new System.Windows.Forms.Label();
          ((System.ComponentModel.ISupportInitialize)(this.DGV_TrafficData)).BeginInit();
          this.CMS_DataGrid_RightMouseButton.SuspendLayout();
          this.SuspendLayout();
          // 
          // DGV_TrafficData
          // 
          this.DGV_TrafficData.AllowUserToAddRows = false;
          this.DGV_TrafficData.AllowUserToDeleteRows = false;
          this.DGV_TrafficData.AllowUserToResizeColumns = false;
          this.DGV_TrafficData.AllowUserToResizeRows = false;
          dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
          dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
          dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
          dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
          dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
          dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
          this.DGV_TrafficData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
          this.DGV_TrafficData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
          this.DGV_TrafficData.Location = new System.Drawing.Point(17, 44);
          this.DGV_TrafficData.MultiSelect = false;
          this.DGV_TrafficData.Name = "DGV_TrafficData";
          this.DGV_TrafficData.ReadOnly = true;
          this.DGV_TrafficData.RowHeadersVisible = false;
          dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          this.DGV_TrafficData.RowsDefaultCellStyle = dataGridViewCellStyle2;
          this.DGV_TrafficData.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          this.DGV_TrafficData.RowTemplate.Height = 20;
          this.DGV_TrafficData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
          this.DGV_TrafficData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
          this.DGV_TrafficData.Size = new System.Drawing.Size(830, 309);
          this.DGV_TrafficData.TabIndex = 0;
          this.DGV_TrafficData.TabStop = false;
          this.DGV_TrafficData.DoubleClick += new System.EventHandler(this.DGV_TrafficData_DoubleClick);
          this.DGV_TrafficData.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DGV_TrafficData_MouseUp);
          // 
          // CMS_DataGrid_RightMouseButton
          // 
          this.CMS_DataGrid_RightMouseButton.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearToolStripMenuItem});
          this.CMS_DataGrid_RightMouseButton.Name = "CMS_DataGrid_RightMouseButton";
          this.CMS_DataGrid_RightMouseButton.Size = new System.Drawing.Size(123, 26);
          // 
          // clearToolStripMenuItem
          // 
          this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
          this.clearToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
          this.clearToolStripMenuItem.Text = "Delete all";
          this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
          // 
          // RB_Service
          // 
          this.RB_Service.AutoSize = true;
          this.RB_Service.Checked = true;
          this.RB_Service.Location = new System.Drawing.Point(178, 16);
          this.RB_Service.Name = "RB_Service";
          this.RB_Service.Size = new System.Drawing.Size(61, 17);
          this.RB_Service.TabIndex = 1;
          this.RB_Service.TabStop = true;
          this.RB_Service.Text = "Service";
          this.RB_Service.UseVisualStyleBackColor = true;
          this.RB_Service.Click += new System.EventHandler(this.RB_Service_Click);
          // 
          // RB_LocalIP
          // 
          this.RB_LocalIP.AutoSize = true;
          this.RB_LocalIP.Location = new System.Drawing.Point(264, 16);
          this.RB_LocalIP.Name = "RB_LocalIP";
          this.RB_LocalIP.Size = new System.Drawing.Size(64, 17);
          this.RB_LocalIP.TabIndex = 2;
          this.RB_LocalIP.Text = "Local IP";
          this.RB_LocalIP.UseVisualStyleBackColor = true;
          this.RB_LocalIP.Click += new System.EventHandler(this.RB_LocalIP_Click);
          // 
          // RB_RemoteIP
          // 
          this.RB_RemoteIP.AutoSize = true;
          this.RB_RemoteIP.Location = new System.Drawing.Point(349, 16);
          this.RB_RemoteIP.Name = "RB_RemoteIP";
          this.RB_RemoteIP.Size = new System.Drawing.Size(75, 17);
          this.RB_RemoteIP.TabIndex = 3;
          this.RB_RemoteIP.Text = "Remote IP";
          this.RB_RemoteIP.UseVisualStyleBackColor = true;
          this.RB_RemoteIP.Click += new System.EventHandler(this.RB_RemoteIP_Click);
          // 
          // label1
          // 
          this.label1.AutoSize = true;
          this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          this.label1.Location = new System.Drawing.Point(29, 19);
          this.label1.Name = "label1";
          this.label1.Size = new System.Drawing.Size(127, 13);
          this.label1.TabIndex = 4;
          this.label1.Text = "Accounting based on";
          // 
          // PluginIPAccountingUC
          // 
          this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
          this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
          this.BackColor = System.Drawing.Color.Transparent;
          this.Controls.Add(this.label1);
          this.Controls.Add(this.RB_RemoteIP);
          this.Controls.Add(this.RB_LocalIP);
          this.Controls.Add(this.RB_Service);
          this.Controls.Add(this.DGV_TrafficData);
          this.Name = "PluginIPAccountingUC";
          this.Size = new System.Drawing.Size(996, 368);
          this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PluginIPAccountingUC_MouseDown);
          ((System.ComponentModel.ISupportInitialize)(this.DGV_TrafficData)).EndInit();
          this.CMS_DataGrid_RightMouseButton.ResumeLayout(false);
          this.ResumeLayout(false);
          this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView DGV_TrafficData;
        private System.Windows.Forms.ContextMenuStrip CMS_DataGrid_RightMouseButton;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.RadioButton RB_Service;
        private System.Windows.Forms.RadioButton RB_LocalIP;
        private System.Windows.Forms.RadioButton RB_RemoteIP;
        private System.Windows.Forms.Label label1;
    }
}
