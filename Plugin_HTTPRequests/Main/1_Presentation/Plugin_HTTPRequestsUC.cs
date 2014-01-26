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
using Simsang.MiniBrowser;
using Plugin.Main.HTTPRequest;


namespace Plugin.Main
{
  public partial class PluginHTTPRequestsUC : UserControl, IPlugin, IObserver
  {

    #region MEMBERS

    private List<String> cTargetList;
    private BindingList<HTTPRequests> cHTTPRequests;
    private const int cMaxTableRows = 128;
    private List<String> cDataBatch;
    private TaskFacade cTask;

    #endregion


    #region PUBLIC

    public PluginHTTPRequestsUC(PluginParameters pPluginParams)
    {
      InitializeComponent();

      #region DATAGRID HEADER

      DataGridViewTextBoxColumn cMACCol = new DataGridViewTextBoxColumn();
      cMACCol.DataPropertyName = "SrcMAC";
      cMACCol.Name = "SrcMAC";
      cMACCol.HeaderText = "MAC address";
      cMACCol.ReadOnly = true;
      cMACCol.Width = 140;
      DGV_HTTPRequests.Columns.Add(cMACCol);


      DataGridViewTextBoxColumn cSrcIPCol = new DataGridViewTextBoxColumn();
      cSrcIPCol.DataPropertyName = "SrcIP";
      cSrcIPCol.Name = "SrcIP";
      cSrcIPCol.HeaderText = "Source IP";
      cSrcIPCol.ReadOnly = true;
      cSrcIPCol.Width = 120;
      DGV_HTTPRequests.Columns.Add(cSrcIPCol);


      DataGridViewTextBoxColumn cTimestampCol = new DataGridViewTextBoxColumn();
      cTimestampCol.DataPropertyName = "Timestamp";
      cTimestampCol.Name = "Timestamp";
      cTimestampCol.HeaderText = "Timestamp";
      cTimestampCol.ReadOnly = true;
      cTimestampCol.Visible = false;
      cTimestampCol.Width = 120;
      DGV_HTTPRequests.Columns.Add(cTimestampCol);


      DataGridViewTextBoxColumn cRequestMethodCol = new DataGridViewTextBoxColumn();
      cRequestMethodCol.DataPropertyName = "Method";
      cRequestMethodCol.Name = "Method";
      cRequestMethodCol.HeaderText = "Method";
      cRequestMethodCol.ReadOnly = true;
      cRequestMethodCol.Visible = true;
      cRequestMethodCol.Width = 60;
      DGV_HTTPRequests.Columns.Add(cRequestMethodCol);


      DataGridViewTextBoxColumn cRemHostCol = new DataGridViewTextBoxColumn();
      cRemHostCol.DataPropertyName = "RemoteHost";
      cRemHostCol.Name = "RemoteHost";
      cRemHostCol.HeaderText = "Server";
      cRemHostCol.ReadOnly = true;
      cRemHostCol.Width = 150;
      DGV_HTTPRequests.Columns.Add(cRemHostCol);

      DataGridViewTextBoxColumn cRemFileNameCol = new DataGridViewTextBoxColumn();
      cRemFileNameCol.DataPropertyName = "RemoteFile";
      cRemFileNameCol.Name = "RemoteFile";
      cRemFileNameCol.HeaderText = "File name";
      cRemFileNameCol.ReadOnly = true;
      cRemFileNameCol.Width = 216;// 173;
      cRemFileNameCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      DGV_HTTPRequests.Columns.Add(cRemFileNameCol);


      DataGridViewTextBoxColumn cURLCol = new DataGridViewTextBoxColumn();
      cURLCol.DataPropertyName = "URL";
      cURLCol.Name = "URL";
      cURLCol.HeaderText = "URL";
      cURLCol.Visible = false;
      DGV_HTTPRequests.Columns.Add(cURLCol);


      DataGridViewTextBoxColumn cCookiesCol = new DataGridViewTextBoxColumn();
      cCookiesCol.DataPropertyName = "SessionCookies";
      cCookiesCol.Name = "SessionCookies";
      cCookiesCol.HeaderText = "Cookies";
      cCookiesCol.Visible = false;
      DGV_HTTPRequests.Columns.Add(cCookiesCol);


      DataGridViewTextBoxColumn cRequestCol = new DataGridViewTextBoxColumn();
      cRequestCol.DataPropertyName = "Request";
      cRequestCol.Name = "Request";
      cRequestCol.HeaderText = "Request";
      cRequestCol.Visible = false;
      DGV_HTTPRequests.Columns.Add(cRequestCol);

      cHTTPRequests = new BindingList<HTTPRequests>();
      DGV_HTTPRequests.DataSource = cHTTPRequests;

      #endregion


      /*
       * Plugin configuration
       */
      PluginParameters = pPluginParams;
      String lBaseDir = String.Format(@"{0}\", (pPluginParams != null) ? pPluginParams.PluginDirectoryFullPath : Directory.GetCurrentDirectory());
      String lSessionDir = (pPluginParams != null) ? pPluginParams.SessionDirectoryFullPath : String.Format("{0}sessions", lBaseDir);

      Config = new PluginProperties()
      {
        BaseDir = lBaseDir,
        SessionDir = lSessionDir,
        PluginName = "HTTP requests",
        PluginDescription = "Listing client systems HTTP requests.",
        PluginVersion = "0.7",
        Ports = "TCP:80;TCP:443;",
        IsActive = true
      };


      cDataBatch = new List<String>();

      // Make it double buffered.
      typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, DGV_HTTPRequests, new object[] { true });
      T_GUIUpdate.Start();

      cTask = TaskFacade.getInstance(this);
      DomainFacade.getInstance(this).addObserver(this);
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
        List<HTTPRequests> lNewRecords = new List<HTTPRequests>();
        List<String> lNewData;
        bool lIsLastLine = false;
        int lLastPosition = -1;
        int lLastRowIndex = -1;
        int lSelectedIndex = -1;


        /*
         * Remember DGV positions
         */
        if (DGV_HTTPRequests.CurrentRow != null && DGV_HTTPRequests.CurrentRow == DGV_HTTPRequests.Rows[DGV_HTTPRequests.Rows.Count - 1])
          lIsLastLine = true;

        lLastPosition = DGV_HTTPRequests.FirstDisplayedScrollingRowIndex;
        lLastRowIndex = DGV_HTTPRequests.Rows.Count - 1;

        if (DGV_HTTPRequests.CurrentCell != null)
          lSelectedIndex = DGV_HTTPRequests.CurrentCell.RowIndex;


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
              String[] lSplit = Regex.Split(lEntry, @"\|\|");
              if (lSplit.Length > 0)
              {
                String lProto = lSplit[0];
                String lMAC = lSplit[1];
                String lSrcIP = lSplit[2];
                String lSrcPort = lSplit[3];
                String lDstIP = lSplit[4];
                String lDstPort = lSplit[5];
                String lData = lSplit[6];
                Match lMatchURI;
                Match lMatchHost;
                Match lMatchCookies;
                String lMethod = String.Empty;
                String lRemoteHost = String.Empty;
                String lReqString = String.Empty;
                String lCookies = String.Empty;


                if (((lMatchURI = Regex.Match(lData, @"(\s+|^)(GET|POST)\s+([^\s]+)\s+HTTP\/"))).Success &&
                    ((lMatchHost = Regex.Match(lData, @"\.\.Host\s*:\s*([\w\d\.]+?)\.\.", RegexOptions.IgnoreCase))).Success &&
                    ((lMatchCookies = Regex.Match(lData, @"\.\.Cookie\s*:\s*(.*?)(\.\.|$)", RegexOptions.IgnoreCase))).Success)
                {
                  lMethod = lMatchURI.Groups[2].Value.ToString();
                  lRemoteHost = lMatchHost.Groups[1].Value.ToString();
                  lReqString = lMatchURI.Groups[3].Value.ToString();
                  lCookies = lMatchCookies.Groups[1].Value.ToString();


                  lNewRecords.Add(new HTTPRequests(lMAC, lSrcIP, lMethod, lRemoteHost, lReqString, lCookies, lData));

                  try
                  {
                    cHTTPRequests.Add(new HTTPRequests(lMAC, lSrcIP, lMethod, lRemoteHost, lReqString, lCookies, lData));
                    if (cHTTPRequests.Count > cMaxTableRows)
                      cHTTPRequests.RemoveAt(0);
                  }
                  catch (Exception lEx)
                  {
                    PluginParameters.HostApplication.LogMessage(lEx.StackTrace);
                  }


                } // if (lDstPort == "80" ...
              } // if (lSpli...
            } // if (pData.Le...
          }
          catch (Exception lEx)
          {
            MessageBox.Show(String.Format("{0} : {1}", Config.PluginName, lEx.ToString()));
          }
        } // for (String lE...

        cTask.addRecords(lNewRecords);

        //UseFilter();

        /*
         * Filter
         */
        try
        {
          if (!CompareToFilter(DGV_HTTPRequests.Rows[lLastRowIndex + 1].Cells["URL"].Value.ToString()))
            DGV_HTTPRequests.Rows[lLastRowIndex + 1].Visible = false;
        }
        catch { }

        try
        {
          if (lIsLastLine)
          {
            DGV_HTTPRequests.Rows[DGV_HTTPRequests.Rows.Count - 1].Selected = true;
            DGV_HTTPRequests.FirstDisplayedScrollingRowIndex = lLastPosition + 1;
          }
          else
            DGV_HTTPRequests.FirstDisplayedScrollingRowIndex = lLastPosition;
        }
        catch { }

        if (lSelectedIndex >= 0)
          DGV_HTTPRequests.CurrentCell = DGV_HTTPRequests.Rows[lSelectedIndex].Cells[0];

        DGV_HTTPRequests.Refresh();
      } // if (cDat...
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pInputData"></param>
    /// <returns></returns>
    private bool CompareToFilter(String pInputData)
    {
      bool lRetVal = false;

      if (pInputData != null && pInputData.Length > 0)
        if (Regex.Match(pInputData, Regex.Escape(TB_Filter.Text), RegexOptions.IgnoreCase).Success)
          lRetVal = true;

      return (lRetVal);
    }



    /// <summary>
    /// 
    /// </summary>
    private void UseFilter()
    {
      // without this line we will get an exception :/ da fuq!

      DGV_HTTPRequests.CurrentCell = null;
      for (int lCounter = 0; lCounter < DGV_HTTPRequests.RowCount; lCounter++)
      {
        try
        {
          // this.dataGridView1.Columns["CustomerID"].Visible = false;
          if (TB_Filter.Text.Length <= 0)
            DGV_HTTPRequests.Rows[lCounter].Visible = true;
          else
          {
            try
            {
              String lData = DGV_HTTPRequests.Rows[lCounter].Cells["URL"].Value.ToString();

              if (!Regex.Match(lData, Regex.Escape(TB_Filter.Text), RegexOptions.IgnoreCase).Success)
                DGV_HTTPRequests.Rows[lCounter].Visible = false;
              else
                DGV_HTTPRequests.Rows[lCounter].Visible = true;
            }
            catch (Exception lEx)
            {
              PluginParameters.HostApplication.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));     
            }
          }
        }
        catch (Exception lEx)
        {
          PluginParameters.HostApplication.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));     
        }
      }

      DGV_HTTPRequests.Refresh();
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
      } // if (cIsActi...
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
    public delegate void onShutDownDelegate();
    public void onShutDown()
    {
      if (InvokeRequired)
      {
        BeginInvoke(new onShutDownDelegate(onShutDown), new object[] { });
        return;
      } // if (InvokeS
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
    /// Remove session file with serialized data. 
    /// </summary>
    /// <param name="pSessionFileName"></param>
    public delegate void onDeleteSessionDataDelegate(String pSessionID);
    public void onDeleteSessionData(String pSessionID)
    {
      if (InvokeRequired)
      {
        BeginInvoke(new onDeleteSessionDataDelegate(onDeleteSessionData), new object[] { pSessionID });
        return;
      } // if (InvokeRequired)

      try
      {
        cTask.deleteSession(pSessionID);
      }
      catch (Exception lEx)
      {
        PluginParameters.HostApplication.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));
      }
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
      cTask.emptyRequestList();
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


        try
        {
          cTask.saveSession(pSessionName);
        }
        catch (Exception lEx)
        {
          PluginParameters.HostApplication.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));
        }
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
        if (DGV_HTTPRequests.InvokeRequired)
        {
          BeginInvoke(new onNewDataDelegate(onNewData), new object[] { pData });
          return;
        } // if (InvokeRequired)


        lock (this)
        {
          if (cDataBatch != null && pData != null && pData.Length > 0)
          {
            cDataBatch.Add(pData);
          }
        } // lock (this)
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
      cTask.emptyRequestList();
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DGV_HTTPRequests_MouseUp(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Right)
      {
        try
        {
          DataGridView.HitTestInfo hti = DGV_HTTPRequests.HitTest(e.X, e.Y);
          if (hti.RowIndex >= 0)
            CMS_HTTPRequests.Show(DGV_HTTPRequests, e.Location);
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
    private void BT_Set_Click(object sender, EventArgs e)
    {
      UseFilter();
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
    private void DGV_HTTPRequests_DoubleClick(object sender, EventArgs e)
    {
      try
      {
        BindingList<HTTPRequests> lTmpHosts = new BindingList<HTTPRequests>();
        int lCurIndex = DGV_HTTPRequests.CurrentCell.RowIndex;
        String lRequest = DGV_HTTPRequests.Rows[lCurIndex].Cells["Request"].Value.ToString();

        lRequest = Regex.Replace(lRequest, @"\.\.", "\r\n");
        ShowRequest lRequestDetails = new ShowRequest(lRequest);
        lRequestDetails.ShowDialog();

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
    private void T_GUIUpdate_Tick(object sender, EventArgs e)
    {
      ProcessEntries();
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
        BindingList<HTTPRequests> lTmpHosts = new BindingList<HTTPRequests>();
        int lCurIndex = DGV_HTTPRequests.CurrentCell.RowIndex;
        //String lHostName = DGV_HTTPRequests.Rows[lCurIndex].Cells["RemoteHost"].Value.ToString();

        cTask.removeElementAt(lCurIndex);
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
    private void DGV_HTTPRequests_MouseDown(object sender, MouseEventArgs e)
    {
      try
      {
        DataGridView.HitTestInfo hti = DGV_HTTPRequests.HitTest(e.X, e.Y);

        if (hti.RowIndex >= 0)
        {
          DGV_HTTPRequests.ClearSelection();
          DGV_HTTPRequests.Rows[hti.RowIndex].Selected = true;
          DGV_HTTPRequests.CurrentCell = DGV_HTTPRequests.Rows[hti.RowIndex].Cells[0];
        }
      }
      catch (Exception lEx)
      {
        PluginParameters.HostApplication.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));     
        DGV_HTTPRequests.ClearSelection();
      }
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void requestDetailsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      String lURL = String.Empty;
      String lCookie = String.Empty;
      String lSrcIP = String.Empty;
      String lUserAgent = String.Empty;

      try
      {
        lURL = DGV_HTTPRequests.SelectedRows[0].Cells["URL"].Value.ToString();
        lCookie = DGV_HTTPRequests.SelectedRows[0].Cells["SessionCookies"].Value.ToString();
        lSrcIP = DGV_HTTPRequests.SelectedRows[0].Cells["SrcIP"].Value.ToString();
        lUserAgent = String.Empty; //DGV_HTTPRequests.SelectedRows[0].Cells[1].Value.ToString();
      }
      catch (ArgumentOutOfRangeException lEx)
      {
        PluginParameters.HostApplication.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));     
        return;
      }
      catch (Exception lEx)
      {
        PluginParameters.HostApplication.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));     
      }


      try
      {
        Browser lMiniBrowser = new Browser(lURL, lCookie, lSrcIP, lUserAgent);
        lMiniBrowser.Show(); // Dialog();
      }
      catch (Exception lEx)
      {
        PluginParameters.HostApplication.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));     
        MessageBox.Show("MiniBrowser unexpectedly crashed : " + lEx.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    #endregion


    #region OBSERVER INTERFACE METHODS

    public void update(List<HTTPRequests> pHTTPReqList)
    {
      cHTTPRequests.Clear();
      foreach (HTTPRequests lTmp in pHTTPReqList)
        cHTTPRequests.Add(lTmp);
    }


    #endregion

  }
}
