using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Diagnostics;
using System.Threading;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Reflection;

using Simsang.Plugin;
using Plugin.Main.IPAccounting;
using Plugin.Main.IPAccounting.Config;
using ManageServices = Plugin.Main.IPAccounting.ManageServices;


namespace Plugin.Main
{
  public partial class PluginIPAccountingUC : UserControl, IPlugin, IObserver
  {

    #region MEMBERS

    private List<String> cTargetList;
    private BindingList<AccountingItem> cAccountingRecords;
    private TaskFacade cTask;
    private String cAccountingBasis = "-p";
    private PluginParameters cPluginParams;

    #endregion


    #region PUBLIC

    /// <summary>
    /// 
    /// </summary>
    public PluginIPAccountingUC(PluginParameters pPluginParams)
    {
      InitializeComponent();


      /*
       * Set DGV double buffer on
       */
      Type dgvType = DGV_TrafficData.GetType();
      PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
      pi.SetValue(DGV_TrafficData, true, null);


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
        PluginName = "IP accounting",
        PluginDescription = "Listing data traffic statistics.",
        PluginVersion = "0.8",
        Ports = "",
        IsActive = true
      };


      /*
       * Initialisation
       */
      RB_Service_Click(null, null);

      cAccountingRecords = new BindingList<AccountingItem>();
      DGV_TrafficData.DataSource = cAccountingRecords;

      IPAccountingConfig lConfig = new IPAccountingConfig()
                                       {
                                         BasisDirectory = Config.BaseDir,
                                         isDebuggingOn = false, //cPluginParams.HostApplication.IsDebuggingOn(),
                                         Interface = null, //cPluginParams.HostApplication.GetInterface(),
                                         onUpdateList = update,
                                         onIPAccountingExit = null
                                       };
      cTask = TaskFacade.getInstance(lConfig, this);
      DomainFacade.getInstance(lConfig, this).addObserver(this);
    }

    #endregion


    #region PRIVATE

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private delegate void onIPAccountingExitedDelegate(); 
    private void onIPAccountingExited()
    {
      if (InvokeRequired)
      {
        BeginInvoke(new onIPAccountingExitedDelegate(onIPAccountingExited), new object[] { });
        return;
      }

      setGUIActive();
      cTask.onStop();
      cPluginParams.HostApplication.PluginSetStatus(this, "red");
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

      RB_Service.Enabled = true;
      RB_RemoteIP.Enabled = true;
      RB_LocalIP.Enabled = true;
    }



    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private delegate void setGUIInactiveDelegate();
    private void setGUIInactive()
    {
      if (InvokeRequired)
      {
        BeginInvoke(new setGUIInactiveDelegate(setGUIInactive), new object[] { });
        return;
      }

      RB_Service.Enabled = false;
      RB_RemoteIP.Enabled = false;
      RB_LocalIP.Enabled = false;
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

        /*
         * Start accounting application.
         */

        try
        {
          cTask.onInit();
          setGUIInactive();

          IPAccountingConfig lConfig = new IPAccountingConfig
          {
            BasisDirectory = Config.BaseDir,
            isDebuggingOn = cPluginParams.HostApplication.IsDebuggingOn(),
            onUpdateList = update,
            onIPAccountingExit = onIPAccountingExited,
            Interface = cPluginParams.HostApplication.GetInterface(),
            StructureParameter = cAccountingBasis
          };

          cTask.onStart(lConfig);
          cPluginParams.HostApplication.PluginSetStatus(this, "green");
        }
        catch (Exception)
        {
          cTask.onStop();
          cPluginParams.HostApplication.PluginSetStatus(this, "red");
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
        return ("");
      } // if (InvokeRequired)



      String lRetVal = String.Empty;
      int lCounter = 0;
      int lTmpCounter = 0;

      if (cAccountingRecords != null && cAccountingRecords.Count > 0)
      {
        foreach (AccountingItem lTmp in cAccountingRecords)
        {
          if (Int32.TryParse(lTmp.DataVolume, NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out lTmpCounter))
            lCounter += lTmpCounter;
        } // foreach (Se...
      } // if (mServices != ...
      lRetVal = lCounter.ToString();

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


      cTask.emptyRecordList();
      setGUIActive();
      cPluginParams.HostApplication.PluginSetStatus(this, "grey");
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


    #region EVENTS


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BT_Clear_Click(object sender, EventArgs e)
    {
      cTask.emptyRecordList();
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DGV_TrafficData_MouseUp(object sender, MouseEventArgs e)
    {
      DataGridView.HitTestInfo hitTestInfo;
      if (e.Button == MouseButtons.Right)
      {
        hitTestInfo = DGV_TrafficData.HitTest(e.X, e.Y);

        // If cell selection is valid
        if (hitTestInfo.ColumnIndex >= 0 && hitTestInfo.RowIndex >= 0)
        {
          //DGV_TrafficData.Rows[hitTestInfo.RowIndex].Selected = true;
          CMS_DataGrid_RightMouseButton.Show(DGV_TrafficData, new Point(e.X, e.Y));
        }
      }
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void clearToolStripMenuItem_Click(object sender, EventArgs e)
    {
      cTask.emptyRecordList();
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DGV_TrafficData_DoubleClick(object sender, EventArgs e)
    {
      ManageServices.Form_ManageServices lManageServices;

      lManageServices = new ManageServices.Form_ManageServices(cPluginParams.HostApplication);
      lManageServices.ShowDialog();
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PluginIPAccountingUC_MouseDown(object sender, MouseEventArgs e)
    {
      try
      {
        DataGridView.HitTestInfo hti = DGV_TrafficData.HitTest(e.X, e.Y);

        if (hti.RowIndex >= 0)
        {
          DGV_TrafficData.ClearSelection();
          DGV_TrafficData.Rows[hti.RowIndex].Selected = true;
          DGV_TrafficData.CurrentCell = DGV_TrafficData.Rows[hti.RowIndex].Cells[0];
        }
      }
      catch (Exception lEx)
      {
        cPluginParams.HostApplication.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));     
        DGV_TrafficData.ClearSelection();
      }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void RB_Service_Click(object sender, EventArgs e)
    {
      cAccountingBasis = "-p";

      #region SERVICES DATAGRID VIEW
      DGV_TrafficData.Columns.Clear();

      DataGridViewTextBoxColumn cServiceNameCol = new DataGridViewTextBoxColumn();
      cServiceNameCol.DataPropertyName = "Basis";
      cServiceNameCol.Name = "Basis";
      cServiceNameCol.HeaderText = "Service";
      cServiceNameCol.ReadOnly = true;
      //cServiceNameCol.Width = 120;
      cServiceNameCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      cServiceNameCol.Resizable = System.Windows.Forms.DataGridViewTriState.False;
      DGV_TrafficData.Columns.Add(cServiceNameCol);


      DataGridViewTextBoxColumn cPacketCounterCol = new DataGridViewTextBoxColumn();
      cPacketCounterCol.DataPropertyName = "PacketCounter";
      cPacketCounterCol.Name = "PacketCounter";
      cPacketCounterCol.HeaderText = "No. packets";
      cPacketCounterCol.ReadOnly = true;
      cPacketCounterCol.Width = 120;
      cPacketCounterCol.Resizable = System.Windows.Forms.DataGridViewTriState.False;
      DGV_TrafficData.Columns.Add(cPacketCounterCol);
      DGV_TrafficData.Columns["PacketCounter"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

      DataGridViewTextBoxColumn cDataVolumeCol = new DataGridViewTextBoxColumn();
      cDataVolumeCol.DataPropertyName = "DataVolume";
      cDataVolumeCol.Name = "DataVolume";
      cDataVolumeCol.HeaderText = "Data volume";
      cDataVolumeCol.ReadOnly = true;
      cDataVolumeCol.Width = 120;
      cDataVolumeCol.Resizable = System.Windows.Forms.DataGridViewTriState.False;
      DGV_TrafficData.Columns.Add(cDataVolumeCol);
      DGV_TrafficData.Columns["DataVolume"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

      DataGridViewTextBoxColumn cLastUpdateCol = new DataGridViewTextBoxColumn();
      cLastUpdateCol.DataPropertyName = "LastUpdate";
      cLastUpdateCol.Name = "LastUpdate";
      cLastUpdateCol.HeaderText = "Last update";
      cLastUpdateCol.ReadOnly = true;
      cLastUpdateCol.Width = 267;
      //cLastUpdateCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      cLastUpdateCol.Resizable = System.Windows.Forms.DataGridViewTriState.False;
      DGV_TrafficData.Columns.Add(cLastUpdateCol);


      //cDataArray = new BindingList<AccountingItem>();
      //DGV_TrafficData.DataSource = cDataArray;

      #endregion

    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void RB_LocalIP_Click(object sender, EventArgs e)
    {
      cAccountingBasis = "-s";

      #region LocalIP Datagridview
      DGV_TrafficData.Columns.Clear();
      DataGridViewTextBoxColumn cServiceNameCol = new DataGridViewTextBoxColumn();
      cServiceNameCol.DataPropertyName = "Basis";
      cServiceNameCol.Name = "Basis";
      cServiceNameCol.HeaderText = "Local IP";
      cServiceNameCol.ReadOnly = true;
      //cServiceNameCol.Width = 120;
      cServiceNameCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      cServiceNameCol.Resizable = System.Windows.Forms.DataGridViewTriState.False;
      DGV_TrafficData.Columns.Add(cServiceNameCol);


      DataGridViewTextBoxColumn cPacketCounterCol = new DataGridViewTextBoxColumn();
      cPacketCounterCol.DataPropertyName = "PacketCounter";
      cPacketCounterCol.Name = "PacketCounter";
      cPacketCounterCol.HeaderText = "No. packets";
      cPacketCounterCol.ReadOnly = true;
      cPacketCounterCol.Width = 120;
      cPacketCounterCol.Resizable = System.Windows.Forms.DataGridViewTriState.False;
      DGV_TrafficData.Columns.Add(cPacketCounterCol);
      DGV_TrafficData.Columns["PacketCounter"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

      DataGridViewTextBoxColumn cDataVolumeCol = new DataGridViewTextBoxColumn();
      cDataVolumeCol.DataPropertyName = "DataVolume";
      cDataVolumeCol.Name = "DataVolume";
      cDataVolumeCol.HeaderText = "Data volume";
      cDataVolumeCol.ReadOnly = true;
      cDataVolumeCol.Width = 120;
      cDataVolumeCol.Resizable = System.Windows.Forms.DataGridViewTriState.False;
      DGV_TrafficData.Columns.Add(cDataVolumeCol);
      DGV_TrafficData.Columns["DataVolume"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

      DataGridViewTextBoxColumn cLastUpdateCol = new DataGridViewTextBoxColumn();
      cLastUpdateCol.DataPropertyName = "LastUpdate";
      cLastUpdateCol.Name = "LastUpdate";
      cLastUpdateCol.HeaderText = "Last update";
      cLastUpdateCol.ReadOnly = true;
      cLastUpdateCol.Width = 267;
      //cLastUpdateCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      cLastUpdateCol.Resizable = System.Windows.Forms.DataGridViewTriState.False;
      DGV_TrafficData.Columns.Add(cLastUpdateCol);


      //cDataArray = new BindingList<AccountingItem>();
      //DGV_TrafficData.DataSource = cDataArray;

      #endregion

    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void RB_RemoteIP_Click(object sender, EventArgs e)
    {
      cAccountingBasis = "-d";

      #region REMOTE IP DATAGRID VIEW

      DGV_TrafficData.Columns.Clear();
      DataGridViewTextBoxColumn cServiceNameCol = new DataGridViewTextBoxColumn();
      cServiceNameCol.DataPropertyName = "Basis";
      cServiceNameCol.Name = "Basis";
      cServiceNameCol.HeaderText = "Remote IP";
      cServiceNameCol.ReadOnly = true;
      //      cServiceNameCol.Width = 120;
      cServiceNameCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      cServiceNameCol.Resizable = System.Windows.Forms.DataGridViewTriState.False;
      DGV_TrafficData.Columns.Add(cServiceNameCol);


      DataGridViewTextBoxColumn cPacketCounterCol = new DataGridViewTextBoxColumn();
      cPacketCounterCol.DataPropertyName = "PacketCounter";
      cPacketCounterCol.Name = "PacketCounter";
      cPacketCounterCol.HeaderText = "No. packets";
      cPacketCounterCol.ReadOnly = true;
      cPacketCounterCol.Width = 120;
      cPacketCounterCol.Resizable = System.Windows.Forms.DataGridViewTriState.False;
      DGV_TrafficData.Columns.Add(cPacketCounterCol);
      DGV_TrafficData.Columns["PacketCounter"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

      DataGridViewTextBoxColumn cDataVolumeCol = new DataGridViewTextBoxColumn();
      cDataVolumeCol.DataPropertyName = "DataVolume";
      cDataVolumeCol.Name = "DataVolume";
      cDataVolumeCol.HeaderText = "Data volume";
      cDataVolumeCol.ReadOnly = true;
      cDataVolumeCol.Width = 120;
      cDataVolumeCol.Resizable = System.Windows.Forms.DataGridViewTriState.False;
      DGV_TrafficData.Columns.Add(cDataVolumeCol);
      DGV_TrafficData.Columns["DataVolume"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

      DataGridViewTextBoxColumn cLastUpdateCol = new DataGridViewTextBoxColumn();
      cLastUpdateCol.DataPropertyName = "LastUpdate";
      cLastUpdateCol.Name = "LastUpdate";
      cLastUpdateCol.HeaderText = "Last update";
      cLastUpdateCol.ReadOnly = true;
      cLastUpdateCol.Width = 267;
      cLastUpdateCol.Resizable = System.Windows.Forms.DataGridViewTriState.False;
      DGV_TrafficData.Columns.Add(cLastUpdateCol);


      //cDataArray = new BindingList<AccountingItem>();
      //DGV_TrafficData.DataSource = cDataArray;
      #endregion

    }

    #endregion


    #region OBSERVER INTERFACE METHODS

    private delegate void updateDelegate(List<AccountingItem> pRecordList);
    public void update(List<AccountingItem> pRecordList)
    {
      if (InvokeRequired)
      {
        BeginInvoke(new updateDelegate(update), new object[] { pRecordList });
        return;
      }
      
      cAccountingRecords.Clear();
      foreach (AccountingItem lTmp in pRecordList)
        cAccountingRecords.Add(lTmp);

      DGV_TrafficData.Refresh();
    }

    #endregion

  }
}
