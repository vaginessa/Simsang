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
using System.Configuration;

using Simsang.Plugin;
using Plugin.Main.Systems;
using Plugin.Main.Systems.Config;
using ManageSystems = Plugin.Main.Systems.ManageSystems;


namespace Plugin.Main
{

  public partial class PluginSystemsUC : UserControl, IPlugin, IRecordObserver, ISystemPatternObserver
  {

    #region DATATYPES

    private enum EntryType
    {
      Empty,
      Half,
      Full
    }

    #endregion


    #region MEMBERS

    private List<String> cTargetList;
    private BindingList<SystemRecord> cSystems;
    public List<ManageSystems.SystemPattern> cSystemPatterns;
    private List<String> cDataBatch;
    private TaskFacade cTask;
    private PluginParameters cPluginParams;

    #endregion


    #region PUBLIC

    /// <summary>
    /// Constructor.
    /// Instantiate the UserControl.
    /// </summary>
    public PluginSystemsUC(PluginParameters pPluginParams)
    {
      InitializeComponent();

      #region DATAGRID HEADER

      DGV_Systems.AutoGenerateColumns = false;

      DataGridViewTextBoxColumn cMACCol = new DataGridViewTextBoxColumn();
      cMACCol.DataPropertyName = "SrcMAC";
      cMACCol.Name = "SrcMAC";
      cMACCol.HeaderText = "MAC address";
      cMACCol.ReadOnly = true;
      cMACCol.Width = 120;
      cMACCol.Resizable = System.Windows.Forms.DataGridViewTriState.False;
      DGV_Systems.Columns.Add(cMACCol);


      DataGridViewTextBoxColumn cSrcIPCol = new DataGridViewTextBoxColumn();
      cSrcIPCol.DataPropertyName = "SrcIP";
      cSrcIPCol.Name = "SrcIP";
      cSrcIPCol.HeaderText = "Source IP";
      cSrcIPCol.Width = 140;
      cSrcIPCol.ReadOnly = true;
      cSrcIPCol.Resizable = System.Windows.Forms.DataGridViewTriState.False;
      DGV_Systems.Columns.Add(cSrcIPCol);

      DataGridViewTextBoxColumn cAppURLCol = new DataGridViewTextBoxColumn();
      cAppURLCol.DataPropertyName = "OperatingSystem";
      cAppURLCol.Name = "OperatingSystem";
      cAppURLCol.HeaderText = "Operating System";
      cAppURLCol.ReadOnly = true;
      cAppURLCol.Width = 200; // 373;
      cAppURLCol.Resizable = System.Windows.Forms.DataGridViewTriState.False;
      DGV_Systems.Columns.Add(cAppURLCol);

      DataGridViewTextBoxColumn cHWVendorCol = new DataGridViewTextBoxColumn();
      cHWVendorCol.DataPropertyName = "HWVendor";
      cHWVendorCol.Name = "HWVendor";
      cHWVendorCol.HeaderText = "Hardware vendor";
      cHWVendorCol.ReadOnly = true;
      cHWVendorCol.Width = 200; // 373;
      cHWVendorCol.Resizable = System.Windows.Forms.DataGridViewTriState.False;
      DGV_Systems.Columns.Add(cHWVendorCol);


      DataGridViewTextBoxColumn cLastSeenCol = new DataGridViewTextBoxColumn();
      cLastSeenCol.DataPropertyName = "LastSeen";
      cLastSeenCol.Name = "LastSeen";
      cLastSeenCol.HeaderText = "Last seen";
      cLastSeenCol.ReadOnly = true;
      //cLastSeenCol.Width = 120;
      cLastSeenCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      cLastSeenCol.Resizable = System.Windows.Forms.DataGridViewTriState.False;
      DGV_Systems.Columns.Add(cLastSeenCol);

      cSystems = new BindingList<SystemRecord>();
      DGV_Systems.DataSource = cSystems;

      #endregion


      /*
       * Plugin configuration
       */
      cPluginParams = pPluginParams;
      String lBaseDir = String.Format(@"{0}\", (pPluginParams != null) ? pPluginParams.PluginDirectoryFullPath : Directory.GetCurrentDirectory());
      String lSessionDir = (pPluginParams != null) ? pPluginParams.SessionDirectoryFullPath : String.Format("{0}sessions", lBaseDir);

      Config = new PluginProperties()
      {
        BaseDir = lBaseDir,
        SessionDir = lSessionDir,
        PluginName = "Systems",
        PluginDescription = "Listing detected client systems, their OS type and the timestamp when it was last seen.",
        PluginVersion = "0.8",
        Ports = "TCP:80;TCP:443;",
        IsActive = true
      };

      cDataBatch = new List<String>();

      // Make it double buffered.
      typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, DGV_Systems, new object[] { true });
      T_GUIUpdate.Start();


      cTask = TaskFacade.getInstance(this);
      DomainFacade.getInstance(this).addRecordObserver(this);
      DomainFacade.getInstance(this).addSystemPatternObserver(this);
      cSystemPatterns = new List<ManageSystems.SystemPattern>();
    }

    #endregion


    #region PRIVATE


    /// <summary>
    /// 
    /// </summary>
    public void ProcessEntries()
    {
      if (cDataBatch != null && cDataBatch.Count > 0)
      {
        List<SystemRecord> lNewRecords = new List<SystemRecord>();
        List<String> lNewData;

        lock (this)
        {
          lNewData = new List<String>(cDataBatch);
          cDataBatch.Clear();
        } // lock (this)...


        foreach (String lEntry in lNewData)
        {
          try
          {
            if (lEntry != null && lEntry.Length > 0)
            {
              String[] lSplitter = Regex.Split(lEntry, @"\|\|");

              if (lSplitter.Length == 7)
              {
                String lProto = lSplitter[0];
                String lSMAC = lSplitter[1];
                String lSIP = lSplitter[2];
                String lSPort = lSplitter[3];
                String lDIP = lSplitter[4];
                String lDPort = lSplitter[5];
                String lData = lSplitter[6];
                String lOperatingSystem = String.Empty;
                String lUserAgent = String.Empty;
                Match lMatchUserAgent;
                EntryType lEntryType;
                DataGridViewRow lTabelRow;

                lSMAC = Regex.Replace(lSMAC, @"-", ":");
                lEntryType = FullEntryExists(lSMAC, lSIP);

                /*
                 * Determine the operating system due to the HTTP User-Agent string.
                 */
                if (((lMatchUserAgent = Regex.Match(lData, @"\.\.User-Agent\s*:\s*(.+?)\.\.", RegexOptions.IgnoreCase))).Success)
                {
                  try
                  {
//                    lLastPosition = DGV_Systems.FirstDisplayedScrollingRowIndex;
                    lUserAgent = lMatchUserAgent.Groups[1].Value.ToString();
                    lOperatingSystem = GetOperatingSystem(lUserAgent);
                  }
                  catch (Exception lEx)
                  {
                    cPluginParams.HostApplication.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));
                  }


                  /*
                   * 
                   */
                  try
                  {
                    if (lEntryType != EntryType.Full && lOperatingSystem.Length > 0)
                    {
                      if (lEntryType == EntryType.Empty)
                      {
                        lock (this)
                        {
                          cTask.addRecord(new SystemRecord(lSMAC, lSIP, lUserAgent, String.Empty, lOperatingSystem, String.Empty));
                        }
                      }
                      else if (lEntryType == EntryType.Half)
                        SetOS(lSMAC, lSIP, lOperatingSystem);


                      if ((lTabelRow = GetRowByMAC(lSMAC)) != null)
                        lTabelRow.Cells["OperatingSystem"].ToolTipText = lUserAgent;

//                      if (lLastPosition >= 0)
//                        DGV_Systems.FirstDisplayedScrollingRowIndex = lLastPosition;
                    }
                    else if (lSIP.Length > 0 && lSMAC.Length > 0)
                    {
//                      lLastPosition = DGV_Systems.FirstDisplayedScrollingRowIndex;
                      lock (this)
                      {
                        cTask.addRecord(new SystemRecord(lSMAC, lSIP, lUserAgent, String.Empty, String.Empty, String.Empty));
                      }

//                      if (lLastPosition >= 0)
//                        DGV_Systems.FirstDisplayedScrollingRowIndex = lLastPosition;
                    }
                  }
                  catch (RecordException lEx)
                  {
                    cPluginParams.HostApplication.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));
                  }
                  catch (RecordExistsException lEx)
                  {
                  }
                  catch (Exception lEx)
                  {
                    cPluginParams.HostApplication.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));
                  }

                /*
                 * The operating system cant be determined.
                 */
                }
                else if (lEntryType == EntryType.Empty && lSIP.Length > 0 && lSMAC.Length > 0)
                {
//                  lLastPosition = DGV_Systems.FirstDisplayedScrollingRowIndex;
                  try
                  {
                    lock (this)
                    {
                      cTask.addRecord(new SystemRecord(lSMAC, lSIP, String.Empty, lUserAgent, String.Empty, String.Empty));
                    }
                  }
                  catch (RecordException)
                  {
                  }

//                  if (lLastPosition >= 0)
//                    DGV_Systems.FirstDisplayedScrollingRowIndex = lLastPosition;
                } // if (lDstPort...


                /*
                 * Updating LastSeen column.
                 */
                using (DataGridViewRow lRow = ListEntryExists(lSMAC))
                {
                  if (lRow != null && lRow.Cells["LastSeen"] != null)
                    lRow.Cells["LastSeen"].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }

              } // if (lSplit.Leng..
            } // if (pData.Leng...
          }
          catch (Exception lEx)
          {
            MessageBox.Show(String.Format("{0} : {1}", Config.PluginName, lEx.ToString()));
          }
        } // foreach (St...
      } // if (cDataBat...
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pMAC"></param>
    /// <returns></returns>
    private DataGridViewRow GetRowByMAC(String pMAC)
    {
      DataGridViewRow lRetVal = null;

      if (DGV_Systems.RowCount > 0)
      {
        foreach (DataGridViewRow lRow in DGV_Systems.Rows)
        {
          if (lRow.Cells["SrcMAC"].Value.ToString() == pMAC)
          {
            lRetVal = lRow;
            break;
          } //if (lRow.Cel ...
        } // foreach (DataG...
      } // if (DGV_Syste...

      return (lRetVal);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSrcMAC"></param>
    /// <param name="pSrcIP"></param>
    /// <returns></returns>
    private EntryType FullEntryExists(String pSrcMAC, String pSrcIP)
    {
      EntryType lRetVal = EntryType.Empty;

      if (cSystems != null && cSystems.Count > 0)
      {
        foreach (SystemRecord lSystem in cSystems)
        {
          String lSrcMACReal = pSrcMAC.Replace('-', ':');
          String lSystemMacReal = lSystem.SrcMAC.Replace('-', ':');

          if (lSystemMacReal == lSrcMACReal && lSystem.SrcIP == pSrcIP && lSystem.OperatingSystem.Length > 0)
          {
            lRetVal = EntryType.Full;
            break;
          }
          else if (lSystemMacReal == lSrcMACReal && lSystem.SrcIP == pSrcIP)
          {
            lRetVal = EntryType.Half;
            break;
          }
        } // foreach (System...
      } // if (mSyste...

      return (lRetVal);
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSrcMAC"></param>
    /// <param name="pSrcIP"></param>
    /// <param name="pOS"></param>
    /// <returns></returns>
    private bool SetOS(String pSrcMAC, String pSrcIP, String pOS)
    {
      bool lRetVal = false;

      if (cSystems != null && cSystems.Count > 0)
      {

        foreach (SystemRecord lSystem in cSystems)
        {
          String lSrcMACReal = pSrcMAC.Replace('-', ':');
          String lSystemMacReal = lSystem.SrcMAC.Replace('-', ':');

          if (lSrcMACReal == lSystemMacReal && lSystem.SrcIP == pSrcIP)
          {
            lSystem.OperatingSystem = pOS;
            lRetVal = true;
            break;
          } // if (lSystem.Sr ...
        } // foreach (System...
      } // if (mSyste...


      return (lRetVal);
    }




    /// <summary>
    /// 
    /// </summary>
    /// <param name="pUserAgent"></param>
    /// <returns></returns>
    private String GetOperatingSystem(String pUserAgent)
    {
      String lRetVal = String.Empty;

      foreach (ManageSystems.SystemPattern lTempSys in cSystemPatterns)
      {
        if (pUserAgent != null && Regex.Match(pUserAgent, @lTempSys.SystemPatternString, RegexOptions.IgnoreCase).Success)
        {
          lRetVal = lTempSys.SystemName;
          break;
        } // if (lSplit2.L.. 
      }

      return (lRetVal);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pMAC"></param>
    /// <returns></returns>
    private DataGridViewRow ListEntryExists(String pMAC)
    {
      DataGridViewRow lRetVal = null;

      foreach (DataGridViewRow lRow in DGV_Systems.Rows)
      {
        if (lRow.Cells["SrcMAC"].Value.ToString() == pMAC)
        {
          lRetVal = lRow;
          break;
        } // if (lSys.Src...
      } // foreach (Systems...

      return (lRetVal);
    }

    #endregion


    #region PROPERTIES

    public Control PluginControl { get { return (this); } }
    public IPluginHost PluginHost { get { return cPluginParams.HostApplication;  } }
    #endregion


    #region IPlugin Member

    /// <summary>
    /// 
    /// </summary>
    public PluginProperties Config { set; get; }


    /// <summary>
    /// 
    /// </summary>
    public delegate void onInitDelegate();
    public void onInit()
    {
      if (InvokeRequired)
      {
        BeginInvoke(new onInitDelegate(onInit), new object[] { });
        return;
      } // if (InvokeRequired)

      cPluginParams.HostApplication.Register(this);
      cPluginParams.HostApplication.PluginSetStatus(this, "grey");
      cTask.readSystemPatterns();
    }


    /// <summary>
    /// 
    /// </summary>
    public delegate void onStartAttackDelegate();
    public void onStartAttack()
    {
      if (Config.IsActive)
      {
        if (InvokeRequired)
        {
          BeginInvoke(new onStartAttackDelegate(onStartAttack), new object[] { });
          return;
        } // if (InvokeRequired)

        // Add all system from ARP scan to the list
        cTask.removeAllRecords();
        List<Tuple<String, String, String>> lReachableSystems = cPluginParams.HostApplication.GetAllReachableSystems();
        foreach (Tuple<String, String, String> lTmp in lReachableSystems)
        {
          try
          {
            cTask.addRecord(new SystemRecord(lTmp.Item1, lTmp.Item2, String.Empty, lTmp.Item3, String.Empty, String.Empty));
          }
          catch (RecordException) 
          {
          }
        }
        cPluginParams.HostApplication.PluginSetStatus(this, "green");
      } // if (cIsActiv...
    }



    /// <summary>
    /// 
    /// </summary>
    public delegate void onStopAttackDelegate();
    public void onStopAttack()
    {
      if (InvokeRequired)
      {
        BeginInvoke(new onStopAttackDelegate(onStopAttack), new object[] { });
        return;
      } // if (InvokeRequired)

      cPluginParams.HostApplication.PluginSetStatus(this, "grey");
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public delegate String getDataDelegate();
    public String getData()
    {
      if (InvokeRequired)
      {
        BeginInvoke(new getDataDelegate(getData), new object[] { });
        return ("");
      } // if (InvokeRequired)

      return ("");
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionID"></param>
    /// <returns></returns>
    public delegate String onGetSessionDataDelegate(String pSessionID);
    public String onGetSessionData(String pSessionID)
    {
      if (InvokeRequired)
      {
        BeginInvoke(new onGetSessionDataDelegate(onGetSessionData), new object[] { pSessionID });
        return (String.Empty);
      } // if (InvokeRequired)

      String lRetVal = String.Empty;

      lRetVal = cTask.getSessionData(pSessionID);

      return (lRetVal);
    }



    /// <summary>
    /// 
    /// </summary>
    public delegate void onShutDownDelegate();
    public void onShutDown()
    {
      if (InvokeRequired)
      {
        BeginInvoke(new onShutDownDelegate(onShutDown), new object[] { });
        return;
      } // if (Invoke
    }



    /// <summary>
    /// 
    /// </summary>
    public delegate void onResetPluginDelegate();
    public void onResetPlugin()
    {
      if (InvokeRequired)
      {
        BeginInvoke(new onResetPluginDelegate(onResetPlugin), new object[] { });
        return;
      } // if (InvokeRequired)


      if (cSystems != null)
        cTask.removeAllRecords();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionName"></param>
    public delegate void onLoadSessionDataFromFileDelegate(String pSessionName);
    public void onLoadSessionDataFromFile(String pSessionName)
    {
      if (InvokeRequired)
      {
        BeginInvoke(new onLoadSessionDataFromFileDelegate(onLoadSessionDataFromFile), new object[] { pSessionName });
        return;
      } // if (InvokeRequired)


      try
      {
        onResetPlugin();
      }
      catch (Exception lEx)
      {
        cPluginParams.HostApplication.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));
      }

      try
      {
        cTask.loadSessionData(pSessionName);
      }
      catch (Exception lEx)
      {
        cPluginParams.HostApplication.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message)); 
      }
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionData"></param>
    public delegate void onLoadSessionDataFromStringDelegate(String pSessionData);
    public void onLoadSessionDataFromString(String pSessionData)
    {
      if (InvokeRequired)
      {
        BeginInvoke(new onLoadSessionDataFromStringDelegate(onLoadSessionDataFromString), new object[] { pSessionData });
        return;
      } // if (InvokeRequired)

      cTask.loadSessionDataFromString(pSessionData);
    }


    /// <summary>
    /// Remove session file with serialized data.
    /// </summary>
    /// <param name="pSessionName"></param>
    public delegate void onDeleteSessionDataDelegate(String pSessionName);
    public void onDeleteSessionData(String pSessionName)
    {
      if (InvokeRequired)
      {
        BeginInvoke(new onDeleteSessionDataDelegate(onDeleteSessionData), new object[] { pSessionName });
        return;
      } // if (InvokeRequired)

      cTask.deleteSession(pSessionName);
    }


    /// <summary>
    /// Serialize session data
    /// </summary>
    /// <param name="pSessionName"></param>
    public delegate void onSaveSessionDataDelegate(String pSessionName);
    public void onSaveSessionData(String pSessionName)
    {
      if (Config.IsActive)
      {
        if (InvokeRequired)
        {
          BeginInvoke(new onSaveSessionDataDelegate(onSaveSessionData), new object[] { pSessionName });
          return;
        } // if (InvokeRequired)

        cTask.saveSession(pSessionName);
      } // if (cIsActiv...
    }


    /// <summary>
    /// New input data arrived
    /// </summary>
    /// <param name="pData"></param>
    public delegate void onNewDataDelegate(String pData);
    public void onNewData(String pData)
    {
      if (Config.IsActive)
      {
        if (InvokeRequired)
        {
          BeginInvoke(new onNewDataDelegate(onNewData), new object[] { pData });
          return;
        } // if (InvokeRequired)

        lock (this)
        {
          if (cDataBatch != null && pData != null && pData.Length > 0)
            cDataBatch.Add(pData);
        } // lock (this)
      } // if (cIsActiv...
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pTargetList"></param>
    public void SetTargets(List<String> pTargetList)
    {
      cTargetList = pTargetList;
    }

    #endregion


    #region EVENTS

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void T_GUIUpdate_Tick(object sender, EventArgs e)
    {
      ProcessEntries();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TSMI_Clear_Click(object sender, EventArgs e)
    {
      cTask.removeAllRecords();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DGV_Systems_MouseUp(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Right)
      {
        CMS_Systems.Show(DGV_Systems, e.Location);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DGV_Systems_MouseUp_1(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Right)
      {
        try
        {
          DataGridView.HitTestInfo hti = DGV_Systems.HitTest(e.X, e.Y);
          if (hti.RowIndex >= 0)
            CMS_Systems.Show(DGV_Systems, e.Location);
        }
        catch (Exception lEx) { }
      }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void deleteEntryToolStripMenuItem_Click(object sender, EventArgs e)
    {
      int lCurIndex = DGV_Systems.CurrentCell.RowIndex;
      cTask.removeRecordAt(lCurIndex);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DGV_Systems_DoubleClick(object sender, EventArgs e)
    {
      ManageSystems.Form_ManageSystems lManageSystems = new ManageSystems.Form_ManageSystems(this);
      lManageSystems.ShowDialog();
      cTask.readSystemPatterns();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DGV_Systems_MouseDown(object sender, MouseEventArgs e)
    {
      try
      {
        DataGridView.HitTestInfo hti = DGV_Systems.HitTest(e.X, e.Y);

        if (hti.RowIndex >= 0)
        {
          DGV_Systems.ClearSelection();
          DGV_Systems.Rows[hti.RowIndex].Selected = true;
          DGV_Systems.CurrentCell = DGV_Systems.Rows[hti.RowIndex].Cells[0];
        }
      }
      catch (Exception)
      {
        DGV_Systems.ClearSelection();
      }
    }

    #endregion


    #region OBSERVER INTERFACE METHODS

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pRecordList"></param>
    public void updateRecordList(List<SystemRecord> pRecordList)
    {
      bool lIsLastLine = false;
      int lLastPosition = -1;
      int lLastRowIndex = -1;
      int lSelectedIndex = -1;

      lock (this)
      {
        /*
         * Remember DGV positions
         */
        if (DGV_Systems.CurrentRow != null && DGV_Systems.CurrentRow == DGV_Systems.Rows[DGV_Systems.Rows.Count - 1])
          lIsLastLine = true;

        lLastPosition = DGV_Systems.FirstDisplayedScrollingRowIndex;
        lLastRowIndex = DGV_Systems.Rows.Count - 1;

        if (DGV_Systems.CurrentCell != null)
          lSelectedIndex = DGV_Systems.CurrentCell.RowIndex;
        

        cSystems.Clear();
        if (pRecordList != null)
          foreach (SystemRecord lTmp in pRecordList)
            cSystems.Add(new SystemRecord(lTmp.SrcMAC, lTmp.SrcIP, lTmp.UserAgent, lTmp.HWVendor, lTmp.OperatingSystem, lTmp.LastSeen));

        // Selected cell/row
        try
        {
          if (lSelectedIndex >= 0)
            DGV_Systems.CurrentCell = DGV_Systems.Rows[lSelectedIndex].Cells[0];
        }
        catch (Exception) { }


        // Reset position
        try
        {
          if (lLastPosition >= 0)
            DGV_Systems.FirstDisplayedScrollingRowIndex = lLastPosition;
        }
        catch (Exception) { }

        DGV_Systems.Refresh();
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pPatternList"></param>
    public void updateSystemPatternList(List<ManageSystems.SystemPattern> pPatternList)
    {
      cSystemPatterns.Clear();
      if (pPatternList != null)
        foreach (ManageSystems.SystemPattern lTmp in pPatternList)
          cSystemPatterns.Add(new ManageSystems.SystemPattern(lTmp.SystemPatternString, lTmp.SystemName));
    }

    #endregion

  }
}
