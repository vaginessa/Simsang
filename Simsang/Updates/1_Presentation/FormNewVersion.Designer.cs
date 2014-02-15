namespace Simsang.Updates
{
  partial class FormNewVersion
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormNewVersion));
      this.BT_Close = new System.Windows.Forms.Button();
      this.P_UpdateMsg = new System.Windows.Forms.Panel();
      this.RTB_SimsangUpdate = new System.Windows.Forms.RichTextBox();
      this.LL_DownloadURL = new System.Windows.Forms.LinkLabel();
      this.P_UpdateMsg.SuspendLayout();
      this.SuspendLayout();
      // 
      // BT_Close
      // 
      this.BT_Close.Location = new System.Drawing.Point(471, 226);
      this.BT_Close.Name = "BT_Close";
      this.BT_Close.Size = new System.Drawing.Size(75, 23);
      this.BT_Close.TabIndex = 0;
      this.BT_Close.Text = "Close";
      this.BT_Close.UseVisualStyleBackColor = true;
      this.BT_Close.Click += new System.EventHandler(this.BT_Close_Click);
      // 
      // P_UpdateMsg
      // 
      this.P_UpdateMsg.BackColor = System.Drawing.SystemColors.ButtonHighlight;
      this.P_UpdateMsg.Controls.Add(this.RTB_SimsangUpdate);
      this.P_UpdateMsg.Controls.Add(this.LL_DownloadURL);
      this.P_UpdateMsg.Location = new System.Drawing.Point(8, 8);
      this.P_UpdateMsg.Name = "P_UpdateMsg";
      this.P_UpdateMsg.Size = new System.Drawing.Size(554, 212);
      this.P_UpdateMsg.TabIndex = 1;
      // 
      // RTB_SimsangUpdate
      // 
      this.RTB_SimsangUpdate.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.RTB_SimsangUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.RTB_SimsangUpdate.Location = new System.Drawing.Point(9, 10);
      this.RTB_SimsangUpdate.Name = "RTB_SimsangUpdate";
      this.RTB_SimsangUpdate.Size = new System.Drawing.Size(529, 179);
      this.RTB_SimsangUpdate.TabIndex = 1;
      this.RTB_SimsangUpdate.Text = "";
      // 
      // LL_DownloadURL
      // 
      this.LL_DownloadURL.AutoSize = true;
      this.LL_DownloadURL.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.LL_DownloadURL.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
      this.LL_DownloadURL.Location = new System.Drawing.Point(10, 192);
      this.LL_DownloadURL.Name = "LL_DownloadURL";
      this.LL_DownloadURL.Size = new System.Drawing.Size(124, 16);
      this.LL_DownloadURL.TabIndex = 0;
      this.LL_DownloadURL.TabStop = true;
      this.LL_DownloadURL.Tag = "";
      this.LL_DownloadURL.Text = "Simsang web page";
      this.LL_DownloadURL.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LL_DownloadURL_LinkClicked);
      // 
      // FormNewVersion
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(574, 258);
      this.Controls.Add(this.P_UpdateMsg);
      this.Controls.Add(this.BT_Close);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "FormNewVersion";
      this.Text = " New version available ...";
      this.P_UpdateMsg.ResumeLayout(false);
      this.P_UpdateMsg.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button BT_Close;
    private System.Windows.Forms.Panel P_UpdateMsg;
    private System.Windows.Forms.LinkLabel LL_DownloadURL;
    private System.Windows.Forms.RichTextBox RTB_SimsangUpdate;
  }
}