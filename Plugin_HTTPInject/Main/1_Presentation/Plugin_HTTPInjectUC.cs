﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Configuration;

using Plugin.Main.HTTPInject;
using Simsang.Plugin;



namespace Plugin.Main
{

  public partial class PluginHTTPInjectUC : UserControl, IPlugin, IObserver
  {

    #region MEMBERS

    private BindingList<InjectedURLRecord> cInjectedURLs;
    private TaskFacade cTask;
    private InjectionConfig cConfigParams;

    #endregion


    #region PUBLIC

    public PluginHTTPInjectUC(PluginParameters pPluginParams)
    {
      InitializeComponent();

      #region DATAGRID HEADER

      DataGridViewTextBoxColumn cTypeCol = new DataGridViewTextBoxColumn();
      cTypeCol.DataPropertyName = "Type";
      cTypeCol.Name = "Type";
      cTypeCol.HeaderText = "Type";
      cTypeCol.ReadOnly = true;
      cTypeCol.Width = 70;
      DGV_Inject.Columns.Add(cTypeCol);

      DataGridViewTextBoxColumn cReqHostCol = new DataGridViewTextBoxColumn();
      cReqHostCol.DataPropertyName = "RequestedHost";
      cReqHostCol.Name = "RequestedHost";
      cReqHostCol.HeaderText = "Requested host";
      cReqHostCol.ReadOnly = true;
      cReqHostCol.Width = 250;
      DGV_Inject.Columns.Add(cReqHostCol);

      DataGridViewTextBoxColumn cReqURLCol = new DataGridViewTextBoxColumn();
      cReqURLCol.DataPropertyName = "RequestedURL";
      cReqURLCol.Name = "RequestedURL";
      cReqURLCol.HeaderText = "Requested URL";
      cReqURLCol.ReadOnly = true;
      cReqURLCol.Width = 250;
      DGV_Inject.Columns.Add(cReqURLCol);


      DataGridViewTextBoxColumn cInjHostNameCol = new DataGridViewTextBoxColumn();
      cInjHostNameCol.DataPropertyName = "InjectedHost";
      cInjHostNameCol.Name = "InjectedHost";
      cInjHostNameCol.HeaderText = "Injected host";
      cInjHostNameCol.ReadOnly = true;
      cInjHostNameCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      DGV_Inject.Columns.Add(cInjHostNameCol);

      DataGridViewTextBoxColumn cInjURLNameCol = new DataGridViewTextBoxColumn();
      cInjURLNameCol.DataPropertyName = "InjectedURL";
      cInjURLNameCol.Name = "InjectedURL";
      cInjURLNameCol.HeaderText = "Injected URL/file";
      cInjURLNameCol.ReadOnly = true;
      cInjURLNameCol.Width = 250;
      DGV_Inject.Columns.Add(cInjURLNameCol);


      DataGridViewTextBoxColumn cInjURLFullPathCol = new DataGridViewTextBoxColumn();
      cInjURLFullPathCol.DataPropertyName = "InjectedFileFullPath";
      cInjURLFullPathCol.Name = "InjectedFileFullPath";
      cInjURLFullPathCol.HeaderText = String.Empty;
      cInjURLFullPathCol.ReadOnly = true;
      //      cInjURLNameCol.Width = 250;
      cInjURLFullPathCol.Visible = false;
      DGV_Inject.Columns.Add(cInjURLFullPathCol);

      cInjectedURLs = new BindingList<InjectedURLRecord>();
      DGV_Inject.DataSource = cInjectedURLs;

      #endregion

      RB_Redirect.Checked = true;
      RB_Redirect_CheckedChanged(null, null);

      /*
       * Plugin configuration
       */
      PluginParameters = pPluginParams;
      String lBaseDir = String.Format(@"{0}\", (PluginParameters != null) ? PluginParameters.PluginDirectoryFullPath : Directory.GetCurrentDirectory());
      String lSessionDir = (PluginParameters != null) ? PluginParameters.SessionDirectoryFullPath : String.Format("{0}sessions", lBaseDir);

      Config = new PluginProperties()
      {
        BaseDir = lBaseDir,
        SessionDir = lSessionDir,
        PluginName = "HTTP inject",
        PluginDescription = "Injecting data packets in an established HTTP data connection.",
        PluginVersion = "0.5",
        Ports = "TCP:80;",
        IsActive = true
      };


      /*
       * Proxy server configuration
       */
      cConfigParams = new InjectionConfig
      {
        isDebuggingOn = (PluginParameters != null) ? PluginParameters.HostApplication.IsDebuggingOn() : false,
        BasisDirectory = Config.BaseDir,
        onWebServerExit = onMicroWebExited,
        InjectionRulesPath = (PluginParameters != null) ? PluginParameters.HostApplication.GetAPEInjectionRulesFile() : String.Empty
      };

      cTask = TaskFacade.getInstance(cConfigParams, this);
      DomainFacade.getInstance(cConfigParams, this).addObserver(this);
    }

    #endregion


    #region PRIVATE

    /// <summary>
    /// 
    /// </summary>
    private delegate void onMicroWebExitedDelegate();
    private void onMicroWebExited()
    {
      if (InvokeRequired)
      {
        BeginInvoke(new onMicroWebExitedDelegate(onMicroWebExited), new object[] { });
        return;
      }

      setGUIActive();
      PluginParameters.HostApplication.PluginSetStatus(this, "red");
    }



    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private void setGUIInactive()
    {
      // Disable relevant GUI elements
      TB_ReplacementURL.Enabled = false;
      TB_RequestedHost.Enabled = false;
      TB_RequestedURL.Enabled = false;
      BT_Add.Enabled = false;
      BT_InjectFile.Enabled = false;
      RB_Inject.Enabled = false;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private delegate void setGUIActiveDelegate();
    private void setGUIActive()
    {
      if (InvokeRequired)
      {
        BeginInvoke(new setGUIActiveDelegate(setGUIActive), new object[] { });
        return;
      }

      TB_ReplacementURL.Enabled = true;
      TB_RequestedHost.Enabled = true;
      TB_RequestedURL.Enabled = true;
      BT_Add.Enabled = true;
      BT_InjectFile.Enabled = true;
      RB_Inject.Enabled = true;
    }

    #endregion


    #region PROPERTIES

    public Control PluginControl { get { return (this); } }
    public PluginParameters PluginParameters { get; private set; }

    #endregion


    #region MEMBERS IPlugin

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

      cTask.onInit();
      PluginParameters.HostApplication.Register(this);
      setGUIActive();
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


        try
        {
          setGUIInactive();
          cTask.onStart();
          PluginParameters.HostApplication.PluginSetStatus(this, "green");
        }
        catch (InjWarningException lEx)
        {
          PluginParameters.HostApplication.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));
          PluginParameters.HostApplication.PluginSetStatus(this, "grey");
          cTask.onStop();
        }
        catch (Exception lEx)
        {
          setGUIActive();
          PluginParameters.HostApplication.PluginSetStatus(this, "red");
          PluginParameters.HostApplication.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));
          cTask.onStop();
        }

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
        PluginParameters.HostApplication.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));
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
      setGUIActive();
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

      TB_RequestedHost.Text = String.Empty;
      TB_RequestedURL.Text = String.Empty;
      TB_ReplacementURL.Text = String.Empty;

      cTask.emptyInjectionList();
      setGUIActive();
      PluginParameters.HostApplication.PluginSetStatus(this, "grey");
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
    }


    #endregion


    #region EVENTS

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OFD_InjectedFile_FileOk(object sender, CancelEventArgs e)
    {
      //      MessageBox.Show("InjectedFile : " + OFD_InjectedFile.FileName);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BT_InjectFile_Click(object sender, EventArgs e)
    {
      String lIconPath = String.Empty;

      try
      {
        OFD_InjectedFile.ShowDialog();
        lIconPath = String.Format("{0}\\{1}", Path.GetDirectoryName(OFD_InjectedFile.FileName), Path.GetFileName(OFD_InjectedFile.FileName));
      }
      catch (Exception lEx)
      {
        PluginParameters.HostApplication.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));     
      }


      if (!String.IsNullOrEmpty(lIconPath))
      {
        try
        {
          if (File.Exists(lIconPath))
            TB_ReplacementURL.Text = lIconPath;
          else
            TB_ReplacementURL.Text = String.Empty;
        }
        catch (Exception lEx)
        {
          PluginParameters.HostApplication.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));     
          TB_ReplacementURL.Text = String.Empty;
        }
      }
      else
        TB_ReplacementURL.Text = String.Empty;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void RB_Redirect_CheckedChanged(object sender, EventArgs e)
    {

      if (RB_Inject.Checked)
      {
        L_Replacement_Host.Text = "Injected host";
        L_Replacement_URL.Text = "Inject file";
        BT_InjectFile.Visible = true;
        TB_ReplacementHost.Enabled = false;
        TB_ReplacementHost.Enabled = false;
        TB_ReplacementURL.Text = String.Empty;
        TB_ReplacementURL.RightToLeft = RightToLeft.Yes;
      }
      else
      {
        L_Replacement_Host.Text = "Redirect to host";
        L_Replacement_URL.Text = "Redirect to URL";
        BT_InjectFile.Visible = false;
        TB_ReplacementHost.Enabled = true;
        TB_ReplacementHost.Enabled = true;
        TB_ReplacementURL.Enabled = true;
        TB_ReplacementURL.RightToLeft = RightToLeft.No;
      }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BT_Add_Click(object sender, EventArgs e)
    {
      String lType = RB_Inject.Checked ? "Inject" : "Redirect";
      String lRequestedURL = TB_RequestedURL.Text;
      String lRequestedHost = TB_RequestedHost.Text;
      String lReplacementHost = TB_ReplacementHost.Text;
      String lReplacementURL = TB_ReplacementURL.Text;
      String lErrorMsg = String.Empty;

      try
      {
        if (RB_Inject.Checked)
          cTask.addRecord(lType, lRequestedHost, lRequestedURL, PluginParameters.HostApplication.GetCurrentIP().ToString(), Path.GetFileName(lReplacementURL), lReplacementURL);
        else
          cTask.addRecord(lType, lRequestedHost, lRequestedURL, lReplacementHost, lReplacementURL, String.Empty);

        TB_RequestedHost.Text = String.Empty;
        TB_RequestedURL.Text = String.Empty;
        TB_ReplacementHost.Text = String.Empty;
        TB_ReplacementURL.Text = String.Empty;
      }
      catch (Exception lEx)
      {
        PluginParameters.HostApplication.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));     
        MessageBox.Show(lEx.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DGV_Inject_MouseUp(object sender, MouseEventArgs e)
    {
      DataGridView.HitTestInfo hti;
      if (e.Button == MouseButtons.Right)
      {
        try
        {
          hti = DGV_Inject.HitTest(e.X, e.Y);

          // If cell selection is valid
          if (hti.ColumnIndex >= 0 && hti.RowIndex >= 0)
          {
            DGV_Inject.CurrentRow.Selected = false;
            DGV_Inject.CurrentCell = DGV_Inject.Rows[hti.RowIndex].Cells[hti.ColumnIndex];
            DGV_Inject.Rows[hti.RowIndex].Selected = true;
            CMS_DataGrid_RightMouseButton.Show(DGV_Inject, new Point(e.X, e.Y));
            //CMS_SessionMgmt.Show(DGV_Inject, new Point(e.X, e.Y));
          }
        }
        catch (Exception) { }
      }
      //if (e.Button == MouseButtons.Right)
      //{
      //  hitTestInfo = DGV_Inject.HitTest(e.X, e.Y);

      //  // If cell selection is valid
      //  if (hitTestInfo.ColumnIndex >= 0 && hitTestInfo.RowIndex >= 0)
      //  {
      //    DGV_Inject.Rows[hitTestInfo.RowIndex].Cells[hitTestInfo.ColumnIndex].Selected = true;
      //    CMS_DataGrid_RightMouseButton.Show(DGV_Inject, new Point(e.X, e.Y));
      //  }
      //}
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void deleteEntryToolStripMenuItem_Click(object sender, EventArgs e)
    {
      int lRowIndex = -1;
      String lRequestedHost = String.Empty;
      String lRequestedURL = String.Empty;

      try
      {
        lRowIndex = DGV_Inject.SelectedRows[0].Index;
        lRequestedHost = DGV_Inject.Rows[lRowIndex].Cells["RequestedHost"].Value.ToString();
        lRequestedURL = DGV_Inject.Rows[lRowIndex].Cells["RequestedURL"].Value.ToString();

        cTask.removeItemFromList(lRequestedHost, lRequestedURL);
      }
      catch (Exception lEx)
      {
        PluginParameters.HostApplication.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));     
        return;
      }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void deleteAllToolStripMenuItem_Click(object sender, EventArgs e)
    {
      cTask.emptyInjectionList();
    }

    #endregion


    #region MEMBERS IObserver

    public void update(List<InjectedURLRecord> oDict)
    {
      cInjectedURLs.Clear();
      foreach (InjectedURLRecord lTmp in oDict)
        cInjectedURLs.Add(lTmp);
    }

    #endregion

  }
}
