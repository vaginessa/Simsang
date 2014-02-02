using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using System.Net;
using System.Collections.Specialized;
using System.Xml.Linq;
using System.Xml;

using Simsang.Plugin;
using Simsang.ARPScan.Main;
using Simsang.LogConsole.Main;
using Simsang.Contribute;
using Simsang.Wifi;


namespace Simsang
{

  public partial class SimsangMain
  {

    #region MEMBERS

    public TabPage TPDefault { get { return (TP_default); } }
    public TabControl TCPlugins { get { return (TC_Plugins); } }
    public BindingList<UsedPlugins> DGVUsedPlugins { get { return (mUsedPlugins); } }

    #endregion


    #region PUBLIC

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pTitle"></param>
    public void setAppTitle(String pTitle)
    {
      if (pTitle.Length > 0)
        Text = String.Format("{0}  {1:0.0} - {2} ({3})", Config.ToolName, Config.ToolVersion, Config.VersionType, pTitle);
      else
        Text = String.Format("{0}  {1:0.0} - {2}", Config.ToolName, Config.ToolVersion, Config.VersionType);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pBaseDir"></param>
    /// <returns></returns>
    public bool ConfigOK(String pBaseDir)
    {

      try
      {
        if (!File.Exists(pBaseDir + Config.BinaryDir + Config.APEBinary))
        {
          MessageBox.Show("APE.exe was not found. If you run " + Config.ToolName + " for the first time,\r\n" +
                          "click on the menu bar on 'Help' and  on 'Update' to download the\r\n " +
                          "latest plugins and binaries. Otherwise " + Config.ToolName + " wont run correctly!\r\n",
                          "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
          return (false);
        } // if (! File...
      }
      catch (Exception lEx)
      {
        LogConsole.Main.LogConsole.pushMsg(lEx.StackTrace);
      }


      return (true);
    }



    /// <summary>
    /// 
    /// </summary>
    public void LoadNICSettings()
    {
      String lTemp = String.Empty;

      /*
       * Empty Interfaces ComboBox and repopulate it with found interfaces.
       */
      CB_Interfaces.Items.Clear();

      if (NetworkInterface.GetIsNetworkAvailable())
      {

        try
        {
          mNCards = NetworkInterface.GetAllNetworkInterfaces();
        }
        catch (NetworkInformationException lEx)
        {
          LogConsole.Main.LogConsole.pushMsg(lEx.StackTrace);
          return;
        }


        foreach (NetworkInterface lIFC in mNCards)
        {
          if (lIFC.OperationalStatus == OperationalStatus.Up)
          {
            /*
             * Set start/stop IP address
             */
            if (lIFC.GetIPProperties() != null &&
                lIFC.GetIPProperties().UnicastAddresses.Count > 0)
            {
              UnicastIPAddressInformation lIPAddr = null;
              // Find entry with valid IPv4 address
              foreach (UnicastIPAddressInformation lTmpIPaddr in lIFC.GetIPProperties().UnicastAddresses)
              {
                if (lTmpIPaddr.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                  lIPAddr = lTmpIPaddr;
                  break;
                } // if (lTmpIPaddr.Addr...
              } // foreach (Uni...


              if (lIPAddr != null && lIPAddr.IPv4Mask != null)
              {
                try
                {
                  String lIfcBcastAddr = IPCalc.GetBroadcastAddress(lIPAddr.Address, lIPAddr.IPv4Mask).ToString();
                  String lIfcNetAddr = IPCalc.GetNetworkAddress(lIPAddr.Address, lIPAddr.IPv4Mask).ToString();
                  String lIfcDescr = lIFC.Description;
                  String lIfcID = lIFC.Id;
                  String lIfcIPAddr = lIPAddr.Address.ToString();
                  String lIfcName = lIFC.Name;
                  String lDefaultGW = lIFC.GetIPProperties().GatewayAddresses.Count > 0 ? lIFC.GetIPProperties().GatewayAddresses[0].Address.ToString() : "";
                  String lGWMAC = IPCalc.GetMACFromIP(lDefaultGW);

                  Config.SetInterfaceInstance(lIfcID, lIfcName, lIfcDescr, lIfcIPAddr, lIfcNetAddr, lIfcBcastAddr, lDefaultGW, lGWMAC);

                  /*
                   * Determine interface name
                   */
                  if (lIFC.Description.Length > 50)
                    lTemp = lIFC.Description.Substring(0, 50) + " ...";
                  else
                    lTemp = lIFC.Description;

                  CB_Interfaces.Items.Add(lTemp);
                }
                catch (Exception lEx)
                {
                  LogConsole.Main.LogConsole.pushMsg(lEx.Message);
                }
              } // if (lIPAddr.IPv...
            } // if (lIFC.GetIPPro...
          } // if (lIFC.Opera...
        } // foreach (NetworkI...

        if (Config.NumInterfaces() > 0)
        {
          /*
           * Dump all interfaces to the Log console
           */
          foreach (Config.sInterfaces lTmpIfc in Config.Interfaces)
          {
            if (lTmpIfc.sUsed)
            {
              String lIFCData = String.Format("Ifc descr : {0}\r\nIfc ID : {1}\r\nIP : {2}\r\nGW IP : {3}\r\nGW MAC : {4}",
              lTmpIfc.sIfcDescr, lTmpIfc.sIfcID, lTmpIfc.sIfcIPAddr, lTmpIfc.sIfcDefaultGW, lTmpIfc.sIfcGWMAC);
              LogConsole.Main.LogConsole.pushMsg(lIFCData);
            } // if (lTmpI...
          } // foreach (Con...


          /*
           * Select a default interface.
           */
          if (TB_StartIP.Text.Length == 0)
            TB_StartIP.Text = Config.Interfaces[0].sIfcNetAddr;


          if (TB_StopIP.Text.Length == 0)
            TB_StopIP.Text = Config.Interfaces[0].sIfcBcastAddr;

          try
          {
            if (CB_Interfaces.Items.Count > 0)
              CB_Interfaces.SelectedIndex = 0;
          }
          catch (Exception lEx)
          {
            LogConsole.Main.LogConsole.pushMsg(lEx.StackTrace);
          }
        } // if (Conf...
      } // if (NetworkInter...
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pData"></param>
    private delegate void UpdateMainTBDelegate(String pData);
    public void UpdateMainTB(String pData)
    {
      String[] lSplitter;
      String lTemp;


      if (InvokeRequired)
      {
        BeginInvoke(new UpdateMainTBDelegate(UpdateMainTB), new object[] { pData });
        return;
      } // if (Invok...


      // TCP||192.168.0.123||51984||74.125.79.136||80||GET ...
      lSplitter = Regex.Split(pData, @"\|\|");


      /*
       * We got TCP data.
       */
      if (lSplitter[0] == "TCP")
      {
        lTemp = "TCP:" + lSplitter[5] + ";";
        foreach (IPlugin lPlugin in mPluginModule.PluginList)
        {
          try
          {
            if (lPlugin != null && lPlugin.Config.IsActive)
              if (lPlugin.Config.Ports.Contains(lTemp))
                lPlugin.onNewData(pData);

          }
          catch (Exception lEx)
          {
            LogConsole.Main.LogConsole.pushMsg(lEx.StackTrace);
          }
        } // foreach (IP...



        /*
         * We got UDP data.
         */
      }
      else if (lSplitter[0] == "DNSREP" || lSplitter[0] == "DNSREQ" || lSplitter[0] == "UDP")
      {
        lTemp = "UDP:" + lSplitter[5] + ";";
        foreach (IPlugin lPlugin in mPluginModule.PluginList)
        {
          try
          {
            if (lPlugin != null && lPlugin.Config.IsActive)
              if (lPlugin.Config.Ports.Contains(lTemp))
                lPlugin.onNewData(pData);
          }
          catch (Exception lEx)
          {
            LogConsole.Main.LogConsole.pushMsg(lEx.StackTrace);
          }
        } // foreach (IP...
      }
      else if (lSplitter[0] == "GENERIC")
      {
        lTemp = "UDP:" + lSplitter[5] + ";";
        foreach (IPlugin lPlugin in mPluginModule.PluginList)
        {
          try
          {
            if (lPlugin != null && lPlugin.Config.IsActive)
              if (lPlugin.Config.Ports.Contains(lTemp))
                lPlugin.onNewData(pData);
          }
          catch (Exception lEx)
          {
            LogConsole.Main.LogConsole.pushMsg(lEx.StackTrace);
          }
        } // foreach (IP...
      } // if (lSplitte...
    }

    #endregion


    #region PRIVATE

    /// <summary>
    /// 
    /// </summary>
    private delegate void SetPSniffingBTOnStartedDelegate();
    private void SetSniffingBTOnStarted()
    {
      if (InvokeRequired)
      {
        BeginInvoke(new SetPSniffingBTOnStartedDelegate(SetSniffingBTOnStarted), new object[] { });
        return;
      } // if (InvokeRequired)


      String lPoisonPath = String.Format(@"{0}\{1}\{2}", Directory.GetCurrentDirectory(), Config.BinaryDir, Config.APEBinary);
      CB_Interfaces.Enabled = false;
      BT_ScanLAN.Enabled = false;
      mSnifferProc = new Process();
      mSnifferProc.StartInfo.FileName = lPoisonPath;
      mSnifferProc.StartInfo.Arguments = "-s " + Config.GetIDByIndex(CB_Interfaces.SelectedIndex);
      mSnifferProc.StartInfo.WorkingDirectory = String.Format(@"{0}{1}", Directory.GetCurrentDirectory(), Config.BinaryDir);

      mSnifferProc.StartInfo.WindowStyle = Config.DebugOn() ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden;
      mSnifferProc.StartInfo.CreateNoWindow = Config.DebugOn() ? true : false;
      mSnifferProc.EnableRaisingEvents = true;
      mSnifferProc.Exited += onSniffingExited;

      try
      {
        mSnifferProc.Start();
      }
      catch (Exception lEx)
      {
        MessageBox.Show("Can't start sniffer! ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        LogConsole.Main.LogConsole.pushMsg("Can't start sniffer! - " + lEx.StackTrace);
      }
    }



    /// <summary>
    /// 
    /// </summary>
    private delegate void setPSniffingBTOnStoppedDelegate();
    private void setSniffingBTOnStopped()
    {
      if (InvokeRequired)
      {
        BeginInvoke(new setPSniffingBTOnStoppedDelegate(setSniffingBTOnStopped), new object[] { });
        return;
      } // if (InvokeRequired)


      String lIfcID = String.Empty;
      Config.sInterfaces lIfcSelected;

      BT_Attack.BackgroundImage = Properties.Resources.Start;
      lIfcID = Config.GetIDByIndex(CB_Interfaces.SelectedIndex);
      lIfcSelected = Config.GetIfcByID(lIfcID);

      CB_Interfaces.Enabled = true;
      BT_ScanLAN.Enabled = true;

      try
      {
        if (mSnifferProc != null && !mSnifferProc.HasExited && mSnifferProc.Responding)
          mSnifferProc.Kill();
      }
      catch (Exception lEx)
      {
        LogConsole.Main.LogConsole.pushMsg(lEx.StackTrace);
      }
    }


    /// <summary>
    /// 
    /// </summary>
    private delegate void SetPoisoningBTOnStartedDelegate();
    private void SetPoisoningBTOnStarted()
    {
      if (InvokeRequired)
      {
        BeginInvoke(new SetPoisoningBTOnStartedDelegate(SetPoisoningBTOnStarted), new object[] { });
        return;
      }

      String lTargetHosts = String.Empty;
      ARPScan.Main.ARPScan lARPScan;


      if ((lARPScan = ARPScan.Main.ARPScan.GetInstance()) != null)
      {
        /*
         * Write APE Target list fileAPETargetHosts
         * APETargetHosts
         */
        foreach (TargetRecord lTmp in lARPScan.TargetList())
          if (lTmp.Status)
            lTargetHosts += String.Format("{0},{1}\r\n", lTmp.IP, lTmp.MAC);

        lTargetHosts = lTargetHosts.Trim();

        if (lTargetHosts.Length > 0)
        {
          String lTimeStamp = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
          int lIfcIndex = CB_Interfaces.SelectedIndex;
          String lIfcID = Config.GetIDByIndex(CB_Interfaces.SelectedIndex);
          Config.sInterfaces lIfcSelected = Config.GetIfcByID(lIfcID);
          String lTargetHostsPath = Config.APETargetHostsPath;

          using (StreamWriter lOutFile = new StreamWriter(lTargetHostsPath))
          {
            lOutFile.Write(lTargetHosts);
          }



          /*
           * Customize GUI elements
           */
          TSMI_ShowSessions.Enabled = false;
          TSMI_ResetPlugins.Enabled = false;
          TSMI_SaveSession.Enabled = false;
          TSMI_GetUpdates.Enabled = false;
          TSMI_Exit.Enabled = false;
          TSMI_Contribute.Enabled = false;
          TSMI_ResetAllPlugins.Enabled = false;
          TSMI_SearchInterfaces.Enabled = false;
          TSMI_ImportSessions.Enabled = false;
          TSMI_ExportSessions.Enabled = false;
          CB_Interfaces.Enabled = false;
          TB_StartIP.Enabled = false;
          TB_StopIP.Enabled = false;
          TB_Session.Enabled = false;
          DGV_MainPlugins.Enabled = false;


          /*
           * onAttackStart() 
           */
          foreach (IPlugin lPlugin in mPluginModule.PluginList)
            if (lPlugin != null)
              lPlugin.onStartAttack();



          /*
           * Start process
           */
          String lPoisonPath = String.Format(@"{0}\{1}\{2}", Directory.GetCurrentDirectory(), Config.BinaryDir, Config.APEBinary);

          mPoisoningEngProc = new Process();
          mPoisoningEngProc.StartInfo.WorkingDirectory = String.Format(@"{0}{1}", Directory.GetCurrentDirectory(), Config.BinaryDir);
          mPoisoningEngProc.StartInfo.FileName = lPoisonPath;
          mPoisoningEngProc.StartInfo.Arguments = String.Format("-x {0}", lIfcID);
          mPoisoningEngProc.StartInfo.WindowStyle = Config.DebugOn() ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden;
          mPoisoningEngProc.StartInfo.CreateNoWindow = Config.DebugOn() ? true : false;
          mPoisoningEngProc.EnableRaisingEvents = true;
          mPoisoningEngProc.Exited += onPoisoningExited;

          try
          {
            mCurrentSession.StartTime = lTimeStamp;
            mCurrentSession.Name = TB_Session.Text;
            mCurrentSession.Description = "Session startet on " + lTimeStamp;

            BT_Attack.BackgroundImage = Properties.Resources.Stop;
            mPoisoningEngProc.Start();
          }
          catch (Exception lEx)
          {
            MessageBox.Show("Can't start poisoning engine!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            LogConsole.Main.LogConsole.pushMsg("Can't start poisoning engine! - " + lEx.StackTrace + "\n" + lEx.ToString());
          }
        }
      }
    }


    /// <summary>
    /// 
    /// </summary>
    private delegate void setPluginsStoppedDelegate();
    private void setPluginsStopped()
    {
      if (InvokeRequired)
      {
        BeginInvoke(new setPluginsStoppedDelegate(setPluginsStopped), new object[] { });
        return;
      }
      try
      {
        foreach (IPlugin lPlugin in mPluginModule.PluginList)
          if (lPlugin != null)
            lPlugin.onStopAttack();
      }
      catch (Exception)
      { }

    }



    /// <summary>
    /// 
    /// </summary>
    private delegate void setPoisoningBTOnStoppedDelegate();
    private void setPoisoningBTOnStopped()
    {
      if (InvokeRequired)
      {
        BeginInvoke(new setPoisoningBTOnStoppedDelegate(setPoisoningBTOnStopped), new object[] { });
        return;
      }


      int lIfcIndex = CB_Interfaces.SelectedIndex;
      String lIfcID = Config.GetIDByIndex(CB_Interfaces.SelectedIndex);
      Config.sInterfaces lIfcSelected = Config.GetIfcByID(lIfcID);

      BT_Attack.BackgroundImage = Properties.Resources.Start;


      /*
       * Customize GUI elements.
       */
      if (!mAttackStarted)
        CB_Interfaces.Enabled = true;

      TSMI_ShowSessions.Enabled = true;
      TSMI_ResetPlugins.Enabled = true;
      TSMI_SaveSession.Enabled = true;
      TSMI_GetUpdates.Enabled = true;
      TSMI_Exit.Enabled = true;
      TSMI_Contribute.Enabled = true;
      TSMI_ResetAllPlugins.Enabled = true;
      TSMI_SearchInterfaces.Enabled = true;
      TSMI_ImportSessions.Enabled = true;
      TSMI_ExportSessions.Enabled = true;
      TB_StartIP.Enabled = true;
      TB_StopIP.Enabled = true;
      TB_Session.Enabled = true;
      DGV_MainPlugins.Enabled = true;

      try
      {
        if (mPoisoningEngProc != null && !mPoisoningEngProc.HasExited && mPoisoningEngProc.Responding)
        {
          // 1. kill process
          mPoisoningEngProc.Kill();
          mPoisoningEngProc.CloseMainWindow();
        }
      }
      catch (Exception lEx)
      {
        LogConsole.Main.LogConsole.pushMsg(lEx.StackTrace);
      }

      try
      {
        // 2. start depoisoning procedure...
        Process mDepoisoningProc = new Process();
        mDepoisoningProc.StartInfo.FileName = Config.APEPath;
        mDepoisoningProc.StartInfo.Arguments = "-d " + Config.GetIDByIndex(CB_Interfaces.SelectedIndex);
        mDepoisoningProc.StartInfo.WorkingDirectory = Config.LocalBinariesPath;
        mDepoisoningProc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        mDepoisoningProc.StartInfo.CreateNoWindow = true;
        mDepoisoningProc.EnableRaisingEvents = true;

        mDepoisoningProc.Start();
      }
      catch (Exception lEx)
      {
        LogConsole.Main.LogConsole.pushMsg(lEx.StackTrace);
      }
    }



    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private String GetNewSessionName()
    {
      String lRetVal = String.Empty;

      DateTime lTime = DateTime.Now;
      try
      { lRetVal = lTime.ToString("yyyy-MM-dd-HH:mm:ss"); }
      catch (Exception lEx)
      {
        LogConsole.Main.LogConsole.pushMsg(lEx.StackTrace);
      }

      return (lRetVal);
    }



    /// <summary>
    /// 
    /// </summary>
    private void contributeDataFacade()
    {
      IPlugin lPlugin = null;
      Config.sInterfaces lIfcStruct;
      String lData = String.Empty;
      int lDataVolume = 0;
      String lDateTime = String.Empty;
      String lContributeData = String.Empty;
      String lWifiName = String.Empty;
      String lGWMAC = String.Empty;


      /*
       * Determine SSID
       */
      try
      {
        if (Contribute.Infrastructure.Settings.getInstance().isSSIDStatusSet())
          lWifiName = Wifi.Wifi.getInstance().getCurrentWifiName().Trim();
      }
      catch (Exception lEx)
      {
        LogConsole.Main.LogConsole.pushMsg(String.Format("Can't determine Wifi name : {0}", lEx.StackTrace));
      }

      /*
       * Determine data volume. If volume <= 0 then ARP MITM was not successful
       */
      try
      {
        lIfcStruct = Config.GetIfcByID(Config.Interfaces[CB_Interfaces.SelectedIndex].sIfcID);
        lPlugin = mPluginModule.getPluginByName("accounting");

        if (lIfcStruct.sIfcName != null && lIfcStruct.sIfcName.Length > 0)
        {
          lGWMAC = lIfcStruct.sIfcGWMAC;
          lData = lPlugin.getData();
          Int32.TryParse(lData, out lDataVolume);
        }
      }
      catch (Exception lEx)
      {
        LogConsole.Main.LogConsole.pushMsg(lEx.StackTrace);
      }


      /*
       * Send contribution mail
       */
      try
      {
        if (lDataVolume > 0)
        {
          lDateTime = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss zz");
          lContributeData = String.Format("<contribution><timestamp>{0}</timestamp>\r\n<ssid>{1}</ssid>\r\n<gw>{2}</gw>\r\n<version>{3}</version></contribution>\r\n", lDateTime, lWifiName, lGWMAC, Config.ToolVersion);

          Contribute.Infrastructure.ContributionProcessing.getInstance().createContributionMessage(lContributeData);
        } // if (lDataVol...
      }
      catch (Exception lEx)
      {
        LogConsole.Main.LogConsole.pushMsg(lEx.StackTrace);
      }
    }

    #endregion

  }
}
