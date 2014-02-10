namespace Simsang.ARPScan._1_Presentation
{
  partial class SystemFingerprint
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SystemFingerprint));
      this.GB_Fingerprint = new System.Windows.Forms.GroupBox();
      this.SuspendLayout();
      // 
      // GB_Fingerprint
      // 
      this.GB_Fingerprint.Location = new System.Drawing.Point(13, 13);
      this.GB_Fingerprint.Name = "GB_Fingerprint";
      this.GB_Fingerprint.Size = new System.Drawing.Size(519, 239);
      this.GB_Fingerprint.TabIndex = 0;
      this.GB_Fingerprint.TabStop = false;
      // 
      // SystemFingerprint
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(544, 274);
      this.Controls.Add(this.GB_Fingerprint);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "SystemFingerprint";
      this.Text = "System fingerprint";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox GB_Fingerprint;
  }
}