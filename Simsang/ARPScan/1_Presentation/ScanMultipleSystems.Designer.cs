namespace Simsang.ARPScan
{
  partial class ScanMultipleSystems
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScanMultipleSystems));
      this.SS_Fingerprinting = new System.Windows.Forms.StatusStrip();
      this.TSSL_Title = new System.Windows.Forms.ToolStripStatusLabel();
      this.PGB_Systems = new System.Windows.Forms.ProgressBar();
      this.TSSL_CurrentSystem = new System.Windows.Forms.ToolStripStatusLabel();
      this.BGW_Scanner = new System.ComponentModel.BackgroundWorker();
      this.SS_Fingerprinting.SuspendLayout();
      this.SuspendLayout();
      // 
      // SS_Fingerprinting
      // 
      this.SS_Fingerprinting.AutoSize = false;
      this.SS_Fingerprinting.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSSL_Title,
            this.TSSL_CurrentSystem});
      this.SS_Fingerprinting.Location = new System.Drawing.Point(0, 49);
      this.SS_Fingerprinting.Name = "SS_Fingerprinting";
      this.SS_Fingerprinting.Size = new System.Drawing.Size(424, 22);
      this.SS_Fingerprinting.SizingGrip = false;
      this.SS_Fingerprinting.TabIndex = 0;
      // 
      // TSSL_Title
      // 
      this.TSSL_Title.AutoSize = false;
      this.TSSL_Title.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
      this.TSSL_Title.Name = "TSSL_Title";
      this.TSSL_Title.Size = new System.Drawing.Size(100, 17);
      this.TSSL_Title.Text = "TSSL_Title";
      this.TSSL_Title.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // PGB_Systems
      // 
      this.PGB_Systems.Location = new System.Drawing.Point(5, 13);
      this.PGB_Systems.Name = "PGB_Systems";
      this.PGB_Systems.Size = new System.Drawing.Size(410, 23);
      this.PGB_Systems.TabIndex = 1;
      // 
      // TSSL_CurrentSystem
      // 
      this.TSSL_CurrentSystem.AutoSize = false;
      this.TSSL_CurrentSystem.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
      this.TSSL_CurrentSystem.Name = "TSSL_CurrentSystem";
      this.TSSL_CurrentSystem.Size = new System.Drawing.Size(320, 17);
      this.TSSL_CurrentSystem.Text = "TSSL_CurrentSystem";
      this.TSSL_CurrentSystem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // BGW_Scanner
      // 
      this.BGW_Scanner.WorkerSupportsCancellation = true;
      this.BGW_Scanner.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BGW_Scanner_DoWork);
      this.BGW_Scanner.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BGW_Scanner_RunWorkerCompleted);
      // 
      // ScanMultipleSystems
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(424, 71);
      this.Controls.Add(this.PGB_Systems);
      this.Controls.Add(this.SS_Fingerprinting);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ScanMultipleSystems";
      this.Text = " Fingerprinting system ...";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ScanMultipleSystems_FormClosing);
      this.SS_Fingerprinting.ResumeLayout(false);
      this.SS_Fingerprinting.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.StatusStrip SS_Fingerprinting;
    private System.Windows.Forms.ToolStripStatusLabel TSSL_Title;
    private System.Windows.Forms.ToolStripStatusLabel TSSL_CurrentSystem;
    private System.Windows.Forms.ProgressBar PGB_Systems;
    private System.ComponentModel.BackgroundWorker BGW_Scanner;

  }
}