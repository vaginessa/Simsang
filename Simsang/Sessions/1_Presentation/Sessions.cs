using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Windows.Forms;

using Simsang.Plugin;



namespace Simsang
{
  public partial class Sessions : Form
  {

    #region MEMBERS

    private BindingList<SessionRecord> mSessionRecord;
    private static String mSessionDir = String.Format("{0}{1}", Directory.GetCurrentDirectory(), Config.SessionDir);
    private ACMain mACMain;
    private static Sessions mInstance;

    #endregion


    #region PUBLIC

    public Sessions(ACMain pACMain)
    {
      InitializeComponent();

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

      mACMain = pACMain;
      mSessionRecord = new BindingList<SessionRecord>();
      LoadSessionData();
      DGV_Sessions.DataSource = mSessionRecord;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pACMain"></param>
    /// <returns></returns>
    public static Sessions getInstance(ACMain pACMain)
    {
      if (mInstance == null)
        mInstance = new Sessions(pACMain);

      return (mInstance);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionFileName"></param>
    /// <param name="pPluginList"></param>
    public static void RemoveSession(String pSessionFileName, IPlugin[] pPluginList)
    {
      String lFileName = String.Empty;


      /*
       * Save main session information.
       */
      try
      {
        lFileName = String.Format("{0}{1}", mSessionDir, pSessionFileName);
        if (File.Exists(lFileName))
          File.Delete(lFileName);
      }
      catch (Exception lEx)
      {
        LogConsole.Main.LogConsole.pushMsg(lEx.StackTrace);
      }

      /*
       * Remove plugin session data.
       */
      foreach (IPlugin lPlugin in pPluginList)
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
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionFileName"></param>
    /// <returns></returns>
    public bool loadSession(String pSessionFileName)
    {
      bool lRetVal = false;

      if (!String.IsNullOrEmpty(pSessionFileName))
      {
        /*
         * Load main GUI session data.
         */
        String lSessionFilePath = String.Format("{0}{1}.xml", mSessionDir, pSessionFileName);
        FileStream lFS = null;
        XmlSerializer lXMLSerial;
        AttackSession lAttackSession;
        String lStartIP = String.Empty;
        String lStopIP = String.Empty;


        try
        {
          lFS = new FileStream(lSessionFilePath, FileMode.Open);
          lXMLSerial = new XmlSerializer(typeof(AttackSession));
          lAttackSession = (AttackSession)lXMLSerial.Deserialize(lFS);

          mACMain.SetStartIP(lAttackSession.StartIP);
          mACMain.SetStopIP(lAttackSession.StopIP);
          mACMain.SetSessionName(lAttackSession.Name);
        }
        catch (Exception lEx)
        {
          LogConsole.Main.LogConsole.pushMsg(lEx.StackTrace);
          return (lRetVal);
        }
        finally
        {
          if (lFS != null)
            lFS.Close();
        }


        /*
         * Load plugin session data
         */
        if (!String.IsNullOrEmpty(lSessionFilePath))
        {
          foreach (IPlugin lPlugIn in mACMain.PluginsModule.PluginList)
          {
            try { lPlugIn.onLoadSessionDataFromFile(pSessionFileName); }
            catch { }
          }
        } // if (lSessionFil...
        lRetVal = true;
      } // if (!String....

      return (lRetVal);
    }

    #endregion


    #region PRIVATE

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


    /// <summary>
    /// 
    /// </summary>
    private void LoadSessionData()
    {
      mSessionRecord.Clear();
      List<AttackSession> lSessionRecord = AttackSession.GetAllSessions(mSessionDir);

      if (lSessionRecord != null && lSessionRecord.Count > 0)
      {
        foreach (AttackSession lSess in lSessionRecord)
        {
          try
          {
            //                        mSessionRecord.Add(new SessionRecord(lSess.FileName, lSess.Name, lSess.Description, lSess.StartTime, lSess.StopTime));
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
      if (e.Button == MouseButtons.Right)
      {
        try
        {
          DataGridView.HitTestInfo hti = DGV_Sessions.HitTest(e.X, e.Y);
          if (hti.RowIndex >= 0)
          {
            DGV_Sessions.CurrentRow.Selected = false;
            DGV_Sessions.Rows[hti.RowIndex].Selected = true;
            CMS_SessionMgmt.Show(DGV_Sessions, e.Location);
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
        lSessionFilePath = String.Format("{0}{1}.xml", mSessionDir, lFileName);

      }
      catch (Exception lEx)
      {
        LogConsole.Main.LogConsole.pushMsg(lEx.StackTrace);
      }

      /*
       * Load main GUI session data.
       */
      FileStream lFS = null;
      XmlSerializer lXMLSerial;
      AttackSession lAttackSession;
      String lStartIP = String.Empty;
      String lStopIP = String.Empty;

      try
      {
        lFS = new FileStream(lSessionFilePath, FileMode.Open);
        lXMLSerial = new XmlSerializer(typeof(AttackSession));
        lAttackSession = (AttackSession)lXMLSerial.Deserialize(lFS);

        mACMain.SetStartIP(lAttackSession.StartIP);
        mACMain.SetStopIP(lAttackSession.StopIP);
        mACMain.SetSessionName(lAttackSession.Name);
      }
      catch (Exception lEx)
      {
        LogConsole.Main.LogConsole.pushMsg(lEx.StackTrace);
      }
      finally
      {
        if (lFS != null)
          lFS.Close();
      }


      /*
       * Load plugin session data
       */
      if (lSessionFilePath.Length > 0)
        foreach (IPlugin lPlugIn in mACMain.PluginsModule.PluginList)
          lPlugIn.onLoadSessionDataFromFile(lFileName);

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
        lSessionFileName = DGV_Sessions.CurrentRow.Cells[0].Value.ToString();

        if (lSessionName.Length > 0 && lSessionFileName.Length > 0)
        {
          RemoveSession(lSessionFileName, mACMain.PluginsModule.PluginList);
          LoadSessionData();
        }
      }
      catch (Exception lEx)
      {
        LogConsole.Main.LogConsole.pushMsg(lEx.StackTrace);
      }

      MessageBox.Show("The session was deleted successfully.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
    #endregion

  }
}
