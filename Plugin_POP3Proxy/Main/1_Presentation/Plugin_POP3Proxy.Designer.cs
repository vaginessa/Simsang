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
      this.components = new System.ComponentModel.Container();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
      this.DGV_Accounts = new System.Windows.Forms.DataGridView();
      this.L_RemoteHost = new System.Windows.Forms.Label();
      this.TB_POP3Server = new System.Windows.Forms.TextBox();
      this.T_GUIUpdate = new System.Windows.Forms.Timer(this.components);
      ((System.ComponentModel.ISupportInitialize)(this.DGV_Accounts)).BeginInit();
      this.SuspendLayout();
      // 
      // DGV_Accounts
      // 
      this.DGV_Accounts.AllowUserToAddRows = false;
      this.DGV_Accounts.AllowUserToDeleteRows = false;
      this.DGV_Accounts.AllowUserToResizeColumns = false;
      this.DGV_Accounts.AllowUserToResizeRows = false;
      dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.DGV_Accounts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
      this.DGV_Accounts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.DGV_Accounts.Location = new System.Drawing.Point(17, 44);
      this.DGV_Accounts.MultiSelect = false;
      this.DGV_Accounts.Name = "DGV_Accounts";
      this.DGV_Accounts.RowHeadersVisible = false;
      dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.DGV_Accounts.RowsDefaultCellStyle = dataGridViewCellStyle4;
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
      // T_GUIUpdate
      // 
      this.T_GUIUpdate.Interval = 500;
      this.T_GUIUpdate.Tick += new System.EventHandler(this.T_GUIUpdate_Tick);
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
    private System.Windows.Forms.Timer T_GUIUpdate;
  }
}
