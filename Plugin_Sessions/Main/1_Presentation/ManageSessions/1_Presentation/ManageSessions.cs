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
using System.Xml.Linq;
using System.Xml.Serialization;

using Simsang.Plugin;
using Plugin.Main.Session.Config;
using Plugin.Main.Session.ManageSessions.Config;


namespace Plugin.Main.Session.ManageSessions
{
  public partial class ManageSessions : Form, IObserver
  {

    #region MEMBERS

    private BindingList<SessionPattern> cSessionPatternRecords;
    private String cIconsPath = @"plugins\Sessions\Icons";
    private PluginSessionsUC cPluginMain;
    private TaskFacade cTask;

    #endregion


    #region PUBLIC

    public ManageSessions(PluginSessionsUC pPluginMain)
    {
      InitializeComponent();

      cPluginMain = pPluginMain;

      #region DGV Header definitions

      DataGridViewTextBoxColumn cSessionNameCol = new DataGridViewTextBoxColumn();
      cSessionNameCol.DataPropertyName = "sessionname";
      cSessionNameCol.Name = "sessionname";
      cSessionNameCol.HeaderText = "Session name";
      cSessionNameCol.ReadOnly = true;
      cSessionNameCol.Visible = true;
      cSessionNameCol.Width = 120;
      cSessionNameCol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
      DGV_SessionPatterns.Columns.Add(cSessionNameCol);

      DataGridViewTextBoxColumn cHTTPHostCol = new DataGridViewTextBoxColumn();
      cHTTPHostCol.DataPropertyName = "httphost";
      cHTTPHostCol.Name = "httphost";
      cHTTPHostCol.HeaderText = "HTTP Host regex";
      cHTTPHostCol.ReadOnly = true;
      cHTTPHostCol.Visible = true;
      cHTTPHostCol.Width = 180;
      cHTTPHostCol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
      DGV_SessionPatterns.Columns.Add(cHTTPHostCol);

      DataGridViewTextBoxColumn cWebPageCol = new DataGridViewTextBoxColumn();
      cWebPageCol.DataPropertyName = "webpage";
      cWebPageCol.Name = "webpage";
      cWebPageCol.HeaderText = "Web page";
      cWebPageCol.ReadOnly = true;
      cWebPageCol.Visible = true;
      cWebPageCol.Width = 180;
      cWebPageCol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
      DGV_SessionPatterns.Columns.Add(cWebPageCol);

      DataGridViewTextBoxColumn cSessionPatternRegexCol = new DataGridViewTextBoxColumn();
      cSessionPatternRegexCol.DataPropertyName = "sessionpatternstring";
      cSessionPatternRegexCol.Name = "sessionpatternstring";
      cSessionPatternRegexCol.HeaderText = "Session regex";
      cSessionPatternRegexCol.ReadOnly = true;
      cSessionPatternRegexCol.Visible = true;
      //cSessionPatternRegexCol.Width = 90;
      cSessionPatternRegexCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      cSessionPatternRegexCol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
      DGV_SessionPatterns.Columns.Add(cSessionPatternRegexCol);

      cSessionPatternRecords = new BindingList<SessionPattern>();
      DGV_SessionPatterns.DataSource = cSessionPatternRecords;
      #endregion

      cTask = TaskFacade.getInstance();
      cTask.addObserver(this);
      
      try
      {
        cTask.readSessionPatterns();
      }
      catch (FileNotFoundException lEx)
      {
        cPluginMain.PluginHost.LogMessage(String.Format("ManageSessions() : {0}", lEx.Message));
      }
      catch (DirectoryNotFoundException lEx)
      {
        cPluginMain.PluginHost.LogMessage(String.Format("ManageSessions() : {0}", lEx.Message));
      }
      catch (Exception lEx)
      {
        MessageBox.Show("Error ocurred: " + lEx.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        cPluginMain.PluginHost.LogMessage(String.Format("ManageSessions() : {0}", lEx.Message));
      }

      initInputFields();
    }

    #endregion


    #region PRIVATE

    /// <summary>
    /// 
    /// </summary>
    private void initInputFields()
    {
      TB_HostRegex.Text = String.Empty;
      TB_Icon.Text = String.Empty;
      TB_SessionCookiesPattern.Text = String.Empty;
      TB_SessionName.Text = String.Empty;
      TB_WebPage.Text = String.Empty;
    }

    #endregion


    #region EVENTS

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ManageSessions_MouseUp(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Right)
      {
        try
        {
          DataGridView.HitTestInfo hti = DGV_SessionPatterns.HitTest(e.X, e.Y);
          if (hti.RowIndex >= 0)
          {
            DGV_SessionPatterns.ClearSelection();
            DGV_SessionPatterns.Rows[hti.RowIndex].Selected = true;
            CMS_ManageSessions.Show(DGV_SessionPatterns, e.Location);
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
    private void deleteSessionPatternToolStripMenuItem_Click(object sender, EventArgs e)
    {
      int lRowIndex = DGV_SessionPatterns.SelectedRows[0].Index;
      String lSessionName = DGV_SessionPatterns.Rows[lRowIndex].Cells["sessionname"].Value.ToString();


      /*
       * Remove entry from list
       */
      try
      {
        if (cSessionPatternRecords != null && cSessionPatternRecords.Count > 0)
          foreach (SessionPattern lTmp in cSessionPatternRecords.ToList())
            if (lTmp.SessionName == lSessionName)
            {
              try
              {
                String lFileName = lSessionName.ToLower();
                String lFileExtension = Path.GetExtension(TB_Icon.Text);
                String lFilePattern = String.Format(@"{1}.*", cIconsPath, lFileName);

                foreach (String lTmpFile in Directory.GetFiles(cIconsPath, lFilePattern))
                  File.Delete(lTmpFile);

              }
              catch (Exception) { }


              try
              {
                cTask.removeRecord(lTmp);
              }
              catch (Exception lEx)
              {
                MessageBox.Show(lEx.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cPluginMain.PluginHost.LogMessage(String.Format("ManageSessions() : {0}", lEx.Message));
              }
            }
      }
      catch (Exception)
      {
      }

    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BT_Icon_Click(object sender, EventArgs e)
    {
      String lIconPath = String.Empty;

      try
      {
        OFD_Icon.ShowDialog();

        String lDirectoryName = !String.IsNullOrEmpty(OFD_Icon.FileName) ? Path.GetDirectoryName(OFD_Icon.FileName) : Directory.GetCurrentDirectory();
        String lFileName = !String.IsNullOrEmpty(OFD_Icon.FileName) ? Path.GetFileName(OFD_Icon.FileName) : String.Empty;

        lIconPath = String.Format("{0}/{1}", lDirectoryName, lFileName);

        //OFD_Icon.ShowDialog();
        //lIconPath = String.Format("{0}/{1}", Path.GetDirectoryName(OFD_Icon.FileName), Path.GetFileName(OFD_Icon.FileName));
      }
      catch (Exception) { }

      if (!string.IsNullOrEmpty(lIconPath))
      {
        try
        {
          if (File.Exists(lIconPath))
            TB_Icon.Text = lIconPath;
          else
            TB_Icon.Text = String.Empty;
        }
        catch (Exception)
        {
          TB_Icon.Text = String.Empty;
        }
      }
      else
        TB_Icon.Text = String.Empty;

    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BT_Add_Click(object sender, EventArgs e)
    {
      String lSessionName = TB_SessionName.Text;
      String lHostRegex = @TB_HostRegex.Text;
      String lWebPage = TB_WebPage.Text;
      String lSessionCookiesPattern = @TB_SessionCookiesPattern.Text;
      String lIconPath = TB_Icon.Text;


      /*
       * Copy the icon and add the record to the data grid view
       */
      if (TB_Icon.Text.Length > 0)
      {
        try
        {
          String lFileName = lSessionName.ToLower();
          String lFileExtension = Path.GetExtension(TB_Icon.Text);
          String lNewFileName = String.Format(@"{0}\{1}{2}", cIconsPath, lFileName, lFileExtension);

          File.Copy(TB_Icon.Text, lNewFileName);
        }
        catch (Exception) 
        {
        }
      }

      try
      {
        cTask.addRecord(new SessionPattern(lSessionCookiesPattern, lSessionName, lHostRegex, lWebPage));
      }
      catch (Exception lEx)
      {
        MessageBox.Show(lEx.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        cPluginMain.PluginHost.LogMessage(String.Format("ManageSessions() : {0}", lEx.Message));
        return;
      }

      initInputFields();
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

    public void update(List<SessionPattern> pPatterns)
    {
      if (cSessionPatternRecords != null)
      {
        cSessionPatternRecords.Clear();
        foreach (SessionPattern tmp in pPatterns)
          cSessionPatternRecords.Add(tmp);
      }
    }

    #endregion

  }
}
