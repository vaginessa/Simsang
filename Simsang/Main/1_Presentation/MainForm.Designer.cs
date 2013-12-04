namespace Simsang
{
    partial class SimsangMain
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (mSnifferProc.Responding)
                    mSnifferProc.Kill();

                if (mPoisoningEngProc.Responding)
                    mPoisoningEngProc.Kill();

            }
            catch (System.Exception)
            {
            }


            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
      this.components = new System.ComponentModel.Container();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimsangMain));
      this.GB_Interfaces = new System.Windows.Forms.GroupBox();
      this.BT_Attack = new System.Windows.Forms.Button();
      this.TB_Session = new System.Windows.Forms.TextBox();
      this.L_Netrange_Separator = new System.Windows.Forms.Label();
      this.BT_ScanLAN = new System.Windows.Forms.Button();
      this.L_Session = new System.Windows.Forms.Label();
      this.CB_Interfaces = new System.Windows.Forms.ComboBox();
      this.TB_StopIP = new System.Windows.Forms.TextBox();
      this.TB_StartIP = new System.Windows.Forms.TextBox();
      this.LAB_Interface = new System.Windows.Forms.Label();
      this.LAB_StartIP = new System.Windows.Forms.Label();
      this.GB_TargetRange = new System.Windows.Forms.GroupBox();
      this.TB_GatewayMAC = new System.Windows.Forms.TextBox();
      this.L_GatewayMAC = new System.Windows.Forms.Label();
      this.TB_Vendor = new System.Windows.Forms.TextBox();
      this.L_VendorTitle = new System.Windows.Forms.Label();
      this.TB_GatewayIP = new System.Windows.Forms.TextBox();
      this.L_GatewayIP = new System.Windows.Forms.Label();
      this.TC_Plugins = new System.Windows.Forms.TabControl();
      this.TP_default = new System.Windows.Forms.TabPage();
      this.L_PCAPVersion = new System.Windows.Forms.Label();
      this.L_SimsangLink = new System.Windows.Forms.LinkLabel();
      this.DGV_MainPlugins = new System.Windows.Forms.DataGridView();
      this.IL_PluginStat = new System.Windows.Forms.ImageList(this.components);
      this.TIM_Ads = new System.Windows.Forms.Timer(this.components);
      this.MS_MainWindow = new System.Windows.Forms.MenuStrip();
      this.TSM_File = new System.Windows.Forms.ToolStripMenuItem();
      this.TSMI_Exit = new System.Windows.Forms.ToolStripMenuItem();
      this.TSM_Sessions = new System.Windows.Forms.ToolStripMenuItem();
      this.TSMI_SaveSession = new System.Windows.Forms.ToolStripMenuItem();
      this.TSMI_ResetPlugins = new System.Windows.Forms.ToolStripMenuItem();
      this.TSMI_ShowSessions = new System.Windows.Forms.ToolStripMenuItem();
      this.TSMI_ImportSessions = new System.Windows.Forms.ToolStripMenuItem();
      this.TSMI_ExportSessions = new System.Windows.Forms.ToolStripMenuItem();
      this.TSMI_Tools = new System.Windows.Forms.ToolStripMenuItem();
      this.TSMI_Minibrowser = new System.Windows.Forms.ToolStripMenuItem();
      this.actionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.TSMI_ResetAllPlugins = new System.Windows.Forms.ToolStripMenuItem();
      this.TSMI_SearchInterfaces = new System.Windows.Forms.ToolStripMenuItem();
      this.TSMI_AttackingMode = new System.Windows.Forms.ToolStripMenuItem();
      this.TSMI_ARPPoisoning = new System.Windows.Forms.ToolStripMenuItem();
      this.TSMI_DHCPPoisoning = new System.Windows.Forms.ToolStripMenuItem();
      this.TSM_Help = new System.Windows.Forms.ToolStripMenuItem();
      this.TSMI_GetUpdates = new System.Windows.Forms.ToolStripMenuItem();
      this.TSMI_Debugging = new System.Windows.Forms.ToolStripMenuItem();
      this.TSMI_LogConsole = new System.Windows.Forms.ToolStripMenuItem();
      this.TSMI_Contribute = new System.Windows.Forms.ToolStripMenuItem();
      this.TIM_Contributions = new System.Windows.Forms.Timer(this.components);
      this.OFD_ImportSession = new System.Windows.Forms.OpenFileDialog();
      this.WB_Ads = new System.Windows.Forms.WebBrowser();
      this.GB_Interfaces.SuspendLayout();
      this.GB_TargetRange.SuspendLayout();
      this.TC_Plugins.SuspendLayout();
      this.TP_default.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.DGV_MainPlugins)).BeginInit();
      this.MS_MainWindow.SuspendLayout();
      this.SuspendLayout();
      // 
      // GB_Interfaces
      // 
      this.GB_Interfaces.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.GB_Interfaces.Controls.Add(this.BT_Attack);
      this.GB_Interfaces.Controls.Add(this.TB_Session);
      this.GB_Interfaces.Controls.Add(this.L_Netrange_Separator);
      this.GB_Interfaces.Controls.Add(this.BT_ScanLAN);
      this.GB_Interfaces.Controls.Add(this.L_Session);
      this.GB_Interfaces.Controls.Add(this.CB_Interfaces);
      this.GB_Interfaces.Controls.Add(this.TB_StopIP);
      this.GB_Interfaces.Controls.Add(this.TB_StartIP);
      this.GB_Interfaces.Controls.Add(this.LAB_Interface);
      this.GB_Interfaces.Controls.Add(this.LAB_StartIP);
      this.GB_Interfaces.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.GB_Interfaces.Location = new System.Drawing.Point(422, 23);
      this.GB_Interfaces.Name = "GB_Interfaces";
      this.GB_Interfaces.Size = new System.Drawing.Size(605, 105);
      this.GB_Interfaces.TabIndex = 2;
      this.GB_Interfaces.TabStop = false;
      // 
      // BT_Attack
      // 
      this.BT_Attack.BackgroundImage = global::Simsang.Properties.Resources.Start;
      this.BT_Attack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
      this.BT_Attack.Location = new System.Drawing.Point(492, 14);
      this.BT_Attack.Margin = new System.Windows.Forms.Padding(0);
      this.BT_Attack.Name = "BT_Attack";
      this.BT_Attack.Size = new System.Drawing.Size(80, 64);
      this.BT_Attack.TabIndex = 6;
      this.BT_Attack.UseVisualStyleBackColor = true;
      this.BT_Attack.Click += new System.EventHandler(this.BT_Attack_Click);
      // 
      // TB_Session
      // 
      this.TB_Session.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.TB_Session.Location = new System.Drawing.Point(98, 72);
      this.TB_Session.Name = "TB_Session";
      this.TB_Session.RightToLeft = System.Windows.Forms.RightToLeft.No;
      this.TB_Session.Size = new System.Drawing.Size(280, 20);
      this.TB_Session.TabIndex = 4;
      // 
      // L_Netrange_Separator
      // 
      this.L_Netrange_Separator.AutoSize = true;
      this.L_Netrange_Separator.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.L_Netrange_Separator.Location = new System.Drawing.Point(235, 49);
      this.L_Netrange_Separator.Name = "L_Netrange_Separator";
      this.L_Netrange_Separator.Size = new System.Drawing.Size(10, 13);
      this.L_Netrange_Separator.TabIndex = 9;
      this.L_Netrange_Separator.Text = "-";
      // 
      // BT_ScanLAN
      // 
      this.BT_ScanLAN.BackgroundImage = global::Simsang.Properties.Resources.Hosts;
      this.BT_ScanLAN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
      this.BT_ScanLAN.Location = new System.Drawing.Point(398, 14);
      this.BT_ScanLAN.Margin = new System.Windows.Forms.Padding(0);
      this.BT_ScanLAN.Name = "BT_ScanLAN";
      this.BT_ScanLAN.Size = new System.Drawing.Size(80, 64);
      this.BT_ScanLAN.TabIndex = 5;
      this.BT_ScanLAN.UseVisualStyleBackColor = true;
      this.BT_ScanLAN.Click += new System.EventHandler(this.BT_ScanLAN_Click);
      // 
      // L_Session
      // 
      this.L_Session.AutoSize = true;
      this.L_Session.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.L_Session.Location = new System.Drawing.Point(9, 75);
      this.L_Session.Name = "L_Session";
      this.L_Session.Size = new System.Drawing.Size(51, 13);
      this.L_Session.TabIndex = 6;
      this.L_Session.Text = "Session";
      // 
      // CB_Interfaces
      // 
      this.CB_Interfaces.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.CB_Interfaces.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.CB_Interfaces.FormattingEnabled = true;
      this.CB_Interfaces.Location = new System.Drawing.Point(98, 17);
      this.CB_Interfaces.Name = "CB_Interfaces";
      this.CB_Interfaces.Size = new System.Drawing.Size(280, 21);
      this.CB_Interfaces.TabIndex = 3;
      this.CB_Interfaces.SelectedIndexChanged += new System.EventHandler(this.CB_Interfaces_SelectedIndexChanged);
      // 
      // TB_StopIP
      // 
      this.TB_StopIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.TB_StopIP.Location = new System.Drawing.Point(251, 45);
      this.TB_StopIP.Name = "TB_StopIP";
      this.TB_StopIP.ReadOnly = true;
      this.TB_StopIP.Size = new System.Drawing.Size(127, 20);
      this.TB_StopIP.TabIndex = 0;
      this.TB_StopIP.TabStop = false;
      // 
      // TB_StartIP
      // 
      this.TB_StartIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.TB_StartIP.Location = new System.Drawing.Point(100, 45);
      this.TB_StartIP.Name = "TB_StartIP";
      this.TB_StartIP.ReadOnly = true;
      this.TB_StartIP.Size = new System.Drawing.Size(129, 20);
      this.TB_StartIP.TabIndex = 0;
      this.TB_StartIP.TabStop = false;
      // 
      // LAB_Interface
      // 
      this.LAB_Interface.AutoSize = true;
      this.LAB_Interface.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.LAB_Interface.Location = new System.Drawing.Point(9, 20);
      this.LAB_Interface.Name = "LAB_Interface";
      this.LAB_Interface.Size = new System.Drawing.Size(58, 13);
      this.LAB_Interface.TabIndex = 0;
      this.LAB_Interface.Text = "Interface";
      // 
      // LAB_StartIP
      // 
      this.LAB_StartIP.AutoSize = true;
      this.LAB_StartIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.LAB_StartIP.Location = new System.Drawing.Point(9, 48);
      this.LAB_StartIP.Name = "LAB_StartIP";
      this.LAB_StartIP.Size = new System.Drawing.Size(63, 13);
      this.LAB_StartIP.TabIndex = 0;
      this.LAB_StartIP.Text = "Net range";
      // 
      // GB_TargetRange
      // 
      this.GB_TargetRange.Controls.Add(this.TB_GatewayMAC);
      this.GB_TargetRange.Controls.Add(this.L_GatewayMAC);
      this.GB_TargetRange.Controls.Add(this.TB_Vendor);
      this.GB_TargetRange.Controls.Add(this.L_VendorTitle);
      this.GB_TargetRange.Controls.Add(this.TB_GatewayIP);
      this.GB_TargetRange.Controls.Add(this.L_GatewayIP);
      this.GB_TargetRange.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.GB_TargetRange.Location = new System.Drawing.Point(12, 23);
      this.GB_TargetRange.Name = "GB_TargetRange";
      this.GB_TargetRange.Size = new System.Drawing.Size(366, 105);
      this.GB_TargetRange.TabIndex = 1;
      this.GB_TargetRange.TabStop = false;
      // 
      // TB_GatewayMAC
      // 
      this.TB_GatewayMAC.Location = new System.Drawing.Point(109, 45);
      this.TB_GatewayMAC.Name = "TB_GatewayMAC";
      this.TB_GatewayMAC.ReadOnly = true;
      this.TB_GatewayMAC.Size = new System.Drawing.Size(240, 20);
      this.TB_GatewayMAC.TabIndex = 13;
      this.TB_GatewayMAC.TabStop = false;
      // 
      // L_GatewayMAC
      // 
      this.L_GatewayMAC.AutoSize = true;
      this.L_GatewayMAC.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.L_GatewayMAC.Location = new System.Drawing.Point(14, 48);
      this.L_GatewayMAC.Name = "L_GatewayMAC";
      this.L_GatewayMAC.Size = new System.Drawing.Size(86, 13);
      this.L_GatewayMAC.TabIndex = 12;
      this.L_GatewayMAC.Text = "Gateway MAC";
      // 
      // TB_Vendor
      // 
      this.TB_Vendor.Location = new System.Drawing.Point(109, 72);
      this.TB_Vendor.Name = "TB_Vendor";
      this.TB_Vendor.ReadOnly = true;
      this.TB_Vendor.Size = new System.Drawing.Size(240, 20);
      this.TB_Vendor.TabIndex = 11;
      this.TB_Vendor.TabStop = false;
      // 
      // L_VendorTitle
      // 
      this.L_VendorTitle.AutoSize = true;
      this.L_VendorTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.L_VendorTitle.Location = new System.Drawing.Point(14, 75);
      this.L_VendorTitle.Name = "L_VendorTitle";
      this.L_VendorTitle.Size = new System.Drawing.Size(47, 13);
      this.L_VendorTitle.TabIndex = 10;
      this.L_VendorTitle.Text = "Vendor";
      // 
      // TB_GatewayIP
      // 
      this.TB_GatewayIP.Location = new System.Drawing.Point(109, 18);
      this.TB_GatewayIP.Name = "TB_GatewayIP";
      this.TB_GatewayIP.ReadOnly = true;
      this.TB_GatewayIP.Size = new System.Drawing.Size(240, 20);
      this.TB_GatewayIP.TabIndex = 0;
      this.TB_GatewayIP.TabStop = false;
      // 
      // L_GatewayIP
      // 
      this.L_GatewayIP.AutoSize = true;
      this.L_GatewayIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.L_GatewayIP.Location = new System.Drawing.Point(14, 20);
      this.L_GatewayIP.Name = "L_GatewayIP";
      this.L_GatewayIP.Size = new System.Drawing.Size(72, 13);
      this.L_GatewayIP.TabIndex = 0;
      this.L_GatewayIP.Text = "Gateway IP";
      // 
      // TC_Plugins
      // 
      this.TC_Plugins.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.TC_Plugins.Controls.Add(this.TP_default);
      this.TC_Plugins.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.TC_Plugins.ImageList = this.IL_PluginStat;
      this.TC_Plugins.ItemSize = new System.Drawing.Size(79, 19);
      this.TC_Plugins.Location = new System.Drawing.Point(12, 147);
      this.TC_Plugins.Multiline = true;
      this.TC_Plugins.Name = "TC_Plugins";
      this.TC_Plugins.SelectedIndex = 0;
      this.TC_Plugins.Size = new System.Drawing.Size(1020, 416);
      this.TC_Plugins.TabIndex = 7;
      // 
      // TP_default
      // 
      this.TP_default.BackColor = System.Drawing.Color.White;
      this.TP_default.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
      this.TP_default.Controls.Add(this.L_PCAPVersion);
      this.TP_default.Controls.Add(this.L_SimsangLink);
      this.TP_default.Controls.Add(this.DGV_MainPlugins);
      this.TP_default.Location = new System.Drawing.Point(4, 23);
      this.TP_default.Name = "TP_default";
      this.TP_default.Padding = new System.Windows.Forms.Padding(3);
      this.TP_default.Size = new System.Drawing.Size(1012, 389);
      this.TP_default.TabIndex = 0;
      this.TP_default.Text = "Simsang";
      // 
      // L_PCAPVersion
      // 
      this.L_PCAPVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.L_PCAPVersion.AutoSize = true;
      this.L_PCAPVersion.Location = new System.Drawing.Point(907, 368);
      this.L_PCAPVersion.Name = "L_PCAPVersion";
      this.L_PCAPVersion.Size = new System.Drawing.Size(98, 13);
      this.L_PCAPVersion.TabIndex = 2;
      this.L_PCAPVersion.Text = "PCAP VERSION";
      // 
      // L_SimsangLink
      // 
      this.L_SimsangLink.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.L_SimsangLink.AutoSize = true;
      this.L_SimsangLink.Location = new System.Drawing.Point(43, 369);
      this.L_SimsangLink.Name = "L_SimsangLink";
      this.L_SimsangLink.Size = new System.Drawing.Size(127, 13);
      this.L_SimsangLink.TabIndex = 1;
      this.L_SimsangLink.TabStop = true;
      this.L_SimsangLink.Text = "http://www.buglist.io";
      this.L_SimsangLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.L_MPLink_LinkClicked);
      // 
      // DGV_MainPlugins
      // 
      this.DGV_MainPlugins.AllowUserToAddRows = false;
      this.DGV_MainPlugins.AllowUserToDeleteRows = false;
      this.DGV_MainPlugins.AllowUserToResizeColumns = false;
      this.DGV_MainPlugins.AllowUserToResizeRows = false;
      this.DGV_MainPlugins.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.DGV_MainPlugins.BackgroundColor = System.Drawing.Color.White;
      this.DGV_MainPlugins.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.DGV_MainPlugins.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
      this.DGV_MainPlugins.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
      dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
      dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
      dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White;
      dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.DGV_MainPlugins.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
      this.DGV_MainPlugins.ColumnHeadersHeight = 25;
      this.DGV_MainPlugins.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
      dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.DGV_MainPlugins.DefaultCellStyle = dataGridViewCellStyle2;
      this.DGV_MainPlugins.EnableHeadersVisualStyles = false;
      this.DGV_MainPlugins.GridColor = System.Drawing.Color.White;
      this.DGV_MainPlugins.Location = new System.Drawing.Point(46, 20);
      this.DGV_MainPlugins.MultiSelect = false;
      this.DGV_MainPlugins.Name = "DGV_MainPlugins";
      this.DGV_MainPlugins.ReadOnly = true;
      dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.DGV_MainPlugins.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
      this.DGV_MainPlugins.RowHeadersVisible = false;
      dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.DGV_MainPlugins.RowsDefaultCellStyle = dataGridViewCellStyle4;
      this.DGV_MainPlugins.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.DGV_MainPlugins.RowTemplate.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
      this.DGV_MainPlugins.RowTemplate.ReadOnly = true;
      this.DGV_MainPlugins.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
      this.DGV_MainPlugins.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.DGV_MainPlugins.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.DGV_MainPlugins.Size = new System.Drawing.Size(944, 339);
      this.DGV_MainPlugins.TabIndex = 0;
      this.DGV_MainPlugins.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_MainPlugins_CellContentClick);
      this.DGV_MainPlugins.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.DGV_MainPlugins_DataError);
      // 
      // IL_PluginStat
      // 
      this.IL_PluginStat.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("IL_PluginStat.ImageStream")));
      this.IL_PluginStat.TransparentColor = System.Drawing.Color.Transparent;
      this.IL_PluginStat.Images.SetKeyName(0, "green");
      this.IL_PluginStat.Images.SetKeyName(1, "red");
      this.IL_PluginStat.Images.SetKeyName(2, "grey");
      // 
      // TIM_Ads
      // 
      this.TIM_Ads.Interval = 5000;
      // 
      // MS_MainWindow
      // 
      this.MS_MainWindow.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSM_File,
            this.TSM_Sessions,
            this.TSMI_Tools,
            this.actionToolStripMenuItem,
            this.TSM_Help});
      this.MS_MainWindow.Location = new System.Drawing.Point(0, 0);
      this.MS_MainWindow.Name = "MS_MainWindow";
      this.MS_MainWindow.Size = new System.Drawing.Size(1048, 24);
      this.MS_MainWindow.TabIndex = 0;
      this.MS_MainWindow.Text = "menuStrip1";
      // 
      // TSM_File
      // 
      this.TSM_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_Exit});
      this.TSM_File.Name = "TSM_File";
      this.TSM_File.Size = new System.Drawing.Size(37, 20);
      this.TSM_File.Text = "File";
      // 
      // TSMI_Exit
      // 
      this.TSMI_Exit.Name = "TSMI_Exit";
      this.TSMI_Exit.Size = new System.Drawing.Size(92, 22);
      this.TSMI_Exit.Text = "Exit";
      this.TSMI_Exit.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
      // 
      // TSM_Sessions
      // 
      this.TSM_Sessions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_SaveSession,
            this.TSMI_ResetPlugins,
            this.TSMI_ShowSessions,
            this.TSMI_ImportSessions,
            this.TSMI_ExportSessions});
      this.TSM_Sessions.Name = "TSM_Sessions";
      this.TSM_Sessions.Size = new System.Drawing.Size(63, 20);
      this.TSM_Sessions.Text = "Sessions";
      // 
      // TSMI_SaveSession
      // 
      this.TSMI_SaveSession.Name = "TSMI_SaveSession";
      this.TSMI_SaveSession.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
      this.TSMI_SaveSession.Size = new System.Drawing.Size(201, 22);
      this.TSMI_SaveSession.Text = "Save session";
      this.TSMI_SaveSession.Click += new System.EventHandler(this.saveSessionToolStripMenuItem_Click);
      // 
      // TSMI_ResetPlugins
      // 
      this.TSMI_ResetPlugins.Name = "TSMI_ResetPlugins";
      this.TSMI_ResetPlugins.Size = new System.Drawing.Size(201, 22);
      this.TSMI_ResetPlugins.Text = "Reset session";
      this.TSMI_ResetPlugins.Click += new System.EventHandler(this.resetPluginsToolStripMenuItem_Click);
      // 
      // TSMI_ShowSessions
      // 
      this.TSMI_ShowSessions.Name = "TSMI_ShowSessions";
      this.TSMI_ShowSessions.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
      this.TSMI_ShowSessions.Size = new System.Drawing.Size(201, 22);
      this.TSMI_ShowSessions.Text = "Load sessions ...";
      this.TSMI_ShowSessions.Click += new System.EventHandler(this.listToolStripMenuItem_Click);
      // 
      // TSMI_ImportSessions
      // 
      this.TSMI_ImportSessions.Name = "TSMI_ImportSessions";
      this.TSMI_ImportSessions.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
      this.TSMI_ImportSessions.Size = new System.Drawing.Size(201, 22);
      this.TSMI_ImportSessions.Text = "Import ...";
      this.TSMI_ImportSessions.Click += new System.EventHandler(this.sessionImportToolStripMenuItem_Click);
      // 
      // TSMI_ExportSessions
      // 
      this.TSMI_ExportSessions.Name = "TSMI_ExportSessions";
      this.TSMI_ExportSessions.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
      this.TSMI_ExportSessions.Size = new System.Drawing.Size(201, 22);
      this.TSMI_ExportSessions.Text = "Export ...";
      this.TSMI_ExportSessions.Click += new System.EventHandler(this.sessionExportToolStripMenuItem_Click);
      // 
      // TSMI_Tools
      // 
      this.TSMI_Tools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_Minibrowser});
      this.TSMI_Tools.Name = "TSMI_Tools";
      this.TSMI_Tools.Size = new System.Drawing.Size(48, 20);
      this.TSMI_Tools.Text = "Tools";
      // 
      // TSMI_Minibrowser
      // 
      this.TSMI_Minibrowser.Name = "TSMI_Minibrowser";
      this.TSMI_Minibrowser.Size = new System.Drawing.Size(152, 22);
      this.TSMI_Minibrowser.Text = "Minibrowser ...";
      this.TSMI_Minibrowser.Click += new System.EventHandler(this.TSMI_Minibrowser_Click);
      // 
      // actionToolStripMenuItem
      // 
      this.actionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_ResetAllPlugins,
            this.TSMI_SearchInterfaces,
            this.TSMI_AttackingMode});
      this.actionToolStripMenuItem.Name = "actionToolStripMenuItem";
      this.actionToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
      this.actionToolStripMenuItem.Text = "Action";
      // 
      // TSMI_ResetAllPlugins
      // 
      this.TSMI_ResetAllPlugins.Name = "TSMI_ResetAllPlugins";
      this.TSMI_ResetAllPlugins.Size = new System.Drawing.Size(209, 22);
      this.TSMI_ResetAllPlugins.Text = "Reset plugins";
      this.TSMI_ResetAllPlugins.Click += new System.EventHandler(this.initAllPluginsToolStripMenuItem_Click);
      // 
      // TSMI_SearchInterfaces
      // 
      this.TSMI_SearchInterfaces.Name = "TSMI_SearchInterfaces";
      this.TSMI_SearchInterfaces.Size = new System.Drawing.Size(209, 22);
      this.TSMI_SearchInterfaces.Text = "Search network interfaces";
      this.TSMI_SearchInterfaces.Click += new System.EventHandler(this.searchInterfacesToolStripMenuItem_Click);
      // 
      // TSMI_AttackingMode
      // 
      this.TSMI_AttackingMode.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_ARPPoisoning,
            this.TSMI_DHCPPoisoning});
      this.TSMI_AttackingMode.Name = "TSMI_AttackingMode";
      this.TSMI_AttackingMode.Size = new System.Drawing.Size(209, 22);
      this.TSMI_AttackingMode.Text = "Attacking mode";
      // 
      // TSMI_ARPPoisoning
      // 
      this.TSMI_ARPPoisoning.Checked = true;
      this.TSMI_ARPPoisoning.CheckOnClick = true;
      this.TSMI_ARPPoisoning.CheckState = System.Windows.Forms.CheckState.Checked;
      this.TSMI_ARPPoisoning.Name = "TSMI_ARPPoisoning";
      this.TSMI_ARPPoisoning.Size = new System.Drawing.Size(162, 22);
      this.TSMI_ARPPoisoning.Text = "ARP poisoning";
      this.TSMI_ARPPoisoning.Click += new System.EventHandler(this.TSMI_ARPPoisoning_Click);
      // 
      // TSMI_DHCPPoisoning
      // 
      this.TSMI_DHCPPoisoning.Name = "TSMI_DHCPPoisoning";
      this.TSMI_DHCPPoisoning.Size = new System.Drawing.Size(162, 22);
      this.TSMI_DHCPPoisoning.Text = "DHCP poisoning";
      this.TSMI_DHCPPoisoning.Click += new System.EventHandler(this.TSMI_DHCPPoisoning_Click);
      // 
      // TSM_Help
      // 
      this.TSM_Help.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_GetUpdates,
            this.TSMI_Debugging,
            this.TSMI_LogConsole,
            this.TSMI_Contribute});
      this.TSM_Help.Name = "TSM_Help";
      this.TSM_Help.Size = new System.Drawing.Size(44, 20);
      this.TSM_Help.Text = "Help";
      // 
      // TSMI_GetUpdates
      // 
      this.TSMI_GetUpdates.Name = "TSMI_GetUpdates";
      this.TSMI_GetUpdates.Size = new System.Drawing.Size(190, 22);
      this.TSMI_GetUpdates.Text = "Check for updates ...";
      this.TSMI_GetUpdates.Click += new System.EventHandler(this.getUpdatesToolStripMenuItem_Click);
      // 
      // TSMI_Debugging
      // 
      this.TSMI_Debugging.Name = "TSMI_Debugging";
      this.TSMI_Debugging.Size = new System.Drawing.Size(190, 22);
      this.TSMI_Debugging.Text = "Debuggin (is off)";
      this.TSMI_Debugging.Click += new System.EventHandler(this.debugginOnToolStripMenuItem_Click);
      // 
      // TSMI_LogConsole
      // 
      this.TSMI_LogConsole.Name = "TSMI_LogConsole";
      this.TSMI_LogConsole.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
      this.TSMI_LogConsole.Size = new System.Drawing.Size(190, 22);
      this.TSMI_LogConsole.Text = "Log console ...";
      this.TSMI_LogConsole.Click += new System.EventHandler(this.logConsoleToolStripMenuItem_Click);
      // 
      // TSMI_Contribute
      // 
      this.TSMI_Contribute.Name = "TSMI_Contribute";
      this.TSMI_Contribute.Size = new System.Drawing.Size(190, 22);
      this.TSMI_Contribute.Text = "Contribute ...";
      this.TSMI_Contribute.Click += new System.EventHandler(this.TSMI_Contribute_Click);
      // 
      // TIM_Contributions
      // 
      this.TIM_Contributions.Interval = 10000;
      this.TIM_Contributions.Tick += new System.EventHandler(this.TIM_Contributions_Tick);
      // 
      // OFD_ImportSession
      // 
      this.OFD_ImportSession.DefaultExt = "sim";
      this.OFD_ImportSession.Filter = "Simsang session file | *.sim";
      this.OFD_ImportSession.Title = "Select Simsang session file";
      // 
      // WB_Ads
      // 
      this.WB_Ads.AllowNavigation = false;
      this.WB_Ads.AllowWebBrowserDrop = false;
      this.WB_Ads.IsWebBrowserContextMenuEnabled = false;
      this.WB_Ads.Location = new System.Drawing.Point(16, 550);
      this.WB_Ads.MinimumSize = new System.Drawing.Size(20, 20);
      this.WB_Ads.Name = "WB_Ads";
      this.WB_Ads.ScriptErrorsSuppressed = true;
      this.WB_Ads.ScrollBarsEnabled = false;
      this.WB_Ads.Size = new System.Drawing.Size(629, 80);
      this.WB_Ads.TabIndex = 3;
      this.WB_Ads.TabStop = false;
      this.WB_Ads.Url = new System.Uri("http://www.megapanzer.com/proxy/banner.php", System.UriKind.Absolute);
      this.WB_Ads.Visible = false;
      this.WB_Ads.WebBrowserShortcutsEnabled = false;
      // 
      // SimsangMain
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1048, 589);
      this.Controls.Add(this.WB_Ads);
      this.Controls.Add(this.TC_Plugins);
      this.Controls.Add(this.GB_TargetRange);
      this.Controls.Add(this.GB_Interfaces);
      this.Controls.Add(this.MS_MainWindow);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MainMenuStrip = this.MS_MainWindow;
      this.MaximizeBox = false;
      this.MinimumSize = new System.Drawing.Size(723, 578);
      this.Name = "SimsangMain";
      this.Text = " Simsang ";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ACMain_FormClosing);
      this.GB_Interfaces.ResumeLayout(false);
      this.GB_Interfaces.PerformLayout();
      this.GB_TargetRange.ResumeLayout(false);
      this.GB_TargetRange.PerformLayout();
      this.TC_Plugins.ResumeLayout(false);
      this.TP_default.ResumeLayout(false);
      this.TP_default.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.DGV_MainPlugins)).EndInit();
      this.MS_MainWindow.ResumeLayout(false);
      this.MS_MainWindow.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox GB_Interfaces;
        private System.Windows.Forms.ComboBox CB_Interfaces;
        private System.Windows.Forms.Label LAB_Interface;
        private System.Windows.Forms.GroupBox GB_TargetRange;
        private System.Windows.Forms.TextBox TB_StopIP;
        private System.Windows.Forms.TextBox TB_StartIP;
        private System.Windows.Forms.Label LAB_StartIP;
        private System.Windows.Forms.TabControl TC_Plugins;
        private System.Windows.Forms.TabPage TP_default;
        private System.Windows.Forms.WebBrowser WB_Ads;
        private System.Windows.Forms.Timer TIM_Ads;
        private System.Windows.Forms.DataGridView DGV_MainPlugins;
        private System.Windows.Forms.LinkLabel L_SimsangLink;
        private System.Windows.Forms.MenuStrip MS_MainWindow;
        private System.Windows.Forms.ToolStripMenuItem TSM_File;
        private System.Windows.Forms.ToolStripMenuItem TSMI_Exit;
        private System.Windows.Forms.ToolStripMenuItem TSM_Help;
        private System.Windows.Forms.ToolStripMenuItem TSMI_GetUpdates;
        private System.Windows.Forms.TextBox TB_Session;
        private System.Windows.Forms.Label L_Session;
        private System.Windows.Forms.ToolStripMenuItem TSMI_Debugging;
        private System.Windows.Forms.Label L_PCAPVersion;
        private System.Windows.Forms.ToolStripMenuItem TSMI_LogConsole;
        private System.Windows.Forms.ToolStripMenuItem TSM_Sessions;
        private System.Windows.Forms.ToolStripMenuItem TSMI_SaveSession;
        private System.Windows.Forms.ToolStripMenuItem TSMI_ResetPlugins;
        private System.Windows.Forms.Button BT_ScanLAN;
        private System.Windows.Forms.Label L_GatewayIP;
        private System.Windows.Forms.TextBox TB_GatewayIP;
        private System.Windows.Forms.Button BT_Attack;
        private System.Windows.Forms.ToolStripMenuItem TSMI_Contribute;
        private System.Windows.Forms.Label L_Netrange_Separator;
        private System.Windows.Forms.ImageList IL_PluginStat;
        private System.Windows.Forms.Timer TIM_Contributions;
        private System.Windows.Forms.TextBox TB_Vendor;
        private System.Windows.Forms.Label L_VendorTitle;
        private System.Windows.Forms.ToolStripMenuItem TSMI_Tools;
        private System.Windows.Forms.ToolStripMenuItem TSMI_Minibrowser;
        private System.Windows.Forms.ToolStripMenuItem actionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem TSMI_ResetAllPlugins;
        private System.Windows.Forms.ToolStripMenuItem TSMI_SearchInterfaces;
        private System.Windows.Forms.TextBox TB_GatewayMAC;
        private System.Windows.Forms.Label L_GatewayMAC;
        private System.Windows.Forms.ToolStripMenuItem TSMI_ShowSessions;
        private System.Windows.Forms.ToolStripMenuItem TSMI_ImportSessions;
        private System.Windows.Forms.ToolStripMenuItem TSMI_ExportSessions;
        private System.Windows.Forms.OpenFileDialog OFD_ImportSession;
        private System.Windows.Forms.ToolStripMenuItem TSMI_AttackingMode;
        private System.Windows.Forms.ToolStripMenuItem TSMI_ARPPoisoning;
        private System.Windows.Forms.ToolStripMenuItem TSMI_DHCPPoisoning;
    }
}

