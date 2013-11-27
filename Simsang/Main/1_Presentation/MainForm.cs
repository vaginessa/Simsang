using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Net.Sockets;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using System.IO.Pipes;
using System.Net;
using System.Collections;
using System.Xml;
using System.Xml.Linq;

using SHDocVw;  // "add reference" -> "browse" -> "c:\windows\system32\shdocvw.dll"

using Simsang.Plugin;
using Simsang.ARPScan.Main;
using Simsang.Contribute.Infrastructure;
using Simsang.Contribute.Main;
using Simsang.MACVendors;
using Simsang.MiniBrowser;
using Simsang.LogConsole.Main;
using Simsang.SessionsExport;


namespace Simsang
{

  public partial class ACMain : Form
  {

    #region MEMBERS

    private Process mSnifferProc;
    private Process mPoisoningEngProc;
    private NetworkInterface[] mNCards;
    private BindingList<UsedPlugins> mUsedPlugins;
    private BindingList<String> mTargetList;
    private TabPageHandler mTPHandler;
    private InputModule mInputModule;
    private PluginModule mPluginModule;
    private AttackSession mCurrentSession;
    private String mCurrentIPAddress = String.Empty;
    private Browser cMiniBrowser;
    private bool mAttackStarted;

    #endregion


    #region PROPERTIES

    public String GetInterface()
    {
      String lRetVal = String.Empty;

      try { lRetVal = Config.GetIDByIndex(CB_Interfaces.SelectedIndex); }
      catch (Exception) { }
      return (lRetVal);
    }

    public string GetCurrentIP() { return (mCurrentIPAddress); }
    public string GetCurrentGWIP() { return (TB_GatewayIP.Text); }
    public String GetStartIP() { return (TB_StartIP.Text); }
    public void SetStartIP(String pStartIP) { TB_StartIP.Text = pStartIP; }
    public String GetStopIP() { return (TB_StopIP.Text); }
    public void SetStopIP(String pStopIP) { TB_StopIP.Text = pStopIP; }
    public String GetSessionName() { return (TB_Session.Text); }
    public void SetSessionName(String pSessionName) { TB_Session.Text = pSessionName; }

    public TabPageHandler GetTabPageHandler { get { return (mTPHandler); } }
    public BindingList<UsedPlugins> UsedPlugins { get { return (mUsedPlugins); } }

    public LogConsole.Main.LogConsole GetLogConsole { get { return (LogConsole.Main.LogConsole.getInstance()); } }
    public PluginModule GetPluginModule { get { return (mPluginModule); } }

    public PluginModule PluginsModule { get { return (mPluginModule); } }

    #endregion


    #region PUBLIC

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pPluginName"></param>
    /// <returns></returns>
    public IPlugin GetPluginByName(String pPluginName)
    {
      IPlugin lRetVal = null;

      for (int lCounter = 0; lCounter < mPluginModule.PluginList.Length; lCounter++)
      {
        if (mPluginModule.PluginList[lCounter] != null && mPluginModule.PluginList[lCounter].Config.PluginName == pPluginName)
        {
          lRetVal = mPluginModule.PluginList[lCounter];
          break;
        }
      }

      return (lRetVal);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="args"></param>
    public ACMain(String[] args)
    {
      InitializeComponent();


      #region Datagrid header


      DataGridViewTextBoxColumn mPluginNameCol = new DataGridViewTextBoxColumn();
      mPluginNameCol.DataPropertyName = "PluginName";
      mPluginNameCol.HeaderText = "Plugin name";
      mPluginNameCol.ReadOnly = true;
      mPluginNameCol.Width = 120;
      DGV_MainPlugins.Columns.Add(mPluginNameCol);


      DataGridViewTextBoxColumn mPluginVersionCol = new DataGridViewTextBoxColumn();
      mPluginVersionCol.DataPropertyName = "PluginVersion";
      mPluginVersionCol.HeaderText = "Version";
      mPluginVersionCol.ReadOnly = true;
      DGV_MainPlugins.Columns.Add(mPluginVersionCol);


      DataGridViewCheckBoxColumn mActivatedCol = new DataGridViewCheckBoxColumn();
      mActivatedCol.DataPropertyName = "Active";
      mActivatedCol.Name = "Active";
      mActivatedCol.HeaderText = "Activated";
      mActivatedCol.FalseValue = "0";
      mActivatedCol.TrueValue = "1";
      mActivatedCol.Visible = true;
      DGV_MainPlugins.Columns.Add(mActivatedCol);

      DataGridViewTextBoxColumn mPluginDescriptionCol = new DataGridViewTextBoxColumn();
      mPluginDescriptionCol.DataPropertyName = "PluginDescription";
      mPluginDescriptionCol.HeaderText = "Description";
      mPluginDescriptionCol.ReadOnly = true;
      mPluginDescriptionCol.Width = 120;
      mPluginDescriptionCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      DGV_MainPlugins.Columns.Add(mPluginDescriptionCol);



      mUsedPlugins = new BindingList<UsedPlugins>();
      DGV_MainPlugins.DataSource = mUsedPlugins;

      #endregion

      //var curr = new BindingSource(list, null);               
      mTargetList = new BindingList<string>();
      mInputModule = new InputModule(this);
      mPluginModule = new PluginModule(this);
      mCurrentSession = new AttackSession(Directory.GetCurrentDirectory() + Config.SessionDir);
      mCurrentSession.StartTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

      // Check Pcap version
      try
      {
        L_PCAPVersion.Text = Config.WinPcap = Config.IsProgrammInstalled("winpcap").ToLower();
        if (Config.WinPcap == null || Config.WinPcap.Length <= 0)
        {
          String lMsg = String.Format("WinPcap is not present and {0} probably won't work as expected. You can download WinPcap under http://www.winpcap.org", Config.ToolName);
          MessageBox.Show(lMsg, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
        } // if (Config.WinP...
      }
      catch (Exception)
      {
        String lMsg = String.Format("WinPcap is not present and {0} probably won't work as expected. You can download WinPcap under http://www.winpcap.org", Config.ToolName);
        MessageBox.Show(lMsg, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
      }


      /*
       * Init configuration
       */
      try { Config.OS = String.Format("{0}.{1}", Environment.OSVersion.Version.Major, Environment.OSVersion.Version.Minor); }
      catch (Exception) { }
      try { Config.Architecture = Environment.Is64BitOperatingSystem ? "x64" : "x86"; }
      catch (Exception) { }
      try { Config.Language = System.Globalization.CultureInfo.CurrentCulture.ToString(); }
      catch (Exception) { }
      try { Config.Processor = System.Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER"); }
      catch (Exception) { }
      try { Config.NumProcessors = System.Environment.GetEnvironmentVariable("NUMBER_OF_PROCESSORS"); }
      catch (Exception) { }
      try { Config.DotNetVersion = System.Runtime.InteropServices.RuntimeEnvironment.GetSystemVersion(); }
      catch (Exception) { }
      try { Config.CommonLanguateRuntime = Environment.Version.ToString(); }
      catch (Exception) { }


      /*
       * Init LogConsole
       */
      LogConsole.Main.LogConsole.initLogConsole();


      /*
       * Check if an other instance is running.
       */
      Process[] lACInstances;

      if ((lACInstances = Process.GetProcessesByName(Config.APEName)) != null && lACInstances.Length > 0)
        if (MessageBox.Show("Other Simsang instance is running. Do you want to stop that process?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
          foreach (Process lProc in lACInstances)
            try { lProc.Kill(); }
            catch (Exception) { }


      /*
       * Check if configuration is ok.
       */
      this.ConfigOK(Directory.GetCurrentDirectory());


      /*
       * Set right Debugging modus in GUI
       */
      if (Config.DebugOn())
      {
        TSMI_Debugging.Text = "Turn debugging off";
        this.setAppTitle("Debugging");
      }
      else
      {
        TSMI_Debugging.Text = "Turn debugging on";
        this.setAppTitle("");
      }

      /*
       * Populate network interface.
       */
      this.LoadNICSettings();
      LogConsole.Main.LogConsole.pushMsg(String.Format("Current directory : {0}", Directory.GetCurrentDirectory()));


      /*
       * Start data input thread.
       */
      mInputModule.startInputThread();

      /*
       * Load all plugins.
       */
      mPluginModule.LoadPlugins();

      /*
       * Tab page controler. To hide and show tab pages.
       */
      if (mPluginModule != null)
        mTPHandler = new TabPageHandler(TC_Plugins, mPluginModule.GetPluginPosition);


      /*
       * Hide all plugins with status "off"
       */
      mPluginModule.CloseInactivePlugins();

      // And at last a new session
      //            TB_Session.Text = GetNewSessionName();


      if (CB_Interfaces.Items.Count <= 0)
      {
        MessageBox.Show("No active network adapter found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      else
      {
        /*
         * Init ARPScan console
         */
        try
        {
          // Simsang.ARPScan.ARPScan.InitARPScan(this, Config.Interfaces[CB_Interfaces.SelectedIndex].sIfcID, TB_StartIP.Text, TB_StopIP.Text, TB_GatewayIP.Text, ref mTargetList);
          Simsang.ARPScan.Main.ARPScan.InitARPScan(this, ref mTargetList);
        }
        catch (Exception lEx)
        {
          LogConsole.Main.LogConsole.pushMsg("Main() : " + lEx.StackTrace);
          Environment.Exit(0);
        }
        mAttackStarted = false;
      }


      /*
       * Check confirmation
       */
      String lRegistrationValue = Config.GetRegistryValue(Config.RegistryContributionKey, Config.RegistryContributionValue);
      if (String.IsNullOrEmpty(lRegistrationValue) || lRegistrationValue.ToLower() != "ok")
      {
        ContributeConfirmation lConfirmation = new ContributeConfirmation();
        lConfirmation.ShowDialog();
      }

      restrictFeaturesFacade();


      // Check if new version is available
      Thread lUpdateAvailableThread = new Thread(delegate()
      {
        if (NetworkInterface.GetAllNetworkInterfaces().Any(x => x.OperationalStatus == OperationalStatus.Up))
        {
          if (UpdatesBinaries.IsUpdateAvailable())
          {
            SimsangUpdates.FormNewVersion lNewVersion = new SimsangUpdates.FormNewVersion();
            lNewVersion.TopMost = true;
            lNewVersion.ShowDialog();
          }
          else
            LogConsole.Main.LogConsole.pushMsg("No new updates available.");
        }
        else
          LogConsole.Main.LogConsole.pushMsg("Can't check for new updates as no internet connection is available.");
      });
      lUpdateAvailableThread.Start();

      // Donate a click if internet connection is up.
      Thread lClickDonationThread = new Thread(delegate()
      {
        if (NetworkInterface.GetAllNetworkInterfaces().Any(x => x.OperationalStatus == OperationalStatus.Up))
          WB_Ads.Navigate(Config.BuglistURL);
      });
      lClickDonationThread.Start();


      // Initialize all plugins
      mPluginModule.initAllPlugins();


      /*
       * Load session file passed as a command line parameter.
       * 1. Import it
       * 2. Load it
       * 3. Remove session
       */
      if (args != null && args.Length > 0)
      {
        loadDefaultSession(args[0]);
      }
    }

    /// <summary>
    /// Pass all relevant data like TargetList to the plugins.
    /// </summary>
    public void UpdatePlugins()
    {
      List<String> lStrTargetList = new List<String>();
      BindingList<TargetRecord> lTargetList = ARPScan.Main.ARPScan.GetInstance().TargetList();

      if (lTargetList != null && lTargetList.Count > 0)
      {
        foreach (TargetRecord lTemp in lTargetList.ToList())
        {
          try
          {
            if (lTemp.Status)
              lStrTargetList.Add(lTemp.IP);
          }
          catch (Exception)
          { }
        } // foreac...

        lStrTargetList.Add("0.0.0.0");

        foreach (IPlugin lPlugin in mPluginModule.PluginList)
        {
          if (lPlugin != null)
          {
            try
            {
              lPlugin.SetTargets(lStrTargetList.ToList());
            }
            catch (Exception lEx)
            {
              MessageBox.Show(String.Format("Error : {0}", lEx.StackTrace));
            }
          } // if (lPl...
        } // foreach (IP...
      } // if (lTarge...
    }



    /// <summary>
    /// 
    /// </summary>
    public void restrictFeaturesFacade()
    {
      if (!Contribute.Infrastructure.Settings.getInstance().isContributeSet())
      {
        TSMI_GetUpdates.Enabled = false;
        TSMI_Debugging.Enabled = false;
        TSM_Sessions.Enabled = false;
        TB_Session.Enabled = false;
        TSMI_ImportSessions.Enabled = false;
        TSMI_ExportSessions.Enabled = false;
        Config.enableDebugging(false);
      }
      else
      {
        TSMI_GetUpdates.Enabled = true;
        TSMI_Debugging.Enabled = true;
        TSM_Sessions.Enabled = true;
        TSMI_ImportSessions.Enabled = true;
        TSMI_ExportSessions.Enabled = true;
        TB_Session.Enabled = true;
      }
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionFileName"></param>
    /// <param name="pImportForced"></param>
    /// <returns></returns>
    public String importSessionData(String pSessionFileName, bool pImportForced = false)
    {
      String lRetVal = String.Empty;

      if (!String.IsNullOrEmpty(pSessionFileName))
      {
        String lSessionData = String.Empty;
        String lSessionFileName = String.Empty;
        XDocument lXMLDoc = null;

        lSessionData = File.ReadAllText(pSessionFileName);
        if (!String.IsNullOrEmpty(lSessionData))
        {
          lXMLDoc = XDocument.Parse(lSessionData);
        } // if (!Strin...

        /*
         * Process session data
         */
        XElement la = lXMLDoc.Element("SessionExport").Element("Session");
        lSessionFileName = la.Attribute("filename").Value.ToString();

        foreach (XElement lTmp in la.Elements())
        {
          if (lTmp.Name.LocalName.ToString() == "AttackSession")
          {
            String lSessionFile = String.Format("{0}{1}{2}.xml", Directory.GetCurrentDirectory(), Config.SessionDir, lSessionFileName);
            if (File.Exists(lSessionFile))
            {
              if (pImportForced == false)
              {
                DialogResult lDR = DialogResult.No;

                lDR = MessageBox.Show("This session already exists.\r\nDo you want to overwrite it?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (lDR == DialogResult.No)
                  return (String.Empty);
              } // if (pImp...

              File.Delete(lSessionFile);
            } // if (lTmp...


            File.WriteAllText(lSessionFile, lTmp.ToString());
            break;
          } // if (lTmp.Name...
        } // foreach (XElement...



        /*
         * Process plugin data
         */
        String lPluginName = String.Empty;
        XElement lAllPlugins = null;
        String lOccurredErrors = String.Empty;

        var query = from plugin in lXMLDoc.Descendants("SessionExport")
                    select plugin.Element("Plugins");


        if ((lAllPlugins = query.SingleOrDefault()) != null)
        {
          XDocument lPluginsInSession = XDocument.Parse(lAllPlugins.ToString());

          foreach (XElement lTmpElement in lPluginsInSession.Descendants("Plugin"))
          {
            if (lTmpElement.HasElements)
            {
              String lName = lTmpElement.Attribute("name").Value.ToString();
              String lPluginDirName = lTmpElement.Attribute("dirname").Value.ToString();
              String lSessionFile = String.Format("{0}\\{1}{2}{3}{4}.xml", Directory.GetCurrentDirectory(), Config.PluginDir, lPluginDirName, Config.SessionDir, lSessionFileName);
              String lData = String.Empty;

              try
              {
                if (File.Exists(lSessionFile))
                  File.Delete(lSessionFile);
              }
              catch (Exception lEx)
              {
                String lErrorMsg = String.Format("Error occurred while deleting {0}\r\n{1}", lSessionFileName, lEx.Message);
                LogConsole.Main.LogConsole.pushMsg(lErrorMsg);
              }


              try
              {
                lData = lTmpElement.FirstNode.ToString();
                File.WriteAllText(lSessionFile, lData);
              }
              catch (Exception lEx)
              {
                String lErrorMsg = String.Format("Error occurred while creating {0}\r\n{1}", lSessionFileName, lEx.Message);
                LogConsole.Main.LogConsole.pushMsg(lErrorMsg);

                lOccurredErrors = String.Format("\r\n{0}\r\n", lOccurredErrors);
              }
            } // if (lTmpEle...
          } // foreach (XElem...
        } // if ((lAllPlug...
      } // if (!String...

      return (lRetVal);
    }

    #endregion


    #region PRIVATE

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionFile"></param>
    private void loadDefaultSession(String pSessionFile)
    {
      if (!String.IsNullOrEmpty(pSessionFile))
      {

        try
        {
          /*
           * 1. Import it
           */
          if (File.Exists(pSessionFile))
          {

            try
            {
              String lFuncRetVal = importSessionData(pSessionFile);

              /*
               * Show error message if errors occurred during the import procedure
               */
              if (!String.IsNullOrEmpty(lFuncRetVal))
              {
                lFuncRetVal = String.Format("The following errors occurred while importing {0}:\r\n{1}\r\n", pSessionFile, lFuncRetVal);
                MessageBox.Show(lFuncRetVal, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
              } // if (!Strin...
            }
            catch (Exception lEx)
            {
              String lErrorMsg = String.Format("An error occurred while importing session {0}\r\n{1}", pSessionFile, lEx.Message);
              LogConsole.Main.LogConsole.pushMsg(lErrorMsg);
              MessageBox.Show(lErrorMsg, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


            /*
             * 2. Load it.
             */
            try
            {
              XDocument lXMLDoc = XDocument.Load(pSessionFile);
              String lSessionName = lXMLDoc.Element("SessionExport").Element("Session").Attribute("filename").Value.ToString();

              if (!Sessions.getInstance(this).loadSession(lSessionName))
                MessageBox.Show("An error occurred while loading session " + lSessionName, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception lEx)
            {
              String lErrorMsg = String.Format("An error occurred while importing session {0}\r\n{1}", pSessionFile, lEx.Message);
              LogConsole.Main.LogConsole.pushMsg(lErrorMsg);
              MessageBox.Show(lErrorMsg, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

          } // if (File.E...
        }
        catch (Exception lEx)
        {
        }

      } // if (!Stri...
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pTargetList"></param>
    /// <returns></returns>
    private int targetCounter(BindingList<TargetRecord> pTargetList)
    {
      int lRetVal = 0;

      if (pTargetList != null && pTargetList.Count > 0)
        foreach (TargetRecord lTmp in pTargetList)
          if (lTmp.Status)
            lRetVal++;

      return (lRetVal);
    }



    /// <summary>
    /// Poisoning exited.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void onPoisoningExited(object sender, System.EventArgs e)
    {
      setSniffingBTOnStopped();
      setPoisoningBTOnStopped();
      setPluginsStopped();
      mAttackStarted = false;
    }



    /// <summary>
    /// Sniffing exited.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void onSniffingExited(object sender, System.EventArgs e)
    {
      setSniffingBTOnStopped();
      setPoisoningBTOnStopped();
      setPluginsStopped();
      mAttackStarted = false;
    }


    #endregion


    #region EVENTS

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CB_Interfaces_SelectedIndexChanged(object sender, EventArgs e)
    {
      try
      {
        Config.sInterfaces lIfcStruct = Config.GetIfcByID(Config.Interfaces[CB_Interfaces.SelectedIndex].sIfcID);

        if (lIfcStruct.sIfcName != null && lIfcStruct.sIfcName.Length > 0)
        {
          String lIfcName = lIfcStruct.sIfcName.Length > 40 ? lIfcStruct.sIfcName.Substring(0, 40) + " ..." : lIfcStruct.sIfcName;
          String lVendor = MACVendor.getInstance().getVendorByMAC(lIfcStruct.sIfcGWMAC);

          if (lVendor != null && lVendor.Length > 50)
            lVendor = String.Format("{0} ...", lVendor.Substring(0, 50));

          TB_GatewayIP.Text = lIfcStruct.sIfcDefaultGW;
          TB_GatewayMAC.Text = lIfcStruct.sIfcGWMAC;
          TB_Vendor.Text = lVendor;
          TB_StartIP.Text = lIfcStruct.sIfcNetAddr;
          TB_StopIP.Text = lIfcStruct.sIfcBcastAddr;
          GB_Interfaces.Text = String.Format("{0} / {1}", lIfcStruct.sIfcIPAddr, lIfcName);

          mCurrentIPAddress = lIfcStruct.sIfcIPAddr;
        }
      }
      catch (Exception lEx)
      {
        LogConsole.Main.LogConsole.pushMsg(lEx.StackTrace);
      }
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ACMain_FormClosing(object sender, FormClosingEventArgs e)
    {
      /*
       * 1. Stop data input thread (named pipe)
       * 2. Stop poisoning thread
       * 3. Stop sniffing thread
       * 4. Shut down all plugins.
       * 
       */

      // 0. Set the Wait cursor.
      this.Cursor = Cursors.WaitCursor;


      // 1. Stop sniffing thread
      setSniffingBTOnStopped();
      Thread.Sleep(250);

      // 2. Stop poisoning thread
      setPoisoningBTOnStopped();
      Thread.Sleep(250);

      setPluginsStopped();
      // 3. Stop data input thread
      //            if (mStopThread == false)
      //                mInputModule.stopInputThreads();

      // 4. Shut down all plugins
      mPluginModule.StopAllPlugins();

      // 5. Kill all APE processes : WARNING!!!  it kills also the APE Depoisoning process!!
      Process[] lAPEProcesses;
      if ((lAPEProcesses = Process.GetProcessesByName(Config.APEName)) != null && lAPEProcesses.Length > 0)
      {
        foreach (Process lProc in lAPEProcesses)
        {
          try { lProc.Kill(); }
          catch (Exception) { }
        }
      }

      // 6. Remove all static ARP entries

      ProcessStartInfo lPSI = new ProcessStartInfo("arp", "-d *");
      lPSI.WindowStyle = Config.DebugOn() ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden;
      System.Diagnostics.Process p = new System.Diagnostics.Process();
      p.StartInfo = lPSI;
      p.Start();
      p.WaitForExit(3000);
      p.Close();


      // 7. Set the default cursor.
      this.Cursor = Cursors.Default;

      // 8. Sometimes process cant stop correctly and stays running. Therefore ... 'clack clack booom!'
      System.Environment.Exit(0);
    }


    /// <summary>
    /// Activate/Deactivate plugin
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DGV_MainPlugins_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
      if (e.ColumnIndex == 3)
      {
        TabPage lTabPage;
        String lPluginName = String.Empty;

        if (e.RowIndex >= 0)
        {
          lPluginName = DGV_MainPlugins.Rows[e.RowIndex].Cells[0].Value.ToString();

          if ((lTabPage = mTPHandler.FindTabPage(lPluginName)) != null)
          {
            if (mUsedPlugins[e.RowIndex].Active == "1")
            {
              Config.SetRegistryValue(mUsedPlugins[e.RowIndex].PluginName, "state", "off");
              mUsedPlugins[e.RowIndex].Active = "0";
              mTPHandler.HideTabPage(lTabPage);

              try
              {
                GetPluginByName(mUsedPlugins[e.RowIndex].PluginName).Config.IsActive = false;
              }
              catch (Exception) { }

            }
            else
            {
              Config.SetRegistryValue(mUsedPlugins[e.RowIndex].PluginName, "state", "on");
              mUsedPlugins[e.RowIndex].Active = "1";
              mTPHandler.ShowTabPage(lTabPage);

              try
              {
                IPlugin lTmp = GetPluginByName(mUsedPlugins[e.RowIndex].PluginName); //
                lTmp.Config.IsActive = true;
                lTmp.onInit();
              }
              catch (Exception) { }

            } // if (mUsed...
          } // if ((lTab...
        } // if (e.RowInd ...
      } // if (e.Colu..     
    }




    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void L_MPLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      Process.Start(L_SimsangLink.Text);
    }




    /// <summary>
    /// Check for plugins, sniffer and main app updates
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void getUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (NetworkInterface.GetAllNetworkInterfaces().Any(x => x.OperationalStatus == OperationalStatus.Up))
      {
        Thread lUpdateAvailableThread = new Thread(delegate()
        {
          if (NetworkInterface.GetAllNetworkInterfaces().Any(x => x.OperationalStatus == OperationalStatus.Up))
          {
            if (UpdatesBinaries.IsUpdateAvailable())
            {
              SimsangUpdates.FormNewVersion lNewVersion = new SimsangUpdates.FormNewVersion();
              lNewVersion.TopMost = true;
              lNewVersion.ShowDialog();
            }
            else
            {
              LogConsole.Main.LogConsole.pushMsg("No new updates available.");
              MessageBox.Show("No new updates available.", "Update information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
          }
        });
        lUpdateAvailableThread.Start();
      }
      else
      {
        LogConsole.Main.LogConsole.pushMsg("Can't check for updates. Internet connection is down.");
        MessageBox.Show("Can't check for updates. Internet connection is down.", "Update information", MessageBoxButtons.OK, MessageBoxIcon.Information);
      }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void exitToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ACMain_FormClosing(null, null);
      Dispose();
    }




    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void debugginOnToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (File.Exists(Config.DebuggingFile))
      {
        TSMI_Debugging.Text = "Turn debugging on";
        this.setAppTitle("");
        Config.enableDebugging(false);
      }
      else
      {
        TSMI_Debugging.Text = "Turn debugging off";
        this.setAppTitle("Debugging");
        Config.enableDebugging(true);
      } // if (File...
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void logConsoleToolStripMenuItem_Click(object sender, EventArgs e)
    {
      LogConsole.Main.LogConsole.showLogConsole();
    }


    /// <summary>
    /// Load SessionList view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void listToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Sessions.getInstance(this).ShowDialog();
    }




    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void saveSessionToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (TB_Session.Text.Length <= 0)
        MessageBox.Show("You didn't define a session name", "Info - Session", MessageBoxButtons.OK, MessageBoxIcon.Information);
      else
      {
        AttackSession lASession = mCurrentSession.GetSessionByName(TB_Session.Text);

        if (lASession != null)
        {
          String lMsg = "A session with this name exists already!\nDo you want to overwrite the existing session data?";
          if (MessageBox.Show(lMsg, "Info - Session", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            Sessions.RemoveSession(lASession.SessionFileName, PluginsModule.PluginList);
          //                        Sessions.RemoveSession(lASession.FileName, PluginsModule.PluginList);
          else
            return;
        } // if (lASess...


        /*
         * Save main session information.
         */
        if (mCurrentSession.StartTime.Length <= 0)
          mCurrentSession.StartTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

        mCurrentSession.StartIP = TB_StartIP.Text;
        mCurrentSession.StopIP = TB_StopIP.Text;
        mCurrentSession.StopTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
        mCurrentSession.Name = TB_Session.Text;
        mCurrentSession.SaveSessionData();



        /*
         * Save plugin session data.
         */
        foreach (IPlugin lPlugin in mPluginModule.PluginList)
        {
          try
          {
            if (lPlugin != null)
              lPlugin.onSaveSessionData(Path.GetFileNameWithoutExtension(mCurrentSession.SessionFileName));
            //                            lPlugin.onSaveSessionData(mCurrentSession.FileName);
          }
          catch (Exception lEx)
          {
            LogConsole.Main.LogConsole.pushMsg(lEx.StackTrace);
          }
        } // foreach (IP...



        /*
         * Initialize new session.
         */
        mCurrentSession.StartTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
        mCurrentSession.StartTime = String.Empty;
        mCurrentSession.Name = String.Empty;
        mCurrentSession.Description = String.Empty;

        MessageBox.Show("The session was saved.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
      } // if (TB_S...
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void resetPluginsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      foreach (IPlugin lPlugin in mPluginModule.PluginList)
      {
        try
        {
          if (lPlugin != null)
            lPlugin.onResetPlugin();
        }
        catch (Exception lEx)
        {
          LogConsole.Main.LogConsole.pushMsg(lEx.StackTrace);
        }
      } // foreach (IP...
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BT_ScanLAN_Click(object sender, EventArgs e)
    {
      if (CB_Interfaces.SelectedIndex < 0)
        MessageBox.Show("No network interface selected", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      else
        Simsang.ARPScan.Main.ARPScan.showARPScanGUI(this, Config.Interfaces[CB_Interfaces.SelectedIndex].sIfcID, TB_StartIP.Text, TB_StopIP.Text, TB_GatewayIP.Text, ref mTargetList);

    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BT_Attack_Click(object sender, EventArgs e)
    {

      if (CB_Interfaces.SelectedIndex < 0)
        MessageBox.Show("No network interface selected", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      else if (mAttackStarted)
      {
        this.Cursor = Cursors.WaitCursor;
        /*
         * Stop scanning process and reset GUI
         */
        setSniffingBTOnStopped();
        setPoisoningBTOnStopped();
        setPluginsStopped();


        mAttackStarted = false;
        BT_Attack.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.Start));


        /*
         * Do contribution check
         */
        if (!Contribute.Infrastructure.Settings.getInstance().isContributeSet())
        {
          ContributeConfirmation lConfirmation = new ContributeConfirmation();
          lConfirmation.ShowDialog();
        }

        restrictFeaturesFacade();
        this.Cursor = Cursors.Default;


        /*
         * If user accepted contribution feature collect data and send
         * it to server.
         */
        if (Contribute.Infrastructure.Settings.getInstance().isContributeSet())
          contributeDataFacade();

      }
      else
      {
        if (targetCounter(ARPScan.Main.ARPScan.GetInstance().TargetList()) <= 0)
        {
          this.Cursor = Cursors.WaitCursor;
          setSniffingBTOnStopped();
          setPoisoningBTOnStopped();
          setPluginsStopped();
          mAttackStarted = false;
          this.Cursor = Cursors.Default;

          BT_Attack.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.Start));
          MessageBox.Show("You have to select at least one target system.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        else
        {
          this.Cursor = Cursors.WaitCursor;
          SetSniffingBTOnStarted();
          SetPoisoningBTOnStarted();
          mAttackStarted = true;
          this.Cursor = Cursors.Default;

          BT_Attack.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.Stop));
        } // if (targetCount...
      } // if (CB_Interfaces...
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TSMI_Contribute_Click(object sender, EventArgs e)
    {
      ContributeConfirmation lConfirmation = new ContributeConfirmation();
      lConfirmation.ShowDialog();
      restrictFeaturesFacade();
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TIM_Contributions_Tick(object sender, EventArgs e)
    {
      String lRegistrationValue = Config.GetRegistryValue(Config.RegistryContributionKey, Config.RegistryContributionValue);
      if (!String.IsNullOrEmpty(lRegistrationValue) && lRegistrationValue.ToLower() == "ok")
        ContributionProcessing.getInstance().processMessages();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Minibrowser_FormClosed(Object sender, FormClosedEventArgs e)
    {
      cMiniBrowser = null;
    }

    private void TSMI_Minibrowser_Click(object sender, EventArgs e)
    {
      if (cMiniBrowser == null)
      {
        cMiniBrowser = new Browser("", "", "", "");
        cMiniBrowser.FormClosed += Minibrowser_FormClosed;
        cMiniBrowser.Show();
      }
      else
      {
        MessageBox.Show("Another instance of Minibrowser is already running", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void initAllPluginsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      mPluginModule.resetAllPlugins();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void searchInterfacesToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.LoadNICSettings();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DGV_MainPlugins_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
      LogConsole.Main.LogConsole.pushMsg(String.Format("Error occurred ({0}) : {1}", sender.ToString(), e.ToString()));
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void sessionImportToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (OFD_ImportSession.ShowDialog() == DialogResult.OK)
      {
        String lSessionFileName = OFD_ImportSession.FileName;

        try
        {
          String lFuncRetVal = importSessionData(OFD_ImportSession.FileName);

          /*
           * Show error message if errors occurred during the import procedure
           */
          if (!String.IsNullOrEmpty(lFuncRetVal))
          {
            lFuncRetVal = String.Format("The following errors occurred while importing {0}:\r\n{1}\r\n", lSessionFileName, lFuncRetVal);
            MessageBox.Show(lFuncRetVal, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
          }
          else
          {
            MessageBox.Show(String.Format("Session {0} imported successfully.", lSessionFileName), "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
          } // if (!Strin...
        }
        catch (Exception lEx)
        {
          String lErrorMsg = String.Format("An error occurred while importing session {0}\r\n{1}", lSessionFileName, lEx.Message);
          LogConsole.Main.LogConsole.pushMsg(lErrorMsg);
          MessageBox.Show(lErrorMsg, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
      } // if (OFD_Impor...   
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void sessionExportToolStripMenuItem_Click(object sender, EventArgs e)
    {
      SessionExport lSessExport = new SessionsExport.SessionExport(this);
      lSessExport.ShowDialog();
      lSessExport.Dispose();
      lSessExport = null;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TSMI_ARPPoisoning_Click(object sender, EventArgs e)
    {
      TSMI_ARPPoisoning.Checked = true;
      TSMI_DHCPPoisoning.Checked = false;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TSMI_DHCPPoisoning_Click(object sender, EventArgs e)
    {
      TSMI_ARPPoisoning.Checked = false;
      TSMI_DHCPPoisoning.Checked = true;
    }

    #endregion

  }
}

