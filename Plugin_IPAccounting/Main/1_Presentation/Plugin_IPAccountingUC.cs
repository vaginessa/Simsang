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

    private IPluginHost cHost;
    private List<String> cTargetList;
    private BindingList<AccountingItem> cAccountingRecords;
    private TaskFacade cTask;
    private String cAccountingBasis = "-p";

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
                                         isDebuggingOn = false, //cHost.IsDebuggingOn(),
                                         Interface = null, //cHost.GetInterface(),
                                         onUpdateList = update,
                                         onIPAccountingExit = null
                                       };
      cTask = TaskFacade.getInstance(lConfig, this);
    }

    #endregion


    #region PRIVATE

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private delegate void onIPAccountingExitedDelegate(object sender, System.EventArgs e);
    private void onIPAccountingExited(object sender, System.EventArgs e)
    {
      if (InvokeRequired)
      {
        BeginInvoke(new onIPAccountingExitedDelegate(onIPAccountingExited), new object[] { sender, e });
        return;
      }

      SetIPAccountingBTOnStopped();
      cHost.PluginSetStatus(this, "red");
    }



    /// <summary>
    /// 
    /// </summary>
    private delegate void SetIPAccountingBTOnStoppedDelegate();
    private void SetIPAccountingBTOnStopped()
    {
      if (InvokeRequired)
      {
        BeginInvoke(new SetIPAccountingBTOnStoppedDelegate(SetIPAccountingBTOnStopped), new object[] { });
        return;
      }

      /*
       * Set GUI parameters
       */
      RB_Service.Enabled = true;
      RB_RemoteIP.Enabled = true;
      RB_LocalIP.Enabled = true;

      cTask.onStop();
    }



    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private delegate bool SetIPAccountingBTOnStartedDelegate();
    private bool SetIPAccountingBTOnStarted()
    {
      if (InvokeRequired)
      {
        BeginInvoke(new SetIPAccountingBTOnStartedDelegate(SetIPAccountingBTOnStarted), new object[] { });
        return (false);
      }

      bool lRetVal = false;
      IPAccountingConfig lConfig;

      /*
       * Set GUI parameters
       */
      RB_Service.Enabled = false;
      RB_RemoteIP.Enabled = false;
      RB_LocalIP.Enabled = false;


      try
      {
        /*
         * Start accounting application.
         */
        cTask.onInit();

        lConfig = new IPAccountingConfig
        {
          BasisDirectory = Config.BaseDir,
          isDebuggingOn = cHost.IsDebuggingOn(),
          onUpdateList = update,
          onIPAccountingExit = null,
          Interface = cHost.GetInterface()
        };
        cTask.onStart(lConfig);
        lRetVal = true;
      }
      catch (Exception lEx)
      {
        lRetVal = false;
      }

      return (lRetVal);
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


        if (SetIPAccountingBTOnStarted())
          cHost.PluginSetStatus(this, "green");
        else
          cHost.PluginSetStatus(this, "red");
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

      SetIPAccountingBTOnStopped();
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


      SetIPAccountingBTOnStopped();
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
      SetIPAccountingBTOnStopped();
      //      cDataArray.Clear();
      //      cData = String.Empty;
      //      UpdateTextBox("");

      Thread.Sleep(500);
      SetIPAccountingBTOnStarted();
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DGV_TrafficData_DoubleClick(object sender, EventArgs e)
    {
      ManageServices.Form_ManageServices lManageServices;

      lManageServices = new ManageServices.Form_ManageServices(cHost);
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
        cHost.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));     
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

    public void update(List<AccountingItem> pRecordList)
    {
      pRecordList.Clear();
      foreach (AccountingItem lTmp in pRecordList)
        cAccountingRecords.Add(lTmp);

      DGV_TrafficData.Refresh();
    }

    #endregion

  }
}
