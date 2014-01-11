namespace Simsang.Session
{
    partial class Sessions
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
      this.components = new System.ComponentModel.Container();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Sessions));
      this.GB_Session = new System.Windows.Forms.GroupBox();
      this.DGV_Sessions = new System.Windows.Forms.DataGridView();
      this.CMS_SessionMgmt = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.loadSessionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.removeSessionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.GB_Session.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.DGV_Sessions)).BeginInit();
      this.CMS_SessionMgmt.SuspendLayout();
      this.SuspendLayout();
      // 
      // GB_Session
      // 
      this.GB_Session.Controls.Add(this.DGV_Sessions);
      this.GB_Session.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.GB_Session.Location = new System.Drawing.Point(11, 8);
      this.GB_Session.Name = "GB_Session";
      this.GB_Session.Size = new System.Drawing.Size(532, 243);
      this.GB_Session.TabIndex = 0;
      this.GB_Session.TabStop = false;
      this.GB_Session.Text = " Sessions ";
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
      this.DGV_Sessions.Location = new System.Drawing.Point(15, 23);
      this.DGV_Sessions.MultiSelect = false;
      this.DGV_Sessions.Name = "DGV_Sessions";
      this.DGV_Sessions.ReadOnly = true;
      this.DGV_Sessions.RowHeadersVisible = false;
      dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.DGV_Sessions.RowsDefaultCellStyle = dataGridViewCellStyle2;
      this.DGV_Sessions.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.DGV_Sessions.RowTemplate.Height = 20;
      this.DGV_Sessions.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.DGV_Sessions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.DGV_Sessions.Size = new System.Drawing.Size(502, 206);
      this.DGV_Sessions.TabIndex = 0;
      this.DGV_Sessions.DoubleClick += new System.EventHandler(this.DGV_Sessions_DoubleClick);
      this.DGV_Sessions.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DGV_Session_MouseUp);
      // 
      // CMS_SessionMgmt
      // 
      this.CMS_SessionMgmt.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadSessionToolStripMenuItem,
            this.removeSessionToolStripMenuItem});
      this.CMS_SessionMgmt.Name = "CMS_SessionMgmt";
      this.CMS_SessionMgmt.Size = new System.Drawing.Size(159, 48);
      // 
      // loadSessionToolStripMenuItem
      // 
      this.loadSessionToolStripMenuItem.Name = "loadSessionToolStripMenuItem";
      this.loadSessionToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
      this.loadSessionToolStripMenuItem.Text = "Load session";
      this.loadSessionToolStripMenuItem.Click += new System.EventHandler(this.TSMI_Load_Click);
      // 
      // removeSessionToolStripMenuItem
      // 
      this.removeSessionToolStripMenuItem.Name = "removeSessionToolStripMenuItem";
      this.removeSessionToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
      this.removeSessionToolStripMenuItem.Text = "Remove session";
      this.removeSessionToolStripMenuItem.Click += new System.EventHandler(this.TSMI_Remove_Click);
      // 
      // Sessions
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(557, 263);
      this.Controls.Add(this.GB_Session);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "Sessions";
      this.Text = " Simsang - Load session";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Sessions_FormClosing);
      this.GB_Session.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.DGV_Sessions)).EndInit();
      this.CMS_SessionMgmt.ResumeLayout(false);
      this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GB_Session;
        private System.Windows.Forms.DataGridView DGV_Sessions;
        private System.Windows.Forms.ContextMenuStrip CMS_SessionMgmt;
        private System.Windows.Forms.ToolStripMenuItem loadSessionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeSessionToolStripMenuItem;
    }
}