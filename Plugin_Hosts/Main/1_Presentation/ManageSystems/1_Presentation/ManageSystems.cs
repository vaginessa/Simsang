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
using Plugin.Main;
using Plugin.Main.Systems.Config;


namespace Plugin.Main.Systems.ManageSystems
{
  public partial class Form_ManageSystems : Form, IObserver
  {

    #region MEMBERS

    private BindingList<SystemPattern> cSystemPatterns;
    private IPluginHost cHost;
    private TaskFacade cTask;

    #endregion


    #region PUBLIC

    public Form_ManageSystems(IPluginHost pHost)
    {
      InitializeComponent();

      cHost = pHost;

      #region Datagrid headers

      DataGridViewTextBoxColumn mSystemNameCol = new DataGridViewTextBoxColumn();
      mSystemNameCol.DataPropertyName = "systemname";
      mSystemNameCol.Name = "systemname";
      mSystemNameCol.HeaderText = "System name";
      mSystemNameCol.ReadOnly = true;
      mSystemNameCol.Visible = true;
      mSystemNameCol.Width = 140;
      DGV_Systems.Columns.Add(mSystemNameCol);

      DataGridViewTextBoxColumn mSystemPatternCol = new DataGridViewTextBoxColumn();
      mSystemPatternCol.DataPropertyName = "systempatternstring";
      mSystemPatternCol.Name = "systempatternstring";
      mSystemPatternCol.HeaderText = "System pattern";
      mSystemPatternCol.ReadOnly = true;
      mSystemPatternCol.Visible = true;
      mSystemPatternCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      DGV_Systems.Columns.Add(mSystemPatternCol);

      cSystemPatterns = new BindingList<SystemPattern>();
      DGV_Systems.DataSource = cSystemPatterns;

      #endregion

      cTask = TaskFacade.getInstance();
      cTask.addObserver(this);

      try
      {
        cTask.readSystemPatterns();
      }
      catch (FileNotFoundException lEx)
      {
        cHost.LogMessage(String.Format("Form_ManageSystems() : {0}", lEx.Message));
        return;
      }
      catch (Exception lEx)
      {
        MessageBox.Show(lEx.StackTrace, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        cHost.LogMessage(String.Format("Form_ManageSystems() : {0}", lEx.Message));
        return;
      }
    }

    #endregion


    #region EVENTS

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void deleteSystemToolStripMenuItem_Click(object sender, EventArgs e)
    {
      int lRowIndex = -1;
      String lApplicationID = String.Empty;

      try
      {
        lRowIndex = DGV_Systems.SelectedRows[0].Index;
        lApplicationID = DGV_Systems.Rows[lRowIndex].Cells["systempatternstring"].Value.ToString();
      }
      catch (Exception)
      {
        return;
      }


      /*
       * Remove entry from list
       */
      try
      {
        if (cSystemPatterns != null && cSystemPatterns.Count > 0)
        {
          foreach (SystemPattern lTmp in cSystemPatterns.ToList())
          {
            if (lTmp.SystemPatternString == lApplicationID)
            {
              try
              {
                cTask.removeRecord(lTmp);
              }
              catch (Exception lEx)
              {
                MessageBox.Show(lEx.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cHost.LogMessage(String.Format("Form_ManageSystems() : {0}", lEx.Message));
              }
            }
          }
        }
      }
      catch (Exception)
      {
        return;
      }
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DGV_Systems_MouseUp(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Right)
      {
        try
        {
          DataGridView.HitTestInfo hti = DGV_Systems.HitTest(e.X, e.Y);
          if (hti.RowIndex >= 0)
          {
            DGV_Systems.ClearSelection();
            DGV_Systems.Rows[hti.RowIndex].Selected = true;
            CMS_ManageSystems.Show(DGV_Systems, e.Location);
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
    private void button1_Click(object sender, EventArgs e)
    {
      String lSystemName = TB_SystemName.Text;
      String lSystemPattern = TB_SystemPattern.Text;
      String lErrorMsg = String.Empty;

      cTask.addRecord(new SystemPattern(lSystemPattern, lSystemName));
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

    public void update(List<SystemPattern> pSystemList)
    {
      cSystemPatterns.Clear();
      foreach (SystemPattern lTmp in pSystemList)
        cSystemPatterns.Add(lTmp);
    }

    #endregion

  }
}
