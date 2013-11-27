namespace Plugin.Main.IPAccounting.ManageServices
{
  partial class Form_ManageServices
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_ManageServices));
      this.GB_ManageServices = new System.Windows.Forms.GroupBox();
      this.TB_LowerPort = new System.Windows.Forms.TextBox();
      this.TB_UpperPort = new System.Windows.Forms.TextBox();
      this.TB_ServiceName = new System.Windows.Forms.TextBox();
      this.L_ServiceName = new System.Windows.Forms.Label();
      this.L_UpperPort = new System.Windows.Forms.Label();
      this.L_PortLower = new System.Windows.Forms.Label();
      this.BT_Add = new System.Windows.Forms.Button();
      this.DGV_Services = new System.Windows.Forms.DataGridView();
      this.CMS_ManageServices = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.deleteServiceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.GB_ManageServices.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.DGV_Services)).BeginInit();
      this.CMS_ManageServices.SuspendLayout();
      this.SuspendLayout();
      // 
      // GB_ManageServices
      // 
      this.GB_ManageServices.Controls.Add(this.TB_LowerPort);
      this.GB_ManageServices.Controls.Add(this.TB_UpperPort);
      this.GB_ManageServices.Controls.Add(this.TB_ServiceName);
      this.GB_ManageServices.Controls.Add(this.L_ServiceName);
      this.GB_ManageServices.Controls.Add(this.L_UpperPort);
      this.GB_ManageServices.Controls.Add(this.L_PortLower);
      this.GB_ManageServices.Controls.Add(this.BT_Add);
      this.GB_ManageServices.Location = new System.Drawing.Point(13, 13);
      this.GB_ManageServices.Name = "GB_ManageServices";
      this.GB_ManageServices.Size = new System.Drawing.Size(414, 90);
      this.GB_ManageServices.TabIndex = 0;
      this.GB_ManageServices.TabStop = false;
      // 
      // TB_LowerPort
      // 
      this.TB_LowerPort.Location = new System.Drawing.Point(101, 25);
      this.TB_LowerPort.Name = "TB_LowerPort";
      this.TB_LowerPort.Size = new System.Drawing.Size(52, 20);
      this.TB_LowerPort.TabIndex = 1;
      // 
      // TB_UpperPort
      // 
      this.TB_UpperPort.Location = new System.Drawing.Point(238, 22);
      this.TB_UpperPort.Name = "TB_UpperPort";
      this.TB_UpperPort.Size = new System.Drawing.Size(52, 20);
      this.TB_UpperPort.TabIndex = 2;
      // 
      // TB_ServiceName
      // 
      this.TB_ServiceName.Location = new System.Drawing.Point(101, 61);
      this.TB_ServiceName.Name = "TB_ServiceName";
      this.TB_ServiceName.Size = new System.Drawing.Size(189, 20);
      this.TB_ServiceName.TabIndex = 3;
      // 
      // L_ServiceName
      // 
      this.L_ServiceName.AutoSize = true;
      this.L_ServiceName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.L_ServiceName.Location = new System.Drawing.Point(12, 61);
      this.L_ServiceName.Name = "L_ServiceName";
      this.L_ServiceName.Size = new System.Drawing.Size(84, 13);
      this.L_ServiceName.TabIndex = 0;
      this.L_ServiceName.Text = "Service name";
      // 
      // L_UpperPort
      // 
      this.L_UpperPort.AutoSize = true;
      this.L_UpperPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.L_UpperPort.Location = new System.Drawing.Point(166, 25);
      this.L_UpperPort.Name = "L_UpperPort";
      this.L_UpperPort.Size = new System.Drawing.Size(67, 13);
      this.L_UpperPort.TabIndex = 0;
      this.L_UpperPort.Text = "Upper port";
      // 
      // L_PortLower
      // 
      this.L_PortLower.AutoSize = true;
      this.L_PortLower.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.L_PortLower.Location = new System.Drawing.Point(12, 25);
      this.L_PortLower.Name = "L_PortLower";
      this.L_PortLower.Size = new System.Drawing.Size(67, 13);
      this.L_PortLower.TabIndex = 0;
      this.L_PortLower.Text = "Lower port";
      // 
      // BT_Add
      // 
      this.BT_Add.BackgroundImage = global::Plugin_IPAccounting.Properties.Resources.Add_Service;
      this.BT_Add.Location = new System.Drawing.Point(322, 16);
      this.BT_Add.Name = "BT_Add";
      this.BT_Add.Size = new System.Drawing.Size(80, 64);
      this.BT_Add.TabIndex = 4;
      this.BT_Add.UseVisualStyleBackColor = true;
      this.BT_Add.Click += new System.EventHandler(this.BT_Add_Click);
      // 
      // DGV_Services
      // 
      this.DGV_Services.AllowUserToOrderColumns = true;
      this.DGV_Services.AllowUserToResizeColumns = false;
      this.DGV_Services.AllowUserToResizeRows = false;
      dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.DGV_Services.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
      this.DGV_Services.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.DGV_Services.Location = new System.Drawing.Point(12, 120);
      this.DGV_Services.Name = "DGV_Services";
      this.DGV_Services.RowHeadersVisible = false;
      this.DGV_Services.RowTemplate.Height = 18;
      this.DGV_Services.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.DGV_Services.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.DGV_Services.Size = new System.Drawing.Size(415, 263);
      this.DGV_Services.TabIndex = 5;
      this.DGV_Services.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DGV_Services_MouseUp);
      // 
      // CMS_ManageServices
      // 
      this.CMS_ManageServices.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteServiceToolStripMenuItem});
      this.CMS_ManageServices.Name = "CMS_ManageServices";
      this.CMS_ManageServices.Size = new System.Drawing.Size(147, 26);
      // 
      // deleteServiceToolStripMenuItem
      // 
      this.deleteServiceToolStripMenuItem.Name = "deleteServiceToolStripMenuItem";
      this.deleteServiceToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
      this.deleteServiceToolStripMenuItem.Text = "Delete service";
      this.deleteServiceToolStripMenuItem.Click += new System.EventHandler(this.deleteServiceToolStripMenuItem_Click);
      // 
      // Form_ManageServices
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(440, 396);
      this.Controls.Add(this.DGV_Services);
      this.Controls.Add(this.GB_ManageServices);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "Form_ManageServices";
      this.Text = "  Manage services";
      this.GB_ManageServices.ResumeLayout(false);
      this.GB_ManageServices.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.DGV_Services)).EndInit();
      this.CMS_ManageServices.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox GB_ManageServices;
    private System.Windows.Forms.DataGridView DGV_Services;
    private System.Windows.Forms.Button BT_Add;
    private System.Windows.Forms.Label L_ServiceName;
    private System.Windows.Forms.Label L_UpperPort;
    private System.Windows.Forms.Label L_PortLower;
    private System.Windows.Forms.TextBox TB_LowerPort;
    private System.Windows.Forms.TextBox TB_UpperPort;
    private System.Windows.Forms.TextBox TB_ServiceName;
    private System.Windows.Forms.ContextMenuStrip CMS_ManageServices;
    private System.Windows.Forms.ToolStripMenuItem deleteServiceToolStripMenuItem;
  }
}