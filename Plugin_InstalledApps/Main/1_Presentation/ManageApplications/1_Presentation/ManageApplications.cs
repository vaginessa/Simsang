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
using System.Xml;
using System.Xml.Serialization;

using Simsang.Plugin;


namespace Plugin.Main.Applications.ManageApplications
{
  public partial class Form_ManageApps : Form, IObserver
  {

    #region MEMBERS

    private BindingList<ApplicationPattern> cApplicationPatterns;
    private IPluginHost cHost;
    private TaskFacade cTask;

    #endregion


    #region PUBLIC

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pHost"></param>
    public Form_ManageApps(IPluginHost pHost)
    {
      InitializeComponent();

      cHost = pHost;

      #region Datagrid header

      DataGridViewTextBoxColumn mApplicationNameCol = new DataGridViewTextBoxColumn();
      mApplicationNameCol.DataPropertyName = "applicationname";
      mApplicationNameCol.Name = "applicationname";
      mApplicationNameCol.HeaderText = "Application name";
      mApplicationNameCol.ReadOnly = true;
      mApplicationNameCol.Visible = true;
      mApplicationNameCol.Width = 140;
      DGV_ApplicationPatterns.Columns.Add(mApplicationNameCol);

      DataGridViewTextBoxColumn mCompanyURLCol = new DataGridViewTextBoxColumn();
      mCompanyURLCol.DataPropertyName = "companyurl";
      mCompanyURLCol.Name = "companyurl";
      mCompanyURLCol.HeaderText = "Company URL";
      mCompanyURLCol.ReadOnly = true;
      mCompanyURLCol.Visible = true;
      mCompanyURLCol.Width = 170;
      DGV_ApplicationPatterns.Columns.Add(mCompanyURLCol);

      DataGridViewTextBoxColumn mApplicationPatternCol = new DataGridViewTextBoxColumn();
      mApplicationPatternCol.DataPropertyName = "applicationpatternstring";
      mApplicationPatternCol.Name = "applicationpatternstring";
      mApplicationPatternCol.HeaderText = "Application pattern";
      mApplicationPatternCol.ReadOnly = true;
      mApplicationPatternCol.Visible = true;
      //      mApplicationPatternCol.Width = 280;
      mApplicationPatternCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      DGV_ApplicationPatterns.Columns.Add(mApplicationPatternCol);

      cApplicationPatterns = new BindingList<ApplicationPattern>();
      DGV_ApplicationPatterns.DataSource = cApplicationPatterns;


      #endregion

      cTask = TaskFacade.getInstance();
      cTask.addObserver(this);

      try
      {
        cTask.readApplicationPatterns();
      }
      catch (FileNotFoundException)
      {
      }
      catch (Exception lEx)
      {
        MessageBox.Show(lEx.StackTrace, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        cHost.LogMessage(String.Format("Form_ManageApps() : {0}", lEx.Message));
      }
    }

    #endregion


    #region PRIVATE


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BT_Add_Click(object sender, EventArgs e)
    {
      String lURLPattern = TB_URLPattern.Text;
      String lCompany = TB_ProgramName.Text;
      String lCompanyWebPage = TB_WebPage.Text;

      try
      {
        cTask.addRecord(new ApplicationPattern(lURLPattern, lCompany, lCompanyWebPage));
      }
      catch (Exception lEx)
      {
        MessageBox.Show(lEx.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        cHost.LogMessage(String.Format("Form_ManageApps() : {0}", lEx.Message));
      }
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DGV_ApplicationPatterns_MouseUp(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Right)
      {
        try
        {
          DataGridView.HitTestInfo hti = DGV_ApplicationPatterns.HitTest(e.X, e.Y);
          if (hti.RowIndex >= 0)
          {
            DGV_ApplicationPatterns.ClearSelection();
            DGV_ApplicationPatterns.Rows[hti.RowIndex].Selected = true;
            CMS_ManagePatterns.Show(DGV_ApplicationPatterns, e.Location);
          }
        }
        catch (Exception) { }
      } // if (e.But...
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
    {
      int lRowIndex = DGV_ApplicationPatterns.SelectedRows[0].Index;
      String lApplicationID = DGV_ApplicationPatterns.Rows[lRowIndex].Cells["applicationpatternstring"].Value.ToString();


      try
      {
        if (cApplicationPatterns != null && cApplicationPatterns.Count > 0)
          foreach (ApplicationPattern lTmp in cApplicationPatterns.ToList())
            if (lTmp.ApplicationPatternString == lApplicationID)
              cTask.removeRecord(lTmp);
      }
      catch (Exception lEx)
      {
      }
    }

    #endregion


    #region EVENTS

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


    #region OBSERVER

    public void update(List<ApplicationPattern> pRecordList)
    {
      cApplicationPatterns.Clear();
      foreach (ApplicationPattern lTmp in pRecordList)
        cApplicationPatterns.Add(lTmp);
    }

    #endregion

  }
}
