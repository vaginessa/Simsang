using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

using Simsang.Plugin;
using Plugin.Main.IMAP4Proxy.Config;


namespace Plugin.Main.IMAP4Proxy
{
  public class InfrastructureFacade
  {

    #region MEMBERS

    private static InfrastructureFacade cInstance;
    private int cIMAP4Port = 143;
    private int cIMAP4SPort = 993;
    private String cIMAP4RevProxyBin = "IMAP4ReverseProxyServer.exe";
    private String cIMAP4SRevProxyBin = "IMAP4SReverseProxyServer.exe";
    private String cIMAP4RevProxyPath;
    private String cIMAP4SRevProxyPath;
    private String cIMAP4RevProxyName = "IMAP4ReverseProxyServer";
    private String cIMAP4SRevProxyName = "IMAP4SReverseProxyServer";
    private Process cIMAP4RevProxyProc;
    private Process cIMAP4SRevProxyProc;
    private ProxyConfig cProxyConfig;
    private IPlugin cPlugin;

    #endregion


    #region PUBLIC

    private InfrastructureFacade(ProxyConfig pProxyConfig, IPlugin pPlugin)
    {
      cPlugin = pPlugin;
      cProxyConfig = pProxyConfig;

      cIMAP4RevProxyPath = String.Format(@"{0}{1}", cProxyConfig.BasisDirectory, cIMAP4RevProxyBin);
      cIMAP4SRevProxyPath = String.Format(@"{0}{1}", cProxyConfig.BasisDirectory, cIMAP4SRevProxyBin);
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

    #endregion


    #region EVENTS

    /// <summary>
    /// 
    /// </summary>
    public void onInit()
    {
      /*
       * Kill previousliy started IMAP4(S)Proxy instance 
       */
      Process[] lProcInstances;

      if ((lProcInstances = Process.GetProcessesByName(cIMAP4RevProxyName)) != null && lProcInstances.Length > 0)
        foreach (Process lProc in lProcInstances)
          try { lProc.Kill(); }
          catch (Exception) { }

      if ((lProcInstances = Process.GetProcessesByName(cIMAP4SRevProxyName)) != null && lProcInstances.Length > 0)
        foreach (Process lProc in lProcInstances)
          try { lProc.Kill(); }
          catch (Exception) { }
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="pProxyConfig"></param>
    public void onStart(ProxyConfig pProxyConfig)
    {
      String lFuncRetVal = String.Empty;

      /*
       * 1. Check if proxy binaries are at the right place.
       */
      if (!File.Exists(cIMAP4RevProxyPath))
        throw new ExceptionWarning("The IMAP4 proxy binary was not found.");

      if (!File.Exists(cIMAP4SRevProxyPath))
        throw new ExceptionWarning("The IMAP4S proxy binary was not found.");

      /*
       * 2. Check hostname
       */
      if (String.IsNullOrEmpty(pProxyConfig.RemoteHostName))
        throw new ExceptionWarning("You have to define a remote host name.");

      if (!Regex.Match(pProxyConfig.RemoteHostName, @"^[a-z0-9_\-\.]+$", RegexOptions.IgnoreCase).Success)
        throw new ExceptionWarning("Something is wrong with the remote host.");


      /*
       * 3. Check if proxy ports (143 and 943) are ready to use.
       */
      if ((lFuncRetVal = GeneralMethods.FindProc(cIMAP4Port)).Length > 0)
        throw new ExceptionWarning("Port 143 is used by an other process. You have to stop that process to start this module.");

      if ((lFuncRetVal = GeneralMethods.FindProc(cIMAP4SPort)).Length > 0)
        throw new ExceptionWarning("Port 993 is used by an other process. You have to stop that process to start this module.");



      /*
       * Create process objects.
       */
      var lProcStartInfoIMAP4RevProxy = new ProcessStartInfo();
      lProcStartInfoIMAP4RevProxy.WorkingDirectory = pProxyConfig.BasisDirectory;

      var lProcStartInfoIMAP4SRevProxy = new ProcessStartInfo();
      lProcStartInfoIMAP4SRevProxy.WorkingDirectory = pProxyConfig.BasisDirectory;


      cIMAP4RevProxyProc = new Process();
      cIMAP4RevProxyProc.StartInfo.WorkingDirectory = pProxyConfig.BasisDirectory;
      cIMAP4RevProxyProc.StartInfo = lProcStartInfoIMAP4RevProxy;
      cIMAP4RevProxyProc.StartInfo.FileName = cIMAP4RevProxyPath;
      cIMAP4RevProxyProc.StartInfo.Arguments = String.Format("{0} /rhp:{1};{0} /d", cIMAP4Port, cProxyConfig.RemoteHostName);

      cIMAP4RevProxyProc.StartInfo.WindowStyle = cProxyConfig.isDebuggingOn ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden;
      //cIMAP4RevProxyProc.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
      cIMAP4RevProxyProc.EnableRaisingEvents = true;
      cIMAP4RevProxyProc.Exited += new EventHandler(onProxyStopp);



      cIMAP4SRevProxyProc = new Process();
      cIMAP4SRevProxyProc.StartInfo.WorkingDirectory = pProxyConfig.BasisDirectory;
      cIMAP4SRevProxyProc.StartInfo = lProcStartInfoIMAP4SRevProxy;
      cIMAP4SRevProxyProc.StartInfo.FileName = cIMAP4SRevProxyPath;
      cIMAP4SRevProxyProc.StartInfo.Arguments = String.Format("{0} /rhp:{1};{0} /d", cIMAP4SPort, cProxyConfig.RemoteHostName);

      cIMAP4SRevProxyProc.StartInfo.WindowStyle = pProxyConfig.isDebuggingOn ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden;
      //cIMAP4SRevProxyProc.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
      cIMAP4SRevProxyProc.EnableRaisingEvents = true;
      cIMAP4SRevProxyProc.Exited += new EventHandler(onProxyStopp);


      /*
       * Start process
       */
      try
      {
        cIMAP4RevProxyProc.Start();
        cIMAP4SRevProxyProc.Start();
      }
      catch (Exception lEx)
      {
        throw new ExceptionError(String.Format("Can't start the proxy servers.\r\nMessage: {0}", lEx.Message));
      }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void onProxyStopp(object sender, System.EventArgs e)
    {
      onStop();

      if (cProxyConfig.onProxyExit != null)
        cProxyConfig.onProxyExit();
    }



    /// <summary>
    /// 
    /// </summary>
    public void onStop()
    {
      /*
       * Deactivate Exit event
       */
      if (cIMAP4RevProxyProc != null)
      {
        cIMAP4RevProxyProc.EnableRaisingEvents = false;
        cIMAP4RevProxyProc.Exited += null;
      } // if (cIMA...

      if (cIMAP4SRevProxyProc != null)
      {
        cIMAP4SRevProxyProc.EnableRaisingEvents = false;
        cIMAP4SRevProxyProc.Exited += null;
      } // if (cIMAP...


      /*       
       * Kill running proxy processes 
       */
      killProcessByName(cIMAP4RevProxyName);
      killProcessByName(cIMAP4SRevProxyName);
    }

    #endregion


    #region SESSION

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="pSessionFile"></param>
    /// <returns></returns>
    public List<T> loadSessionData<T>(String pSessionName)
    {
      List<T> lRetVal = null;
      FileStream lFS = null;
      XmlSerializer lXMLSerial;
      String lSessionFilePath = String.Format(@"{0}\{1}.xml", cProxyConfig.SessionDirectory, pSessionName);

      try
      {
        lFS = new FileStream(lSessionFilePath, FileMode.Open);
        lXMLSerial = new XmlSerializer(typeof(List<T>));
        lRetVal = (List<T>)lXMLSerial.Deserialize(lFS);
      }
      catch (Exception lEx)
      {
      }
      finally
      {
        if (lFS != null)
          lFS.Close();
      }

      return (lRetVal);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionFileName"></param>
    public void deleteSession(String pSessionName)
    {
      String lSessionFilePath = String.Format(@"{0}\{1}.xml", cProxyConfig.SessionDirectory, pSessionName);

      if (File.Exists(lSessionFilePath))
        File.Delete(lSessionFilePath);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionFile"></param>
    /// <returns></returns>
    public String getSessionData(String pSessionName)
    {
      String lRetVal = String.Empty;
      String lSessionFilePath = String.Format(@"{0}\{1}.xml", cProxyConfig.SessionDirectory, pSessionName);

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
        String lSessionFilePath = String.Format(@"{0}\{1}.xml", cProxyConfig.SessionDirectory, pSessionName);

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
    public BindingList<T> loadSessionDataFromString<T>(String pSessionData)
    {
      BindingList<T> lRecords = new BindingList<T>();
      var lSerializer = new XmlSerializer(typeof(BindingList<T>));

      using (TextReader lTextReader = new StringReader(pSessionData))
      {
        lRecords = (BindingList<T>)lSerializer.Deserialize(lTextReader);
      } // using (TextRe...

      return (lRecords);
    }

    #endregion


    #region PRIVATE

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

  }
}

