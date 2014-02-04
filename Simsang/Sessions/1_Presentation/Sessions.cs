using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

using Simsang.Plugin;
using Simsang.Session.Config;



namespace Simsang.Session
{
  public partial class Sessions : Form, IObserver
  {

    #region MEMBERS

    private BindingList<SessionRecord> mSessionRecord;
    private SimsangMain mACMain;
    private static Sessions mInstance;
    private TaskFacade mTask;

    #endregion


    #region PUBLIC

    public Sessions(SimsangMain pACMain)
    {
      InitializeComponent();

      mACMain = pACMain;


      #region Datagrid header

      DataGridViewTextBoxColumn mNameCol = new DataGridViewTextBoxColumn();
      mNameCol.DataPropertyName = "Name";
      mNameCol.HeaderText = "Name";
      mNameCol.ReadOnly = true;
      mNameCol.Width = 230;
      DGV_Sessions.Columns.Add(mNameCol);


      DataGridViewTextBoxColumn mDescrCol = new DataGridViewTextBoxColumn();
      mDescrCol.DataPropertyName = "Description";
      mDescrCol.HeaderText = "Description";
      mDescrCol.ReadOnly = true;
      mDescrCol.Visible = false;
      mDescrCol.Width = 125;
      DGV_Sessions.Columns.Add(mDescrCol);

      DataGridViewTextBoxColumn mStartCol = new DataGridViewTextBoxColumn();
      mStartCol.DataPropertyName = "Start";
      mStartCol.HeaderText = "Start";
      mStartCol.ReadOnly = true;
      mStartCol.Width = 135;
      DGV_Sessions.Columns.Add(mStartCol);

      DataGridViewTextBoxColumn mStopCol = new DataGridViewTextBoxColumn();
      mStopCol.DataPropertyName = "Stop";
      mStopCol.HeaderText = "Stop";
      mStopCol.ReadOnly = true;
      mStopCol.Width = 135;
      DGV_Sessions.Columns.Add(mStopCol);

      DataGridViewTextBoxColumn mFileNameCol = new DataGridViewTextBoxColumn();
      mFileNameCol.DataPropertyName = "File";
      mFileNameCol.HeaderText = "File";
      mFileNameCol.ReadOnly = true;
      mFileNameCol.Visible = false;
      mFileNameCol.Width = 0;
      DGV_Sessions.Columns.Add(mFileNameCol);

      #endregion


      mSessionRecord = new BindingList<SessionRecord>();
      DGV_Sessions.DataSource = mSessionRecord;

      mTask = TaskFacade.getInstance();
      mTask.addObserver(this);
      mTask.findAllSessions();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pACMain"></param>
    /// <returns></returns>
    public static Sessions getInstance(SimsangMain pACMain)
    {
      if (mInstance == null)
        mInstance = new Sessions(pACMain);

      return (mInstance);
    }


    /// <summary>
    /// 
    /// </summary>
    public void showDialogWindow()
    {
      mTask.findAllSessions();
      this.ShowDialog();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionFileName"></param>
    /// <returns></returns>
    public bool loadSession(String pSessionName)
    {
      bool lRetVal = false;
      AttackSession AttackSession;

      if (!String.IsNullOrEmpty(pSessionName))
      {
        // Load main GUI session data.
        AttackSession = mTask.loadSession(pSessionName);

        mACMain.SetStartIP(AttackSession.StartIP);
        mACMain.SetStopIP(AttackSession.StopIP);
        mACMain.SetSessionName(AttackSession.Name);

        // Load plugin session data
        foreach (IPlugin lPlugIn in mACMain.PluginsModule.PluginList)
        {
          try { lPlugIn.onLoadSessionDataFromFile(pSessionName); }
          catch { }
        }
        lRetVal = true;
      } // if (!String....

      return (lRetVal);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionName"></param>
    public void getSessionByName(String pSessionName)
    {
      mTask.getSessionByName(pSessionName);
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionName"></param>
    /// <param name="pSessionFileName"></param>
    public void removeSession(String pSessionName, String pSessionFileName)
    {
      if (!String.IsNullOrEmpty(pSessionName) && !String.IsNullOrEmpty(pSessionFileName))
      {
        mTask.removeSession(pSessionFileName);

        foreach (IPlugin lPlugin in mACMain.PluginsModule.PluginList)
        {
          try
          {
            if (lPlugin != null)
              lPlugin.onDeleteSessionData(Path.GetFileNameWithoutExtension(pSessionFileName));
          }
          catch (Exception lEx)
          {
            LogConsole.Main.LogConsole.pushMsg(lEx.StackTrace);
          }
        } // foreach (IP...
      } // if (lSession... 
    }

    #endregion


    #region EVENTS

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BT_Close_Click(object sender, EventArgs e)
    {
      Hide();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DGV_Session_MouseUp(object sender, MouseEventArgs e)
    {
      DataGridView.HitTestInfo hti;

      if (e.Button == MouseButtons.Right)
      {
        try
        {
          hti  = DGV_Sessions.HitTest(e.X, e.Y);

          // If cell selection is valid
          if (hti.ColumnIndex >= 0 && hti.RowIndex >= 0)
          {
            DGV_Sessions.CurrentRow.Selected = false;
            DGV_Sessions.CurrentCell = DGV_Sessions.Rows[hti.RowIndex].Cells[hti.ColumnIndex];
            DGV_Sessions.Rows[hti.RowIndex].Selected = true;
            CMS_SessionMgmt.Show(DGV_Sessions, new Point(e.X, e.Y));
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
    private void TSMI_Load_Click(object sender, EventArgs e)
    {
      String lSessionFilePath = String.Empty;
      String lFileName = String.Empty;

      try
      {
        lFileName = Path.GetFileNameWithoutExtension(DGV_Sessions.CurrentRow.Cells[0].Value.ToString());
        loadSession(lFileName);
      }
      catch (Exception lEx)
      {
        LogConsole.Main.LogConsole.pushMsg(lEx.StackTrace);
      }

      Hide();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TSMI_Remove_Click(object sender, EventArgs e)
    {
      String lSessionFileName = String.Empty;
      String lSessionName = String.Empty;

      try
      {
        lSessionName = DGV_Sessions.CurrentRow.Cells[1].Value.ToString();
        lSessionFileName = Path.GetFileNameWithoutExtension(DGV_Sessions.CurrentRow.Cells[0].Value.ToString());
        removeSession(lSessionName, lSessionFileName);

        // Reload session listing
        mTask.findAllSessions();

        MessageBox.Show("The session was deleted successfully.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
      catch (Exception lEx)
      {
        String lMsg = String.Format("Sessions: {0}", lEx.Message);
        LogConsole.Main.LogConsole.pushMsg(lMsg);
        MessageBox.Show(lMsg, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);        
      }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DGV_Sessions_DoubleClick(object sender, EventArgs e)
    {
      String lSessionFilePath = String.Empty;
      String lFileName = String.Empty;

      try
      {
        lFileName = Path.GetFileNameWithoutExtension(DGV_Sessions.CurrentRow.Cells[0].Value.ToString());
        loadSession(lFileName);
      }
      catch (Exception lEx)
      {
        LogConsole.Main.LogConsole.pushMsg(lEx.StackTrace);
      }

      Hide();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Sessions_FormClosing(object sender, FormClosingEventArgs e)
    {
      this.Hide();
      e.Cancel = true;
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


    #region IOBSERVER

    /// <summary>
    /// 
    /// </summary>
    /// <param name="oDict"></param>
    public void update(List<AttackSession> pRecords)
    {
      mSessionRecord.Clear();

      if (pRecords != null && pRecords.Count > 0)
      {
        foreach (AttackSession lSess in pRecords)
        {
          try
          {
            mSessionRecord.Add(new SessionRecord(lSess.SessionFileName, lSess.Name, lSess.Description, lSess.StartTime, lSess.StopTime));
          }
          catch (Exception lEx)
          {
            LogConsole.Main.LogConsole.pushMsg(lEx.StackTrace);
          }
        } // foreach (Attac...
      } // if (lSessionRecord ...
    }

    #endregion

  }
}
