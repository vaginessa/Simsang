using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Diagnostics;
using System.Text.RegularExpressions;

using Simsang.Plugin;
using Plugin.Main.POP3Proxy.Config;




namespace Plugin.Main.POP3Proxy
{
  class InfrastructureFacade
  {

    #region MEMBERS

    private int cPOP3Port = 110;
    private int cPOP3SPort = 995;
    private String cPOP3RevProxyBin = "POP3ReverseProxyServer.exe";
    private String cPOP3SRevProxyBin = "POP3SReverseProxyServer.exe";
    private String cPOP3RevProxyName = "POP3ReverseProxyServer";
    private String cPOP3SRevProxyName = "POP3SReverseProxyServer";
    private String cPOP3RevProxyPath;
    private String cPOP3SRevProxyPath;
    private Process cPOP3RevProxyProc;
    private Process cPOP3SRevProxyProc;
    private static InfrastructureFacade cInstance;
    private ProxyConfig cProxyConfig;
    private IPlugin cPlugin;

    #endregion


    #region PUBLIC

    private InfrastructureFacade(ProxyConfig pProxyConfig, IPlugin pPlugin)
    {
      cPlugin = pPlugin;

      cPOP3RevProxyPath = String.Format(@"{0}{1}", cPlugin.Config.BaseDir, cPOP3RevProxyBin);
      cPOP3SRevProxyPath = String.Format(@"{0}{1}", cPlugin.Config.BaseDir, cPOP3SRevProxyBin);
    }


    /// <summary>
    /// Create single instance
    /// </summary>
    /// <param name="pProxyConfig"></param>
    /// <returns></returns>
    public static InfrastructureFacade getInstance(ProxyConfig pProxyConfig, IPlugin pPlugin)
    {
      if (cInstance == null)
        cInstance = new InfrastructureFacade(pProxyConfig, pPlugin);

      return (cInstance);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pConfig"></param>
    private void init(ProxyConfig pConfig)
    {
      if (cProxyConfig == null)
        cProxyConfig = new ProxyConfig();

      if (pConfig != null)
      {
        cProxyConfig.BasisDirectory = pConfig.BasisDirectory != null ? pConfig.BasisDirectory : cProxyConfig.BasisDirectory;
        cProxyConfig.isDebuggingOn = pConfig.isDebuggingOn;
        cProxyConfig.onProxyExit = pConfig.onProxyExit != null ? pConfig.onProxyExit : cProxyConfig.onProxyExit;
        cProxyConfig.RemoteHostName = pConfig.RemoteHostName != null ? pConfig.RemoteHostName : cProxyConfig.RemoteHostName;

        //        cIPAccountingPath = String.Format(@"{0}\{1}", pConfig.BasisDirectory, cIPAccountingBin);
      } // if (pConfi...
    }

    #endregion


    #region EVENTS

    /// <summary>
    /// 
    /// </summary>
    public void onInit()
    {
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pProxyConfig"></param>
    public void onStart(ProxyConfig pProxyConfig)
    {
      String lFuncRetVal = String.Empty;

      init(pProxyConfig);


      /*
       * 1. Check hostname
       */
      if (String.IsNullOrEmpty(cProxyConfig.RemoteHostName))
        throw new ExceptionWarning("You have to define a remote host name.");

      else if (!Regex.Match(cProxyConfig.RemoteHostName, @"^[a-z0-9_\-\.]+$", RegexOptions.IgnoreCase).Success)
        throw new ExceptionWarning("Something is wrong with the remote host.");


      /*
       * 2. Check if proxy ports (80 and 443) are ready to use.
       */
      if ((lFuncRetVal = GeneralMethods.FindProc(cPOP3Port)).Length > 0)
        throw new ExceptionWarning("POP3 port is used by an other process.");

      else if ((lFuncRetVal = GeneralMethods.FindProc(cPOP3SPort)).Length > 0)
        throw new ExceptionWarning("POP3S port is used by an other process.");

      /*
       * 3. Proxy server binaries at the right place?
       */
      if (!File.Exists(cPOP3RevProxyPath))
        throw new ExceptionWarning("The POP3 proxy binary was not found.");

      if (!File.Exists(cPOP3SRevProxyPath))
        throw new ExceptionWarning("The POP3S proxy binary was not found.");


      /*
       * Create process objects.
       */
      var lProcStartInfoPOP3RevProxy = new ProcessStartInfo();
      lProcStartInfoPOP3RevProxy.WorkingDirectory = cProxyConfig.BasisDirectory;

      var lProcStartInfoPOP3SRevProxy = new ProcessStartInfo();
      lProcStartInfoPOP3SRevProxy.WorkingDirectory = cProxyConfig.BasisDirectory;


      cPOP3RevProxyProc = new Process();
      cPOP3RevProxyProc.StartInfo.WorkingDirectory = cProxyConfig.BasisDirectory;
      cPOP3RevProxyProc.StartInfo = lProcStartInfoPOP3RevProxy;
      cPOP3RevProxyProc.StartInfo.FileName = cPOP3RevProxyPath;
      cPOP3RevProxyProc.StartInfo.Arguments = String.Format("{0} /rhp:{1};{0} /d", cPOP3Port, cProxyConfig.RemoteHostName);

      cPOP3RevProxyProc.StartInfo.WindowStyle = cProxyConfig.isDebuggingOn ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden;
      cPOP3RevProxyProc.EnableRaisingEvents = true;
      cPOP3RevProxyProc.Exited += new EventHandler(onPOP3ProxyExited);

      cPOP3SRevProxyProc = new Process();
      cPOP3SRevProxyProc.StartInfo.WorkingDirectory = cProxyConfig.BasisDirectory;
      cPOP3SRevProxyProc.StartInfo = lProcStartInfoPOP3SRevProxy;
      cPOP3SRevProxyProc.StartInfo.FileName = cPOP3SRevProxyPath;
      cPOP3SRevProxyProc.StartInfo.Arguments = String.Format("{0} /rhp:{1};{0} /d", cPOP3SPort, cProxyConfig.RemoteHostName);

      cPOP3SRevProxyProc.StartInfo.WindowStyle = cProxyConfig.isDebuggingOn ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden;
      cPOP3SRevProxyProc.EnableRaisingEvents = true;
      cPOP3SRevProxyProc.Exited += new EventHandler(onPOP3ProxyExited);


      /*
       * Start process
       */
      try
      {
        cPOP3RevProxyProc.Start();
        cPOP3SRevProxyProc.Start();
      }
      catch (Exception lEx)
      {
        stopPOP3ProxyServers();
        throw new ExceptionError(String.Format("Error occurred while starting proxy servers.\r\nMessage: {0}", lEx.Message));
      }
    }


    /// <summary>
    /// 
    /// </summary>
    public void onStop()
    {
      /*
       * Deactivate Exit event
       */
      if (cPOP3RevProxyProc != null)
      {
        cPOP3RevProxyProc.EnableRaisingEvents = false;
        cPOP3RevProxyProc.Exited += null;
      }

      if (cPOP3SRevProxyProc != null)
      {
        cPOP3SRevProxyProc.EnableRaisingEvents = false;
        cPOP3SRevProxyProc.Exited += null;
      }

      /*
       * Stop all running POP3(S) proxy instances.
       */
      stopPOP3ProxyServers();
    }


    /// <summary>
    /// 
    /// </summary>
    private void stopPOP3ProxyServers()
    {
      /*
       * Kill running proxy servers. POP3 and POP3S
       */
      try
      {
        if (cPOP3RevProxyProc != null && cPOP3RevProxyProc.Responding)
          cPOP3RevProxyProc.Kill();
      }
      catch (Exception) { }


      try
      {
        if (cPOP3SRevProxyProc != null && cPOP3SRevProxyProc.Responding)
          cPOP3SRevProxyProc.Kill();
      }
      catch (Exception) { }


      /*
       * Killing the proxy processes 
       */
      killProcessByName(cPOP3RevProxyName);
      killProcessByName(cPOP3SRevProxyName);

    }



    /// <summary>
    /// 
    /// </summary>
    public delegate void onPOP3ProxyExitedDelegate(object sender, System.EventArgs e);
    private void onPOP3ProxyExited(object sender, System.EventArgs e)
    {
      Process[] lRevProxProcesses;
      if ((lRevProxProcesses = Process.GetProcessesByName(cPOP3SRevProxyName)) != null && lRevProxProcesses.Length > 0)
      {
        foreach (Process lProc in lRevProxProcesses)
        {
          try { lProc.Kill(); }
          catch (Exception) { }
        }
      }


      // Call back to the GUI.
      if (cProxyConfig.onProxyExit != null)
        cProxyConfig.onProxyExit();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pProcName"></param>
    private void killProcessByName(String pProcName)
    {
      if (!String.IsNullOrEmpty(pProcName))
      {
        foreach (Process lProc in Process.GetProcessesByName(pProcName))
        {
          try
          {
            Process.GetProcessById(lProc.Id).Kill();
          }
          catch (Exception) { }
        } // foreach (Process...
      } // if (!String.IsN...
    }

    #endregion


    #region SESSIONS


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="pSessionName"></param>
    /// <returns></returns>
    public List<T> loadSessionData<T>(String pSessionName)
    {
      List<T> lRecords = null;
      FileStream lFS = null;
      XmlSerializer lXMLSerial;
      String lSessionFilePath = String.Format(@"{0}\{1}.xml", cPlugin.Config.SessionDir, pSessionName);

      try
      {
        lFS = new FileStream(lSessionFilePath, FileMode.Open);
        lXMLSerial = new XmlSerializer(typeof(List<T>));
        lRecords = (List<T>)lXMLSerial.Deserialize(lFS);
      }
      finally
      {
        if (lFS != null)
          lFS.Close();
      }

      return (lRecords);
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionName"></param>
    public void deleteSession(String pSessionName)
    {
      String lSessionFilePath = String.Format(@"{0}\{1}.xml", cPlugin.Config.SessionDir, pSessionName);

      if (File.Exists(lSessionFilePath))
        File.Delete(lSessionFilePath);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionName"></param>
    /// <returns></returns>
    public String getSessionData(String pSessionName)
    {
      String lRetVal = String.Empty;
      String lSessionFilePath = String.Format(@"{0}\{1}.xml", cPlugin.Config.SessionDir, pSessionName);

      try
      {
        lRetVal = File.ReadAllText(lSessionFilePath);
      }
      catch (Exception)
      { }

      return (lRetVal);
    }



    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="pSessionName"></param>
    /// <param name="pRecords"></param>
    public void saveSessionData<T>(String pSessionName, List<T> pRecords)
    {
      if (pSessionName.Length > 0)
      {
        XmlSerializer lSerializer;
        FileStream lFS = null;
        String lSessionFilePath = String.Format(@"{0}\{1}.xml", cPlugin.Config.SessionDir, pSessionName);

        try
        {
          lSerializer = new XmlSerializer(pRecords.GetType());
          lFS = new FileStream(lSessionFilePath, FileMode.Create);
          lSerializer.Serialize(lFS, pRecords);
        }
        finally
        {
          if (lFS != null)
            lFS.Close();
        }
      } // if (pSessi...
    }



    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="pSessionData"></param>
    /// <returns></returns>
    public List<T> loadSessionDataFromString<T>(String pSessionData)
    {
      List<T> lRecords = new List<T>();
      var lSerializer = new XmlSerializer(typeof(List<T>));

      using (TextReader lTextReader = new StringReader(pSessionData))
      {
        lRecords = (List<T>)lSerializer.Deserialize(lTextReader);
      } // using (TextRe...

      return (lRecords);
    }

    #endregion

  }
}

