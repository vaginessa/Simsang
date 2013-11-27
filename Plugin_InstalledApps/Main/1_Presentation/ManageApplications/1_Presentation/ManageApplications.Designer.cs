namespace Plugin.Main.Applications.ManageApplications
{
  partial class Form_ManageApps
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_ManageApps));
      this.DGV_ApplicationPatterns = new System.Windows.Forms.DataGridView();
      this.GB_NewPattern = new System.Windows.Forms.GroupBox();
      this.BT_Add = new System.Windows.Forms.Button();
      this.TB_WebPage = new System.Windows.Forms.TextBox();
      this.TB_ProgramName = new System.Windows.Forms.TextBox();
      this.TB_URLPattern = new System.Windows.Forms.TextBox();
      this.L_WebPage = new System.Windows.Forms.Label();
      this.L_ProgramName = new System.Windows.Forms.Label();
      this.L_URLPattern = new System.Windows.Forms.Label();
      this.CMS_ManagePatterns = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.deleteToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
      ((System.ComponentModel.ISupportInitialize)(this.DGV_ApplicationPatterns)).BeginInit();
      this.GB_NewPattern.SuspendLayout();
      this.CMS_ManagePatterns.SuspendLayout();
      this.SuspendLayout();
      // 
      // DGV_ApplicationPatterns
      // 
      this.DGV_ApplicationPatterns.AllowUserToAddRows = false;
      this.DGV_ApplicationPatterns.AllowUserToDeleteRows = false;
      this.DGV_ApplicationPatterns.AllowUserToResizeColumns = false;
      this.DGV_ApplicationPatterns.AllowUserToResizeRows = false;
      dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.DGV_ApplicationPatterns.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
      this.DGV_ApplicationPatterns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.DGV_ApplicationPatterns.Location = new System.Drawing.Point(12, 120);
      this.DGV_ApplicationPatterns.Name = "DGV_ApplicationPatterns";
      this.DGV_ApplicationPatterns.RowHeadersVisible = false;
      this.DGV_ApplicationPatterns.RowTemplate.Height = 20;
      this.DGV_ApplicationPatterns.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.DGV_ApplicationPatterns.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.DGV_ApplicationPatterns.Size = new System.Drawing.Size(612, 277);
      this.DGV_ApplicationPatterns.TabIndex = 5;
      this.DGV_ApplicationPatterns.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DGV_ApplicationPatterns_MouseUp);
      // 
      // GB_NewPattern
      // 
      this.GB_NewPattern.Controls.Add(this.BT_Add);
      this.GB_NewPattern.Controls.Add(this.TB_WebPage);
      this.GB_NewPattern.Controls.Add(this.TB_ProgramName);
      this.GB_NewPattern.Controls.Add(this.TB_URLPattern);
      this.GB_NewPattern.Controls.Add(this.L_WebPage);
      this.GB_NewPattern.Controls.Add(this.L_ProgramName);
      this.GB_NewPattern.Controls.Add(this.L_URLPattern);
      this.GB_NewPattern.Location = new System.Drawing.Point(13, 13);
      this.GB_NewPattern.Name = "GB_NewPattern";
      this.GB_NewPattern.Size = new System.Drawing.Size(611, 89);
      this.GB_NewPattern.TabIndex = 0;
      this.GB_NewPattern.TabStop = false;
      // 
      // BT_Add
      // 
      this.BT_Add.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BT_Add.BackgroundImage")));
      this.BT_Add.Location = new System.Drawing.Point(521, 15);
      this.BT_Add.Name = "BT_Add";
      this.BT_Add.Size = new System.Drawing.Size(80, 64);
      this.BT_Add.TabIndex = 4;
      this.BT_Add.UseVisualStyleBackColor = true;
      this.BT_Add.Click += new System.EventHandler(this.BT_Add_Click);
      // 
      // TB_WebPage
      // 
      this.TB_WebPage.Location = new System.Drawing.Point(367, 23);
      this.TB_WebPage.Name = "TB_WebPage";
      this.TB_WebPage.Size = new System.Drawing.Size(135, 20);
      this.TB_WebPage.TabIndex = 3;
      // 
      // TB_ProgramName
      // 
      this.TB_ProgramName.Location = new System.Drawing.Point(105, 22);
      this.TB_ProgramName.Name = "TB_ProgramName";
      this.TB_ProgramName.Size = new System.Drawing.Size(178, 20);
      this.TB_ProgramName.TabIndex = 1;
      // 
      // TB_URLPattern
      // 
      this.TB_URLPattern.Location = new System.Drawing.Point(105, 53);
      this.TB_URLPattern.Name = "TB_URLPattern";
      this.TB_URLPattern.Size = new System.Drawing.Size(178, 20);
      this.TB_URLPattern.TabIndex = 2;
      // 
      // L_WebPage
      // 
      this.L_WebPage.AutoSize = true;
      this.L_WebPage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.L_WebPage.Location = new System.Drawing.Point(298, 25);
      this.L_WebPage.Name = "L_WebPage";
      this.L_WebPage.Size = new System.Drawing.Size(65, 13);
      this.L_WebPage.TabIndex = 0;
      this.L_WebPage.Text = "Web page";
      // 
      // L_ProgramName
      // 
      this.L_ProgramName.AutoSize = true;
      this.L_ProgramName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.L_ProgramName.Location = new System.Drawing.Point(12, 25);
      this.L_ProgramName.Name = "L_ProgramName";
      this.L_ProgramName.Size = new System.Drawing.Size(87, 13);
      this.L_ProgramName.TabIndex = 0;
      this.L_ProgramName.Text = "Program name";
      // 
      // L_URLPattern
      // 
      this.L_URLPattern.AutoSize = true;
      this.L_URLPattern.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.L_URLPattern.Location = new System.Drawing.Point(12, 57);
      this.L_URLPattern.Name = "L_URLPattern";
      this.L_URLPattern.Size = new System.Drawing.Size(76, 13);
      this.L_URLPattern.TabIndex = 0;
      this.L_URLPattern.Text = "URL pattern";
      // 
      // CMS_ManagePatterns
      // 
      this.CMS_ManagePatterns.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem1});
      this.CMS_ManagePatterns.Name = "CMS_ManagePatterns";
      this.CMS_ManagePatterns.Size = new System.Drawing.Size(149, 26);
      // 
      // deleteToolStripMenuItem1
      // 
      this.deleteToolStripMenuItem1.Name = "deleteToolStripMenuItem1";
      this.deleteToolStripMenuItem1.Size = new System.Drawing.Size(148, 22);
      this.deleteToolStripMenuItem1.Text = "Delete pattern";
      this.deleteToolStripMenuItem1.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
      // 
      // Form_ManageApps
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(636, 409);
      this.Controls.Add(this.GB_NewPattern);
      this.Controls.Add(this.DGV_ApplicationPatterns);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "Form_ManageApps";
      this.Text = "  Manage applications";
      ((System.ComponentModel.ISupportInitialize)(this.DGV_ApplicationPatterns)).EndInit();
      this.GB_NewPattern.ResumeLayout(false);
      this.GB_NewPattern.PerformLayout();
      this.CMS_ManagePatterns.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.DataGridView DGV_ApplicationPatterns;
    private System.Windows.Forms.GroupBox GB_NewPattern;
    private System.Windows.Forms.Button BT_Add;
    private System.Windows.Forms.TextBox TB_WebPage;
    private System.Windows.Forms.TextBox TB_ProgramName;
    private System.Windows.Forms.TextBox TB_URLPattern;
    private System.Windows.Forms.Label L_WebPage;
    private System.Windows.Forms.Label L_ProgramName;
    private System.Windows.Forms.Label L_URLPattern;
    private System.Windows.Forms.ContextMenuStrip CMS_ManagePatterns;
    private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem1;
  }
}