using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Configuration;
using System.Reflection;

using Simsang.Plugin;
using Plugin.Main.DNSRequest;
using Plugin.Main.DNSRequest.Config;


namespace Plugin.Main
{
  public partial class PluginDNSRequestsUC : UserControl, IPlugin, IObserver
  {

    #region MEMBERS

    private List<String> cTargetList;
    private BindingList<DNSRequestRecord> cDNSRequests;
    private List<String> cDataBatch;
    private TaskFacade cTask;
    private DomainFacade cDomain;

    #endregion


    #region PUBLIC

    /// <summary>
    /// 
    /// </summary>
    public PluginDNSRequestsUC(PluginParameters pPluginParams)
    {
      InitializeComponent();

      #region DATAGRID HEADERS

      DataGridViewTextBoxColumn cMACCol = new DataGridViewTextBoxColumn();
      cMACCol.DataPropertyName = "SrcMAC";
      cMACCol.Name = "SrcMAC";
      cMACCol.HeaderText = "MAC address";
      cMACCol.ReadOnly = true;
      cMACCol.Width = 140;
      DGV_DNSRequests.Columns.Add(cMACCol);


      DataGridViewTextBoxColumn cSrcIPCol = new DataGridViewTextBoxColumn();
      cSrcIPCol.DataPropertyName = "SrcIP";
      cSrcIPCol.Name = "SrcIP";
      cSrcIPCol.HeaderText = "Source IP";
      cSrcIPCol.ReadOnly = true;
      cSrcIPCol.Width = 120;
      DGV_DNSRequests.Columns.Add(cSrcIPCol);


      DataGridViewTextBoxColumn cTimestampCol = new DataGridViewTextBoxColumn();
      cTimestampCol.DataPropertyName = "Timestamp";
      cTimestampCol.Name = "Timestamp";
      cTimestampCol.HeaderText = "Timestamp";
      cTimestampCol.ReadOnly = true;
      cTimestampCol.Width = 120;
      DGV_DNSRequests.Columns.Add(cTimestampCol);


      DataGridViewTextBoxColumn cRemHostCol = new DataGridViewTextBoxColumn();
      cRemHostCol.DataPropertyName = "DNSHostname";
      cRemHostCol.Name = "DNSHostname";
      cRemHostCol.HeaderText = "DNS request";
      cRemHostCol.ReadOnly = true;
      cRemHostCol.Width = 180;
      cRemHostCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      DGV_DNSRequests.Columns.Add(cRemHostCol);

      DataGridViewTextBoxColumn cPacketTypeCol = new DataGridViewTextBoxColumn();
      cPacketTypeCol.DataPropertyName = "PacketType";
      cPacketTypeCol.Name = "PacketType";
      cPacketTypeCol.HeaderText = "Packet type";
      cPacketTypeCol.ReadOnly = true;
      //cRemHostCol.Width = 280;
      cPacketTypeCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      DGV_DNSRequests.Columns.Add(cPacketTypeCol);

      cDNSRequests = new BindingList<DNSRequestRecord>();
      DGV_DNSRequests.DataSource = cDNSRequests;

      #endregion


      /*
       * Plugin configuration
       */
      T_GUIUpdate.Interval = 1000;
      PluginParameters = pPluginParams;
      String lBaseDir = String.Format(@"{0}\", (pPluginParams != null) ? pPluginParams.PluginDirectoryFullPath : Directory.GetCurrentDirectory());
      String lSessionDir = (pPluginParams != null) ? pPluginParams.SessionDirectoryFullPath : String.Format("{0}sessions", lBaseDir);

      Config = new PluginProperties()
      {
        BaseDir = lBaseDir,
        SessionDir = lSessionDir,
        PluginName = "DNS requests",
        PluginDescription = "Listing client systems DNS requests.",
        PluginVersion = "0.6",
        Ports = "UDP:53;",
        IsActive = true
      };

      cDataBatch = new List<String>();

      // Make it double buffered.
      typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, DGV_DNSRequests, new object[] { true });
      T_GUIUpdate.Start();

      cTask = TaskFacade.getInstance(this);
      cDomain = DomainFacade.getInstance(this);

      cDomain.addObserver(this);
    }

    #endregion


    #region PROPERTIES

    public Control PluginControl { get { return (this); } }
    public PluginParameters PluginParameters { get; private set; }

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

      PluginParameters.HostApplication.Register(this);
      PluginParameters.HostApplication.PluginSetStatus(this, "grey");
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

        PluginParameters.HostApplication.PluginSetStatus(this, "green");
      } // if (cIsActive)
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

      PluginParameters.HostApplication.PluginSetStatus(this, "grey");
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
        PluginParameters.HostApplication.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));
      }


      try
      {
        cTask.loadSessionData(pSessionName);
      }
      catch (Exception lEx)
      {
        PluginParameters.HostApplication.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));
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

      try
      {
        cTask.loadSessionDataFromString(pSessionData);
      }
      catch (Exception lEx)
      {
        PluginParameters.HostApplication.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));
      }
    }




    /// <summary>
    /// Remove session file with serialized data.
    /// </summary>
    /// <param name="pSessionFileName"></param>
    public delegate void onDeleteSessionDataDelegate(String pSessionName);
    public void onDeleteSessionData(String pSessionName)
    {
      if (InvokeRequired)
      {
        BeginInvoke(new onDeleteSessionDataDelegate(onDeleteSessionData), new object[] { pSessionName });
        return;
      } // if (InvokeRequired)


      try
      {
        cTask.deleteSession(pSessionName);
      }
      catch (Exception lEx)
      {
        PluginParameters.HostApplication.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));
      }
    }



    /// <summary>
    /// Serialize session data
    /// </summary>
    /// <param name="pSessionName"></param>
    public delegate void onSaveSessionDataDelegate(string pSessionName);
    public void onSaveSessionData(string pSessionName)
    {
      if (Config.IsActive)
      {
        if (InvokeRequired)
        {
          BeginInvoke(new onSaveSessionDataDelegate(onSaveSessionData), new object[] { pSessionName });
          return;
        } // if (InvokeRequired)

        try
        {
          cTask.saveSession(pSessionName);
        }
        catch (Exception lEx)
        {
          PluginParameters.HostApplication.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));
        }
      } // if (cIsActive)
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
    public delegate String onGetSessionDataDelegate(String pSessionName);
    public String onGetSessionData(String pSessionName)
    {
      if (InvokeRequired)
      {
        BeginInvoke(new onGetSessionDataDelegate(onGetSessionData), new object[] { pSessionName });
        return (String.Empty);
      } // if (InvokeRequired)

      String lRetVal = String.Empty;

      try
      {
        lRetVal = cTask.getSessionData(pSessionName);
      }
      catch (Exception lEx)
      {
        PluginParameters.HostApplication.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));
      }

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

      TB_Filter.Text = String.Empty;

      cTask.clearRecordList();

      PluginParameters.HostApplication.PluginSetStatus(this, "grey");
    }


    /// <summary>
    /// 
    /// </summary>
    public void onShutDown()
    {
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
      } // if (cIsActive)
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


    #region PRIVATE


    /// <summary>
    /// 
    /// </summary>
    public void ProcessEntries()
    {
      if (cDataBatch != null && cDataBatch.Count > 0)
      {
        List<DNSRequestRecord> lNewRecords = new List<DNSRequestRecord>();
        List<String> lNewData;
        String[] lSplitter;
        String lProto; 
        String lSMAC;
        String lSIP;
        String lSrcPort; 
        String lDstIP;
        String lDstPort;
        String lHostName;

        lock (this)
        {
          lNewData = new List<String>(cDataBatch);
          cDataBatch.Clear();
        } // lock (this)...


        foreach (String lEntry in lNewData)
        {
          try
          {
            if (!String.IsNullOrEmpty(lEntry))
            {
              if ((lSplitter = Regex.Split(lEntry, @"\|\|")).Length == 7)
              {
                lProto = lSplitter[0];
                lSMAC = lSplitter[1];
                lSIP = lSplitter[2];
                lSrcPort = lSplitter[3];
                lDstIP = lSplitter[4];
                lDstPort = lSplitter[5];
                lHostName = lSplitter[6];

                if (lDstPort != null && lDstPort == "53")
                  lNewRecords.Add(new DNSRequestRecord(lSMAC, lSIP, lHostName, lProto));
              } // if (lSplitter...
            } // if (pData.Le... 
          }
          catch (Exception lEx)
          {
            if (PluginParameters.HostApplication != null)
              PluginParameters.HostApplication.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));
          }
        } // foreach(...

        if (lNewRecords.Count > 0)
          cTask.addRecord(lNewRecords);

      } // if (cDataBatch...
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pInputData"></param>
    /// <returns></returns>
    private bool CompareToFilter(String pInputData)
    {
      bool lRetVal = false;

      if (Regex.Match(pInputData, @TB_Filter.Text, RegexOptions.IgnoreCase).Success)
        lRetVal = true;

      return (lRetVal);
    }


    /// <summary>
    /// 
    /// </summary>
    private void UseFilter()
    {
      // without this line we will get an exception :/ da fuq!
      DGV_DNSRequests.CurrentCell = null;
      for (int lCounter = 0; lCounter < DGV_DNSRequests.Rows.Count; lCounter++)
      {
        if (TB_Filter.Text.Length <= 0)
        {
          DGV_DNSRequests.Rows[lCounter].Visible = true;
        }
        else
        {
          try
          {
            String lData = DGV_DNSRequests.Rows[lCounter].Cells["DNSHostname"].Value.ToString();
            if (!Regex.Match(lData, Regex.Escape(TB_Filter.Text), RegexOptions.IgnoreCase).Success)
              DGV_DNSRequests.Rows[lCounter].Visible = false;
            else
              DGV_DNSRequests.Rows[lCounter].Visible = true;
          }
          catch (Exception lEx)
          {
            PluginParameters.HostApplication.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));
          }
        }
      }

      DGV_DNSRequests.Refresh();
    }

    #endregion


    #region EVENTS

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BT_Set_Click(object sender, EventArgs e)
    {
      UseFilter();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TSMI_Clear_Click(object sender, EventArgs e)
    {
      cTask.clearRecordList();
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DGV_DNSRequests_MouseUp(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Right)
      {
        try
        {
          DataGridView.HitTestInfo hti = DGV_DNSRequests.HitTest(e.X, e.Y);
          if (hti.RowIndex >= 0)
            CMS_DNSRequests.Show(DGV_DNSRequests, e.Location);
        }
        catch (Exception lEx)
        {
          PluginParameters.HostApplication.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));
        }
      }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TB_Filter_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Enter)
      {
        UseFilter();
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
        int lCurIndex = DGV_DNSRequests.CurrentCell.RowIndex;
        cTask.removeRecordAt(lCurIndex);
      }
      catch (Exception lEx)
      {
        PluginParameters.HostApplication.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));
      }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void copyHostNameToolStripMenuItem_Click(object sender, EventArgs e)
    {
      try
      {
        BindingList<DNSRequestRecord> lTmpHosts = new BindingList<DNSRequestRecord>();
        int lCurIndex = DGV_DNSRequests.CurrentCell.RowIndex;
        String lHostName = DGV_DNSRequests.Rows[lCurIndex].Cells["DNSHostname"].Value.ToString();
        Clipboard.SetText(lHostName);
      }
      catch (Exception lEx)
      {
        PluginParameters.HostApplication.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));
      }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DGV_DNSRequests_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
      try
      {
        BindingList<DNSRequestRecord> lTmpHosts = new BindingList<DNSRequestRecord>();
        int lCurIndex = DGV_DNSRequests.CurrentCell.RowIndex;
        String lHostName = DGV_DNSRequests.Rows[lCurIndex].Cells["DNSHostname"].Value.ToString();
        Clipboard.SetText(lHostName);
      }
      catch (Exception lEx)
      {
        PluginParameters.HostApplication.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));
      }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DGV_DNSRequests_MouseDown(object sender, MouseEventArgs e)
    {
      try
      {
        DataGridView.HitTestInfo hti = DGV_DNSRequests.HitTest(e.X, e.Y);

        if (hti.RowIndex >= 0)
        {
          DGV_DNSRequests.ClearSelection();
          DGV_DNSRequests.Rows[hti.RowIndex].Selected = true;
          DGV_DNSRequests.CurrentCell = DGV_DNSRequests.Rows[hti.RowIndex].Cells[0];
        }
      }
      catch (Exception lEx)
      {
        PluginParameters.HostApplication.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));
        DGV_DNSRequests.ClearSelection();
      }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void T_GUIUpdate_Tick(object sender, EventArgs e)
    {
      ProcessEntries();
    }

    #endregion


    #region OBSERVER INTERFACE METHODS

    public void update(BindingList<DNSRequestRecord> pDNSReqList)
    {
      int lLastPosition = -1;
      int lLastRowIndex = -1;
      int lSelectedIndex = -1;
      bool lIsLastLine = false;


      if (pDNSReqList != null)
      {
        if (DGV_DNSRequests.CurrentRow != null && DGV_DNSRequests.CurrentRow == DGV_DNSRequests.Rows[DGV_DNSRequests.Rows.Count - 1])
          lIsLastLine = true;

        lock (this)
        {
          /*
           * Remember last position
           */
          lLastPosition = DGV_DNSRequests.FirstDisplayedScrollingRowIndex;
          lLastRowIndex = DGV_DNSRequests.Rows.Count - 1;

          if (DGV_DNSRequests.CurrentCell != null)
            lSelectedIndex = DGV_DNSRequests.CurrentCell.RowIndex;

          DGV_DNSRequests.SuspendLayout();
          cDNSRequests.Clear();
          foreach (DNSRequestRecord lTmp in pDNSReqList)
            cDNSRequests.Add(lTmp);


          // Filter
          try
          {
            if (!CompareToFilter(DGV_DNSRequests.Rows[lLastRowIndex + 1].Cells["DNSHostname"].Value.ToString()))
              DGV_DNSRequests.Rows[lLastRowIndex + 1].Visible = false;
          }
          catch (Exception) { }

          // Selected cell/row
          try
          {
            if (lSelectedIndex >= 0)
              DGV_DNSRequests.CurrentCell = DGV_DNSRequests.Rows[lSelectedIndex].Cells[0];
          }
          catch (Exception) { }


          // Reset position
          try
          {
            if (lLastPosition >= 0)
              DGV_DNSRequests.FirstDisplayedScrollingRowIndex = lLastPosition;
          }
          catch (Exception) { }

        }

        DGV_DNSRequests.ResumeLayout();

        DGV_DNSRequests.Refresh();
      } // if (pDNSReqL...
    }


    #endregion


  }
}
