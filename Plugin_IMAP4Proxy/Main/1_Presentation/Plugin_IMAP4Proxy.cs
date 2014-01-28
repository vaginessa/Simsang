using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Reflection;

using Simsang.Plugin;
using Plugin.Main.IMAP4Proxy;
using Plugin.Main.IMAP4Proxy.Config;


namespace Plugin.Main
{
  [Serializable]
  public struct PluginData
  {
    public String RemoteHost;
    public BindingList<IMAP4Account> Records;
  }



  public partial class PluginIMAP4ProxyUC : UserControl, IPlugin, IObserver
  {

    #region MEMBERS

    private List<String> cTargetList;
    private BindingList<IMAP4Account> cAccounts;
    private List<String> cDataBatch;
    private TaskFacade cTask;
    private DomainFacade cDomain;

    #endregion


    #region PUBLIC

    /// <summary>
    /// 
    /// </summary>
    public PluginIMAP4ProxyUC(PluginParameters pPluginParams)
    {
      InitializeComponent();

      #region DATAGRID HEADER

      DGV_Accounts.AutoGenerateColumns = false;

      DataGridViewTextBoxColumn cMACCol = new DataGridViewTextBoxColumn();
      cMACCol.DataPropertyName = "SrcMAC";
      cMACCol.Name = "SrcMAC";
      cMACCol.HeaderText = "MAC address";
      cMACCol.ReadOnly = true;
      cMACCol.Width = 120;
      //cMACCol.Visible = false;
      DGV_Accounts.Columns.Add(cMACCol);


      DataGridViewTextBoxColumn cSrcIPCol = new DataGridViewTextBoxColumn();
      cSrcIPCol.DataPropertyName = "SrcIP";
      cSrcIPCol.Name = "SrcIP";
      cSrcIPCol.HeaderText = "Source IP";
      cSrcIPCol.Visible = false;
      cSrcIPCol.ReadOnly = true;
      cSrcIPCol.Width = 120;
      DGV_Accounts.Columns.Add(cSrcIPCol);


      DataGridViewTextBoxColumn cDstIPCol = new DataGridViewTextBoxColumn();
      cDstIPCol.DataPropertyName = "DstIP";
      cDstIPCol.Name = "DstIP";
      cDstIPCol.HeaderText = "Destination";
      cDstIPCol.ReadOnly = true;
      cDstIPCol.Width = 200;
      DGV_Accounts.Columns.Add(cDstIPCol);

      DataGridViewTextBoxColumn cDestPortCol = new DataGridViewTextBoxColumn();
      cDestPortCol.DataPropertyName = "DstPort";
      cDestPortCol.Name = "DstPort";
      cDestPortCol.HeaderText = "Service";
      cDestPortCol.ReadOnly = true;
      cDestPortCol.Width = 60;
      DGV_Accounts.Columns.Add(cDestPortCol);


      DataGridViewTextBoxColumn cUserCol = new DataGridViewTextBoxColumn();
      cUserCol.DataPropertyName = "Username";
      cUserCol.Name = "Username";
      cUserCol.HeaderText = "Username";
      cUserCol.ReadOnly = true;
      cUserCol.Width = 150;
      DGV_Accounts.Columns.Add(cUserCol);


      DataGridViewTextBoxColumn cPassCol = new DataGridViewTextBoxColumn();
      cPassCol.DataPropertyName = "Password";
      cPassCol.Name = "Password";
      cPassCol.HeaderText = "Password";
      cPassCol.ReadOnly = true;
      //cPassCol.Width = 120;
      cPassCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      DGV_Accounts.Columns.Add(cPassCol);

      DataGridViewTextBoxColumn cServerCol = new DataGridViewTextBoxColumn();
      cServerCol.DataPropertyName = "Server";
      cServerCol.Name = "Server";
      cServerCol.HeaderText = "Server";
      cServerCol.ReadOnly = true;
      //cServerCol.Width = 120;
      cServerCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      DGV_Accounts.Columns.Add(cServerCol);

      cAccounts = new BindingList<IMAP4Account>();
      DGV_Accounts.DataSource = cAccounts;
      #endregion


      /*
       * Plugin configuration
       */
      T_GUIUpdate.Interval = 1000;
      PluginParameters = pPluginParams;
      String lBaseDir = String.Format(@"{0}\", (pPluginParams!=null) ? pPluginParams.PluginDirectoryFullPath : Directory.GetCurrentDirectory());
      String lSessionDir = (pPluginParams!=null) ? pPluginParams.SessionDirectoryFullPath :  String.Format("{0}sessions", lBaseDir);

      Config = new PluginProperties()
      {
        BaseDir = lBaseDir,
        SessionDir = lSessionDir,
        PluginName = "IMAP4(S) proxy",
        PluginDescription = "IMAP4(S) reverse proxy server to intercept account data.",
        PluginVersion = "0.4",
        Ports = "TCP:993;TCP:143;",
        IsActive = true
      };

      cDataBatch = new List<String>();

      // Make it double buffered.
      typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, DGV_Accounts, new object[] { true });
      T_GUIUpdate.Start();


      /*
       * Proxy server configuration
       */
      ProxyConfig lProxyConfig = new ProxyConfig()
                  {
                    BasisDirectory = Config.BaseDir,
                    SessionDirectory = Config.SessionDir,
                    //RemoteHostName = String.Empty,
                    isDebuggingOn = (PluginParameters.HostApplication != null) ? PluginParameters.HostApplication.IsDebuggingOn() : false,
                    onProxyExit = onIMAP4ProxyExited
                  };
      cTask = TaskFacade.getInstance(lProxyConfig, this);
      cDomain = DomainFacade.getInstance(lProxyConfig, this);
      cDomain.addObserver(this);
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
        List<IMAP4Account> lNewRecords = new List<IMAP4Account>();
        List<String> lNewData;
        String[] lSplitter;
        String lProto; 
        String lSMAC;
        String lSIP;
        String lSPort; 
        String lDIP;
        String lDPort; 
        String lData;
        String lPassword;
        String lServer;

        lock (this)
        {
          lNewData = new List<String>(cDataBatch);
          cDataBatch.Clear();
        } // lock (this)...


        List<IMAP4Account> lNewRecordsBatch = new List<IMAP4Account>();
        foreach (String lEntry in lNewData)
        {
          if (!String.IsNullOrEmpty(lEntry))
          {
            try
            {
              if ((lSplitter = Regex.Split(lEntry, @"\|\|")).Length == 9)
              {
                lProto = lSplitter[0];
                lSMAC = lSplitter[1];
                lSIP = lSplitter[2];
                lSPort = lSplitter[3];
                lDIP = lSplitter[4];
                lDPort = lSplitter[5];
                lData = lSplitter[6];
                lPassword = lSplitter[7];
                lServer = lSplitter[8];

                lNewRecordsBatch.Add(new IMAP4Account(lSMAC, lSIP, lDIP, lDPort, lData, lPassword, lServer));
              } // if (lSplitter...
            }
            catch (Exception)
            {
            }
          } // if (!String...
        } // foreach (Str...

        if (lNewRecordsBatch.Count > 0)
          cTask.addRecord(lNewRecordsBatch);                
      } // if (cData...
    }



    /// <summary>
    /// 
    /// </summary>
    private delegate void setGUIActiveDelegate();
    private void setGUIActive()
    {
      if (InvokeRequired)
      {
        BeginInvoke(new setGUIActiveDelegate(setGUIActive), new object[] { });
        return;
      }

      TB_ForwardHost.Enabled = true;
    }


    /// <summary>
    /// 
    /// </summary>
    private delegate void setGUIInactiveDelegate();
    private void setGUIInactive()
    {
      if (InvokeRequired)
      {
        BeginInvoke(new setGUIInactiveDelegate(setGUIInactive), new object[] { });
        return;
      }

      TB_ForwardHost.Enabled = false;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void onIMAP4ProxyExitedDelegate(); //object sender, System.EventArgs e);
    private void onIMAP4ProxyExited() //object sender, System.EventArgs e)
    {
      if (InvokeRequired)
      {
        BeginInvoke(new onIMAP4ProxyExitedDelegate(onIMAP4ProxyExited), new object[] { /* sender, e */ });
        return;
      } // if (InvokeRequired)

      PluginParameters.HostApplication.PluginSetStatus(this, "red");
      PluginParameters.HostApplication.LogMessage(String.Format("{0}: Stopped for unknown reason", Config.PluginName));
      setGUIActive();
      cTask.onStop();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private String getPluginPath()
    {
      String lRetVal = String.Empty;

      lRetVal = String.Format("{0}", Directory.GetCurrentDirectory(), Config.SessionDir);

      return (lRetVal);
    }

    #endregion


    #region EVENTS

    private void T_GUIUpdate_Tick(object sender, EventArgs e)
    {
      ProcessEntries();
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
      setGUIActive();
      PluginParameters.HostApplication.PluginSetStatus(this, "grey");
      cTask.onInit();
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


        try
        {
          ProxyConfig lProxyConfig = new ProxyConfig
          {
            BasisDirectory = PluginParameters.HostApplication.GetWorkingDirectory(),
            isDebuggingOn = PluginParameters.HostApplication.IsDebuggingOn(),
            onProxyExit = onIMAP4ProxyExited,
            RemoteHostName = TB_ForwardHost.Text
          };

          setGUIInactive();
          cTask.onStart(lProxyConfig);
          PluginParameters.HostApplication.PluginSetStatus(this, "green");
        }
        catch (ExceptionWarning lEx)
        {
          PluginParameters.HostApplication.PluginSetStatus(this, "grey");
          setGUIActive();
          cTask.onStop();

          PluginParameters.HostApplication.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));
        }
        catch (Exception lEx)
        {
          PluginParameters.HostApplication.PluginSetStatus(this, "red");
          setGUIActive();
          cTask.onStop();

          String lMsg = String.Format("{0}: {1}", Config.PluginName, lEx.Message);
          PluginParameters.HostApplication.LogMessage(lMsg);
          MessageBox.Show(lMsg, "Can't start proxy server", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
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


      setGUIActive();
      PluginParameters.HostApplication.PluginSetStatus(this, "grey");
      cTask.onStop();
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



      String lRetVal = String.Empty;
      //int lCounter = 0;
      //int lTmpCounter = 0;

      //if (cDataArray != null && cDataArray.Count > 0)
      //{
      //  foreach (AccountingItem lTmp in cDataArray)
      //  {
      //    if (Int32.TryParse(lTmp.DataVolume, NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out lTmpCounter))
      //      lCounter += lTmpCounter;
      //  } // foreach (Se...
      //} // if (mServices != ...
      //lRetVal = lCounter.ToString();

      return (lRetVal);
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

      cTask.onStop();
    }



    /// <summary>
    /// New input data arrived (Not relevant in this plugin)
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


      cTask.onInit();
      TB_ForwardHost.Text = String.Empty;
      setGUIActive();
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
    /// <param name="pSessionFileName"></param>
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

    #endregion


    #region OBSERVER INTERFACE METHODS

    public void update(List<IMAP4Account> pRecordList)
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
        if (DGV_Accounts.CurrentRow != null && DGV_Accounts.CurrentRow == DGV_Accounts.Rows[DGV_Accounts.Rows.Count - 1])
          lIsLastLine = true;

        lLastPosition = DGV_Accounts.FirstDisplayedScrollingRowIndex;
        lLastRowIndex = DGV_Accounts.Rows.Count - 1;

        if (DGV_Accounts.CurrentCell != null)
          lSelectedIndex = DGV_Accounts.CurrentCell.RowIndex;


        cAccounts.Clear();
        foreach (IMAP4Account lTmp in pRecordList)
          cAccounts.Add(new IMAP4Account(lTmp.SrcMAC, lTmp.SrcIP, lTmp.DstIP, lTmp.DstPort, lTmp.Username, lTmp.Password, lTmp.Server));

        // Selected cell/row
        try
        {
          if (lSelectedIndex >= 0)
            DGV_Accounts.CurrentCell = DGV_Accounts.Rows[lSelectedIndex].Cells[0];
        }
        catch (Exception) { }


        // Reset position
        try
        {
          if (lLastPosition >= 0)
            DGV_Accounts.FirstDisplayedScrollingRowIndex = lLastPosition;
        }
        catch (Exception) { }

        DGV_Accounts.Refresh();
      } // lock (th...
    }

    #endregion


  }
}
