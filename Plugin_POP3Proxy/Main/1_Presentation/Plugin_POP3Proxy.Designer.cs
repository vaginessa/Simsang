namespace Plugin.Main
{
  partial class PluginPOP3ProxyUC
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
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
      this.DGV_Accounts = new System.Windows.Forms.DataGridView();
      this.L_RemoteHost = new System.Windows.Forms.Label();
      this.TB_POP3Server = new System.Windows.Forms.TextBox();
      ((System.ComponentModel.ISupportInitialize)(this.DGV_Accounts)).BeginInit();
      this.SuspendLayout();
      // 
      // DGV_Accounts
      // 
      this.DGV_Accounts.AllowUserToAddRows = false;
      this.DGV_Accounts.AllowUserToDeleteRows = false;
      this.DGV_Accounts.AllowUserToResizeColumns = false;
      this.DGV_Accounts.AllowUserToResizeRows = false;
      dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.DGV_Accounts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
      this.DGV_Accounts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.DGV_Accounts.Location = new System.Drawing.Point(17, 44);
      this.DGV_Accounts.MultiSelect = false;
      this.DGV_Accounts.Name = "DGV_Accounts";
      this.DGV_Accounts.RowHeadersVisible = false;
      dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.DGV_Accounts.RowsDefaultCellStyle = dataGridViewCellStyle2;
      this.DGV_Accounts.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.DGV_Accounts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.DGV_Accounts.Size = new System.Drawing.Size(830, 309);
      this.DGV_Accounts.TabIndex = 0;
      this.DGV_Accounts.TabStop = false;
      // 
      // L_RemoteHost
      // 
      this.L_RemoteHost.AutoSize = true;
      this.L_RemoteHost.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.L_RemoteHost.Location = new System.Drawing.Point(23, 19);
      this.L_RemoteHost.Name = "L_RemoteHost";
      this.L_RemoteHost.Size = new System.Drawing.Size(95, 13);
      this.L_RemoteHost.TabIndex = 0;
      this.L_RemoteHost.Text = "Forward to host";
      // 
      // TB_POP3Server
      // 
      this.TB_POP3Server.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.TB_POP3Server.Location = new System.Drawing.Point(127, 16);
      this.TB_POP3Server.Name = "TB_POP3Server";
      this.TB_POP3Server.Size = new System.Drawing.Size(200, 20);
      this.TB_POP3Server.TabIndex = 1;
      // 
      // PluginPOP3ProxyUC
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.Transparent;
      this.Controls.Add(this.TB_POP3Server);
      this.Controls.Add(this.L_RemoteHost);
      this.Controls.Add(this.DGV_Accounts);
      this.Name = "PluginPOP3ProxyUC";
      this.Size = new System.Drawing.Size(996, 368);
      ((System.ComponentModel.ISupportInitialize)(this.DGV_Accounts)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.DataGridView DGV_Accounts;
    private System.Windows.Forms.Label L_RemoteHost;
    private System.Windows.Forms.TextBox TB_POP3Server;
  }
}
