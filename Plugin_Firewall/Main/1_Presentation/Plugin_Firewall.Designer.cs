namespace Plugin.Main
{
  partial class PluginFirewallUC
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
      this.DGV_FWRules = new System.Windows.Forms.DataGridView();
      this.L_SrcIP = new System.Windows.Forms.Label();
      this.BT_Add = new System.Windows.Forms.Button();
      this.L_DstIP = new System.Windows.Forms.Label();
      this.TB_SrcPortLower = new System.Windows.Forms.TextBox();
      this.L_SrcPort = new System.Windows.Forms.Label();
      this.L_Dash = new System.Windows.Forms.Label();
      this.TB_SrcPortUpper = new System.Windows.Forms.TextBox();
      this.TB_DstPortUpper = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.L_DstPort = new System.Windows.Forms.Label();
      this.TB_DstPortLower = new System.Windows.Forms.TextBox();
      this.CB_Protocol = new System.Windows.Forms.ComboBox();
      this.CMS_DataGrid_RightMouseButton = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.deleteRuleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.deleteAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.CB_SrcIP = new System.Windows.Forms.ComboBox();
      this.CB_DstIP = new System.Windows.Forms.ComboBox();
      ((System.ComponentModel.ISupportInitialize)(this.DGV_FWRules)).BeginInit();
      this.CMS_DataGrid_RightMouseButton.SuspendLayout();
      this.SuspendLayout();
      // 
      // DGV_FWRules
      // 
      this.DGV_FWRules.AllowUserToAddRows = false;
      this.DGV_FWRules.AllowUserToDeleteRows = false;
      this.DGV_FWRules.AllowUserToResizeColumns = false;
      this.DGV_FWRules.AllowUserToResizeRows = false;
      dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.DGV_FWRules.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
      this.DGV_FWRules.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.DGV_FWRules.Location = new System.Drawing.Point(17, 44);
      this.DGV_FWRules.MultiSelect = false;
      this.DGV_FWRules.Name = "DGV_FWRules";
      this.DGV_FWRules.ReadOnly = true;
      this.DGV_FWRules.RowHeadersVisible = false;
      this.DGV_FWRules.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.DGV_FWRules.RowTemplate.Height = 20;
      this.DGV_FWRules.RowTemplate.ReadOnly = true;
      this.DGV_FWRules.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.DGV_FWRules.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.DGV_FWRules.Size = new System.Drawing.Size(830, 309);
      this.DGV_FWRules.TabIndex = 9;
      this.DGV_FWRules.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DGV_FWRules_MouseUp);
      // 
      // L_SrcIP
      // 
      this.L_SrcIP.AutoSize = true;
      this.L_SrcIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.L_SrcIP.Location = new System.Drawing.Point(97, 18);
      this.L_SrcIP.Name = "L_SrcIP";
      this.L_SrcIP.Size = new System.Drawing.Size(42, 13);
      this.L_SrcIP.TabIndex = 0;
      this.L_SrcIP.Text = "Src IP";
      // 
      // BT_Add
      // 
      this.BT_Add.Location = new System.Drawing.Point(634, 15);
      this.BT_Add.Margin = new System.Windows.Forms.Padding(0);
      this.BT_Add.Name = "BT_Add";
      this.BT_Add.Size = new System.Drawing.Size(20, 21);
      this.BT_Add.TabIndex = 8;
      this.BT_Add.Text = "+";
      this.BT_Add.UseVisualStyleBackColor = true;
      this.BT_Add.Click += new System.EventHandler(this.BT_Add_Click);
      // 
      // L_DstIP
      // 
      this.L_DstIP.AutoSize = true;
      this.L_DstIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.L_DstIP.Location = new System.Drawing.Point(378, 18);
      this.L_DstIP.Name = "L_DstIP";
      this.L_DstIP.Size = new System.Drawing.Size(42, 13);
      this.L_DstIP.TabIndex = 0;
      this.L_DstIP.Text = "Dst IP";
      // 
      // TB_SrcPortLower
      // 
      this.TB_SrcPortLower.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.TB_SrcPortLower.Location = new System.Drawing.Point(257, 14);
      this.TB_SrcPortLower.Name = "TB_SrcPortLower";
      this.TB_SrcPortLower.Size = new System.Drawing.Size(38, 20);
      this.TB_SrcPortLower.TabIndex = 3;
      // 
      // L_SrcPort
      // 
      this.L_SrcPort.AutoSize = true;
      this.L_SrcPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.L_SrcPort.Location = new System.Drawing.Point(245, 17);
      this.L_SrcPort.Name = "L_SrcPort";
      this.L_SrcPort.Size = new System.Drawing.Size(11, 13);
      this.L_SrcPort.TabIndex = 0;
      this.L_SrcPort.Text = ":";
      // 
      // L_Dash
      // 
      this.L_Dash.AutoSize = true;
      this.L_Dash.Location = new System.Drawing.Point(296, 17);
      this.L_Dash.Name = "L_Dash";
      this.L_Dash.Size = new System.Drawing.Size(10, 13);
      this.L_Dash.TabIndex = 0;
      this.L_Dash.Text = "-";
      // 
      // TB_SrcPortUpper
      // 
      this.TB_SrcPortUpper.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.TB_SrcPortUpper.Location = new System.Drawing.Point(306, 14);
      this.TB_SrcPortUpper.Name = "TB_SrcPortUpper";
      this.TB_SrcPortUpper.Size = new System.Drawing.Size(38, 20);
      this.TB_SrcPortUpper.TabIndex = 4;
      // 
      // TB_DstPortUpper
      // 
      this.TB_DstPortUpper.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.TB_DstPortUpper.Location = new System.Drawing.Point(590, 15);
      this.TB_DstPortUpper.Name = "TB_DstPortUpper";
      this.TB_DstPortUpper.Size = new System.Drawing.Size(38, 20);
      this.TB_DstPortUpper.TabIndex = 7;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(579, 18);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(10, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "-";
      // 
      // L_DstPort
      // 
      this.L_DstPort.AutoSize = true;
      this.L_DstPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.L_DstPort.Location = new System.Drawing.Point(528, 18);
      this.L_DstPort.Name = "L_DstPort";
      this.L_DstPort.Size = new System.Drawing.Size(11, 13);
      this.L_DstPort.TabIndex = 0;
      this.L_DstPort.Text = ":";
      // 
      // TB_DstPortLower
      // 
      this.TB_DstPortLower.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.TB_DstPortLower.Location = new System.Drawing.Point(540, 15);
      this.TB_DstPortLower.Name = "TB_DstPortLower";
      this.TB_DstPortLower.Size = new System.Drawing.Size(38, 20);
      this.TB_DstPortLower.TabIndex = 6;
      // 
      // CB_Protocol
      // 
      this.CB_Protocol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.CB_Protocol.FormattingEnabled = true;
      this.CB_Protocol.Location = new System.Drawing.Point(17, 13);
      this.CB_Protocol.Name = "CB_Protocol";
      this.CB_Protocol.Size = new System.Drawing.Size(58, 21);
      this.CB_Protocol.TabIndex = 1;
      // 
      // CMS_DataGrid_RightMouseButton
      // 
      this.CMS_DataGrid_RightMouseButton.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteRuleToolStripMenuItem,
            this.deleteAllToolStripMenuItem});
      this.CMS_DataGrid_RightMouseButton.Name = "CMS_DataGrid_RightMouseButton";
      this.CMS_DataGrid_RightMouseButton.Size = new System.Drawing.Size(131, 48);
      // 
      // deleteRuleToolStripMenuItem
      // 
      this.deleteRuleToolStripMenuItem.Name = "deleteRuleToolStripMenuItem";
      this.deleteRuleToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
      this.deleteRuleToolStripMenuItem.Text = "Delete rule";
      this.deleteRuleToolStripMenuItem.Click += new System.EventHandler(this.deleteRuleToolStripMenuItem_Click);
      // 
      // deleteAllToolStripMenuItem
      // 
      this.deleteAllToolStripMenuItem.Name = "deleteAllToolStripMenuItem";
      this.deleteAllToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
      this.deleteAllToolStripMenuItem.Text = "Delete all";
      this.deleteAllToolStripMenuItem.Click += new System.EventHandler(this.deleteAllToolStripMenuItem_Click);
      // 
      // CB_SrcIP
      // 
      this.CB_SrcIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.CB_SrcIP.FormattingEnabled = true;
      this.CB_SrcIP.Location = new System.Drawing.Point(141, 14);
      this.CB_SrcIP.Name = "CB_SrcIP";
      this.CB_SrcIP.Size = new System.Drawing.Size(103, 21);
      this.CB_SrcIP.TabIndex = 2;
      // 
      // CB_DstIP
      // 
      this.CB_DstIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.CB_DstIP.FormattingEnabled = true;
      this.CB_DstIP.Location = new System.Drawing.Point(424, 14);
      this.CB_DstIP.Name = "CB_DstIP";
      this.CB_DstIP.Size = new System.Drawing.Size(103, 21);
      this.CB_DstIP.TabIndex = 5;
      // 
      // PluginFirewallUC
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.Transparent;
      this.Controls.Add(this.CB_DstIP);
      this.Controls.Add(this.CB_SrcIP);
      this.Controls.Add(this.CB_Protocol);
      this.Controls.Add(this.TB_DstPortUpper);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.L_DstPort);
      this.Controls.Add(this.TB_DstPortLower);
      this.Controls.Add(this.TB_SrcPortUpper);
      this.Controls.Add(this.L_Dash);
      this.Controls.Add(this.L_SrcPort);
      this.Controls.Add(this.TB_SrcPortLower);
      this.Controls.Add(this.L_DstIP);
      this.Controls.Add(this.BT_Add);
      this.Controls.Add(this.L_SrcIP);
      this.Controls.Add(this.DGV_FWRules);
      this.Name = "PluginFirewallUC";
      this.Size = new System.Drawing.Size(996, 368);
      ((System.ComponentModel.ISupportInitialize)(this.DGV_FWRules)).EndInit();
      this.CMS_DataGrid_RightMouseButton.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.DataGridView DGV_FWRules;
    private System.Windows.Forms.Label L_SrcIP;
    private System.Windows.Forms.Button BT_Add;
    private System.Windows.Forms.Label L_DstIP;
    private System.Windows.Forms.TextBox TB_SrcPortLower;
    private System.Windows.Forms.Label L_SrcPort;
    private System.Windows.Forms.Label L_Dash;
    private System.Windows.Forms.TextBox TB_SrcPortUpper;
    private System.Windows.Forms.TextBox TB_DstPortUpper;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label L_DstPort;
    private System.Windows.Forms.TextBox TB_DstPortLower;
    private System.Windows.Forms.ComboBox CB_Protocol;
    private System.Windows.Forms.ContextMenuStrip CMS_DataGrid_RightMouseButton;
    private System.Windows.Forms.ToolStripMenuItem deleteRuleToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem deleteAllToolStripMenuItem;
    private System.Windows.Forms.ComboBox CB_SrcIP;
    private System.Windows.Forms.ComboBox CB_DstIP;
  }
}
