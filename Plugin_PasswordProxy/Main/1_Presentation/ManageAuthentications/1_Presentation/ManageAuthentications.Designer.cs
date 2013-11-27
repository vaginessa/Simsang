namespace Plugin.Main.HTTPProxy.ManageAuthentications
{
  partial class Form_ManageAuthentications
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
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_ManageAuthentications));
      this.DGV_AccountPatterns = new System.Windows.Forms.DataGridView();
      this.GB_AddAuthPattern = new System.Windows.Forms.GroupBox();
      this.TB_WebPage = new System.Windows.Forms.TextBox();
      this.TB_Company = new System.Windows.Forms.TextBox();
      this.TB_DatePattern = new System.Windows.Forms.TextBox();
      this.TB_PathPattern = new System.Windows.Forms.TextBox();
      this.TB_HostPattern = new System.Windows.Forms.TextBox();
      this.TB_Method = new System.Windows.Forms.TextBox();
      this.L_WebPage = new System.Windows.Forms.Label();
      this.L_Company = new System.Windows.Forms.Label();
      this.L_PostDataPattern = new System.Windows.Forms.Label();
      this.L_PathPattern = new System.Windows.Forms.Label();
      this.L_HostPattern = new System.Windows.Forms.Label();
      this.L_Method = new System.Windows.Forms.Label();
      this.BT_Add = new System.Windows.Forms.Button();
      this.CMS_ManageAccounts = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.deletePatternToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      ((System.ComponentModel.ISupportInitialize)(this.DGV_AccountPatterns)).BeginInit();
      this.GB_AddAuthPattern.SuspendLayout();
      this.CMS_ManageAccounts.SuspendLayout();
      this.SuspendLayout();
      // 
      // DGV_AccountPatterns
      // 
      this.DGV_AccountPatterns.AllowUserToAddRows = false;
      this.DGV_AccountPatterns.AllowUserToDeleteRows = false;
      this.DGV_AccountPatterns.AllowUserToResizeColumns = false;
      this.DGV_AccountPatterns.AllowUserToResizeRows = false;
      dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.DGV_AccountPatterns.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
      this.DGV_AccountPatterns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.DGV_AccountPatterns.Location = new System.Drawing.Point(12, 137);
      this.DGV_AccountPatterns.Name = "DGV_AccountPatterns";
      this.DGV_AccountPatterns.RowHeadersVisible = false;
      this.DGV_AccountPatterns.RowTemplate.Height = 18;
      this.DGV_AccountPatterns.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.DGV_AccountPatterns.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.DGV_AccountPatterns.Size = new System.Drawing.Size(793, 260);
      this.DGV_AccountPatterns.TabIndex = 8;
      this.DGV_AccountPatterns.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DGV_AccountPatterns_MouseUp);
      // 
      // GB_AddAuthPattern
      // 
      this.GB_AddAuthPattern.Controls.Add(this.TB_WebPage);
      this.GB_AddAuthPattern.Controls.Add(this.TB_Company);
      this.GB_AddAuthPattern.Controls.Add(this.TB_DatePattern);
      this.GB_AddAuthPattern.Controls.Add(this.TB_PathPattern);
      this.GB_AddAuthPattern.Controls.Add(this.TB_HostPattern);
      this.GB_AddAuthPattern.Controls.Add(this.TB_Method);
      this.GB_AddAuthPattern.Controls.Add(this.L_WebPage);
      this.GB_AddAuthPattern.Controls.Add(this.L_Company);
      this.GB_AddAuthPattern.Controls.Add(this.L_PostDataPattern);
      this.GB_AddAuthPattern.Controls.Add(this.L_PathPattern);
      this.GB_AddAuthPattern.Controls.Add(this.L_HostPattern);
      this.GB_AddAuthPattern.Controls.Add(this.L_Method);
      this.GB_AddAuthPattern.Controls.Add(this.BT_Add);
      this.GB_AddAuthPattern.Location = new System.Drawing.Point(13, 13);
      this.GB_AddAuthPattern.Name = "GB_AddAuthPattern";
      this.GB_AddAuthPattern.Size = new System.Drawing.Size(792, 107);
      this.GB_AddAuthPattern.TabIndex = 1;
      this.GB_AddAuthPattern.TabStop = false;
      // 
      // TB_WebPage
      // 
      this.TB_WebPage.Location = new System.Drawing.Point(446, 73);
      this.TB_WebPage.Name = "TB_WebPage";
      this.TB_WebPage.Size = new System.Drawing.Size(193, 20);
      this.TB_WebPage.TabIndex = 6;
      // 
      // TB_Company
      // 
      this.TB_Company.Location = new System.Drawing.Point(446, 46);
      this.TB_Company.Name = "TB_Company";
      this.TB_Company.Size = new System.Drawing.Size(193, 20);
      this.TB_Company.TabIndex = 5;
      // 
      // TB_DatePattern
      // 
      this.TB_DatePattern.Location = new System.Drawing.Point(446, 18);
      this.TB_DatePattern.Name = "TB_DatePattern";
      this.TB_DatePattern.Size = new System.Drawing.Size(193, 20);
      this.TB_DatePattern.TabIndex = 4;
      // 
      // TB_PathPattern
      // 
      this.TB_PathPattern.Location = new System.Drawing.Point(115, 73);
      this.TB_PathPattern.Name = "TB_PathPattern";
      this.TB_PathPattern.Size = new System.Drawing.Size(193, 20);
      this.TB_PathPattern.TabIndex = 3;
      // 
      // TB_HostPattern
      // 
      this.TB_HostPattern.Location = new System.Drawing.Point(115, 46);
      this.TB_HostPattern.Name = "TB_HostPattern";
      this.TB_HostPattern.Size = new System.Drawing.Size(193, 20);
      this.TB_HostPattern.TabIndex = 2;
      // 
      // TB_Method
      // 
      this.TB_Method.Location = new System.Drawing.Point(115, 18);
      this.TB_Method.Name = "TB_Method";
      this.TB_Method.Size = new System.Drawing.Size(193, 20);
      this.TB_Method.TabIndex = 1;
      // 
      // L_WebPage
      // 
      this.L_WebPage.AutoSize = true;
      this.L_WebPage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.L_WebPage.Location = new System.Drawing.Point(348, 76);
      this.L_WebPage.Name = "L_WebPage";
      this.L_WebPage.Size = new System.Drawing.Size(65, 13);
      this.L_WebPage.TabIndex = 0;
      this.L_WebPage.Text = "Web page";
      // 
      // L_Company
      // 
      this.L_Company.AutoSize = true;
      this.L_Company.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.L_Company.Location = new System.Drawing.Point(348, 49);
      this.L_Company.Name = "L_Company";
      this.L_Company.Size = new System.Drawing.Size(58, 13);
      this.L_Company.TabIndex = 0;
      this.L_Company.Text = "Company";
      // 
      // L_PostDataPattern
      // 
      this.L_PostDataPattern.AutoSize = true;
      this.L_PostDataPattern.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.L_PostDataPattern.Location = new System.Drawing.Point(348, 21);
      this.L_PostDataPattern.Name = "L_PostDataPattern";
      this.L_PostDataPattern.Size = new System.Drawing.Size(78, 13);
      this.L_PostDataPattern.TabIndex = 0;
      this.L_PostDataPattern.Text = "Data pattern";
      // 
      // L_PathPattern
      // 
      this.L_PathPattern.AutoSize = true;
      this.L_PathPattern.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.L_PathPattern.Location = new System.Drawing.Point(12, 76);
      this.L_PathPattern.Name = "L_PathPattern";
      this.L_PathPattern.Size = new System.Drawing.Size(77, 13);
      this.L_PathPattern.TabIndex = 0;
      this.L_PathPattern.Text = "Path pattern";
      // 
      // L_HostPattern
      // 
      this.L_HostPattern.AutoSize = true;
      this.L_HostPattern.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.L_HostPattern.Location = new System.Drawing.Point(12, 49);
      this.L_HostPattern.Name = "L_HostPattern";
      this.L_HostPattern.Size = new System.Drawing.Size(77, 13);
      this.L_HostPattern.TabIndex = 0;
      this.L_HostPattern.Text = "Host pattern";
      // 
      // L_Method
      // 
      this.L_Method.AutoSize = true;
      this.L_Method.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.L_Method.Location = new System.Drawing.Point(12, 21);
      this.L_Method.Name = "L_Method";
      this.L_Method.Size = new System.Drawing.Size(49, 13);
      this.L_Method.TabIndex = 0;
      this.L_Method.Text = "Method";
      // 
      // BT_Add
      // 
      this.BT_Add.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BT_Add.BackgroundImage")));
      this.BT_Add.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.BT_Add.Location = new System.Drawing.Point(683, 17);
      this.BT_Add.Name = "BT_Add";
      this.BT_Add.Size = new System.Drawing.Size(81, 68);
      this.BT_Add.TabIndex = 7;
      this.BT_Add.UseVisualStyleBackColor = true;
      this.BT_Add.Click += new System.EventHandler(this.BT_Add_Click);
      // 
      // CMS_ManageAccounts
      // 
      this.CMS_ManageAccounts.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deletePatternToolStripMenuItem});
      this.CMS_ManageAccounts.Name = "CMS_ManageAccounts";
      this.CMS_ManageAccounts.Size = new System.Drawing.Size(149, 26);
      // 
      // deletePatternToolStripMenuItem
      // 
      this.deletePatternToolStripMenuItem.Name = "deletePatternToolStripMenuItem";
      this.deletePatternToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
      this.deletePatternToolStripMenuItem.Text = "Delete pattern";
      this.deletePatternToolStripMenuItem.Click += new System.EventHandler(this.deletePatternToolStripMenuItem_Click);
      // 
      // Form_ManageAuthentications
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(817, 409);
      this.Controls.Add(this.GB_AddAuthPattern);
      this.Controls.Add(this.DGV_AccountPatterns);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "Form_ManageAuthentications";
      this.Text = "  Manage authentications patterns";
      ((System.ComponentModel.ISupportInitialize)(this.DGV_AccountPatterns)).EndInit();
      this.GB_AddAuthPattern.ResumeLayout(false);
      this.GB_AddAuthPattern.PerformLayout();
      this.CMS_ManageAccounts.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.DataGridView DGV_AccountPatterns;
    private System.Windows.Forms.GroupBox GB_AddAuthPattern;
    private System.Windows.Forms.Button BT_Add;
    private System.Windows.Forms.Label L_Method;
    private System.Windows.Forms.Label L_HostPattern;
    private System.Windows.Forms.TextBox TB_WebPage;
    private System.Windows.Forms.TextBox TB_Company;
    private System.Windows.Forms.TextBox TB_DatePattern;
    private System.Windows.Forms.TextBox TB_PathPattern;
    private System.Windows.Forms.TextBox TB_HostPattern;
    private System.Windows.Forms.TextBox TB_Method;
    private System.Windows.Forms.Label L_WebPage;
    private System.Windows.Forms.Label L_Company;
    private System.Windows.Forms.Label L_PostDataPattern;
    private System.Windows.Forms.Label L_PathPattern;
    private System.Windows.Forms.ContextMenuStrip CMS_ManageAccounts;
    private System.Windows.Forms.ToolStripMenuItem deletePatternToolStripMenuItem;
  }
}