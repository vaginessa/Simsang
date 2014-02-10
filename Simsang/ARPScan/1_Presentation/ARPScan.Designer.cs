namespace Simsang.ARPScan.Main
{
  partial class ARPScan
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

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ARPScan));
      this.BT_Close = new System.Windows.Forms.Button();
      this.DGV_Targets = new System.Windows.Forms.DataGridView();
      this.BT_Scan = new System.Windows.Forms.Button();
      this.GB_Range = new System.Windows.Forms.GroupBox();
      this.RB_Netrange = new System.Windows.Forms.RadioButton();
      this.RB_Subnet = new System.Windows.Forms.RadioButton();
      this.TB_Netrange2 = new System.Windows.Forms.TextBox();
      this.TB_Netrange1 = new System.Windows.Forms.TextBox();
      this.TB_Subnet2 = new System.Windows.Forms.TextBox();
      this.TB_Subnet1 = new System.Windows.Forms.TextBox();
      this.CMS_ManageTargets = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.unselectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      ((System.ComponentModel.ISupportInitialize)(this.DGV_Targets)).BeginInit();
      this.GB_Range.SuspendLayout();
      this.CMS_ManageTargets.SuspendLayout();
      this.SuspendLayout();
      // 
      // BT_Close
      // 
      this.BT_Close.Location = new System.Drawing.Point(472, 301);
      this.BT_Close.Name = "BT_Close";
      this.BT_Close.Size = new System.Drawing.Size(75, 23);
      this.BT_Close.TabIndex = 7;
      this.BT_Close.Text = "Close";
      this.BT_Close.UseVisualStyleBackColor = true;
      this.BT_Close.Click += new System.EventHandler(this.BT_Close_Click);
      // 
      // DGV_Targets
      // 
      this.DGV_Targets.AllowUserToAddRows = false;
      this.DGV_Targets.AllowUserToDeleteRows = false;
      this.DGV_Targets.AllowUserToResizeColumns = false;
      this.DGV_Targets.AllowUserToResizeRows = false;
      dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.DGV_Targets.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
      this.DGV_Targets.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.DGV_Targets.Location = new System.Drawing.Point(7, 92);
      this.DGV_Targets.MultiSelect = false;
      this.DGV_Targets.Name = "DGV_Targets";
      this.DGV_Targets.ReadOnly = true;
      this.DGV_Targets.RowHeadersVisible = false;
      this.DGV_Targets.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
      this.DGV_Targets.RowTemplate.Height = 24;
      this.DGV_Targets.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.DGV_Targets.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.DGV_Targets.Size = new System.Drawing.Size(610, 201);
      this.DGV_Targets.TabIndex = 5;
      this.DGV_Targets.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DGV_Targets_MouseUp);
      // 
      // BT_Scan
      // 
      this.BT_Scan.Location = new System.Drawing.Point(380, 301);
      this.BT_Scan.Name = "BT_Scan";
      this.BT_Scan.Size = new System.Drawing.Size(75, 23);
      this.BT_Scan.TabIndex = 6;
      this.BT_Scan.Text = "Scan";
      this.BT_Scan.UseVisualStyleBackColor = true;
      this.BT_Scan.Click += new System.EventHandler(this.BT_Scan_Click);
      // 
      // GB_Range
      // 
      this.GB_Range.Controls.Add(this.RB_Netrange);
      this.GB_Range.Controls.Add(this.RB_Subnet);
      this.GB_Range.Controls.Add(this.TB_Netrange2);
      this.GB_Range.Controls.Add(this.TB_Netrange1);
      this.GB_Range.Controls.Add(this.TB_Subnet2);
      this.GB_Range.Controls.Add(this.TB_Subnet1);
      this.GB_Range.Location = new System.Drawing.Point(7, 8);
      this.GB_Range.Name = "GB_Range";
      this.GB_Range.Size = new System.Drawing.Size(610, 78);
      this.GB_Range.TabIndex = 0;
      this.GB_Range.TabStop = false;
      this.GB_Range.Text = "Target range";
      // 
      // RB_Netrange
      // 
      this.RB_Netrange.AutoSize = true;
      this.RB_Netrange.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.RB_Netrange.Location = new System.Drawing.Point(14, 48);
      this.RB_Netrange.Name = "RB_Netrange";
      this.RB_Netrange.Size = new System.Drawing.Size(81, 17);
      this.RB_Netrange.TabIndex = 2;
      this.RB_Netrange.TabStop = true;
      this.RB_Netrange.Text = "Net range";
      this.RB_Netrange.UseVisualStyleBackColor = true;
      this.RB_Netrange.CheckedChanged += new System.EventHandler(this.RB_Netrange_CheckedChanged);
      // 
      // RB_Subnet
      // 
      this.RB_Subnet.AutoSize = true;
      this.RB_Subnet.Checked = true;
      this.RB_Subnet.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.RB_Subnet.Location = new System.Drawing.Point(14, 23);
      this.RB_Subnet.Name = "RB_Subnet";
      this.RB_Subnet.Size = new System.Drawing.Size(65, 17);
      this.RB_Subnet.TabIndex = 1;
      this.RB_Subnet.TabStop = true;
      this.RB_Subnet.Text = "Subnet";
      this.RB_Subnet.UseVisualStyleBackColor = true;
      this.RB_Subnet.CheckedChanged += new System.EventHandler(this.RB_Subnet_CheckedChanged);
      // 
      // TB_Netrange2
      // 
      this.TB_Netrange2.Location = new System.Drawing.Point(247, 48);
      this.TB_Netrange2.Name = "TB_Netrange2";
      this.TB_Netrange2.Size = new System.Drawing.Size(100, 20);
      this.TB_Netrange2.TabIndex = 0;
      this.TB_Netrange2.TabStop = false;
      this.TB_Netrange2.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TB_Netrange2_KeyUp);
      // 
      // TB_Netrange1
      // 
      this.TB_Netrange1.Location = new System.Drawing.Point(125, 48);
      this.TB_Netrange1.Name = "TB_Netrange1";
      this.TB_Netrange1.Size = new System.Drawing.Size(102, 20);
      this.TB_Netrange1.TabIndex = 0;
      this.TB_Netrange1.TabStop = false;
      this.TB_Netrange1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TB_Netrange1_KeyUp);
      // 
      // TB_Subnet2
      // 
      this.TB_Subnet2.Location = new System.Drawing.Point(247, 22);
      this.TB_Subnet2.Name = "TB_Subnet2";
      this.TB_Subnet2.ReadOnly = true;
      this.TB_Subnet2.Size = new System.Drawing.Size(100, 20);
      this.TB_Subnet2.TabIndex = 0;
      this.TB_Subnet2.TabStop = false;
      // 
      // TB_Subnet1
      // 
      this.TB_Subnet1.Location = new System.Drawing.Point(124, 22);
      this.TB_Subnet1.Name = "TB_Subnet1";
      this.TB_Subnet1.ReadOnly = true;
      this.TB_Subnet1.Size = new System.Drawing.Size(102, 20);
      this.TB_Subnet1.TabIndex = 0;
      this.TB_Subnet1.TabStop = false;
      // 
      // CMS_ManageTargets
      // 
      this.CMS_ManageTargets.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectAllToolStripMenuItem,
            this.unselectAllToolStripMenuItem});
      this.CMS_ManageTargets.Name = "CMS_ManageTargets";
      this.CMS_ManageTargets.Size = new System.Drawing.Size(135, 48);
      // 
      // selectAllToolStripMenuItem
      // 
      this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
      this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
      this.selectAllToolStripMenuItem.Text = "Select all";
      this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
      // 
      // unselectAllToolStripMenuItem
      // 
      this.unselectAllToolStripMenuItem.Name = "unselectAllToolStripMenuItem";
      this.unselectAllToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
      this.unselectAllToolStripMenuItem.Text = "Unselect all";
      this.unselectAllToolStripMenuItem.Click += new System.EventHandler(this.unselectAllToolStripMenuItem_Click);
      // 
      // ARPScan
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(629, 335);
      this.Controls.Add(this.GB_Range);
      this.Controls.Add(this.BT_Scan);
      this.Controls.Add(this.DGV_Targets);
      this.Controls.Add(this.BT_Close);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ARPScan";
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
      this.Text = " Target systems ...";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ARPScan_FormClosing);
      ((System.ComponentModel.ISupportInitialize)(this.DGV_Targets)).EndInit();
      this.GB_Range.ResumeLayout(false);
      this.GB_Range.PerformLayout();
      this.CMS_ManageTargets.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button BT_Close;
    private System.Windows.Forms.DataGridView DGV_Targets;
    private System.Windows.Forms.Button BT_Scan;
    private System.Windows.Forms.GroupBox GB_Range;
    private System.Windows.Forms.TextBox TB_Subnet1;
    private System.Windows.Forms.TextBox TB_Subnet2;
    private System.Windows.Forms.TextBox TB_Netrange2;
    private System.Windows.Forms.TextBox TB_Netrange1;
    private System.Windows.Forms.RadioButton RB_Subnet;
    private System.Windows.Forms.RadioButton RB_Netrange;
    private System.Windows.Forms.ContextMenuStrip CMS_ManageTargets;
    private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem unselectAllToolStripMenuItem;
  }
}