using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;

using Simsang.Plugin;



namespace Plugin.Main.HTTPInject
{
  public class InfrastructureFacade
  {

    #region IMPORTS

    [DllImport("user32.dll")]
    static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    #endregion


    #region MEMBERS

    private static InfrastructureFacade cInstance;
    private String cMicroWebBin = "MicroWeb.exe";
    private String cMicroWebProcName = "MicroWeb";
    private String cMicroWebPath;
    private Process cMicroWebProc;
//    private InjectionConfig cProxyConfig;
    private IPlugin cPlugin;

    #endregion


    #region PROPERTIES

    public InjectionConfig InjectionConfig { get; set; }

    #endregion


    #region PUBLIC

    private InfrastructureFacade(InjectionConfig pProxyConfig, IPlugin pPlugin)
    {
      InjectionConfig = pProxyConfig;
      cPlugin = pPlugin;

      cMicroWebPath = String.Format(@"{0}{1}", InjectionConfig.BasisDirectory, cMicroWebBin);

      // Create Session directory if it doesn't exist
      try
      {
        if (!Directory.Exists(cPlugin.Config.SessionDir))
          Directory.CreateDirectory(cPlugin.Config.SessionDir);
      }
      catch (Exception lEx)
      {
      }
    }


    /// <summary>
    /// Create single instance
    /// </summary>
    /// <param name="pProxyConfig"></param>
    /// <returns></returns>
    public static InfrastructureFacade getInstance(InjectionConfig pProxyConfig, IPlugin pPlugin)
    {
      if (cInstance == null)
        cInstance = new InfrastructureFacade(pProxyConfig, pPlugin);

      return (cInstance);
    }

    #endregion


    #region SESSION

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


    #region EVENTS

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pURLList"></param>
    public void onStart(List<InjectedURLRecord> pURLList)
    {

      String lInjectionRules = String.Empty;
      var lProcStartInfo = new ProcessStartInfo();
      cMicroWebProc = new Process();
      String lMicroWebFullPath = String.Format(@"{0}\{1}", InjectionConfig.BasisDirectory, cMicroWebBin);

      if (File.Exists(lMicroWebFullPath))
      {
        lProcStartInfo.WorkingDirectory = InjectionConfig.BasisDirectory;
        cMicroWebProc.StartInfo = lProcStartInfo;
        cMicroWebProc.StartInfo.FileName = lMicroWebFullPath;
        //cMicroWebProc.StartInfo.Arguments = String.Format("-i {0} {1} -x -f", cHost.GetInterface(), cAccountingBasis);
        cMicroWebProc.StartInfo.UseShellExecute = false;
        cMicroWebProc.StartInfo.CreateNoWindow = InjectionConfig.isDebuggingOn ? false : true;
        cMicroWebProc.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
        cMicroWebProc.Exited += new EventHandler(onMicroWebExited);

        try
        {
          cMicroWebProc.Start();

          try
          {
            if (!InjectionConfig.isDebuggingOn)
              ShowWindow(cMicroWebProc.MainWindowHandle, 0);
          }
          catch (Exception)
          { }
        }
        catch (Exception lEx)
        {
          InjectionConfig.onWebServerExit();
        }


        if (pURLList != null && pURLList.Count > 0)
        {
          // Write APE HTTPInjection rules file
          if (!String.IsNullOrEmpty(InjectionConfig.InjectionRulesPath))
          {
            if (File.Exists(InjectionConfig.InjectionRulesPath))
              File.Delete(InjectionConfig.InjectionRulesPath);


            /*
             * If inject local files, ...
             *  1. Copy them to the plugin directory
             *  2. Start the web server
             */

            foreach (InjectedURLRecord lTmp in pURLList)
            {
              if (lTmp.Type.ToLower().Contains("inject"))
              {
                String lFileName = Path.GetFileName(lTmp.InjectedFileFullPath);
                String lDstFile = String.Format("{0}{1}", InjectionConfig.BasisDirectory, lFileName);

                lInjectionRules += String.Format("{0},{1},{2}/{3}\r\n", lTmp.RequestedHost, lTmp.RequestedURL, lTmp.InjectedHost, lTmp.InjectedURL);

                if (lDstFile.ToLower() != lTmp.InjectedFileFullPath.ToLower())
                {

                  if (File.Exists(lDstFile))
                  {
                    try
                    {
                      File.Delete(lDstFile);
                    }
                    catch (Exception)
                    { }
                  }

                  try
                  {
                    File.Copy(lTmp.InjectedFileFullPath, lDstFile);
                  }
                  catch (Exception lEx)
                  {
                    throw new InjErrorException(String.Format("Error occurred while copying \"{0}\": {1}", lTmp.InjectedFileFullPath, lEx.Message));
                  }
                } // if (lDstFi...
              }
              else
              {
                lInjectionRules += String.Format("{0},{1},{2}/{3}\r\n", lTmp.RequestedHost, lTmp.RequestedURL, lTmp.InjectedHost, lTmp.InjectedURL);
              } // if (lTmp..
            } // foreach (In...
          }
          else
          {
            throw new InjWarningException("No rule defined. Stopping the pluggin.");
          } // if (cInjec...



          /*
           * Write config file           
           */
          if (pURLList != null && pURLList.Count > 0)
          {
            using (StreamWriter outfile = new StreamWriter(InjectionConfig.InjectionRulesPath))
            {
              outfile.Write(lInjectionRules);
            } // using (Strea...
          } // if (lInjec...
        }
        else
        {
          throw new InjWarningException("No rule defined. Stopping the pluggin.");
        } // if (cInjectedURLs...  
      }
      else
      {
        throw new InjErrorException(String.Format("{0} doesn't exist", cMicroWebPath));
      } // if (File.Exists...
    }



    /// <summary>
    /// 
    /// </summary>
    public void onStop()
    {
      // Deactivate Exit event
      if (cMicroWebProc != null)
      {
        cMicroWebProc.EnableRaisingEvents = false;
        cMicroWebProc.Exited += null;
      }


      // Kill MicroWeb process
      try
      {
        if (cMicroWebProc != null && !cMicroWebProc.HasExited)
        {
          Process.GetProcessById(cMicroWebProc.Id).Kill();
          cMicroWebProc.Kill();
        }
      }
      catch (Exception)
      { }


      // Just to be safe, kill all other IP Accounting instances.
      killProcessByName(cMicroWebProcName);
    }




    /// <summary>
    /// 
    /// </summary>
    public void onInit()
    {
      killAllInstances();
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void onMicroWebExited(object sender, System.EventArgs e)
    {
      killAllInstances();

      if (InjectionConfig.onWebServerExit != null)
        InjectionConfig.onWebServerExit();
    }

    #endregion


    #region PRIVATE

    /// <summary>
    /// 
    /// </summary>
    private void killAllInstances()
    {
      Process[] lProcInstances;

      if ((lProcInstances = Process.GetProcessesByName(cMicroWebProcName)) != null && lProcInstances.Length > 0)
        foreach (Process lProc in lProcInstances)
          try { lProc.Kill(); }
          catch (Exception) { }
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

  }
}

