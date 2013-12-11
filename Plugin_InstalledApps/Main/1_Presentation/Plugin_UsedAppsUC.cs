using System;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Configuration;

using Simsang.Plugin;
using Plugin.Main.Applications;
using Plugin.Main.Applications.Config;
using MngApplication = Plugin.Main.Applications.ManageApplications;


namespace Plugin.Main
{
  public partial class PluginUsedAppsUC : UserControl, IPlugin, IObserver
  {

    #region MEMBERS

    private IPluginHost cHost;
    private List<String> cTargetList;
    private BindingList<ApplicationRecord> cApplications;
    public BindingList<MngApplication.ApplicationPattern> cApplicationPatterns;
    private String cPatternFilePath = @"plugins\UsedApps\Plugin_UsedApps_Patterns.xml";
    private TaskFacade cTask;

    #endregion


    #region PROPERTIES

    public Control PluginControl { get { return (this); } }
    public IPluginHost Host { get { return cHost; } set { cHost = value; cHost.Register(this); } }

    #endregion


    #region PUBLIC

    public PluginUsedAppsUC(PluginParameters pPluginParams)
    {
      InitializeComponent();


      #region DATAGRID HEADER

      DataGridViewTextBoxColumn cMACCol = new DataGridViewTextBoxColumn();
      cMACCol.DataPropertyName = "SrcMAC";
      cMACCol.Name = "SrcMAC";
      cMACCol.HeaderText = "MAC address";
      cMACCol.ReadOnly = true;
      cMACCol.Visible = true;
      cMACCol.Width = 140;
      DGV_Applications.Columns.Add(cMACCol);


      DataGridViewTextBoxColumn cSrcIPCol = new DataGridViewTextBoxColumn();
      cSrcIPCol.DataPropertyName = "SrcIP";
      cSrcIPCol.Name = "SrcIP";
      cSrcIPCol.HeaderText = "Source IP";
      //cSrcIPCol.Visible = false;
      cSrcIPCol.ReadOnly = true;
      cSrcIPCol.Width = 120;
      DGV_Applications.Columns.Add(cSrcIPCol);


      DataGridViewTextBoxColumn cAppNameCol = new DataGridViewTextBoxColumn();
      cAppNameCol.DataPropertyName = "AppName";
      cAppNameCol.Name = "AppName";
      cAppNameCol.HeaderText = "Application name";
      cAppNameCol.ReadOnly = true;
      cAppNameCol.Visible = true;
      cAppNameCol.Width = 160;
      DGV_Applications.Columns.Add(cAppNameCol);

      DataGridViewTextBoxColumn cAppURLCol = new DataGridViewTextBoxColumn();
      cAppURLCol.DataPropertyName = "AppURL";
      cAppURLCol.Name = "AppURL";
      cAppURLCol.HeaderText = "Application URL";
      cAppURLCol.ReadOnly = true;
      cAppURLCol.Visible = true;
      //            cAppURLCol.Width = 230; // 213;
      cAppURLCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      DGV_Applications.Columns.Add(cAppURLCol);



      cApplications = new BindingList<ApplicationRecord>();
      DGV_Applications.DataSource = cApplications;

      #endregion

      /*
       * Plugin configuration
       */
      String lBaseDir = String.Format(@"{0}\", (pPluginParams != null) ? pPluginParams.PluginDirectoryFullPath : Directory.GetCurrentDirectory());
      String lSessionDir = (pPluginParams != null) ? pPluginParams.SessionDirectoryFullPath : String.Format("{0}sessions", lBaseDir);

      Config = new PluginProperties()
      {
        BaseDir = lBaseDir,
        SessionDir = lSessionDir,
        PluginName = "Used apps",
        PluginDescription = "Listing with installed applications per client system.",
        PluginVersion = "0.6",
        Ports = "TCP:80;UDP:53;",
        IsActive = true
      };

      cApplicationPatterns = new BindingList<MngApplication.ApplicationPattern>();
      cTask = TaskFacade.getInstance(this);
    }

    #endregion


    #region PRIVATE

    /// <summary>
    /// 
    /// </summary>
    private void readApplicationPatterns()
    {
      BindingList<MngApplication.ApplicationPattern> lApplicationPatterns = null;

      /*
       * Clear and repopulate DataGridView.
       */
      if (lApplicationPatterns != null && lApplicationPatterns.Count > 0)
      {
        cApplicationPatterns.Clear();
        foreach (MngApplication.ApplicationPattern lReq in lApplicationPatterns)
          cApplicationPatterns.Add(lReq);
      }
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="pMAC"></param>
    /// <param name="pAppName"></param>
    /// <returns></returns>
    private bool ListEntryExists(String pMAC, String pAppName)
    {
      bool lRetVal = false;

      foreach (ApplicationRecord lApp in cApplications)
      {
        if (lApp.SrcMAC == pMAC && lApp.AppName == pAppName)
        {
          lRetVal = true;
          break;
        } // if (lApp.Src...
      } // foreach (Applic...

      return (lRetVal);
    }


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


      cHost.PluginSetStatus(this, "grey");
      readApplicationPatterns();
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

        cHost.PluginSetStatus(this, "green");
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

      cHost.PluginSetStatus(this, "grey");
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
    public delegate void onResetPluginDelegate();
    public void onResetPlugin()
    {
      if (InvokeRequired)
      {
        BeginInvoke(new onResetPluginDelegate(onResetPlugin), new object[] { });
        return;
      } // if (InvokeRequired)

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

      cTask.loadSessionData(pSessionName);
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
    /// Remove session file with serialized data. 
    /// </summary>
    /// <param name="pSessionName"></param>
    public delegate void onDeleteSessionDataDelegate(String pSessionFileName);
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
    /// New input data arrived
    /// TCP||00:11:22:33:44:55||192.168.0.123||51984||74.125.79.136||80||GET...
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


        try
        {
          if (pData != null && pData.Length > 0)
          {
            string[] lSplitter = Regex.Split(pData, @"\|\|");
            if (lSplitter.Length == 7)
            {
              String lProto = lSplitter[0];
              String lSMAC = lSplitter[1];
              String lSIP = lSplitter[2];
              String lSPort = lSplitter[3];
              String lDIP = lSplitter[4];
              String lDPort = lSplitter[5];
              String lData = lSplitter[6];
              Match lMatchURI;
              Match lMatchHost;
              String lRemoteHost = String.Empty;
              String lReqString = String.Empty;
              String lRemotePort = "0";
              String lRemoteString = String.Empty;


              if (lProto == "TCP" && lDPort == "80" &&
                  ((lMatchURI = Regex.Match(lData, @"(\s+|^)(GET|POST|HEAD)\s+([^\s]+)\s+HTTP\/"))).Success &&
                  ((lMatchHost = Regex.Match(lData, @"\.\.Host\s*:\s*([\w\d\.]+?)\.\.", RegexOptions.IgnoreCase))).Success)
              {
                lRemotePort = "80";
                lRemoteHost = lMatchHost.Groups[1].Value.ToString();
                lReqString = lMatchURI.Groups[3].Value.ToString();

                lRemoteString = lRemoteHost + ":" + lRemotePort + lReqString;


                //Write2Pipe() : |DNSREQ||00:1B:77:53:5C:F8||192.168.100.117||35976||192.168.100.1||53||wl.tac.ch
              }
              else if (lProto == "DNSREQ" && lDPort == "53")
              {
                lRemoteString = lData;
              }


              /*
               * Browse through patterns to identify the app
               */
              if (lRemoteString.Length > 5)
              {
                foreach (MngApplication.ApplicationPattern lPattern in this.cApplicationPatterns)
                {
                  if (Regex.Match(lRemoteString, @lPattern.ApplicationPatternString).Success)
                  {
                    ApplicationRecord lNewApplication = new ApplicationRecord(lSMAC, lSIP, lDPort, lRemoteHost, lReqString, lPattern.ApplicationName, lPattern.CompanyURL);
                    if (!cApplications.Contains(lNewApplication))
                      cApplications.Add(lNewApplication);

                  } // if (lSplit2.L...
                } //foreach (st...

                //mApplications.Add(new Applications(lSMAC, lSIP, lDPort, lRemoteHost, lReqString, "", lRemoteString));
              } // if (lRemoteString...
            } // if (lSplitte...
          } // if (pData.Leng...
        }
        catch (Exception lEx)
        {
          MessageBox.Show(String.Format("{0} : {1}", Config.PluginName, lEx.ToString()));
        }
      } // if (cIsActi...
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
    private void TSMI_Clear_Click(object sender, EventArgs e)
    {
      cTask.removeAllRecords();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DGV_Applications_MouseUp(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Right)
      {
        try
        {
          DataGridView.HitTestInfo hti = DGV_Applications.HitTest(e.X, e.Y);
          if (hti.RowIndex >= 0)
            CMS_Applications.Show(DGV_Applications, e.Location);
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
      try
      {
        int lCurIndex = DGV_Applications.CurrentCell.RowIndex;
        cTask.removeRecordAt(lCurIndex);
      }
      catch (Exception lEx)
      {
        cHost.LogMessage(lEx.StackTrace);
      }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DGV_Applications_DoubleClick(object sender, EventArgs e)
    {
      MngApplication.Form_ManageApps lManageApps = new MngApplication.Form_ManageApps(cHost);
      lManageApps.ShowDialog();
      readApplicationPatterns();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DGV_Applications_MouseDown(object sender, MouseEventArgs e)
    {
      try
      {
        DataGridView.HitTestInfo hti = DGV_Applications.HitTest(e.X, e.Y);

        if (hti.RowIndex >= 0)
        {
          DGV_Applications.ClearSelection();
          DGV_Applications.Rows[hti.RowIndex].Selected = true;
          DGV_Applications.CurrentCell = DGV_Applications.Rows[hti.RowIndex].Cells[0];
        }
      }
      catch (Exception)
      {
        DGV_Applications.ClearSelection();
      }
    }

    #endregion


    #region OBSERVER INTERFACE METHODS

    public void update(List<ApplicationRecord> pRecordList)
    {
      pRecordList.Clear();
      foreach (ApplicationRecord lTmp in pRecordList)
        cApplications.Add(lTmp);

      DGV_Applications.Refresh();
    }

    #endregion

  }
}
