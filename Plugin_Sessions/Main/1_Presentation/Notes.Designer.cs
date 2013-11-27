namespace Plugin
{
  partial class Main_Notes
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main_Notes));
      this.TB_Data = new System.Windows.Forms.RichTextBox();
      this.SuspendLayout();
      // 
      // TB_Data
      // 
      this.TB_Data.Dock = System.Windows.Forms.DockStyle.Fill;
      this.TB_Data.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.TB_Data.Location = new System.Drawing.Point(0, 0);
      this.TB_Data.Name = "TB_Data";
      this.TB_Data.Size = new System.Drawing.Size(856, 262);
      this.TB_Data.TabIndex = 0;
      this.TB_Data.Text = "";
      // 
      // Main_Notes
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(856, 262);
      this.Controls.Add(this.TB_Data);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "Main_Notes";
      this.Text = "Notes";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.RichTextBox TB_Data;

  }
}