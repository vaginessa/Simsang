using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using System.Collections;
using System.Net;

using Simsang.MACVendors;
using Simsang.ARPScan.Main.Config;


namespace Simsang.ARPScan.Main
{

  public partial class ARPScan : Form
  {

    #region MEMBERS

    private static ARPScan mARPScan;
    private BindingList<String> mTargetList;
    private String mIfcID;
    private String mStartIP;
    private String mStopIP;
    private String mGatewayIP;
    private String mLocalIP;
    private SimsangMain mACMain;
    private BindingList<TargetRecord> mTargetRecord;
    private TaskFacadeARPScan cTaskARPScan;

    #endregion


    #region PROPERTIES

    public BindingList<TargetRecord> TargetList() { return (mTargetRecord); }
    private String IfcID { get; set; }
    private String StartIP { get; set; }
    private String StopIP { get; set; }

    #endregion


    #region PUBLIC

    public ARPScan()
    {
      InitializeComponent();


      #region Datagrid header

      DataGridViewTextBoxColumn mIPCol = new DataGridViewTextBoxColumn();
      mIPCol.DataPropertyName = "IP";
      mIPCol.Name = "IP";
      mIPCol.HeaderText = "IP address";
      mIPCol.ReadOnly = true;
      mIPCol.MinimumWidth = 130;
      DGV_Targets.Columns.Add(mIPCol);

      DataGridViewTextBoxColumn mMACCol = new DataGridViewTextBoxColumn();
      mMACCol.DataPropertyName = "MAC";
      mMACCol.Name = "MAC";
      mMACCol.HeaderText = "MAC address";
      mMACCol.ReadOnly = true;
      mMACCol.MinimumWidth = 150;
      DGV_Targets.Columns.Add(mMACCol);

      DataGridViewTextBoxColumn mVendorCol = new DataGridViewTextBoxColumn();
      mVendorCol.DataPropertyName = "vendor";
      mVendorCol.Name = "vendor";
      mVendorCol.HeaderText = "Vendor";
      mVendorCol.ReadOnly = true;
      mVendorCol.MinimumWidth = 180;
      DGV_Targets.Columns.Add(mVendorCol);


      DataGridViewCheckBoxColumn mStatusCol = new DataGridViewCheckBoxColumn();
      mStatusCol.DataPropertyName = "status";
      mStatusCol.Name = "status";
      mStatusCol.HeaderText = "Attack";
      mStatusCol.Visible = true;
      mStatusCol.Width = 72;
      DGV_Targets.Columns.Add(mStatusCol);


      DataGridViewTextBoxColumn mLastScanDateCol = new DataGridViewTextBoxColumn();
      mLastScanDateCol.DataPropertyName = "LastScanDate";
      mLastScanDateCol.Name = "LastScanDate";
      mLastScanDateCol.HeaderText = "Scan date";
      mLastScanDateCol.ReadOnly = true;
      mLastScanDateCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      mLastScanDateCol.MinimumWidth = 200;
      DGV_Targets.Columns.Add(mLastScanDateCol);

      DataGridViewTextBoxColumn mNote = new DataGridViewTextBoxColumn();
      mNote.DataPropertyName = "Note";
      mNote.Name = "Note";
      mNote.HeaderText = "Note";
      mNote.ReadOnly = true;
      mNote.Visible = false;
      mNote.Width = 0;
      DGV_Targets.Columns.Add(mNote);


      mTargetRecord = new BindingList<TargetRecord>();
      DGV_Targets.DataSource = mTargetRecord;
      DGV_Targets.CurrentCellDirtyStateChanged += new EventHandler(DGV_CurrentCellDirtyStateChanged);
      DGV_Targets.CellValueChanged += new DataGridViewCellEventHandler(DGV_CellValueChanged);

      DGV_Targets.CellClick += new DataGridViewCellEventHandler(DGV_CellClick);

      #endregion

      mTargetRecord = new BindingList<TargetRecord>();
      DGV_Targets.DataSource = mTargetRecord;

      cTaskARPScan = TaskFacadeARPScan.getInstance();
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="pACMain"></param>
    /// <param name="pTargetList"></param>
    /// <returns></returns>
    public static ARPScan getInstance(SimsangMain pACMain, ref BindingList<String> pTargetList)
    {
      if (mARPScan == null)
        mARPScan = new ARPScan();

      mARPScan.resetValues(pACMain, ref pTargetList);

      return (mARPScan);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static ARPScan getInstance()
    {
      return (mARPScan);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pACMain"></param>
    /// <param name="pTargetList"></param>
    public static void InitARPScan(SimsangMain pACMain, ref BindingList<String> pTargetList)
    {
      if (mARPScan == null)
        mARPScan = getInstance(pACMain, ref pTargetList);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pACMain"></param>
    /// <param name="pIfcID"></param>
    /// <param name="pStartIP"></param>
    /// <param name="pStopIP"></param>
    /// <param name="pGatewayIP"></param>
    /// <param name="pTargetList"></param>
    public static void showARPScanGUI(SimsangMain pACMain, String pIfcID, String pStartIP, String pStopIP, String pGatewayIP, ref BindingList<String> pTargetList)
    {
      try
      {
        getInstance(pACMain, ref pTargetList).ShowDialog();
      }
      catch (Exception lEx)
      {
        LogConsole.Main.LogConsole.pushMsg(String.Format("ARPScan.ShowDialog(): {0}", lEx.Message)); 
      }
    }

    #endregion


    #region PRIVATE

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pACMain"></param>
    /// <param name="pTargetList"></param>
    private void resetValues(SimsangMain pACMain, ref BindingList<String> pTargetList)
    {
      mACMain = pACMain;
      mTargetList = pTargetList;

      mIfcID = pACMain.GetInterface();
      mStartIP = pACMain.GetStartIP();
      mStopIP = pACMain.GetStopIP();
      mGatewayIP = pACMain.GetCurrentGWIP();
      mLocalIP = pACMain.GetCurrentIP();

      TB_Subnet1.Text = mStartIP;
      TB_Subnet2.Text = mStopIP;

      TB_Netrange1.Text = mStartIP;
      TB_Netrange2.Text = mStopIP;

      RB_Subnet.Checked = true;
      RB_Subnet_CheckedChanged(null, null);
    }


    /// <summary>
    /// 
    /// </summary>
    private delegate void setARPScanBTOnStoppedDelegate();
    private void setARPScanBTOnStopped()
    {
      if (InvokeRequired)
      {
        BeginInvoke(new setARPScanBTOnStoppedDelegate(setARPScanBTOnStopped), new object[] { });
        return;
      }


      /*
       * Set GUI parameters
       */
      DGV_Targets.Enabled = true;
      BT_Close.Enabled = true;
      BT_Scan.Enabled = true;
      RB_Netrange.Enabled = true;
      RB_Subnet.Enabled = true;

      if (RB_Netrange.Checked)
      {
        TB_Netrange1.ReadOnly = false;
        TB_Netrange2.ReadOnly = false;

        TB_Netrange1.Enabled = true;
        TB_Netrange2.Enabled = true;
      }

      this.Cursor = Cursors.Default;
      DGV_Targets.Cursor = Cursors.Default;

      /*
       * Set the tool tips!
       */
      foreach (DataGridViewRow lTmpRow in DGV_Targets.Rows)
      {
        foreach (DataGridViewCell lTmpCell in lTmpRow.Cells)
        {
          try
          {
            lTmpCell.ToolTipText = lTmpRow.Cells["Note"].Value.ToString();
          }
          catch (Exception) { }
        } // foreach (Dat...
      } // foreach (Dat...


      /*
       * Stop ARP scan. First the regular, then the brute way.
       */
      try
      {
        cTaskARPScan.stopARPScan();
      }
      catch (Exception lEx)
      {
        LogConsole.Main.LogConsole.pushMsg(String.Format(lEx.StackTrace));
      }

      try
      {
        cTaskARPScan.killAllRunningARPScans();
      }
      catch { }

    }


    /// <summary>
    /// 
    /// </summary>
    private delegate void setARPScanBTOnStartedDelegate();
    private void setARPScanBTOnStarted()
    {
      String lStartIP = String.Empty;
      String lStopIP = String.Empty;

      if (InvokeRequired)
      {
        BeginInvoke(new setARPScanBTOnStartedDelegate(setARPScanBTOnStarted), new object[] { });
        return;
      }


      /*
       * Check start/stop IP addresses.
       */
      if (RB_Subnet.Checked)
      {
        lStartIP = TB_Subnet1.Text;
        lStopIP = TB_Subnet2.Text;
      }
      else
      {
        lStartIP = TB_Netrange1.Text;
        lStopIP = TB_Netrange2.Text;
      }


      mTargetList.Clear();


      /*
       * Set GUI parameters
       */
      DGV_Targets.Enabled = false;

      BT_Close.Enabled = false;
      BT_Scan.Enabled = false;

      RB_Netrange.Enabled = false;
      RB_Subnet.Enabled = false;

      if (RB_Netrange.Checked)
      {
        TB_Netrange1.ReadOnly = true;
        TB_Netrange2.ReadOnly = true;

        TB_Netrange1.Enabled = false;
        TB_Netrange2.Enabled = false;
      }
      this.Cursor = Cursors.WaitCursor;
      DGV_Targets.Cursor = Cursors.WaitCursor;
    }


    /// <summary>
    /// 
    /// </summary>
    private void ARPScanStopped()
    {
      setARPScanBTOnStopped();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pData"></param>
    public delegate void updateTextBoxDelegate(String pData);
    public void updateTextBox(String pData)
    {
      if (InvokeRequired)
      {
        BeginInvoke(new updateTextBoxDelegate(updateTextBox), new object[] { pData });
        return;
      }

      String lType = String.Empty;
      String lIP = String.Empty;
      String lMAC = String.Empty;
      String lVendor = String.Empty;
      SystemDetails lSysDetails;
      String lNote = String.Empty;
      SystemFingerprint.TaskFacadeFingerprint lTaskFacadeFingerprint = SystemFingerprint.TaskFacadeFingerprint.getInstance();
         

      try
      {
        String lData = String.Empty;
        XDocument lXMLContent = XDocument.Parse(pData);

        var lPacketEntries = from lService in lXMLContent.Descendants("arp")
                             select new
                             {
                               Type = lService.Element("type").Value,
                               IP = lService.Element("ip").Value,
                               MAC = lService.Element("mac").Value
                             };

        if (lPacketEntries != null)
        {
          foreach (var lEntry in lPacketEntries)
          {
            try
            {
              lType = lEntry.Type;
              lIP = lEntry.IP;
              lMAC = lEntry.MAC;

              // Determine vendor
              lVendor = MACVendor.getInstance().getVendorByMAC(lMAC);

              if (lIP != mGatewayIP && lIP != mLocalIP)
              {
                mTargetList.Add(lIP);

                lSysDetails = lTaskFacadeFingerprint.getSystemDetails(lMAC);
                lNote = lTaskFacadeFingerprint.getFingerprintNote(lMAC);
                mTargetRecord.Add(new TargetRecord(lIP, lMAC, lVendor, lSysDetails.ScanDate, lNote));
              } // if (lIP != mGa...
            }
            catch { }

          } // foreach (var lEnt...
        } // if (lv1s ...
      }
      catch (Exception lEx)
      {
        LogConsole.Main.LogConsole.pushMsg(lEx.StackTrace);
      }
    }

    #endregion


    #region EVENTS

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void DGV_CellClick(object sender, DataGridViewCellEventArgs e)
    {
      // Ignore clicks that are not on button cells.  
      if (e.RowIndex >= 0)
      {
        String lIP = DGV_Targets.Rows[e.RowIndex].Cells[0].Value.ToString();
        String lMAC = DGV_Targets.Rows[e.RowIndex].Cells[1].Value.ToString();
        String lVendor = DGV_Targets.Rows[e.RowIndex].Cells[2].Value.ToString();

        /*
         * (De)Activate target system
         */
        if (e.ColumnIndex == 3)
        {
          for (int i = 0; i < mTargetRecord.Count; i++)
          {
            if (mTargetRecord[i].MAC == lMAC && mTargetRecord[i].IP == lIP)
            {
              mTargetRecord[i].Status = mTargetRecord[i].Status ? false : true;
              break;
            }
          } // for (in...


        /*
         * Fingerprint target system
         */
        }
        else if (e.ColumnIndex == 4)
        {
          SystemFingerprint.SystemFingerprint lFingerprint = new SystemFingerprint.SystemFingerprint(lMAC, lIP, lVendor);
          lFingerprint.ShowDialog();
        }
      }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void DGV_CurrentCellDirtyStateChanged(object sender, EventArgs e)
    {
      if (DGV_Targets.IsCurrentCellDirty)
        DGV_Targets.CommitEdit(DataGridViewDataErrorContexts.Commit);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="e"></param>
    private void DGV_CellValueChanged(object obj, DataGridViewCellEventArgs e)
    {
      if (e.ColumnIndex == 0) //compare to checkBox column index
      {
        DataGridViewCheckBoxCell check = DGV_Targets[0, e.RowIndex] as DataGridViewCheckBoxCell;
        if (Convert.ToBoolean(check.Value) == true)
        {
          //If tick is added!
          //
        } // if (Convert...
      } // if (e.Col...
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BT_Close_Click(object sender, EventArgs e)
    {
      // Stopping scan process
      cTaskARPScan.stopARPScan();

      // Resetting GUI elements
      setARPScanBTOnStopped();

      // Sending target list to modules
      mACMain.UpdatePlugins();

      // Hiding form
      this.Hide();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ARPScan_FormClosing(object sender, FormClosingEventArgs e)
    {
      // Stopping scan process
      cTaskARPScan.stopARPScan();

      // Resetting GUI elements
      setARPScanBTOnStopped();

      // Sending target list to modules
      mACMain.UpdatePlugins();

      // Hiding form
      this.Hide();
      e.Cancel = true;
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BT_Scan_Click(object sender, EventArgs e)
    {
      mTargetRecord.Clear();
      setARPScanBTOnStarted();

      try
      {
        ARPScanConfig lARPConf = new ARPScanConfig()
        {
          InterfaceID = mIfcID,
          GatewayIP = mGatewayIP,
          LocalIP = mLocalIP,
          StartIP = mStartIP,
          StopIP = mStopIP,
          OnDataReceived = updateTextBox,
          OnARPScanStopped = ARPScanStopped,
          IsDebuggingOn = Simsang.Config.DebugOn()
        };
        cTaskARPScan.startARPScan(lARPConf);
      }
      catch (Exception lEx)
      {
        LogConsole.Main.LogConsole.pushMsg(String.Format("ARPScan : {0}", lEx.Message));
        MessageBox.Show(lEx.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        setARPScanBTOnStopped();
      }
    }

    private void RB_Subnet_CheckedChanged(object sender, EventArgs e)
    {
      TB_Netrange1.ReadOnly = true;
      TB_Netrange2.ReadOnly = true;
    }

    private void RB_Netrange_CheckedChanged(object sender, EventArgs e)
    {
      TB_Netrange1.ReadOnly = false;
      TB_Netrange2.ReadOnly = false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TB_Netrange2_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Enter)
      {
        mTargetRecord.Clear();
        setARPScanBTOnStarted();

        try
        {
          ARPScanConfig lARPConf = new ARPScanConfig()
          {
            InterfaceID = mIfcID,
            GatewayIP = mGatewayIP,
            LocalIP = mLocalIP,
            StartIP = mStartIP,
            StopIP = mStopIP,
            OnDataReceived = updateTextBox,
            OnARPScanStopped = setARPScanBTOnStopped,
            IsDebuggingOn = Simsang.Config.DebugOn()
          };
          cTaskARPScan.startARPScan(lARPConf);
        }
        catch (Exception lEx)
        {
          LogConsole.Main.LogConsole.pushMsg(String.Format("ARPScan : {0}", lEx.Message));
          MessageBox.Show(lEx.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
          setARPScanBTOnStopped();
        }

      } // if (e.KeyCod...
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TB_Netrange1_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Enter)
      {
        mTargetRecord.Clear();
        setARPScanBTOnStarted();

        try
        {
          ARPScanConfig lARPConf = new ARPScanConfig()
          {
            InterfaceID = mIfcID,
            GatewayIP = mGatewayIP,
            LocalIP = mLocalIP,
            StartIP = mStartIP,
            StopIP = mStopIP,
            OnDataReceived = updateTextBox,
            OnARPScanStopped = setARPScanBTOnStopped,
            IsDebuggingOn = Simsang.Config.DebugOn()
          };
          cTaskARPScan.startARPScan(lARPConf);
        }
        catch (Exception lEx)
        {
          LogConsole.Main.LogConsole.pushMsg(String.Format("ARPScan : {0}", lEx.Message));
          MessageBox.Show(lEx.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
          setARPScanBTOnStopped();
        }
      } // if (e.KeyCo...
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (mTargetRecord != null && mTargetRecord.Count > 0)
      {
        for (int i = 0; i < DGV_Targets.Rows.Count; i++)
        {
          try
          {
            DGV_Targets.Rows[i].Cells["status"].Value = true;
          }
          catch (Exception) { }
        } // for (int ....
      } // if (mTarget...
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DGV_Targets_MouseUp(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Right)
      {
        try
        {
          DataGridView.HitTestInfo hti = DGV_Targets.HitTest(e.X, e.Y);
          if (hti.RowIndex >= 0)
            CMS_ManageTargets.Show(DGV_Targets, e.Location);
        }
        catch (Exception) { }
      } // if (e.Bu...
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void unselectAllToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (mTargetRecord != null && mTargetRecord.Count > 0)
      {
        for (int i = 0; i < DGV_Targets.Rows.Count; i++)
        {
          try
          {
            DGV_Targets.Rows[i].Cells["status"].Value = false;
          }
          catch (Exception) { }
        } // for (int i =...
      } // if (mTarg...
    }


    /// <summary>
    /// Close Sessions GUI on Escape.
    /// </summary>
    /// <param name="keyData"></param>
    /// <returns></returns>
    protected override bool ProcessDialogKey(Keys keyData)
    {
      if (keyData == Keys.Escape)
      {
        cTaskARPScan.stopARPScan();
        this.Close();
        return true;
      }
      else
        return base.ProcessDialogKey(keyData);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void allToolStripMenuItem_Click(object sender, EventArgs e)
    {
      List<Tuple<String, String>> lTargetList = new List<Tuple<String, String>>();
      String lIP = String.Empty;
      String lMAC = String.Empty;

      foreach (DataGridViewRow lTmp in DGV_Targets.Rows)
      {
        try
        {
          lIP = lTmp.Cells["IP"].Value.ToString();
          lMAC = lTmp.Cells["MAC"].Value.ToString();
          lTargetList.Add(new Tuple<String, String>(lMAC, lIP));
        }
        catch (Exception lEx)
        { }
      }  // if (lTmp...

      ScanMultipleSystems.getInstance(lTargetList).ShowDialog();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void unscanedSystemsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      List<Tuple<String, String>> lTargetList = new List<Tuple<String, String>>();
      String lIP = String.Empty;
      String lMAC = String.Empty;

      foreach (DataGridViewRow lTmp in DGV_Targets.Rows)
      {
        try
        {
          if (lTmp.Cells["LastScanDate"] == null || lTmp.Cells["LastScanDate"].Value == null || lTmp.Cells["LastScanDate"].Value.ToString().Length <= 0)
          {
            lIP = lTmp.Cells["IP"].Value.ToString();
            lMAC = lTmp.Cells["MAC"].Value.ToString();
            lTargetList.Add(new Tuple<String, String>(lMAC, lIP));
          } // if (lTmp...
        }
        catch (Exception) 
        {
        }
      } // foreach (DataGri...

      ScanMultipleSystems lScan = ScanMultipleSystems.getInstance(lTargetList);
      lScan.ShowDialog();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void thisSystemToolStripMenuItem_Click(object sender, EventArgs e)
    {
      List<Tuple<String, String>> lTargetList = new List<Tuple<String, String>>();
      String lIP = String.Empty;
      String lMAC = String.Empty;

      try
      {
        lIP = DGV_Targets.SelectedRows[0].Cells["IP"].Value.ToString();
        lMAC = DGV_Targets.SelectedRows[0].Cells["MAC"].Value.ToString();
        lTargetList.Add(new Tuple<String, String>(lMAC, lIP));
      }
      catch (Exception lEx)
      { }

      try
      {
        ScanMultipleSystems.getInstance(lTargetList).ShowDialog();
      }
      catch (Exception lEx)
      {
        String lMsg = lEx.Message;
      }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DGV_Targets_MouseDown(object sender, MouseEventArgs e)
    {
      try
      {
        DataGridView.HitTestInfo hti = DGV_Targets.HitTest(e.X, e.Y);

        if (hti.RowIndex >= 0)
        {
          DGV_Targets.ClearSelection();
          DGV_Targets.Rows[hti.RowIndex].Selected = true;
          DGV_Targets.CurrentCell = DGV_Targets.Rows[hti.RowIndex].Cells[0];
        }
      }
      catch (Exception lEx)
      {
        LogConsole.Main.LogConsole.pushMsg(String.Format("ARPScan: {1}", lEx.Message));
        DGV_Targets.ClearSelection();
      }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DGV_Targets_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
      try
      {
        DataGridViewRow lCurrentRow = DGV_Targets.SelectedRows[0];
        String lIP = lCurrentRow.Cells["IP"].Value.ToString();
        String lMAC = lCurrentRow.Cells["MAC"].Value.ToString();
        String lVendor = lCurrentRow.Cells["vendor"].Value.ToString();

        SystemFingerprint.SystemFingerprint lFingerprint = new SystemFingerprint.SystemFingerprint(lMAC, lIP, lVendor);
        lFingerprint.ShowDialog();
      }
      catch (Exception lEx)
      {
        LogConsole.Main.LogConsole.pushMsg(String.Format("ARPScan: {1}", lEx.Message));
        DGV_Targets.ClearSelection();
      }
    }

    #endregion


  }
}
