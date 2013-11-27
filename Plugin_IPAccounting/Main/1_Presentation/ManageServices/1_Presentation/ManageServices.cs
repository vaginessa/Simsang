using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

using Simsang.Plugin;
using ManageServices = Plugin.Main.IPAccounting.ManageServices;


namespace Plugin.Main.IPAccounting.ManageServices
{
  public partial class Form_ManageServices : Form, IObserver
  {

    #region MEMBERS

    private BindingList<ManageServices.ServiceRecord> cServiceRecords;
    private IPluginHost cHost;
    private TaskFacade cTask;

    #endregion


    #region PUBLIC

    public Form_ManageServices(IPluginHost pHost)
    {
      InitializeComponent();


      #region DataGrid Services

      DataGridViewTextBoxColumn cServiceNameCol = new DataGridViewTextBoxColumn();
      cServiceNameCol.DataPropertyName = "servicename";
      cServiceNameCol.Name = "servicename";
      cServiceNameCol.HeaderText = "Service name";
      cServiceNameCol.ReadOnly = true;
      cServiceNameCol.Visible = true;
      cServiceNameCol.Width = 214;
      DGV_Services.Columns.Add(cServiceNameCol);

      DataGridViewTextBoxColumn cLowerPortCol = new DataGridViewTextBoxColumn();
      cLowerPortCol.DataPropertyName = "lowerport";
      cLowerPortCol.Name = "lowerport";
      cLowerPortCol.HeaderText = "Lower port";
      cLowerPortCol.ReadOnly = true;
      cLowerPortCol.Visible = true;
      cLowerPortCol.Width = 90;
      cLowerPortCol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
      DGV_Services.Columns.Add(cLowerPortCol);

      DataGridViewTextBoxColumn cUpperPortCol = new DataGridViewTextBoxColumn();
      cUpperPortCol.DataPropertyName = "upperport";
      cUpperPortCol.Name = "upperport";
      cUpperPortCol.HeaderText = "Upper port";
      cUpperPortCol.ReadOnly = true;
      cUpperPortCol.Visible = true;
      //      cUpperPortCol.Width = 90;
      cUpperPortCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      cUpperPortCol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
      DGV_Services.Columns.Add(cUpperPortCol);

      cServiceRecords = new BindingList<ManageServices.ServiceRecord>();
      DGV_Services.DataSource = cServiceRecords;

      #endregion


      cHost = pHost;
      cTask = TaskFacade.getInstance();
      cTask.addObserver(this);

      try
      {
        cTask.readServicesPatterns();
      }
      catch (FileNotFoundException lEx)
      {
        cHost.LogMessage(String.Format("Form_ManageServices() : {0}", lEx.Message));
      }
      catch (Exception lEx)
      {
        MessageBox.Show(String.Format("Error occurred while opening services definition file\r\nMessage: {0}", lEx.Message), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        cHost.LogMessage(String.Format("Form_ManageServices() : {0}", lEx.Message));
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
      String lLowerPortStr = TB_LowerPort.Text;
      String lUpperPortStr = TB_UpperPort.Text;
      String lServiceName = TB_ServiceName.Text;

      cTask.addRecord(new ServiceRecord(lServiceName, lLowerPortStr, lUpperPortStr));
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DGV_Services_MouseUp(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Right)
      {
        try
        {
          DataGridView.HitTestInfo hti = DGV_Services.HitTest(e.X, e.Y);
          if (hti.RowIndex >= 0)
          {
            DGV_Services.ClearSelection();
            DGV_Services.Rows[hti.RowIndex].Selected = true;
            CMS_ManageServices.Show(DGV_Services, e.Location);
          }
        }
        catch (Exception) { }
      }
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
        this.Close();
        return true;
      }
      else
        return base.ProcessDialogKey(keyData);
    }

    #endregion


    #region EVENTS

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void deleteServiceToolStripMenuItem_Click(object sender, EventArgs e)
    {
      int lRowIndex = DGV_Services.SelectedRows[0].Index;
      String lLowerPort = DGV_Services.Rows[lRowIndex].Cells["lowerport"].Value.ToString();
      String lUpperPort = DGV_Services.Rows[lRowIndex].Cells["upperport"].Value.ToString();

      /*
       * Remove entry from list
       */
      try
      {
        if (cServiceRecords != null && cServiceRecords.Count > 0)
          foreach (ManageServices.ServiceRecord lTmp in cServiceRecords.ToList())
            if (lTmp.LowerPort == lLowerPort && lTmp.UpperPort == lUpperPort)
              cTask.removeRecord(lTmp);
      }
      catch (Exception)
      {
      }
    }

    #endregion


    #region IObserver

    /// <summary>
    /// 
    /// </summary>
    /// <param name="oList"></param>
    public void update(List<ManageServices.ServiceRecord> oList)
    {
      cServiceRecords.Clear();
      foreach (ManageServices.ServiceRecord lTmp in oList)
        cServiceRecords.Add(lTmp);
    }

    #endregion

  }
}
