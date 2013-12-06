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
using Plugin.Main.HTTPProxy.Config;


namespace Plugin.Main.HTTPProxy
{
  public class InfrastructureFacade
  {

    #region MEMBERS

    private static InfrastructureFacade cInstance;
    private int cHTTPPort = 80;
    private int cHTTPSPort = 443;
    private String cHTTPRevProxyBin = "HTTPReverseProxyServer.exe";
    private String cHTTPSRevProxyBin = "HTTPSReverseProxyServer.exe";
    private String cHTTPRevProxyName = "HTTPReverseProxyServer";
    private String cHTTPSRevProxyName = "HTTPSReverseProxyServer";
    private Process cHTTPRevProxyProc;
    private Process cHTTPSRevProxyProc;
    private WebServerConfig cWebServerConfig;
    private String cHTTPRevProxyPath;
    private String cHTTPSRevProxyPath;
    private IPlugin cPlugin;

    #endregion


    #region PUBLIC

    private InfrastructureFacade(WebServerConfig pWebServerConfig, IPlugin pPlugin)
    {
      cWebServerConfig = pWebServerConfig;
      cPlugin = pPlugin;

      cHTTPRevProxyPath = String.Format("{0}{1}", pWebServerConfig.BasisDirectory, cHTTPRevProxyBin);
      cHTTPSRevProxyPath = String.Format("{0}{1}", pWebServerConfig.BasisDirectory, cHTTPSRevProxyBin);
    }



    /// <summary>
    /// Create single instance
    /// </summary>
    /// <param name="pWebServerConfig"></param>
    /// <returns></returns>
    public static InfrastructureFacade getInstance(WebServerConfig pWebServerConfig, IPlugin pPlugin)
    {
      if (cInstance == null)
        cInstance = new InfrastructureFacade(pWebServerConfig, pPlugin);

      return (cInstance);
    }

    #endregion


    #region PRIVATE

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private String GetSessionDir()
    {
      String lRetVal = String.Empty;

      lRetVal = String.Format("{0}{1}", cPlugin.Config.BaseDir, cPlugin.Config.SessionDir);

      try
      {
        if (!Directory.Exists(lRetVal))
          Directory.CreateDirectory(lRetVal);
      }
      catch (Exception lEx)
      {
        cPlugin.Host.LogMessage(lEx.StackTrace);
      }

      return (lRetVal);
    }

    #endregion


    #region SESSION

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="pSessionFile"></param>
    /// <returns></returns>
    public T loadSessionData<T>(String pSessionName)
    {
      object lSessionData = new object();
      FileStream lFS = null;
      XmlSerializer lXMLSerial;
      String lSessionFilePath = String.Format(@"{0}\{1}.xml", GetSessionDir(), pSessionName);

      try
      {
        lFS = new FileStream(lSessionFilePath, FileMode.Open);
        lXMLSerial = new XmlSerializer(typeof(T));
        lSessionData = (T)lXMLSerial.Deserialize(lFS);
      }
      catch (Exception lEx)
      {
        String lMessage = String.Format("HTTPProxy::loadSessionData() : {0}", lEx.Message);
        cPlugin.Host.LogMessage(lMessage);
      }
      finally
      {
        if (lFS != null)
          lFS.Close();
      }

      return ((T)lSessionData);
    }




    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="pSessionData"></param>
    /// <returns></returns>
    public T loadSessionDataFromString<T>(String pSessionData)
    {
      T lRecords;
      var lSerializer = new XmlSerializer(typeof(T));

      using (TextReader lTextReader = new StringReader(pSessionData))
      {
        lRecords = (T)lSerializer.Deserialize(lTextReader);
      } // using (TextRe...

      return (lRecords);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionName"></param>
    public void deleteSession(String pSessionName)
    {
      String lSessionFilePath = String.Format(@"{0}\{1}.xml", GetSessionDir(), pSessionName);

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
      String lSessionFilePath = String.Format(@"{0}\{1}.xml", GetSessionDir(), pSessionName);

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
    public void saveSessionData<T>(String pSessionName, T pRecords)
    {
      if (pSessionName.Length > 0)
      {
        XmlSerializer lSerializer;
        FileStream lFS = null;
        String lSessionFilePath = String.Format(@"{0}\{1}.xml", GetSessionDir(), pSessionName);

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

    #endregion


    #region EVENTS

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pWebServerConfig"></param>
    public void onInit(WebServerConfig pWebServerConfig)
    {
      Process[] lProcInstances;

      cWebServerConfig = pWebServerConfig;

      if ((lProcInstances = Process.GetProcessesByName(cHTTPRevProxyName)) != null && lProcInstances.Length > 0)
        foreach (Process lProc in lProcInstances)
          try { lProc.Kill(); }
          catch (Exception) { }

      if ((lProcInstances = Process.GetProcessesByName(cHTTPSRevProxyName)) != null && lProcInstances.Length > 0)
        foreach (Process lProc in lProcInstances)
          try { lProc.Kill(); }
          catch (Exception) { }
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="pConfig"></param>
    public void startProxies(WebServerConfig pConfig)
    {
      String lFuncRetVal = String.Empty;


      /*
       * 1. Check binaries
       */
      if (!File.Exists(cHTTPRevProxyPath))
        throw new Exception("HTTP proxy binary not found.");

      else if (!File.Exists(cHTTPSRevProxyPath))
        throw new Exception("HTTPS proxy binary not found.");

      /*
       * 2. Check hostname
       */
      else if (!pConfig.isRedirect && String.IsNullOrEmpty(pConfig.RemoteHostName))
        throw new Exception("You forgot to fill in the remote host name.");

      else if (!pConfig.isRedirect && !String.IsNullOrEmpty(pConfig.RemoteHostName) && !Regex.Match(pConfig.RemoteHostName, @"^[a-z0-9_\-\.]+\.[a-z]{2,}$", RegexOptions.IgnoreCase).Success)
        throw new Exception("Something is wrong with the remote host.");

      /*
       * 3. Check redirect to
       */
      else if (pConfig.isRedirect && String.IsNullOrEmpty(pConfig.RedirectToURL))
        throw new Exception("You forgot to fill in the redirection URL.");

////else if (pConfig.isRedirect && !String.IsNullOrEmpty(pConfig.RedirectToURL) && !Regex.Match(pConfig.RedirectToURL, @"^[a-z0-9_\-\.]+\.[a-z]{2,}\/[^\s]*$", RegexOptions.IgnoreCase).Success)
      //else if (pConfig.isRedirect && !String.IsNullOrEmpty(pConfig.RedirectToURL) && !Regex.Match(pConfig.RedirectToURL, @"^.+$", RegexOptions.IgnoreCase).Success)
      //  throw new Exception("Something is wrong with the redirection URL.");

      /*
       * 3. Check if proxy ports (80 and 443) are ready to use.
       */
      else if ((lFuncRetVal = GeneralMethods.FindProc(cHTTPPort)).Length > 0)
        throw new Exception(String.Format("Port 80 is used by an other process. You have to stop that process to start the plugin."));

      else if ((lFuncRetVal = GeneralMethods.FindProc(cHTTPSPort)).Length > 0)
        throw new Exception(String.Format("Port 443 is used by an other process. You have to stop that process to start the plugin."));



      /*
       * Create process objects.
       */
      var lProcStartInfoHTTPRevProxy = new ProcessStartInfo();
      lProcStartInfoHTTPRevProxy.WorkingDirectory = cWebServerConfig.BasisDirectory;

      cHTTPRevProxyProc = new Process();
      cHTTPRevProxyProc.StartInfo.WorkingDirectory = cWebServerConfig.BasisDirectory;
      cHTTPRevProxyProc.StartInfo = lProcStartInfoHTTPRevProxy;
      cHTTPRevProxyProc.StartInfo.FileName = cHTTPRevProxyBin;

      if (cWebServerConfig.isRedirect)
        cHTTPRevProxyProc.StartInfo.Arguments = String.Format("{0} /ru:{1} /d", cHTTPPort, cWebServerConfig.RedirectToURL);
      else
        cHTTPRevProxyProc.StartInfo.Arguments = String.Format("{0} /rhp:{1};{0} /d", cHTTPPort, cWebServerConfig.RemoteHostName);

      cHTTPRevProxyProc.StartInfo.WindowStyle = cWebServerConfig.isDebuggingOn ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden;
      //  //cHTTPRevProxyProc.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
      cHTTPRevProxyProc.EnableRaisingEvents = true;
      cHTTPRevProxyProc.Exited += new EventHandler(onProxyExited);


      var lProcStartInfoHTTPSRevProxy = new ProcessStartInfo();
      lProcStartInfoHTTPSRevProxy.WorkingDirectory = cWebServerConfig.BasisDirectory;

      cHTTPSRevProxyProc = new Process();
      cHTTPSRevProxyProc.StartInfo.WorkingDirectory = cWebServerConfig.BasisDirectory; ;
      cHTTPSRevProxyProc.StartInfo = lProcStartInfoHTTPSRevProxy;
      cHTTPSRevProxyProc.StartInfo.FileName = cHTTPSRevProxyBin;
      //    cHTTPSRevProxyProc.StartInfo.Arguments = String.Format("{0} {1} {2} /d", cHTTPSPort, lRemoteHost, cHTTPSPort);
      if (cWebServerConfig.isRedirect)
        cHTTPSRevProxyProc.StartInfo.Arguments = String.Format("{0} /ru:{1} /d", cHTTPSPort, cWebServerConfig.RedirectToURL);
      else
        cHTTPSRevProxyProc.StartInfo.Arguments = String.Format("{0} /rhp:{1};{0} /d", cHTTPSPort, cWebServerConfig.RemoteHostName);

      cHTTPSRevProxyProc.StartInfo.WindowStyle = cWebServerConfig.isDebuggingOn ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden;
      //  //cHTTPSRevProxyProc.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
      cHTTPSRevProxyProc.EnableRaisingEvents = true;
      cHTTPSRevProxyProc.Exited += new EventHandler(onProxyExited);

      /*
       * Start process
       */
      try
      {
        cHTTPRevProxyProc.Start();
        cHTTPSRevProxyProc.Start();
      }
      catch (Exception lEx)
      {
        Console.WriteLine(String.Format("Exception: {0}\r\n{1}", lEx.Message, lEx.StackTrace));
        throw new Exception("An error occurred while starting HTTP(S) reverse proxy servers.");
      }
    }



    /// <summary>
    /// 
    /// </summary>
    public void stopProxies()
    {
      /*
       * Deactivate Exit events
       */
      if (cHTTPRevProxyProc != null)
      {
        cHTTPRevProxyProc.EnableRaisingEvents = false;
        cHTTPRevProxyProc.Exited += null;
      }

      if (cHTTPSRevProxyProc != null)
      {
        cHTTPSRevProxyProc.EnableRaisingEvents = false;
        cHTTPSRevProxyProc.Exited += null;
      }

      /*
       * Killing the proxy processes 
       */
      try
      {
        if (cHTTPRevProxyProc != null && cHTTPRevProxyProc.Responding)
          cHTTPRevProxyProc.Kill();
      }
      catch (Exception) { }

      try
      {
        if (cHTTPSRevProxyProc != null && cHTTPSRevProxyProc.Responding)
          cHTTPSRevProxyProc.Kill();
      }
      catch (Exception) { }
    }




    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void onProxyExited(object sender, System.EventArgs e)
    {
      /*
       * Kill all running proxy server processes
       */
      Process[] lRevProxProcesses;
      if ((lRevProxProcesses = Process.GetProcessesByName(cHTTPRevProxyName)) != null && lRevProxProcesses.Length > 0)
      {
        foreach (Process lProc in lRevProxProcesses)
        {
          try { lProc.Kill(); }
          catch (Exception) { }
        }
      }



      /*
       * Notify the GUI about this event.
       */
      if (cWebServerConfig != null && cWebServerConfig.onWebServerExit != null)
        cWebServerConfig.onWebServerExit();
    }

    #endregion

  }
}

