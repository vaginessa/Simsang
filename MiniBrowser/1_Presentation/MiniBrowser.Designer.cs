namespace Simsang.MiniBrowser
{
    partial class Browser
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

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Browser));
      this.GB_Details = new System.Windows.Forms.GroupBox();
      this.CB_Cookies = new System.Windows.Forms.CheckBox();
      this.CB_UserAgent = new System.Windows.Forms.CheckBox();
      this.BT_Open = new System.Windows.Forms.Button();
      this.TB_UserAgent = new System.Windows.Forms.TextBox();
      this.TB_Cookies = new System.Windows.Forms.TextBox();
      this.L_UserAgent = new System.Windows.Forms.Label();
      this.L_Cookies = new System.Windows.Forms.Label();
      this.L_URL = new System.Windows.Forms.Label();
      this.TB_URL = new System.Windows.Forms.TextBox();
      this.GB_WebPage = new System.Windows.Forms.GroupBox();
      this.WB_MiniBrowser = new System.Windows.Forms.WebBrowser();
      this.BGW_GetAccessToken = new System.ComponentModel.BackgroundWorker();
      this.GB_Details.SuspendLayout();
      this.GB_WebPage.SuspendLayout();
      this.SuspendLayout();
      // 
      // GB_Details
      // 
      this.GB_Details.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.GB_Details.Controls.Add(this.CB_Cookies);
      this.GB_Details.Controls.Add(this.CB_UserAgent);
      this.GB_Details.Controls.Add(this.BT_Open);
      this.GB_Details.Controls.Add(this.TB_UserAgent);
      this.GB_Details.Controls.Add(this.TB_Cookies);
      this.GB_Details.Controls.Add(this.L_UserAgent);
      this.GB_Details.Controls.Add(this.L_Cookies);
      this.GB_Details.Controls.Add(this.L_URL);
      this.GB_Details.Controls.Add(this.TB_URL);
      this.GB_Details.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.GB_Details.Location = new System.Drawing.Point(11, 5);
      this.GB_Details.Margin = new System.Windows.Forms.Padding(5);
      this.GB_Details.Name = "GB_Details";
      this.GB_Details.Size = new System.Drawing.Size(712, 125);
      this.GB_Details.TabIndex = 0;
      this.GB_Details.TabStop = false;
      // 
      // CB_Cookies
      // 
      this.CB_Cookies.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.CB_Cookies.AutoSize = true;
      this.CB_Cookies.Checked = true;
      this.CB_Cookies.CheckState = System.Windows.Forms.CheckState.Checked;
      this.CB_Cookies.Location = new System.Drawing.Point(630, 63);
      this.CB_Cookies.Name = "CB_Cookies";
      this.CB_Cookies.Size = new System.Drawing.Size(53, 17);
      this.CB_Cookies.TabIndex = 5;
      this.CB_Cookies.Text = "Use it";
      this.CB_Cookies.UseVisualStyleBackColor = true;
      // 
      // CB_UserAgent
      // 
      this.CB_UserAgent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.CB_UserAgent.AutoSize = true;
      this.CB_UserAgent.Location = new System.Drawing.Point(630, 89);
      this.CB_UserAgent.Name = "CB_UserAgent";
      this.CB_UserAgent.Size = new System.Drawing.Size(53, 17);
      this.CB_UserAgent.TabIndex = 6;
      this.CB_UserAgent.Text = "Use it";
      this.CB_UserAgent.UseVisualStyleBackColor = true;
      // 
      // BT_Open
      // 
      this.BT_Open.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.BT_Open.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.BT_Open.Location = new System.Drawing.Point(624, 31);
      this.BT_Open.Name = "BT_Open";
      this.BT_Open.Size = new System.Drawing.Size(59, 23);
      this.BT_Open.TabIndex = 4;
      this.BT_Open.Text = "Open";
      this.BT_Open.UseVisualStyleBackColor = true;
      this.BT_Open.Click += new System.EventHandler(this.BT_Open_Click);
      // 
      // TB_UserAgent
      // 
      this.TB_UserAgent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.TB_UserAgent.Location = new System.Drawing.Point(105, 86);
      this.TB_UserAgent.Name = "TB_UserAgent";
      this.TB_UserAgent.Size = new System.Drawing.Size(498, 20);
      this.TB_UserAgent.TabIndex = 3;
      // 
      // TB_Cookies
      // 
      this.TB_Cookies.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.TB_Cookies.Location = new System.Drawing.Point(105, 60);
      this.TB_Cookies.Name = "TB_Cookies";
      this.TB_Cookies.Size = new System.Drawing.Size(498, 20);
      this.TB_Cookies.TabIndex = 2;
      // 
      // L_UserAgent
      // 
      this.L_UserAgent.AutoSize = true;
      this.L_UserAgent.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.L_UserAgent.Location = new System.Drawing.Point(16, 86);
      this.L_UserAgent.Name = "L_UserAgent";
      this.L_UserAgent.Size = new System.Drawing.Size(69, 13);
      this.L_UserAgent.TabIndex = 0;
      this.L_UserAgent.Text = "User agent";
      // 
      // L_Cookies
      // 
      this.L_Cookies.AutoSize = true;
      this.L_Cookies.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.L_Cookies.Location = new System.Drawing.Point(16, 60);
      this.L_Cookies.Name = "L_Cookies";
      this.L_Cookies.Size = new System.Drawing.Size(52, 13);
      this.L_Cookies.TabIndex = 0;
      this.L_Cookies.Text = "Cookies";
      // 
      // L_URL
      // 
      this.L_URL.AutoSize = true;
      this.L_URL.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.L_URL.Location = new System.Drawing.Point(16, 33);
      this.L_URL.Name = "L_URL";
      this.L_URL.Size = new System.Drawing.Size(32, 13);
      this.L_URL.TabIndex = 0;
      this.L_URL.Text = "URL";
      // 
      // TB_URL
      // 
      this.TB_URL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.TB_URL.Location = new System.Drawing.Point(105, 33);
      this.TB_URL.Name = "TB_URL";
      this.TB_URL.Size = new System.Drawing.Size(498, 20);
      this.TB_URL.TabIndex = 1;
      this.TB_URL.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TB_URL_KeyDown);
      // 
      // GB_WebPage
      // 
      this.GB_WebPage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.GB_WebPage.Controls.Add(this.WB_MiniBrowser);
      this.GB_WebPage.Location = new System.Drawing.Point(12, 149);
      this.GB_WebPage.Name = "GB_WebPage";
      this.GB_WebPage.Size = new System.Drawing.Size(714, 356);
      this.GB_WebPage.TabIndex = 1;
      this.GB_WebPage.TabStop = false;
      // 
      // WB_MiniBrowser
      // 
      this.WB_MiniBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
      this.WB_MiniBrowser.Location = new System.Drawing.Point(3, 16);
      this.WB_MiniBrowser.MinimumSize = new System.Drawing.Size(20, 20);
      this.WB_MiniBrowser.Name = "WB_MiniBrowser";
      this.WB_MiniBrowser.ScriptErrorsSuppressed = true;
      this.WB_MiniBrowser.Size = new System.Drawing.Size(708, 337);
      this.WB_MiniBrowser.TabIndex = 7;
      // 
      // BGW_GetAccessToken
      // 
      this.BGW_GetAccessToken.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BGW_GetAccessToken_DoWork);
      // 
      // Browser
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(740, 517);
      this.Controls.Add(this.GB_WebPage);
      this.Controls.Add(this.GB_Details);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MinimizeBox = false;
      this.Name = "Browser";
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
      this.Text = "MiniBrowser";
      this.GB_Details.ResumeLayout(false);
      this.GB_Details.PerformLayout();
      this.GB_WebPage.ResumeLayout(false);
      this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GB_Details;
        private System.Windows.Forms.TextBox TB_URL;
        private System.Windows.Forms.TextBox TB_UserAgent;
        private System.Windows.Forms.TextBox TB_Cookies;
        private System.Windows.Forms.Label L_UserAgent;
        private System.Windows.Forms.Label L_Cookies;
        private System.Windows.Forms.Label L_URL;
        private System.Windows.Forms.GroupBox GB_WebPage;
        private System.Windows.Forms.WebBrowser WB_MiniBrowser;
        private System.Windows.Forms.Button BT_Open;
        private System.Windows.Forms.CheckBox CB_UserAgent;
        private System.Windows.Forms.CheckBox CB_Cookies;
        private System.ComponentModel.BackgroundWorker BGW_GetAccessToken;
    }
}