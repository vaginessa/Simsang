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

using Simsang.Session.Config;



namespace Simsang.SessionsExport
{
  public partial class SessionExport : Form
  {

    #region MEMBERS

    private BindingList<SessionRecord> mSessionRecord;
    private static String mSessionDir = String.Format("{0}{1}", Directory.GetCurrentDirectory(), Config.SessionDir);
    private SimsangMain mACMain;
    private Simsang.Session.TaskFacade cTaskFacade;

    #endregion


    #region PUBLIC

    public SessionExport(SimsangMain pACMain)
    {
      InitializeComponent();

      #region Datagrid header

      DataGridViewTextBoxColumn mNameCol = new DataGridViewTextBoxColumn();
      mNameCol.DataPropertyName = "Name";
      mNameCol.HeaderText = "Name";
      mNameCol.Name = "Name";
      mNameCol.ReadOnly = true;
      mNameCol.Width = 230;
      DGV_Sessions.Columns.Add(mNameCol);


      DataGridViewTextBoxColumn mDescrCol = new DataGridViewTextBoxColumn();
      mDescrCol.DataPropertyName = "Description";
      mDescrCol.HeaderText = "Description";
      mDescrCol.Name = "Description";
      mDescrCol.ReadOnly = true;
      mDescrCol.Visible = false;
      mDescrCol.Width = 125;
      DGV_Sessions.Columns.Add(mDescrCol);

      DataGridViewTextBoxColumn mStartCol = new DataGridViewTextBoxColumn();
      mStartCol.DataPropertyName = "Start";
      mStartCol.HeaderText = "Start";
      mStartCol.Name = "Start";
      mStartCol.ReadOnly = true;
      mStartCol.Width = 135;
      DGV_Sessions.Columns.Add(mStartCol);

      DataGridViewTextBoxColumn mStopCol = new DataGridViewTextBoxColumn();
      mStopCol.DataPropertyName = "Stop";
      mStopCol.HeaderText = "Stop";
      mStopCol.Name = "Stop";
      mStopCol.ReadOnly = true;
      mStopCol.Width = 135;
      DGV_Sessions.Columns.Add(mStopCol);

      DataGridViewTextBoxColumn mFileNameCol = new DataGridViewTextBoxColumn();
      mFileNameCol.DataPropertyName = "File";
      mFileNameCol.HeaderText = "File";
      mFileNameCol.Name = "File";
      mFileNameCol.ReadOnly = true;
      mFileNameCol.Visible = false;
      mFileNameCol.Width = 0;
      DGV_Sessions.Columns.Add(mFileNameCol);

      #endregion

      mACMain = pACMain;

      cTaskFacade = Simsang.Session.TaskFacade.getInstance();
      mSessionRecord = new BindingList<SessionRecord>();
      LoadSessionData();
      DGV_Sessions.DataSource = mSessionRecord;

    }


    #endregion


    #region PRIVATE

    /// <summary>
    /// 
    /// </summary>
    private void LoadSessionData()
    {
      mSessionRecord.Clear();
      List<AttackSession> lSessionRecord = cTaskFacade.getAllSessions();

      if (lSessionRecord != null && lSessionRecord.Count > 0)
      {
        foreach (AttackSession lSess in lSessionRecord)
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


    #region EVENTS

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DGV_Sessions_DoubleClick(object sender, EventArgs e)
    {
      String lSessionFilePath = String.Empty;
      String lSessionName = String.Empty;
      String lSessionFileName = String.Empty;
      String lSessionDescription = String.Empty;
      String lSessionStart = String.Empty;
      String lSessionStop = String.Empty;
      String lSessionData = String.Empty;

      String lDataString = String.Empty;
      String lDataStringSession = String.Empty;
      String lDataStringPlugins = String.Empty;

      try
      {
        /*
         * Get general session data
         */
        lSessionFileName = Path.GetFileNameWithoutExtension(DGV_Sessions.CurrentRow.Cells["File"].Value.ToString());
        lSessionName = DGV_Sessions.CurrentRow.Cells["Name"].Value.ToString();

        lSessionDescription = DGV_Sessions.Rows[DGV_Sessions.CurrentRow.Index].Cells["Description"].Value.ToString();
        lSessionStart = DGV_Sessions.Rows[DGV_Sessions.CurrentRow.Index].Cells["Start"].Value.ToString();
        lSessionStop = DGV_Sessions.Rows[DGV_Sessions.CurrentRow.Index].Cells["Stop"].Value.ToString();


        lSessionData = cTaskFacade.readMainSessionData(lSessionFileName);
        lDataStringSession = String.Format("<Session name=\"{0}\" filename=\"{1}\" description=\"{2}\" start=\"{3}\" stop=\"{4}\">\n{5}\n</Session>\n", lSessionName, lSessionFileName, lSessionDescription, lSessionStart, lSessionStop, lSessionData);


        /*
         * Get plugin session data
         */
        Simsang.Plugin.IPlugin[] lPlugin = mACMain.GetPluginModule.PluginList;
        for (int lCount = 0; lCount < lPlugin.Count(); lCount++)
        {
          if (lPlugin[lCount] != null)
          {
            String lPluginDir = lPlugin[lCount].Config.BaseDir;
            lPluginDir = Regex.Replace(lPluginDir, @"[\/\\]+$", String.Empty);
            String[] lDirs = Regex.Split(lPluginDir, @"[\/\\]+");

            if (lDirs.Length > 0)
            {
              lDataStringPlugins += String.Format("<Plugin name=\"{0}\" dirname=\"{1}\">\n{2}\n</Plugin>\n", lPlugin[lCount].Config.PluginName, lDirs.Last(), lPlugin[lCount].onGetSessionData(lSessionFileName));
            } // if (lDi...
          } // if (lPlugi...
        } // for (int lCou...

        lDataString = String.Format("<SessionExport>\n{0}\n<Plugins>\n{1}\n</Plugins>\n</SessionExport>\n", lDataStringSession, lDataStringPlugins);
      }
      catch (Exception lEx)
      {
        LogConsole.Main.LogConsole.pushMsg(lEx.StackTrace);
        MessageBox.Show(String.Format("Exception occurred while exporting session \"{0}\"\r\n{1}", lSessionName, lEx.Message), "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        return;
      }

      try
      {
        SFD_SessionExport.FileName = String.Format("{0}.{1}", lSessionFileName, Config.SimsangFileExtension);
        SFD_SessionExport.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        SFD_SessionExport.Filter = String.Format("Simsang session files | *.{0}", Config.SimsangFileExtension);

        if (SFD_SessionExport.ShowDialog() == DialogResult.OK)
        {
          cTaskFacade.writeSessionExportFile(SFD_SessionExport.FileName, lDataString);
          MessageBox.Show(String.Format("Session \"{0}\" exported successfully.", lSessionName), "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        } // if (SFD_Ses...
      }
      catch (Exception lEx)
      {
        MessageBox.Show(String.Format("Exception msg: {0}\r\n{1}", lEx.Message, lEx.StackTrace));
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

  }
}
