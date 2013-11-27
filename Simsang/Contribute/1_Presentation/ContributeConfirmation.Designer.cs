namespace Simsang.Contribute.Main
{
  partial class ContributeConfirmation
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContributeConfirmation));
      this.BT_Continue = new System.Windows.Forms.Button();
      this.CHKB_Agree = new System.Windows.Forms.CheckBox();
      this.RTB_Agreement = new System.Windows.Forms.RichTextBox();
      this.panel1 = new System.Windows.Forms.Panel();
      this.GB_Confirmation = new System.Windows.Forms.GroupBox();
      this.CKB_ContribSSID = new System.Windows.Forms.CheckBox();
      this.panel1.SuspendLayout();
      this.GB_Confirmation.SuspendLayout();
      this.SuspendLayout();
      // 
      // BT_Continue
      // 
      this.BT_Continue.Location = new System.Drawing.Point(348, 328);
      this.BT_Continue.Name = "BT_Continue";
      this.BT_Continue.Size = new System.Drawing.Size(75, 23);
      this.BT_Continue.TabIndex = 0;
      this.BT_Continue.Text = "Continue";
      this.BT_Continue.UseVisualStyleBackColor = true;
      this.BT_Continue.Click += new System.EventHandler(this.BT_OK_Click);
      // 
      // CHKB_Agree
      // 
      this.CHKB_Agree.AutoSize = true;
      this.CHKB_Agree.Checked = true;
      this.CHKB_Agree.CheckState = System.Windows.Forms.CheckState.Checked;
      this.CHKB_Agree.Location = new System.Drawing.Point(192, 332);
      this.CHKB_Agree.Name = "CHKB_Agree";
      this.CHKB_Agree.Size = new System.Drawing.Size(136, 17);
      this.CHKB_Agree.TabIndex = 1;
      this.CHKB_Agree.Text = "I understand and agree";
      this.CHKB_Agree.UseVisualStyleBackColor = true;
      this.CHKB_Agree.CheckedChanged += new System.EventHandler(this.CHKB_Agree_CheckedChanged);
      // 
      // RTB_Agreement
      // 
      this.RTB_Agreement.BackColor = System.Drawing.Color.White;
      this.RTB_Agreement.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.RTB_Agreement.HideSelection = false;
      this.RTB_Agreement.Location = new System.Drawing.Point(9, 0);
      this.RTB_Agreement.Margin = new System.Windows.Forms.Padding(0);
      this.RTB_Agreement.Name = "RTB_Agreement";
      this.RTB_Agreement.ReadOnly = true;
      this.RTB_Agreement.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
      this.RTB_Agreement.Size = new System.Drawing.Size(400, 272);
      this.RTB_Agreement.TabIndex = 2;
      this.RTB_Agreement.TabStop = false;
      this.RTB_Agreement.Text = "";
      // 
      // panel1
      // 
      this.panel1.BackColor = System.Drawing.Color.White;
      this.panel1.Controls.Add(this.RTB_Agreement);
      this.panel1.Location = new System.Drawing.Point(17, 19);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(409, 272);
      this.panel1.TabIndex = 3;
      // 
      // GB_Confirmation
      // 
      this.GB_Confirmation.Controls.Add(this.panel1);
      this.GB_Confirmation.Location = new System.Drawing.Point(16, 6);
      this.GB_Confirmation.Name = "GB_Confirmation";
      this.GB_Confirmation.Size = new System.Drawing.Size(442, 309);
      this.GB_Confirmation.TabIndex = 4;
      this.GB_Confirmation.TabStop = false;
      // 
      // CKB_ContribSSID
      // 
      this.CKB_ContribSSID.AutoSize = true;
      this.CKB_ContribSSID.Checked = true;
      this.CKB_ContribSSID.CheckState = System.Windows.Forms.CheckState.Checked;
      this.CKB_ContribSSID.Location = new System.Drawing.Point(33, 332);
      this.CKB_ContribSSID.Name = "CKB_ContribSSID";
      this.CKB_ContribSSID.Size = new System.Drawing.Size(102, 17);
      this.CKB_ContribSSID.TabIndex = 4;
      this.CKB_ContribSSID.Text = "Contribute SSID";
      this.CKB_ContribSSID.UseVisualStyleBackColor = true;
      // 
      // ContributeConfirmation
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(476, 368);
      this.Controls.Add(this.CKB_ContribSSID);
      this.Controls.Add(this.CHKB_Agree);
      this.Controls.Add(this.GB_Confirmation);
      this.Controls.Add(this.BT_Continue);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ContributeConfirmation";
      this.Text = " User confirmation";
      this.panel1.ResumeLayout(false);
      this.GB_Confirmation.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button BT_Continue;
    private System.Windows.Forms.CheckBox CHKB_Agree;
    private System.Windows.Forms.RichTextBox RTB_Agreement;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.GroupBox GB_Confirmation;
    private System.Windows.Forms.CheckBox CKB_ContribSSID;
  }
}