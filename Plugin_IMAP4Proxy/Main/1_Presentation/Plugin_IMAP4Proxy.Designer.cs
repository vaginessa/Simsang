namespace Plugin.Main
{
  partial class PluginIMAP4ProxyUC
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
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
      this.DGV_Accounts = new System.Windows.Forms.DataGridView();
      this.L_ForwardHost = new System.Windows.Forms.Label();
      this.TB_ForwardHost = new System.Windows.Forms.TextBox();
      ((System.ComponentModel.ISupportInitialize)(this.DGV_Accounts)).BeginInit();
      this.SuspendLayout();
      // 
      // DGV_Accounts
      // 
      this.DGV_Accounts.AllowUserToAddRows = false;
      this.DGV_Accounts.AllowUserToDeleteRows = false;
      this.DGV_Accounts.AllowUserToResizeColumns = false;
      this.DGV_Accounts.AllowUserToResizeRows = false;
      this.DGV_Accounts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.DGV_Accounts.Location = new System.Drawing.Point(17, 44);
      this.DGV_Accounts.MultiSelect = false;
      this.DGV_Accounts.Name = "DGV_Accounts";
      this.DGV_Accounts.RowHeadersVisible = false;
      dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.DGV_Accounts.RowsDefaultCellStyle = dataGridViewCellStyle6;
      this.DGV_Accounts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.DGV_Accounts.Size = new System.Drawing.Size(830, 309);
      this.DGV_Accounts.TabIndex = 0;
      this.DGV_Accounts.TabStop = false;
      // 
      // L_ForwardHost
      // 
      this.L_ForwardHost.AutoSize = true;
      this.L_ForwardHost.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.L_ForwardHost.Location = new System.Drawing.Point(23, 19);
      this.L_ForwardHost.Name = "L_ForwardHost";
      this.L_ForwardHost.Size = new System.Drawing.Size(95, 13);
      this.L_ForwardHost.TabIndex = 0;
      this.L_ForwardHost.Text = "Forward to host";
      // 
      // TB_ForwardHost
      // 
      this.TB_ForwardHost.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.TB_ForwardHost.Location = new System.Drawing.Point(127, 16);
      this.TB_ForwardHost.Name = "TB_ForwardHost";
      this.TB_ForwardHost.Size = new System.Drawing.Size(200, 20);
      this.TB_ForwardHost.TabIndex = 1;
      // 
      // PluginIMAP4ProxyUC
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.Transparent;
      this.Controls.Add(this.TB_ForwardHost);
      this.Controls.Add(this.L_ForwardHost);
      this.Controls.Add(this.DGV_Accounts);
      this.Name = "PluginIMAP4ProxyUC";
      this.Size = new System.Drawing.Size(996, 368);
      ((System.ComponentModel.ISupportInitialize)(this.DGV_Accounts)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.DataGridView DGV_Accounts;
    private System.Windows.Forms.Label L_ForwardHost;
    private System.Windows.Forms.TextBox TB_ForwardHost;
  }
}
