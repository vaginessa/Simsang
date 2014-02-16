using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using System.Text.RegularExpressions;

using Simsang.Plugin;


namespace Simsang
{
  public class PluginModule : IPluginHost
  {

    #region MEMBERS

    private SimsangMain mSimsangMain;
    private IPlugin[] mPluginList;
    private Hashtable mPluginPosition = new Hashtable();

    #endregion


    #region PUBLIC

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pACMain"></param>
    public PluginModule(SimsangMain pACMain)
    {
      mSimsangMain = pACMain;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pPluginName"></param>
    /// <returns></returns>
    public IPlugin getPluginByName(String pPluginName)
    {
      IPlugin lRetVal = null;

      if (mPluginList != null)
      {
        foreach (IPlugin lPlugin in mPluginList)
        {
          try
          {
            if (lPlugin != null && lPlugin.Config.PluginName.ToLower().Contains(pPluginName.ToLower()))
            {
              lRetVal = lPlugin;
              break;
            } // if (lPlugi...
          }
          catch (Exception lEx) { MessageBox.Show(lEx.StackTrace); }
        } // foreach (IPlugi...
      } // if (mPlugi...

      return (lRetVal);
    }


    /// <summary>
    /// Load all activated plugins
    /// </summary>
    public void LoadPlugins()
    {
      Type lObjType = null;
      String[] lSplitter;
      Assembly lAsm;
      String lFileName;
      String lTempPluginPath;
      String[] lPluginList = Config.GetPluginList();


      /*
       * Iterate through all plugin directories.
       */
      mPluginList = new IPlugin[lPluginList.Length];

      for (int lPlugCnt = 0; lPlugCnt < lPluginList.Length; lPlugCnt++)
      {
        lTempPluginPath = lPluginList[lPlugCnt];
        String[] lPluginFiles = Directory.GetFiles(lTempPluginPath, "plugin_*.dll");



        for (int i = 0; i < lPluginFiles.Length; i++)
        {
          lFileName = lPluginFiles[i].Substring(
          lPluginFiles[i].LastIndexOf("\\") + 1,
          lPluginFiles[i].IndexOf(".dll") - lPluginFiles[i].LastIndexOf("\\") - 1);

          /*
           * Load plugin.
           */
          try
          {
            if ((lAsm = Assembly.LoadFile(lPluginFiles[i])) != null)
            {
              lSplitter = Regex.Split(lFileName, "_");

              if (lSplitter.Length == 2)
              {
                String lPluginName = "Plugin.Main.Plugin" + lSplitter[1] + "UC";
                lObjType = lAsm.GetType(lPluginName, false, false);
              } // if (lSplit...
            } // if ((lAsm = As...
          }
          catch (Exception lEx)
          {
            LogConsole.Main.LogConsole.pushMsg("Error occurred while loading plugin " + lFileName + " : " + lEx.StackTrace + "\n" + lEx.ToString());
            MessageBox.Show("Error occurred while loading plugin " + lFileName + " : " + lEx.Message);
          }

          /*
           * OK Lets create the object as we have the Report Type
           */
          try
          {
            if (lObjType != null)
            {
              PluginParameters lPluginParams = new PluginParameters()
              {
                PluginDirectoryFullPath = lTempPluginPath,
                SessionDirectoryFullPath = String.Format(@"{0}{1}", lTempPluginPath, Config.SessionDir),
                HostApplication = (IPluginHost) this
              };

              /*
               * Add loaded plugin to ...
               * - the global plugin list (IPlugin)
               * - the DGV list of used plugins 
               * - the plugin position list (name + position)
               */
              mPluginList[lPlugCnt] = (IPlugin) Activator.CreateInstance(lObjType, lPluginParams);
              mSimsangMain.DGVUsedPlugins.Add(new UsedPlugins(mPluginList[lPlugCnt].Config.PluginName, mPluginList[lPlugCnt].Config.PluginDescription, mPluginList[lPlugCnt].Config.PluginVersion, "1"));
              mPluginPosition.Add(mPluginList[lPlugCnt].Config.PluginName, lPlugCnt);
            } // if (lObjType...
          }
          catch (Exception lEx)
          {
            LogConsole.Main.LogConsole.pushMsg(String.Format("{0}: {1} {2}", lFileName, lEx.Message, lEx.StackTrace));
          }
        } // for (int i = 0;...
      } // for (int i =...
    }



    /// <summary>
    /// 
    /// </summary>
    public void initAllPlugins()
    {

      // 1. Call init event in the plugins, let them register via callback
      for (int i = 0; i < mPluginList.Length; i++)
        if (mPluginList[i] != null)
          mPluginList[i].onInit();



      // 2. Move the Simsang tab page to the end.
      TabPage lTabPageDefault = null;
      foreach (TabPage lPage in mSimsangMain.TCPlugins.TabPages)
      {
        if (lPage.Text.ToLower() == "simsang")
        {
          lTabPageDefault = lPage;
          mSimsangMain.TCPlugins.TabPages.Remove(lPage);
        }
        else
        {
        } // if (lPag...
      } // foreach (TabPag...

      if (lTabPageDefault != null)
        mSimsangMain.TCPlugins.TabPages.Add(lTabPageDefault);
    }


    /// <summary>
    /// 
    /// </summary>
    public void resetAllPlugins()
    {
      for (int lPlugCnt = 0; lPlugCnt < mPluginList.Length; lPlugCnt++)
        if (mPluginList[lPlugCnt] != null)
          mPluginList[lPlugCnt].onResetPlugin();
    }


    /// <summary>
    /// 
    /// </summary>
    public void CloseInactivePlugins()
    {
      foreach (TabPage lTabPage in mSimsangMain.TCPlugins.TabPages)
      {
        String lTabTitle = lTabPage.Text;
        String lPlugState = Config.GetRegistryValue(lTabTitle, "state");

        if (lPlugState == "off")
        {
          int lTabIndex = -1;

          try
          {
            for (int lCounter = 0; lCounter < mSimsangMain.UsedPlugins.Count; lCounter++)
            {
              if (mSimsangMain.UsedPlugins[lCounter].PluginName == lTabPage.Text)
              {
                lTabIndex = lCounter;
                break;
              } // if (mAC...
            } // for (int ...


            //lTabIndex = (int) mSimsangMain.GetTabPageHandler.PluginPosition[lTabTitle];
            if (lTabIndex >= 0)
            {
              mSimsangMain.UsedPlugins[lTabIndex].Active = "0";
              mSimsangMain.GetTabPageHandler.HideTabPage(lTabPage);
              try
              { mSimsangMain.GetPluginByName(lTabPage.Text).Config.IsActive = false; }
              catch (Exception) { }
            }
          }
          catch (Exception lEx)
          {
            LogConsole.Main.LogConsole.pushMsg(lEx.StackTrace);
          }
        }
        else if (lPlugState != "on")
        {
          Config.SetRegistryValue(lTabPage.Text, "state", "on");
        } // if (lPl...
      } // foreach (TabPag...
    }


    /// <summary>
    /// 
    /// </summary>
    public void StopAllPlugins()
    {
      if (mPluginList != null)
      {
        foreach (IPlugin lPlugin in mPluginList)
        {
          try { lPlugin.onShutDown(); }
          catch (Exception)
          {
            //LogConsole.Main.LogConsole.pushMsg("Error occurred while shutting down plugin " + lPlugin.PluginName + " : " + lEx.StackTrace + "\n" + lEx.ToString()); 
          } // try { l...
        } // foreach (IPlug...
      } // if (mPlugin...
    }

    #endregion


    #region INTERFACE IPluginHost

    public IPlugin[] PluginList { get { return (mPluginList); } }
    public Hashtable GetPluginPosition { get { return (mPluginPosition); } }
    public String GetInterface() { return (mSimsangMain.GetInterface()); }
    public String GetStartIP() { return (mSimsangMain.GetStartIP()); }
    public String GetStopIP() { return (mSimsangMain.GetStopIP()); }
    public String GetCurrentIP() { return (mSimsangMain.GetCurrentIP()); }

    public List<Tuple<String, String, String>> GetAllReachableSystems()
    {
      List<Tuple<String, String, String>> lRetVal = new List<Tuple<String, String, String>>();
      foreach (ARPScan.Main.TargetRecord lTmp in ARPScan.Main.ARPScan.getInstance().TargetList())
        lRetVal.Add(new Tuple<String, String, String>(lTmp.MAC, lTmp.IP, lTmp.Vendor));

      return (lRetVal);
    }

    public bool IsDebuggingOn() { return (Config.DebugOn()); }
    public String GetSessionName() { return (mSimsangMain.GetSessionName()); }
    public String GetWorkingDirectory() { return (Directory.GetCurrentDirectory()); }
    public String GetAPEWorkingDirectory() { return (Config.LocalBinariesPath); }
    public String GetAPEFWRulesFile() { return (Config.APEFWRulesPath); }
    public String GetAPEInjectionRulesFile() { return (Config.APEInjectionRulesPath); }
    public String GetDNSPoisoningHostsFile() { return (Config.DNSPoisoningHostsPath); }
    public void LogMessage(String pMsg) { LogConsole.Main.LogConsole.pushMsg(pMsg); }

    public void PluginSetStatus(Object pPluginObj, String pStatus)
    {
      try
      {
        if (pPluginObj != null)
        {
          IPlugin lPlugin = (IPlugin)pPluginObj;
          if (lPlugin != null)
          {
            TabPage lTabPage = mSimsangMain.GetTabPageHandler.FindTabPage(lPlugin.Config.PluginName);

            if (lTabPage != null)
              lTabPage.ImageKey = pStatus;

          } // if (lPlugin...
        } // if (pPlugin...
      }
      catch (Exception lEx)
      {
        LogConsole.Main.LogConsole.pushMsg(String.Format("PluginSetStatus() : {0}", lEx.ToString()));
      }
    }



    /// <summary>
    /// Plugin is connecting back to register itself actively. 
    /// Create ControlTab and customize the plugin/tab appearance.
    /// </summary>
    /// <param name="pPlugin"></param>
    /// <returns></returns>
    public bool Register(IPlugin pPlugin)
    {
      TabPage lTP_Plugin = new TabPage(pPlugin.Config.PluginName);
      lTP_Plugin.Controls.Add(pPlugin.PluginControl);
      lTP_Plugin.BackColor = mSimsangMain.TPDefault.BackColor;

      mSimsangMain.TCPlugins.TabPages.Add(lTP_Plugin);
      // Add lTP_Plugin to plugin list
      mSimsangMain.GetTabPageHandler.TabPages.Add(lTP_Plugin);

      return (true);
    }


    #endregion

  }
}
