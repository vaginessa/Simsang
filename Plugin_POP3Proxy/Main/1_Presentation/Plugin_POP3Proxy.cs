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

using Simsang.Plugin;
using Plugin.Main.POP3Proxy;
using Plugin.Main.POP3Proxy.Config;


namespace Plugin.Main
{
  [Serializable]
  public struct PluginData
  {
    public String RemoteHost;
    public BindingList<POP3Account> Records;
  }

  public partial class PluginPOP3ProxyUC : UserControl, IPlugin, IObserver
  {

    #region MEMBERS

    private IPluginHost cHost;
    private List<String> cTargetList;
    private BindingList<POP3Account> cAccounts;
    private TaskFacade cTask;

    #endregion


    #region PUBLIC

    /// <summary>
    /// 
    /// </summary>
    public PluginPOP3ProxyUC(PluginParameters pPluginParams)
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

      cAccounts = new BindingList<POP3Account>();
      DGV_Accounts.DataSource = cAccounts;
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
        PluginName = "POP3(S) proxy",
        PluginDescription = "POP3(S) reverse proxy server to intercept account data",
        PluginVersion = "0.5",
        Ports = "TCP:995;TCP:110;",
        IsActive = true
      };


      /*
       * Proxy server configuration.
       */
      ProxyConfig lConfig = new ProxyConfig()
                  {
                    BasisDirectory = Config.BaseDir,
                    //RemoteHostName = String.Empty,
                    isDebuggingOn = (cHost != null) ? cHost.IsDebuggingOn() : false,
                    onProxyExit = null //onPOP3ProxyExited
                  };
      cTask = TaskFacade.getInstance(lConfig, this);
      //      DomainFacade.getInstance(lConfig, this).addObserver(this);
    }

    #endregion


    #region PRIVATE

    /// <summary>
    /// 
    /// </summary>
    private delegate void SetPOP3ProxyOnStoppedDelegate();
    private void SetPOP3ProxyOnStopped()
    {
      if (InvokeRequired)
      {
        BeginInvoke(new SetPOP3ProxyOnStoppedDelegate(SetPOP3ProxyOnStopped), new object[] { });
        return;
      }

      // Kill running proxy servers
      cTask.onStop();

      // Adjust GUI parameters
      TB_POP3Server.Enabled = true;
    }


    /// <summary>
    /// 
    /// </summary>
    private delegate void SetPOP3ProxyOnStartedDelegate();
    private void SetPOP3ProxyOnStarted()
    {
      if (InvokeRequired)
      {
        BeginInvoke(new SetPOP3ProxyOnStartedDelegate(SetPOP3ProxyOnStarted), new object[] { });
        return;
      }

      /*
       * Adjust GUI parameters
       */
      TB_POP3Server.Enabled = false;

      try
      {
        ProxyConfig lConfig = new ProxyConfig()
        {
          BasisDirectory = Directory.GetCurrentDirectory(),
          isDebuggingOn = cHost.IsDebuggingOn(),
          onProxyExit = null, // onPOP3ProxyExited,
          RemoteHostName = TB_POP3Server.Text
        };

        cTask.onStart(lConfig);
      }
      catch (Exception lEx)
      {
        cHost.PluginSetStatus(this, "red");
        SetPOP3ProxyOnStopped();
        cTask.onStop();

        String lMsg = String.Format("Error occurred while starting plugin \"{0}\": {1}", Config.PluginName, lEx.Message);
        cHost.LogMessage(lMsg);
        MessageBox.Show(lMsg, "Can't start proxy server", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
    }



    /// <summary>
    /// 
    /// </summary>
    //public delegate void onPOP3ProxyExitedDelegate();
    //private void onPOP3ProxyExited()
    public delegate void onPOP3ProxyExitedDelegate(object sender, System.EventArgs e);
    private void onPOP3ProxyExited(object sender, System.EventArgs e)
    {
      if (InvokeRequired)
      {
        BeginInvoke(new onPOP3ProxyExitedDelegate(onPOP3ProxyExited), new object[] { sender, e });
        return;
      } // if (InvokeRequired)

      cHost.PluginSetStatus(this, "red");
      cHost.LogMessage(String.Format("{0}: Stopped for unknown reason", Config.PluginName));
      SetPOP3ProxyOnStopped();
    }

    #endregion


    #region PROPERTIES

    public Control PluginControl { get { return (this); } }
    public IPluginHost Host { get { return cHost; } set { cHost = value; cHost.Register(this); } }

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


        cHost.PluginSetStatus(this, "green");
        SetPOP3ProxyOnStarted();
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


      SetPOP3ProxyOnStopped();
      cHost.PluginSetStatus(this, "grey");
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
      SetPOP3ProxyOnStopped();
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

        if (!String.IsNullOrEmpty(pData))
        {
          try
          {
            String[] lSplitter = Regex.Split(pData, @"\|\|");
            if (lSplitter.Length == 9)
            {
              String lProto = lSplitter[0];
              String lSMAC = lSplitter[1];
              String lSIP = lSplitter[2];
              String lSPort = lSplitter[3];
              String lDIP = lSplitter[4];
              String lDPort = lSplitter[5];
              String lData = lSplitter[6];
              String lPassword = lSplitter[7];
              String lServer = lSplitter[8];

              cTask.addRecord(new POP3Account(lSMAC, lSIP, lDIP, lDPort, lData, lPassword, lServer));
            } // if (lSplitter...
          }
          catch (Exception)
          {
          }

        } // if (!String....
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
      TB_POP3Server.Text = String.Empty;
      cHost.PluginSetStatus(this, "grey");
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

    public void update(List<POP3Account> pRecordList)
    {
      pRecordList.Clear();
      foreach (POP3Account lTmp in pRecordList)
        cAccounts.Add(new POP3Account(lTmp.SrcMAC, lTmp.SrcIP, lTmp.DstIP, lTmp.DstPort, lTmp.Username, lTmp.Password, lTmp.Server));

      DGV_Accounts.Refresh();
    }

    #endregion

  }
}
