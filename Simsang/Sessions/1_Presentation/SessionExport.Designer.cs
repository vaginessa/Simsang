namespace Simsang.SessionsExport
{
  partial class SessionExport
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
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SessionExport));
      this.GB_SessionExport = new System.Windows.Forms.GroupBox();
      this.DGV_Sessions = new System.Windows.Forms.DataGridView();
      this.SFD_SessionExport = new System.Windows.Forms.SaveFileDialog();
      this.GB_SessionExport.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.DGV_Sessions)).BeginInit();
      this.SuspendLayout();
      // 
      // GB_SessionExport
      // 
      this.GB_SessionExport.Controls.Add(this.DGV_Sessions);
      this.GB_SessionExport.Location = new System.Drawing.Point(13, 23);
      this.GB_SessionExport.Name = "GB_SessionExport";
      this.GB_SessionExport.Size = new System.Drawing.Size(532, 226);
      this.GB_SessionExport.TabIndex = 0;
      this.GB_SessionExport.TabStop = false;
      this.GB_SessionExport.Text = " Sessions ";
      // 
      // DGV_Sessions
      // 
      this.DGV_Sessions.AllowUserToAddRows = false;
      this.DGV_Sessions.AllowUserToDeleteRows = false;
      this.DGV_Sessions.AllowUserToResizeColumns = false;
      this.DGV_Sessions.AllowUserToResizeRows = false;
      dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.DGV_Sessions.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
      this.DGV_Sessions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.DGV_Sessions.DefaultCellStyle = dataGridViewCellStyle2;
      this.DGV_Sessions.Location = new System.Drawing.Point(15, 26);
      this.DGV_Sessions.MultiSelect = false;
      this.DGV_Sessions.Name = "DGV_Sessions";
      this.DGV_Sessions.ReadOnly = true;
      this.DGV_Sessions.RowHeadersVisible = false;
      this.DGV_Sessions.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.DGV_Sessions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.DGV_Sessions.Size = new System.Drawing.Size(502, 183);
      this.DGV_Sessions.TabIndex = 0;
      this.DGV_Sessions.DoubleClick += new System.EventHandler(this.DGV_Sessions_DoubleClick);
      // 
      // SessionExport
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(557, 263);
      this.Controls.Add(this.GB_SessionExport);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "SessionExport";
      this.Text = " Simsang - Export session";
      this.GB_SessionExport.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.DGV_Sessions)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox GB_SessionExport;
    private System.Windows.Forms.DataGridView dataGridView1;
    private System.Windows.Forms.DataGridView DGV_Sessions;
    private System.Windows.Forms.SaveFileDialog SFD_SessionExport;
  }
}