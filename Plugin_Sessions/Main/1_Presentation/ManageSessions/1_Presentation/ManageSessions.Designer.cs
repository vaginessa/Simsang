namespace Plugin.Main.Session.ManageSessions
{
  partial class ManageSessions
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManageSessions));
      this.DGV_SessionPatterns = new System.Windows.Forms.DataGridView();
      this.GB_Session = new System.Windows.Forms.GroupBox();
      this.TB_Icon = new System.Windows.Forms.TextBox();
      this.BT_Icon = new System.Windows.Forms.Button();
      this.L_Icon = new System.Windows.Forms.Label();
      this.BT_Add = new System.Windows.Forms.Button();
      this.TB_SessionName = new System.Windows.Forms.TextBox();
      this.TB_WebPage = new System.Windows.Forms.TextBox();
      this.TB_HostRegex = new System.Windows.Forms.TextBox();
      this.TB_SessionCookiesPattern = new System.Windows.Forms.TextBox();
      this.L_WebPage = new System.Windows.Forms.Label();
      this.L_SessionPatternString = new System.Windows.Forms.Label();
      this.L_HostPattern = new System.Windows.Forms.Label();
      this.L_SessionName = new System.Windows.Forms.Label();
      this.OFD_Icon = new System.Windows.Forms.OpenFileDialog();
      this.CMS_ManageSessions = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.deleteSessionPatternToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      ((System.ComponentModel.ISupportInitialize)(this.DGV_SessionPatterns)).BeginInit();
      this.GB_Session.SuspendLayout();
      this.CMS_ManageSessions.SuspendLayout();
      this.SuspendLayout();
      // 
      // DGV_SessionPatterns
      // 
      this.DGV_SessionPatterns.AllowUserToAddRows = false;
      this.DGV_SessionPatterns.AllowUserToDeleteRows = false;
      this.DGV_SessionPatterns.AllowUserToResizeColumns = false;
      this.DGV_SessionPatterns.AllowUserToResizeRows = false;
      dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.DGV_SessionPatterns.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
      this.DGV_SessionPatterns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.DGV_SessionPatterns.Location = new System.Drawing.Point(12, 124);
      this.DGV_SessionPatterns.Name = "DGV_SessionPatterns";
      this.DGV_SessionPatterns.RowHeadersVisible = false;
      this.DGV_SessionPatterns.RowTemplate.Height = 18;
      this.DGV_SessionPatterns.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.DGV_SessionPatterns.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.DGV_SessionPatterns.Size = new System.Drawing.Size(792, 308);
      this.DGV_SessionPatterns.TabIndex = 8;
      this.DGV_SessionPatterns.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ManageSessions_MouseUp);
      // 
      // GB_Session
      // 
      this.GB_Session.Controls.Add(this.TB_Icon);
      this.GB_Session.Controls.Add(this.BT_Icon);
      this.GB_Session.Controls.Add(this.L_Icon);
      this.GB_Session.Controls.Add(this.BT_Add);
      this.GB_Session.Controls.Add(this.TB_SessionName);
      this.GB_Session.Controls.Add(this.TB_WebPage);
      this.GB_Session.Controls.Add(this.TB_HostRegex);
      this.GB_Session.Controls.Add(this.TB_SessionCookiesPattern);
      this.GB_Session.Controls.Add(this.L_WebPage);
      this.GB_Session.Controls.Add(this.L_SessionPatternString);
      this.GB_Session.Controls.Add(this.L_HostPattern);
      this.GB_Session.Controls.Add(this.L_SessionName);
      this.GB_Session.Location = new System.Drawing.Point(11, 13);
      this.GB_Session.Name = "GB_Session";
      this.GB_Session.Size = new System.Drawing.Size(793, 105);
      this.GB_Session.TabIndex = 0;
      this.GB_Session.TabStop = false;
      // 
      // TB_Icon
      // 
      this.TB_Icon.Enabled = false;
      this.TB_Icon.Location = new System.Drawing.Point(107, 73);
      this.TB_Icon.Name = "TB_Icon";
      this.TB_Icon.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
      this.TB_Icon.Size = new System.Drawing.Size(162, 20);
      this.TB_Icon.TabIndex = 6;
      // 
      // BT_Icon
      // 
      this.BT_Icon.Location = new System.Drawing.Point(49, 78);
      this.BT_Icon.Margin = new System.Windows.Forms.Padding(0);
      this.BT_Icon.Name = "BT_Icon";
      this.BT_Icon.Size = new System.Drawing.Size(15, 15);
      this.BT_Icon.TabIndex = 5;
      this.BT_Icon.Text = "+";
      this.BT_Icon.UseVisualStyleBackColor = true;
      this.BT_Icon.Click += new System.EventHandler(this.BT_Icon_Click);
      // 
      // L_Icon
      // 
      this.L_Icon.AutoSize = true;
      this.L_Icon.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.L_Icon.Location = new System.Drawing.Point(7, 79);
      this.L_Icon.Name = "L_Icon";
      this.L_Icon.Size = new System.Drawing.Size(32, 13);
      this.L_Icon.TabIndex = 0;
      this.L_Icon.Text = "Icon";
      // 
      // BT_Add
      // 
      this.BT_Add.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BT_Add.BackgroundImage")));
      this.BT_Add.Location = new System.Drawing.Point(699, 17);
      this.BT_Add.Name = "BT_Add";
      this.BT_Add.Size = new System.Drawing.Size(80, 64);
      this.BT_Add.TabIndex = 7;
      this.BT_Add.UseVisualStyleBackColor = true;
      this.BT_Add.Click += new System.EventHandler(this.BT_Add_Click);
      // 
      // TB_SessionName
      // 
      this.TB_SessionName.Location = new System.Drawing.Point(107, 15);
      this.TB_SessionName.Name = "TB_SessionName";
      this.TB_SessionName.Size = new System.Drawing.Size(162, 20);
      this.TB_SessionName.TabIndex = 1;
      // 
      // TB_WebPage
      // 
      this.TB_WebPage.Location = new System.Drawing.Point(107, 45);
      this.TB_WebPage.Name = "TB_WebPage";
      this.TB_WebPage.Size = new System.Drawing.Size(162, 20);
      this.TB_WebPage.TabIndex = 3;
      // 
      // TB_HostRegex
      // 
      this.TB_HostRegex.Location = new System.Drawing.Point(434, 15);
      this.TB_HostRegex.Name = "TB_HostRegex";
      this.TB_HostRegex.Size = new System.Drawing.Size(233, 20);
      this.TB_HostRegex.TabIndex = 2;
      // 
      // TB_SessionCookiesPattern
      // 
      this.TB_SessionCookiesPattern.Location = new System.Drawing.Point(434, 45);
      this.TB_SessionCookiesPattern.Name = "TB_SessionCookiesPattern";
      this.TB_SessionCookiesPattern.Size = new System.Drawing.Size(233, 20);
      this.TB_SessionCookiesPattern.TabIndex = 4;
      // 
      // L_WebPage
      // 
      this.L_WebPage.AutoSize = true;
      this.L_WebPage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.L_WebPage.Location = new System.Drawing.Point(7, 48);
      this.L_WebPage.Name = "L_WebPage";
      this.L_WebPage.Size = new System.Drawing.Size(65, 13);
      this.L_WebPage.TabIndex = 0;
      this.L_WebPage.Text = "Web page";
      // 
      // L_SessionPatternString
      // 
      this.L_SessionPatternString.AutoSize = true;
      this.L_SessionPatternString.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.L_SessionPatternString.Location = new System.Drawing.Point(287, 48);
      this.L_SessionPatternString.Name = "L_SessionPatternString";
      this.L_SessionPatternString.Size = new System.Drawing.Size(128, 13);
      this.L_SessionPatternString.TabIndex = 0;
      this.L_SessionPatternString.Text = "Sess. coockies regex";
      // 
      // L_HostPattern
      // 
      this.L_HostPattern.AutoSize = true;
      this.L_HostPattern.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.L_HostPattern.Location = new System.Drawing.Point(287, 18);
      this.L_HostPattern.Name = "L_HostPattern";
      this.L_HostPattern.Size = new System.Drawing.Size(103, 13);
      this.L_HostPattern.TabIndex = 0;
      this.L_HostPattern.Text = "HTTP host regex";
      // 
      // L_SessionName
      // 
      this.L_SessionName.AutoSize = true;
      this.L_SessionName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.L_SessionName.Location = new System.Drawing.Point(7, 18);
      this.L_SessionName.Name = "L_SessionName";
      this.L_SessionName.Size = new System.Drawing.Size(85, 13);
      this.L_SessionName.TabIndex = 0;
      this.L_SessionName.Text = "Session name";
      // 
      // OFD_Icon
      // 
      this.OFD_Icon.Filter = "(*.jpg, *.png, *ico)|*.jpg;*.png;*.ico";
      this.OFD_Icon.Title = "Icon";
      // 
      // CMS_ManageSessions
      // 
      this.CMS_ManageSessions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteSessionPatternToolStripMenuItem});
      this.CMS_ManageSessions.Name = "CMS_ManageSessions";
      this.CMS_ManageSessions.Size = new System.Drawing.Size(190, 26);
      // 
      // deleteSessionPatternToolStripMenuItem
      // 
      this.deleteSessionPatternToolStripMenuItem.Name = "deleteSessionPatternToolStripMenuItem";
      this.deleteSessionPatternToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
      this.deleteSessionPatternToolStripMenuItem.Text = "Delete session pattern";
      this.deleteSessionPatternToolStripMenuItem.Click += new System.EventHandler(this.deleteSessionPatternToolStripMenuItem_Click);
      // 
      // ManageSessions
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(818, 444);
      this.Controls.Add(this.GB_Session);
      this.Controls.Add(this.DGV_SessionPatterns);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "ManageSessions";
      this.Text = "  Manage sessions";
      this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ManageSessions_MouseUp);
      ((System.ComponentModel.ISupportInitialize)(this.DGV_SessionPatterns)).EndInit();
      this.GB_Session.ResumeLayout(false);
      this.GB_Session.PerformLayout();
      this.CMS_ManageSessions.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.DataGridView DGV_SessionPatterns;
    private System.Windows.Forms.GroupBox GB_Session;
    private System.Windows.Forms.Label L_WebPage;
    private System.Windows.Forms.Label L_SessionPatternString;
    private System.Windows.Forms.Label L_HostPattern;
    private System.Windows.Forms.Label L_SessionName;
    private System.Windows.Forms.TextBox TB_SessionName;
    private System.Windows.Forms.TextBox TB_WebPage;
    private System.Windows.Forms.TextBox TB_HostRegex;
    private System.Windows.Forms.TextBox TB_SessionCookiesPattern;
    private System.Windows.Forms.Button BT_Add;
    private System.Windows.Forms.Label L_Icon;
    private System.Windows.Forms.OpenFileDialog OFD_Icon;
    private System.Windows.Forms.ContextMenuStrip CMS_ManageSessions;
    private System.Windows.Forms.ToolStripMenuItem deleteSessionPatternToolStripMenuItem;
    private System.Windows.Forms.TextBox TB_Icon;
    private System.Windows.Forms.Button BT_Icon;
  }
}