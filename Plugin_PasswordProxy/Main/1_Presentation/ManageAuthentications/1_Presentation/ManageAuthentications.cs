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



namespace Plugin.Main.HTTPProxy.ManageAuthentications
{
  public partial class Form_ManageAuthentications : Form, IObserver
  {

    #region MEMBERS

    private BindingList<AccountPattern> cAccountPatterns;
    private IPluginHost cHost;
    private TaskFacade cTask;

    #endregion


    #region PUBLIC

    public Form_ManageAuthentications(IPluginHost pHost)
    {
      InitializeComponent();

      #region DataGrid headers

      DataGridViewTextBoxColumn mMethodCol = new DataGridViewTextBoxColumn();
      mMethodCol.DataPropertyName = "method";
      mMethodCol.Name = "method";
      mMethodCol.HeaderText = "Method";
      mMethodCol.ReadOnly = true;
      mMethodCol.Visible = true;
      mMethodCol.Width = 60;
      DGV_AccountPatterns.Columns.Add(mMethodCol);

      DataGridViewTextBoxColumn mHostPatternCol = new DataGridViewTextBoxColumn();
      mHostPatternCol.DataPropertyName = "host";
      mHostPatternCol.Name = "host";
      mHostPatternCol.HeaderText = "Host pattern";
      mHostPatternCol.ReadOnly = true;
      mHostPatternCol.Visible = true;
      mHostPatternCol.Width = 130;
      DGV_AccountPatterns.Columns.Add(mHostPatternCol);

      DataGridViewTextBoxColumn mPathPatternCol = new DataGridViewTextBoxColumn();
      mPathPatternCol.DataPropertyName = "path";
      mPathPatternCol.Name = "path";
      mPathPatternCol.HeaderText = "Path pattern";
      mPathPatternCol.ReadOnly = true;
      mPathPatternCol.Visible = true;
      mPathPatternCol.Width = 130;
      DGV_AccountPatterns.Columns.Add(mPathPatternCol);

      DataGridViewTextBoxColumn mDataPatternCol = new DataGridViewTextBoxColumn();
      mDataPatternCol.DataPropertyName = "datapattern";
      mDataPatternCol.Name = "datapattern";
      mDataPatternCol.HeaderText = "Data pattern";
      mDataPatternCol.ReadOnly = true;
      mDataPatternCol.Visible = true;
      mDataPatternCol.Width = 170;
      DGV_AccountPatterns.Columns.Add(mDataPatternCol);

      DataGridViewTextBoxColumn mCompanyCol = new DataGridViewTextBoxColumn();
      mCompanyCol.DataPropertyName = "company";
      mCompanyCol.Name = "company";
      mCompanyCol.HeaderText = "Company";
      mCompanyCol.ReadOnly = true;
      mCompanyCol.Visible = true;
      mCompanyCol.Width = 130;
      DGV_AccountPatterns.Columns.Add(mCompanyCol);

      DataGridViewTextBoxColumn mWebPageCol = new DataGridViewTextBoxColumn();
      mWebPageCol.DataPropertyName = "webpage";
      mWebPageCol.Name = "webpage";
      mWebPageCol.HeaderText = "Web page";
      mWebPageCol.ReadOnly = true;
      mWebPageCol.Visible = true;
      mWebPageCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      DGV_AccountPatterns.Columns.Add(mWebPageCol);

      cAccountPatterns = new BindingList<AccountPattern>();
      DGV_AccountPatterns.DataSource = cAccountPatterns;

      #endregion

      cHost = pHost;
      cTask = TaskFacade.getInstance();
      cTask.addObserver(this);


      /*
       * Read pattern file
       */
      try
      {
        cTask.readAccountsPatterns();
      }
      catch (FileNotFoundException lEx)
      {
        cHost.LogMessage(String.Format("Form_ManageAuthentications() : {0}", lEx.Message));
      }
      catch (Exception lEx)
      {
        MessageBox.Show("Error occurred while opening patterns file.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        cHost.LogMessage(String.Format("Form_ManageAuthentications() : {0}", lEx.Message));
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
      String lMethod = TB_Method.Text;
      String lHostPattern = TB_HostPattern.Text;
      String lPathPattern = TB_PathPattern.Text;
      String lDataPattern = TB_DatePattern.Text;
      String lCompany = TB_Company.Text;
      String lWebPage = TB_WebPage.Text;
      String lErrorMsg = String.Empty;


      if (lErrorMsg.Length > 0)
        MessageBox.Show(lErrorMsg, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      else
      {
        foreach (AccountPattern lTmp in cAccountPatterns)
        {
          if (lTmp.Method == lMethod && lTmp.Host == lHostPattern && lTmp.Path == lPathPattern && lTmp.DataPattern == lDataPattern)
          {
            lErrorMsg = "Pattern already exists";
            break;
          }
        } // foreach (Accou...


        if (lErrorMsg.Length > 0)
          MessageBox.Show(lErrorMsg, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        else
        {
          try
          {
            cTask.addRecord(new AccountPattern(lMethod, lHostPattern, lPathPattern, lDataPattern, lCompany, lWebPage));
            cTask.saveAccountPatterns();
          }
          catch (IOException lEx)
          {
            MessageBox.Show(String.Format("Error occurred while write pattern file.\r\n Message: {0}", lEx.Message), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            cHost.LogMessage(String.Format("Form_ManageAuthentications() : {0}", lEx.Message));
          }
          catch (Exception lEx)
          {
            MessageBox.Show(String.Format("Error occurred while adding new account pattern.\r\nMessage: {0}", lEx.Message), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            cHost.LogMessage(String.Format("Error occurred while adding new account pattern : {0}", lEx.Message));
          }
        }
      }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DGV_AccountPatterns_MouseUp(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Right)
      {
        try
        {
          DataGridView.HitTestInfo hti = DGV_AccountPatterns.HitTest(e.X, e.Y);
          if (hti.RowIndex >= 0)
          {
            DGV_AccountPatterns.ClearSelection();
            DGV_AccountPatterns.Rows[hti.RowIndex].Selected = true;
            CMS_ManageAccounts.Show(DGV_AccountPatterns, e.Location);
          }
        }
        catch (Exception) { }
      }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void deletePatternToolStripMenuItem_Click(object sender, EventArgs e)
    {
      int lRowIndex = 0;
      String lMethod = String.Empty;
      String lHost = String.Empty;
      String lPath = String.Empty;
      String lDataPattern = String.Empty;

      try
      {
        lRowIndex = DGV_AccountPatterns.SelectedRows[0].Index;
        lMethod = DGV_AccountPatterns.Rows[lRowIndex].Cells["method"].Value.ToString();
        lHost = DGV_AccountPatterns.Rows[lRowIndex].Cells["host"].Value.ToString();
        lPath = DGV_AccountPatterns.Rows[lRowIndex].Cells["path"].Value.ToString();
        lDataPattern = DGV_AccountPatterns.Rows[lRowIndex].Cells["datapattern"].Value.ToString();
      }
      catch (Exception)
      {
        return;
      }

      /*
       * Remove record from list.
       */
      try
      {
        if (cAccountPatterns != null && cAccountPatterns.Count > 0)
          foreach (AccountPattern lTmp in cAccountPatterns.ToList())
            if (lTmp.Method == lMethod && lTmp.Host == lHost && lTmp.Path == lPath && lTmp.DataPattern == lDataPattern)
              cTask.removeRecord(lTmp);
      }
      catch (Exception)
      {
        return;
      }


      /*
       * Write new Authentication pattern list to file
       */
      try
      {
        cTask.saveAccountPatterns();
      }
      catch (IOException lEx)
      {
        MessageBox.Show(String.Format("Error occurred while write pattern file.\r\n Message: {0}", lEx.Message), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        cHost.LogMessage(String.Format("Form_ManageAuthentications() : {0}", lEx.Message));
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


    #region OBSERVER

    public void update(List<AccountPattern> pPatterns)
    {
      if (cAccountPatterns != null)
      {
        cAccountPatterns.Clear();
        foreach (AccountPattern tmp in pPatterns)
          cAccountPatterns.Add(tmp);
      }
    }

    #endregion

  }
}
