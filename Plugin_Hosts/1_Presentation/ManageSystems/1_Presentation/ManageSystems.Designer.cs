namespace Plugin.Main.Systems.ManageSystems
{
  partial class Form_ManageSystems
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_ManageSystems));
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      this.GB_NewSystem = new System.Windows.Forms.GroupBox();
      this.TB_SystemPattern = new System.Windows.Forms.TextBox();
      this.TB_SystemName = new System.Windows.Forms.TextBox();
      this.L_SystemPattern = new System.Windows.Forms.Label();
      this.L_SystemName = new System.Windows.Forms.Label();
      this.BT_Add = new System.Windows.Forms.Button();
      this.DGV_Systems = new System.Windows.Forms.DataGridView();
      this.CMS_ManageSystems = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.deleteSystemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.GB_NewSystem.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.DGV_Systems)).BeginInit();
      this.CMS_ManageSystems.SuspendLayout();
      this.SuspendLayout();
      // 
      // GB_NewSystem
      // 
      this.GB_NewSystem.Controls.Add(this.TB_SystemPattern);
      this.GB_NewSystem.Controls.Add(this.TB_SystemName);
      this.GB_NewSystem.Controls.Add(this.L_SystemPattern);
      this.GB_NewSystem.Controls.Add(this.L_SystemName);
      this.GB_NewSystem.Controls.Add(this.BT_Add);
      this.GB_NewSystem.Location = new System.Drawing.Point(13, 13);
      this.GB_NewSystem.Name = "GB_NewSystem";
      this.GB_NewSystem.Size = new System.Drawing.Size(435, 90);
      this.GB_NewSystem.TabIndex = 0;
      this.GB_NewSystem.TabStop = false;
      // 
      // TB_SystemPattern
      // 
      this.TB_SystemPattern.Location = new System.Drawing.Point(127, 54);
      this.TB_SystemPattern.Name = "TB_SystemPattern";
      this.TB_SystemPattern.Size = new System.Drawing.Size(194, 20);
      this.TB_SystemPattern.TabIndex = 2;
      // 
      // TB_SystemName
      // 
      this.TB_SystemName.Location = new System.Drawing.Point(127, 22);
      this.TB_SystemName.Name = "TB_SystemName";
      this.TB_SystemName.Size = new System.Drawing.Size(194, 20);
      this.TB_SystemName.TabIndex = 1;
      // 
      // L_SystemPattern
      // 
      this.L_SystemPattern.AutoSize = true;
      this.L_SystemPattern.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.L_SystemPattern.Location = new System.Drawing.Point(12, 57);
      this.L_SystemPattern.Name = "L_SystemPattern";
      this.L_SystemPattern.Size = new System.Drawing.Size(91, 13);
      this.L_SystemPattern.TabIndex = 0;
      this.L_SystemPattern.Text = "System pattern";
      // 
      // L_SystemName
      // 
      this.L_SystemName.AutoSize = true;
      this.L_SystemName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.L_SystemName.Location = new System.Drawing.Point(12, 25);
      this.L_SystemName.Name = "L_SystemName";
      this.L_SystemName.Size = new System.Drawing.Size(81, 13);
      this.L_SystemName.TabIndex = 0;
      this.L_SystemName.Text = "System name";
      // 
      // BT_Add
      // 
      this.BT_Add.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BT_Add.BackgroundImage")));
      this.BT_Add.Location = new System.Drawing.Point(343, 16);
      this.BT_Add.Name = "BT_Add";
      this.BT_Add.Size = new System.Drawing.Size(80, 64);
      this.BT_Add.TabIndex = 3;
      this.BT_Add.UseVisualStyleBackColor = true;
      this.BT_Add.Click += new System.EventHandler(this.button1_Click);
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
      this.DGV_Systems.Location = new System.Drawing.Point(12, 120);
      this.DGV_Systems.Name = "DGV_Systems";
      this.DGV_Systems.RowHeadersVisible = false;
      this.DGV_Systems.RowTemplate.Height = 20;
      this.DGV_Systems.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.DGV_Systems.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.DGV_Systems.Size = new System.Drawing.Size(435, 277);
      this.DGV_Systems.TabIndex = 4;
      this.DGV_Systems.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DGV_Systems_MouseUp);
      // 
      // CMS_ManageSystems
      // 
      this.CMS_ManageSystems.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteSystemToolStripMenuItem});
      this.CMS_ManageSystems.Name = "CMS_ManageSystems";
      this.CMS_ManageSystems.Size = new System.Drawing.Size(148, 26);
      // 
      // deleteSystemToolStripMenuItem
      // 
      this.deleteSystemToolStripMenuItem.Name = "deleteSystemToolStripMenuItem";
      this.deleteSystemToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
      this.deleteSystemToolStripMenuItem.Text = "Delete system";
      this.deleteSystemToolStripMenuItem.Click += new System.EventHandler(this.deleteSystemToolStripMenuItem_Click);
      // 
      // Form_ManageSystems
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(460, 409);
      this.Controls.Add(this.DGV_Systems);
      this.Controls.Add(this.GB_NewSystem);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "Form_ManageSystems";
      this.Text = "  Manage systems";
      this.GB_NewSystem.ResumeLayout(false);
      this.GB_NewSystem.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.DGV_Systems)).EndInit();
      this.CMS_ManageSystems.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox GB_NewSystem;
    private System.Windows.Forms.DataGridView DGV_Systems;
    private System.Windows.Forms.ContextMenuStrip CMS_ManageSystems;
    private System.Windows.Forms.ToolStripMenuItem deleteSystemToolStripMenuItem;
    private System.Windows.Forms.Button BT_Add;
    private System.Windows.Forms.Label L_SystemPattern;
    private System.Windows.Forms.Label L_SystemName;
    private System.Windows.Forms.TextBox TB_SystemName;
    private System.Windows.Forms.TextBox TB_SystemPattern;
  }
}