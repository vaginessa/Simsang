namespace Plugin.Main
{
    partial class PluginDNSPoisonUC
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
          System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
          this.DGV_Spoofing = new System.Windows.Forms.DataGridView();
          this.BT_Add = new System.Windows.Forms.Button();
          this.TB_Host = new System.Windows.Forms.TextBox();
          this.TB_Address = new System.Windows.Forms.TextBox();
          this.L_Host = new System.Windows.Forms.Label();
          this.label1 = new System.Windows.Forms.Label();
          this.CMS_DNSPoison = new System.Windows.Forms.ContextMenuStrip(this.components);
          this.TSMI_Delete = new System.Windows.Forms.ToolStripMenuItem();
          ((System.ComponentModel.ISupportInitialize)(this.DGV_Spoofing)).BeginInit();
          this.CMS_DNSPoison.SuspendLayout();
          this.SuspendLayout();
          // 
          // DGV_Spoofing
          // 
          this.DGV_Spoofing.AllowUserToAddRows = false;
          this.DGV_Spoofing.AllowUserToDeleteRows = false;
          this.DGV_Spoofing.AllowUserToResizeColumns = false;
          this.DGV_Spoofing.AllowUserToResizeRows = false;
          this.DGV_Spoofing.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                      | System.Windows.Forms.AnchorStyles.Left)));
          dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
          dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
          dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
          dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
          dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
          dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
          this.DGV_Spoofing.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
          this.DGV_Spoofing.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
          this.DGV_Spoofing.Location = new System.Drawing.Point(17, 44);
          this.DGV_Spoofing.MultiSelect = false;
          this.DGV_Spoofing.Name = "DGV_Spoofing";
          this.DGV_Spoofing.RowHeadersVisible = false;
          dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          this.DGV_Spoofing.RowsDefaultCellStyle = dataGridViewCellStyle4;
          this.DGV_Spoofing.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          this.DGV_Spoofing.RowTemplate.Height = 20;
          this.DGV_Spoofing.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
          this.DGV_Spoofing.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
          this.DGV_Spoofing.Size = new System.Drawing.Size(830, 309);
          this.DGV_Spoofing.TabIndex = 5;
          this.DGV_Spoofing.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DGV_Spoofing_MouseDown);
          this.DGV_Spoofing.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DGV_Spoofing_MouseUp);
          // 
          // BT_Add
          // 
          this.BT_Add.Location = new System.Drawing.Point(452, 15);
          this.BT_Add.Margin = new System.Windows.Forms.Padding(0);
          this.BT_Add.Name = "BT_Add";
          this.BT_Add.Size = new System.Drawing.Size(20, 21);
          this.BT_Add.TabIndex = 3;
          this.BT_Add.Text = "+";
          this.BT_Add.UseVisualStyleBackColor = true;
          this.BT_Add.Click += new System.EventHandler(this.BT_Add_Click);
          // 
          // TB_Host
          // 
          this.TB_Host.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          this.TB_Host.Location = new System.Drawing.Point(96, 16);
          this.TB_Host.Name = "TB_Host";
          this.TB_Host.Size = new System.Drawing.Size(154, 20);
          this.TB_Host.TabIndex = 1;
          this.TB_Host.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TB_Host_KeyDown);
          // 
          // TB_Address
          // 
          this.TB_Address.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          this.TB_Address.Location = new System.Drawing.Point(341, 15);
          this.TB_Address.Name = "TB_Address";
          this.TB_Address.Size = new System.Drawing.Size(100, 20);
          this.TB_Address.TabIndex = 2;
          this.TB_Address.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TB_Address_KeyDown);
          // 
          // L_Host
          // 
          this.L_Host.AutoSize = true;
          this.L_Host.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          this.L_Host.Location = new System.Drawing.Point(23, 19);
          this.L_Host.Name = "L_Host";
          this.L_Host.Size = new System.Drawing.Size(67, 13);
          this.L_Host.TabIndex = 0;
          this.L_Host.Text = "Host name";
          // 
          // label1
          // 
          this.label1.AutoSize = true;
          this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          this.label1.Location = new System.Drawing.Point(266, 19);
          this.label1.Name = "label1";
          this.label1.Size = new System.Drawing.Size(67, 13);
          this.label1.TabIndex = 0;
          this.label1.Text = "IP address";
          // 
          // CMS_DNSPoison
          // 
          this.CMS_DNSPoison.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_Delete});
          this.CMS_DNSPoison.Name = "CMS_DNSPoison";
          this.CMS_DNSPoison.Size = new System.Drawing.Size(138, 26);
          // 
          // TSMI_Delete
          // 
          this.TSMI_Delete.Name = "TSMI_Delete";
          this.TSMI_Delete.Size = new System.Drawing.Size(137, 22);
          this.TSMI_Delete.Text = "Delete entry";
          this.TSMI_Delete.Click += new System.EventHandler(this.TSMI_Delete_Click);
          // 
          // PluginDNSPoisonUC
          // 
          this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
          this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
          this.BackColor = System.Drawing.Color.Transparent;
          this.Controls.Add(this.label1);
          this.Controls.Add(this.L_Host);
          this.Controls.Add(this.TB_Address);
          this.Controls.Add(this.TB_Host);
          this.Controls.Add(this.BT_Add);
          this.Controls.Add(this.DGV_Spoofing);
          this.Name = "PluginDNSPoisonUC";
          this.Size = new System.Drawing.Size(996, 368);
          this.Load += new System.EventHandler(this.PluginDNSPoisonUC_Load);
          ((System.ComponentModel.ISupportInitialize)(this.DGV_Spoofing)).EndInit();
          this.CMS_DNSPoison.ResumeLayout(false);
          this.ResumeLayout(false);
          this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView DGV_Spoofing;
        private System.Windows.Forms.Button BT_Add;
        private System.Windows.Forms.TextBox TB_Host;
        private System.Windows.Forms.TextBox TB_Address;
        private System.Windows.Forms.Label L_Host;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ContextMenuStrip CMS_DNSPoison;
        private System.Windows.Forms.ToolStripMenuItem TSMI_Delete;
    }
}
