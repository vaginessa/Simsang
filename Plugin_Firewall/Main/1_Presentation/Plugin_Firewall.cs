using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using System.Configuration;

using Simsang.Plugin;
using Plugin.Main.Firewall;


namespace Plugin.Main
{
  public partial class PluginFirewallUC : UserControl, IPlugin, IObserver
  {

    #region MEMBERS

    private IPluginHost cHost;
    private List<String> cSrcTargetList;
    private List<String> cDstTargetList;
    private BindingList<FWRule> cFWRules;
    private TaskFacade cTask;
    private DomainFacade cDomain;

    #endregion


    #region PUBLIC

    public PluginFirewallUC(PluginParameters pPluginParams)
    {
      InitializeComponent();


      #region DATAGRID HEADER

      DataGridViewTextBoxColumn cID = new DataGridViewTextBoxColumn();
      cID.DataPropertyName = "ID";
      cID.Name = "ID";
      cID.HeaderText = "ID";
      cID.ReadOnly = true;
      cID.Width = 0;
      cID.Visible = false;
      DGV_FWRules.Columns.Add(cID);

      DataGridViewTextBoxColumn cProtocolCol = new DataGridViewTextBoxColumn();
      cProtocolCol.DataPropertyName = "Protocol";
      cProtocolCol.Name = "Protocol";
      cProtocolCol.HeaderText = "Prot.";
      cProtocolCol.ReadOnly = true;
      cProtocolCol.Width = 50;
      DGV_FWRules.Columns.Add(cProtocolCol);


      DataGridViewTextBoxColumn cSrcIPCol = new DataGridViewTextBoxColumn();
      cSrcIPCol.DataPropertyName = "SrcIP";
      cSrcIPCol.Name = "SrcIP";
      cSrcIPCol.HeaderText = "Source IP";
      cSrcIPCol.ReadOnly = true;
      cSrcIPCol.Width = 95;
      DGV_FWRules.Columns.Add(cSrcIPCol);


      DataGridViewTextBoxColumn cSrcPortLowerCol = new DataGridViewTextBoxColumn();
      cSrcPortLowerCol.DataPropertyName = "SrcPortLower";
      cSrcPortLowerCol.Name = "SrcPortLower";
      cSrcPortLowerCol.HeaderText = "Src. port (lower)";
      cSrcPortLowerCol.ReadOnly = true;
      cSrcPortLowerCol.Width = 125;
      DGV_FWRules.Columns.Add(cSrcPortLowerCol);


      DataGridViewTextBoxColumn cSrcPortUpperCol = new DataGridViewTextBoxColumn();
      cSrcPortUpperCol.DataPropertyName = "SrcPortUpper";
      cSrcPortUpperCol.Name = "SrcPortUpper";
      cSrcPortUpperCol.HeaderText = "Src. port (upper)";
      cSrcPortUpperCol.ReadOnly = true;
      cSrcPortUpperCol.Width = 125;
      DGV_FWRules.Columns.Add(cSrcPortUpperCol);


      DataGridViewTextBoxColumn cDstIPCol = new DataGridViewTextBoxColumn();
      cDstIPCol.DataPropertyName = "DstIP";
      cDstIPCol.Name = "DstIP";
      cDstIPCol.HeaderText = "Dest. IP";
      cDstIPCol.ReadOnly = true;
      cDstIPCol.Width = 95;
      DGV_FWRules.Columns.Add(cDstIPCol);


      DataGridViewTextBoxColumn cDstPortLowerCol = new DataGridViewTextBoxColumn();
      cDstPortLowerCol.DataPropertyName = "DstPortLower";
      cDstPortLowerCol.Name = "DstPortLower";
      cDstPortLowerCol.HeaderText = "Dst. port (lower)";
      cDstPortLowerCol.ReadOnly = true;
      cDstPortLowerCol.Width = 125;
      DGV_FWRules.Columns.Add(cDstPortLowerCol);


      DataGridViewTextBoxColumn cDstPortUpperCol = new DataGridViewTextBoxColumn();
      cDstPortUpperCol.DataPropertyName = "DstPortUpper";
      cDstPortUpperCol.Name = "DstPortUpper";
      cDstPortUpperCol.HeaderText = "Dst. port (upper)";
      cDstPortUpperCol.ReadOnly = true;
      cDstPortUpperCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      //      cDstPortUpperCol.Width = 127;
      DGV_FWRules.Columns.Add(cDstPortUpperCol);


      cFWRules = new BindingList<FWRule>();
      DGV_FWRules.DataSource = cFWRules;

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
        PluginName = "Firewall",
        PluginDescription = "Letting pass or blocking client systems data packets.",
        PluginVersion = "0.5",
        Ports = "",
        IsActive = true
      };

      // Populate Protocol combobox
      CB_Protocol.Items.Add("TCP");
      CB_Protocol.Items.Add("UDP");
      CB_Protocol.SelectedIndex = 0;

      cTask = TaskFacade.getInstance(this);
      cDomain = DomainFacade.getInstance(this);
      cDomain.addObserver(this);

      cSrcTargetList = new List<String>();
      cDstTargetList = new List<String>();
    }

    #endregion


    #region PROPERTIES

    public Control PluginControl { get { return (this); } }
    public IPluginHost Host { get { return cHost; } set { cHost = value; cHost.Register(this); } }

    #endregion


    #region IPLUGIN MEMBERS

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


        String lFWRulesPath = cHost.GetAPEFWRulesFile();
        cTask.onStart(lFWRulesPath);

        cHost.PluginSetStatus(this, "green");

        /*
         * Block GUI elements
         */
        CB_Protocol.Enabled = false;
        CB_SrcIP.Enabled = false;
        TB_SrcPortLower.Enabled = false;
        TB_SrcPortUpper.Enabled = false;
        CB_DstIP.Enabled = false;
        TB_DstPortLower.Enabled = false;
        TB_DstPortUpper.Enabled = false;
        BT_Add.Enabled = false;
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

      // Delete firewall rules file
      String lFWRulesPath = cHost.GetAPEFWRulesFile();
      cTask.onStop(lFWRulesPath);

      /*
       * Unblock GUI elements
       */
      CB_Protocol.Enabled = true;
      CB_SrcIP.Enabled = true;
      TB_SrcPortLower.Enabled = true;
      TB_SrcPortUpper.Enabled = true;
      CB_DstIP.Enabled = true;
      TB_DstPortLower.Enabled = true;
      TB_DstPortUpper.Enabled = true;
      BT_Add.Enabled = true;
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
    public void onShutDown()
    {
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
        cHost.LogMessage(lEx.StackTrace);
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


      TB_DstPortLower.Text = String.Empty;
      TB_DstPortUpper.Text = String.Empty;
      TB_SrcPortLower.Text = String.Empty;
      TB_SrcPortUpper.Text = String.Empty;
      CB_DstIP.Text = String.Empty;
      CB_SrcIP.Text = String.Empty;

      cTask.emptyRuleList();

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
      } // if (cIsActi..
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="pTargetList"></param>
    public void SetTargets(List<String> pTargetList)
    {
      if (pTargetList != null && pTargetList.Count > 0)
      {
        foreach (String lTmp in pTargetList)
        {
          cSrcTargetList.Add(lTmp);
          cDstTargetList.Add(lTmp);
        }

        CB_SrcIP.DataSource = cSrcTargetList;
        CB_DstIP.DataSource = cDstTargetList;
      }
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
      String lProtocol = CB_Protocol.Text;
      String lSrcIP = CB_SrcIP.Text;
      String lDstIP = CB_DstIP.Text;
      String lSrcPortLowerStr = TB_SrcPortLower.Text;
      String lSrcPortUpperStr = TB_SrcPortUpper.Text;
      String lDstPortLowerStr = TB_DstPortLower.Text;
      String lDstPortUpperStr = TB_DstPortUpper.Text;

      try
      {
        cTask.addRecord(lProtocol, lSrcIP, lDstIP, lSrcPortLowerStr, lSrcPortUpperStr, lDstPortLowerStr, lDstPortUpperStr);
      }
      catch (Exception lEx)
      {
        MessageBox.Show(String.Format(lEx.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning));
      }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BT_Clear_Click(object sender, EventArgs e)
    {
      cTask.emptyRuleList();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DGV_Patterns_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
    {
      String lFWRuleID = String.Empty;
      FWRule lFWRule = null;


      if (e.RowIndex >= 0 && DGV_FWRules.SelectedRows.Count > 0)
      {
        try
        {
          lFWRuleID = DGV_FWRules["ID", e.RowIndex].Value.ToString();
          lFWRule = cFWRules.Where(k => k.ID == lFWRuleID).First();
        }
        catch (Exception lEx)
        {
          MessageBox.Show(String.Format("Error occurred : {0}", lEx.Message), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
          return;
        }

        MessageBox.Show(String.Format("FW rule ID : {0}", lFWRuleID));

      }
    }


    /// <summary>
    /// DGV right mouse button click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DGV_FWRules_MouseUp(object sender, MouseEventArgs e)
    {
      DataGridView.HitTestInfo hitTestInfo;

      if (e.Button == MouseButtons.Right)
      {
        hitTestInfo = DGV_FWRules.HitTest(e.X, e.Y);

        // If cell selection is valid
        if (hitTestInfo.ColumnIndex >= 0 && hitTestInfo.RowIndex >= 0)
        {
          DGV_FWRules.Rows[hitTestInfo.RowIndex].Selected = true;
          CMS_DataGrid_RightMouseButton.Show(DGV_FWRules, new Point(e.X, e.Y));
        }
      }
    }


    /// <summary>
    /// Delete firewall rule
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void deleteRuleToolStripMenuItem_Click(object sender, EventArgs e)
    {
      int lRowIndex = DGV_FWRules.SelectedRows[0].Index;
      String lFWRuleID = DGV_FWRules.Rows[lRowIndex].Cells["ID"].Value.ToString();

      cTask.removeRuleFromList(lFWRuleID);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void deleteAllToolStripMenuItem_Click(object sender, EventArgs e)
    {
      cTask.emptyRuleList();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CB_SrcIP_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CB_DstIP_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    #endregion


    #region IOBSERVER METHODS

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pFirewallRuleList"></param>
    public void update(BindingList<FWRule> pFirewallRuleList)
    {
      cFWRules.Clear();
      if (pFirewallRuleList != null && pFirewallRuleList.Count > 0)
        foreach (FWRule lTmp in pFirewallRuleList)
          cFWRules.Add(lTmp);
    }

    #endregion

  }
}
