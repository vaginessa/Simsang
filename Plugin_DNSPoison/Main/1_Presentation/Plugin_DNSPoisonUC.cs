using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Configuration;

using Simsang.Plugin;
using Plugin.Main.DNSPoison;



namespace Plugin.Main
{
  public partial class PluginDNSPoisonUC : UserControl, IPlugin, IObserver
  {

    #region MEMBERS

    private IPluginHost cHost;
    private List<String> cTargetList;
    private BindingList<DNSPoisonRecord> cDNSPoisonRecords;
    private TaskFacade cTask;

    #endregion


    #region PUBLIC

    public PluginDNSPoisonUC(PluginParameters pPluginParams)
    {
      InitializeComponent();

      #region DATAGRID HEADERS

      DataGridViewTextBoxColumn cHostNameCol = new DataGridViewTextBoxColumn();
      cHostNameCol.DataPropertyName = "HostName";
      cHostNameCol.Name = "HostName";
      cHostNameCol.HeaderText = "Host name";
      cHostNameCol.ReadOnly = true;
      cHostNameCol.Width = 296;
      DGV_Spoofing.Columns.Add(cHostNameCol);


      DataGridViewTextBoxColumn cIPAddressCol = new DataGridViewTextBoxColumn();
      cIPAddressCol.DataPropertyName = "IPAddress";
      cIPAddressCol.Name = "IPAddress";
      cIPAddressCol.HeaderText = "Spoofed IP address";
      cIPAddressCol.ReadOnly = true;
      cIPAddressCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      DGV_Spoofing.Columns.Add(cIPAddressCol);

      cDNSPoisonRecords = new BindingList<DNSPoisonRecord>();
      DGV_Spoofing.DataSource = cDNSPoisonRecords;


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
        PluginName = "DNS Poison",
        PluginDescription = "Poisoning systems DNS request and servers DNS responses.",
        PluginVersion = "0.11",
        Ports = "",
        IsActive = true
      };


      // Get object instance from the lower layer
      cTask = TaskFacade.getInstance(this);

      // Register at the observable
      DomainFacade.getInstance(this).addObserver(this);
    }

    #endregion


    #region PRIVATE

    /// <summary>
    /// 
    /// </summary>
    private delegate void SetDNSHijackBTOnStartedDelegate();
    private void SetDNSHijackBTOnStarted()
    {
      if (InvokeRequired)
      {
        BeginInvoke(new SetDNSHijackBTOnStartedDelegate(SetDNSHijackBTOnStarted), new object[] { });
        return;
      }

      TB_Address.Enabled = false;
      TB_Host.Enabled = false;
      BT_Add.Enabled = false;
      DGV_Spoofing.Enabled = false;
    }


    /// <summary>
    /// 
    /// </summary>
    private delegate void SetDNSHijackBTOnStoppedDelegate();
    private void SetDNSHijackBTOnStopped()
    {
      if (InvokeRequired)
      {
        BeginInvoke(new SetDNSHijackBTOnStoppedDelegate(SetDNSHijackBTOnStopped), new object[] { });
        return;
      }

      TB_Address.Enabled = true;
      TB_Host.Enabled = true;
      BT_Add.Enabled = true;
      DGV_Spoofing.Enabled = true;
    }



    /// <summary>
    /// Poisoning exited.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnDNSHijackExited(object sender, System.EventArgs e)
    {
      SetDNSHijackBTOnStopped();
    }

    #endregion


    #region PROPERTIES

    public Control PluginControl { get { return (this); } }
    public IPluginHost Host { get { return cHost; } set { cHost = value; cHost.Register(this); } }

    #endregion


    #region IPlugin INTERFACE MEMBERS

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
      } // if (InvokeRequired)

      SetDNSHijackBTOnStopped();
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
      } // if (cIsAc...
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


      TB_Address.Text = cHost.GetCurrentIP();
      TB_Host.Text = String.Empty;

      cTask.clearRecordList();
      cHost.PluginSetStatus(this, "grey");
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


        if (cDNSPoisonRecords != null && cDNSPoisonRecords.Count > 0)
        {
          cHost.PluginSetStatus(this, "green");
          String lPoisoningHostsPath = cHost.GetDNSPoisoningHostsFile();
          String lDNSPoisoningHosts = String.Empty;


          /*
           * Write DNS poisoning host list to file
           */
          if (lPoisoningHostsPath != null)
          {
            if (File.Exists(lPoisoningHostsPath))
              File.Delete(lPoisoningHostsPath);


            foreach (DNSPoisonRecord lTmp in cDNSPoisonRecords.ToList())
              lDNSPoisoningHosts += String.Format("{0},{1}\r\n", lTmp.HostName, lTmp.IPAddress);

            using (StreamWriter outfile = new StreamWriter(lPoisoningHostsPath))
            {
              outfile.Write(lDNSPoisoningHosts);
            } // using (Strea...
          } // if (mFWRules...
          SetDNSHijackBTOnStarted();
        }
        else
        {
          cHost.LogMessage(String.Format("{0}: No rule defined. Stopping the pluggin.", Config.PluginName));
          cHost.PluginSetStatus(this, "grey");
        } // if (lFWRulesPath...
      } // if (cIsAct...
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


      String lFWRulesPath = cHost.GetDNSPoisoningHostsFile();

      if (File.Exists(lFWRulesPath))
        File.Delete(lFWRulesPath);

      SetDNSHijackBTOnStopped();
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
        return (String.Empty);
      } // if (InvokeRequired)

      return (String.Empty);
    }

    #endregion


    #region SESSIONS

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
        cHost.LogMessage(lEx.StackTrace);
      }

      return (lRetVal);
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
        cTask.loadSessionData(pSessionName);
      }
      catch (Exception lEx)
      {
        cHost.LogMessage(lEx.Message);
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
        cHost.LogMessage(lEx.StackTrace);
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
          cHost.LogMessage(lEx.Message);
        }
      } // if (cIsActi...
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

      try
      {
        cTask.deleteSession(pSessionName);
      }
      catch (Exception lEx)
      {
        cHost.LogMessage(lEx.StackTrace);
      }
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
    private void BT_Add_Click(object sender, EventArgs e)
    {
      String lHostName = TB_Host.Text.Trim();
      String lIPAddress = TB_Address.Text.Trim();

      try
      {
        cTask.addRecord(lHostName, lIPAddress);
      }
      catch (Exception lEx)
      {
        cHost.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));
        MessageBox.Show(String.Format("An error occurred while adding a record.\r\n{0}", lEx.Message), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DGV_Spoofing_MouseUp(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Right)
      {
        try
        {
          DataGridView.HitTestInfo hti = DGV_Spoofing.HitTest(e.X, e.Y);
          if (hti.RowIndex >= 0)
            CMS_DNSPoison.Show(DGV_Spoofing, e.Location);
        }
        catch (Exception lEx) 
        {
          cHost.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));
        }
      }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TSMI_Delete_Click(object sender, EventArgs e)
    {
      try
      {
        BindingList<DNSPoisonRecord> lTmpHosts = new BindingList<DNSPoisonRecord>();
        int lCurIndex = DGV_Spoofing.CurrentCell.RowIndex;
        String lHostName = DGV_Spoofing.Rows[lCurIndex].Cells["HostName"].Value.ToString();

        cTask.removeRecordAt(lHostName);
      }
      catch (Exception lEx) 
      {
        cHost.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));
      }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DGV_Spoofing_MouseDown(object sender, MouseEventArgs e)
    {
      try
      {
        DataGridView.HitTestInfo hti = DGV_Spoofing.HitTest(e.X, e.Y);

        if (hti.RowIndex >= 0)
        {
          DGV_Spoofing.ClearSelection();
          DGV_Spoofing.Rows[hti.RowIndex].Selected = true;
          DGV_Spoofing.CurrentCell = DGV_Spoofing.Rows[hti.RowIndex].Cells[0];
        }
      }
      catch (Exception)
      {
        DGV_Spoofing.ClearSelection();
      }
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PluginDNSPoisonUC_Load(object sender, EventArgs e)
    {
      if (TB_Address.Text.Length == 0)
        TB_Address.Text = cHost.GetCurrentIP();
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TB_Host_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Enter)
      {
        e.SuppressKeyPress = true;

        String lHostName = TB_Host.Text.Trim();
        String lIPAddress = TB_Address.Text.Trim();

        try
        {
          cTask.addRecord(lHostName, lIPAddress);
        }
        catch (Exception lEx)
        {
          cHost.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));
          MessageBox.Show(String.Format("{0}", lEx.Message), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
      }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TB_Address_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Enter)
      {
        e.SuppressKeyPress = true;

        String lHostName = TB_Host.Text.Trim();
        String lIPAddress = TB_Address.Text.Trim();

        try
        {
          cTask.addRecord(lHostName, lIPAddress);
        }
        catch (Exception lEx)
        {
          cHost.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));
          MessageBox.Show(String.Format("{0}", lEx.Message), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
      }
    }

    #endregion


    #region OBSERVER INTERFACE METHODS

    public void update(BindingList<DNSPoisonRecord> pHostnameIPPair)
    {
      cDNSPoisonRecords.Clear();
      foreach (DNSPoisonRecord lTmp in pHostnameIPPair)
        cDNSPoisonRecords.Add(new DNSPoisonRecord(lTmp.HostName, lTmp.IPAddress));

      DGV_Spoofing.Refresh();
    }

    #endregion

  }
}
