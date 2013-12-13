using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using Simsang.Plugin;
using Plugin.Main.IPAccounting.Config;


namespace Plugin.Main.IPAccounting
{
  public class InfrastructureFacade
  {

    #region IMPORTS

    [DllImport("user32.dll")]
    static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    #endregion


    #region MEMBERS

    private static InfrastructureFacade cInstance;
    private String cIPAccountingBin = "IPAccounting.exe";
    private String cIPAccountingProcName = "IPAccounting";
    private Process cIPAccountingProc;
    private String cIPAccountingPath;
    private IPAccountingConfig cAccountingConfig;
    private String cData;
    private List<AccountingItem> cAccountingRecords;
    private IPlugin cPlugin;

    #endregion


    #region PUBLIC

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pConfig"></param>
    private InfrastructureFacade(IPAccountingConfig pAccountingConfig, IPlugin pPlugin)
    {
      cAccountingConfig = pAccountingConfig;
      cPlugin = pPlugin;

      cAccountingRecords = new List<AccountingItem>();
      cIPAccountingPath = String.Format(@"{0}{1}", cAccountingConfig.BasisDirectory, cIPAccountingBin);
      init(pAccountingConfig);
    }


    /// <summary>
    /// Create single instance
    /// </summary>
    /// <returns></returns>
    public static InfrastructureFacade getInstance(IPAccountingConfig pConfig, IPlugin pPlugin)
    {
      if (cInstance == null)
        cInstance = new InfrastructureFacade(pConfig, pPlugin);

      return (cInstance);
    }

    #endregion


    #region PRIVATE

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pConfig"></param>
    private void init(IPAccountingConfig pConfig)
    {
      if (cAccountingConfig == null)
        cAccountingConfig = new IPAccountingConfig();

      if (pConfig != null)
      {
        cAccountingConfig.BasisDirectory = pConfig.BasisDirectory != null ? pConfig.BasisDirectory : cAccountingConfig.BasisDirectory;
        cAccountingConfig.Interface = pConfig.Interface != null ? pConfig.Interface : cAccountingConfig.Interface;
        cAccountingConfig.isDebuggingOn = pConfig.isDebuggingOn;
        cAccountingConfig.onIPAccountingExit = pConfig.onIPAccountingExit != null ? pConfig.onIPAccountingExit : cAccountingConfig.onIPAccountingExit;
        cAccountingConfig.onUpdateList = pConfig.onUpdateList != null ? pConfig.onUpdateList : cAccountingConfig.onUpdateList;
        cAccountingConfig.StructureParameter = pConfig.StructureParameter != null ? pConfig.StructureParameter : cAccountingConfig.StructureParameter;
      }
    }

    #endregion


    #region SESSIONS

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="pSessionFile"></param>
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
    /// <param name="pSessionFileName"></param>
    public void deleteSession(String pSessionName)
    {
      String lSessionFilePath = String.Format(@"{0}\{1}.xml", cPlugin.Config.SessionDir, pSessionName);

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
    public void onInit()
    {
      killAllInstances();
    }



    /// <summary>
    /// 
    /// </summary>
    public void onStart(IPAccountingConfig pConfig)
    {
      /*
       * Reassign configuration parameters
       */
      init(pConfig);

      /*
       * Check binary
       */
      if (!File.Exists(cIPAccountingPath))
        throw new Exception("The IPAccounting binary was not found.");

      else if (String.IsNullOrEmpty(pConfig.Interface))
        throw new Exception("There was no interface defined.");

      /*
       * Create process objects
       */
      cAccountingRecords.Clear();


      var lProcStartInfo = new ProcessStartInfo();
      cIPAccountingProc = new Process();

      lProcStartInfo.WorkingDirectory = cAccountingConfig.BasisDirectory;
      cIPAccountingProc.StartInfo = lProcStartInfo;
      cIPAccountingProc.StartInfo.FileName = cIPAccountingPath;
      cIPAccountingProc.StartInfo.Arguments = String.Format("-i {0} -x {1}", pConfig.Interface, cAccountingConfig.StructureParameter);
      cIPAccountingProc.StartInfo.UseShellExecute = false;
      cIPAccountingProc.StartInfo.CreateNoWindow = pConfig.isDebuggingOn ? false : true;
      cIPAccountingProc.StartInfo.WindowStyle = ProcessWindowStyle.Normal;


      // set up output redirection
      cIPAccountingProc.StartInfo.RedirectStandardOutput = true;
      //  cIPAccountingProc.StartInfo.RedirectStandardError = true;
      cIPAccountingProc.EnableRaisingEvents = true;

      // Set the data received handlers
      //cIPAccountingProc.ErrorDataReceived += OnDataRecived;
      cIPAccountingProc.OutputDataReceived += onDataRecived;

      // Configure the process exited event
      cIPAccountingProc.Exited += new EventHandler(onIPAccountingExited);


      cIPAccountingProc.Start();
      //cIPAccountingProc.BeginErrorReadLine();
      cIPAccountingProc.BeginOutputReadLine();

      Thread.Sleep(100);

      try
      {
        if (!cAccountingConfig.isDebuggingOn)
          ShowWindow(cIPAccountingProc.MainWindowHandle, 0);
      }
      catch (Exception)
      {
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
      if (cIPAccountingProc != null)
      {
        cIPAccountingProc.EnableRaisingEvents = false;
        cIPAccountingProc.Exited += null;
      }


      /*
       * Kill running IPAccounting instances.
       */
      try
      {
        if (cIPAccountingProc != null && !cIPAccountingProc.HasExited)
        {
          try
          {
            cIPAccountingProc.Kill();
          }
          catch (Exception)
          {
          }

          cIPAccountingProc.Close();
          cIPAccountingProc = null;
        } // if (cIPAc...
      }
      catch (Exception)
      { }


      /*
       * Just to be safe, kill all other IP Accounting instances.
       */
      Process[] lACInstances;
      if ((lACInstances = Process.GetProcessesByName(cIPAccountingProcName)) != null && lACInstances.Length > 0)
      {
        foreach (Process lProc in lACInstances)
        {
          try
          {
            Process.GetProcessById(cIPAccountingProc.Id).Kill();
          }
          catch (Exception)
          {
          }

          try
          {
            lProc.Kill();
          }
          catch (Exception)
          {
          }
        } // foreach (Process...
      } // if ((lACInstan...
    }

    #endregion


    #region PRIVATE


    /// <summary>
    /// 
    /// </summary>
    private void killAllInstances()
    {
      Process[] lProcInstances;

      if ((lProcInstances = Process.GetProcessesByName(cIPAccountingProcName)) != null && lProcInstances.Length > 0)
        foreach (Process lProc in lProcInstances)
          try { lProc.Kill(); }
          catch (Exception) { }
    }




    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void onDataRecived(object sender, DataReceivedEventArgs e)
    {
      if (e.Data != null && e.Data.Length > 0)
      {
        if (e.Data != "<EOF>")
          cData += e.Data + "\n";
        else if (e.Data == "<EOF>")
        {

          Match lTrafficMatch = Regex.Match(cData, @"(<traffic>.*?</traffic>)", RegexOptions.Singleline | RegexOptions.IgnoreCase);

          if (lTrafficMatch.Success)
          {
            UpdateTextBox(lTrafficMatch.Groups[0].Value);
            cData = String.Empty;
          }
          else if (Regex.Match(cData, @"(<traffic>[\t\n\w\d]*</traffic>)", RegexOptions.IgnoreCase).Success)
          {
            UpdateTextBox("");
            cData = String.Empty;
          }
          else if (!Regex.Match(cData, @"^\s*<traffic>", RegexOptions.IgnoreCase).Success)
          {
            UpdateTextBox("");
            cData = String.Empty;
          } // if (Regex.M...
        } // if (e.Data...
      } // if (e.Data..
    }





    /// <summary>
    /// 
    /// </summary>
    /// <param name="pData"></param>
    private void UpdateTextBox(String pData)
    {
      String lData = String.Empty;
      cAccountingRecords.Clear();

      try
      {
        XDocument lXMLContent = XDocument.Parse(pData);
        var lServiceEntries = from lService in lXMLContent.Descendants("entry")
                              select new
                              {
                                Basis = lService.Element("basis").Value,
                                PacketCounter = lService.Element("packetcounter").Value,
                                DataVolume = lService.Element("datavolume").Value,
                                LastUpdate = lService.Element("lastupdate").Value
                              };

        if (lServiceEntries != null)
        {

          foreach (var lEntry in lServiceEntries)
          {
            try
            {
              int lPacketCounterInt = Convert.ToInt32(lEntry.PacketCounter);
              int lDataVolumeInt = Convert.ToInt32(lEntry.DataVolume);

              String lPacketCounterStr = lPacketCounterInt.ToString("#,#", CultureInfo.InvariantCulture);
              String lDataVolumeStr = lDataVolumeInt.ToString("#,#", CultureInfo.InvariantCulture);

              AccountingItem lTmp = new AccountingItem(lEntry.Basis, lPacketCounterStr, lDataVolumeStr, lEntry.LastUpdate);

              String[] lSplitter = Regex.Split(lEntry.Basis, @"\s+");
              if (lSplitter != null && lSplitter.Length == 2 &&
                  lSplitter[1].Length > 0 && lSplitter[1].ToLower() != "unknown")
              {
                if (!cAccountingRecords.Contains(lTmp))
                  cAccountingRecords.Add(lTmp);

              } // if (lSplitter...
            }
            catch { }

          } // foreach (var lEnt...
        } // if (lv1s ...
      }
      catch (Exception lEx)
      {
      }


      /*
       * Update DGV
       */
      if (cAccountingRecords.Count > 0 && cAccountingConfig != null && cAccountingConfig.onUpdateList != null)
        cAccountingConfig.onUpdateList(cAccountingRecords);

    }





    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void onIPAccountingExited(object sender, System.EventArgs e)
    {
      killAllInstances();

      if (cAccountingConfig.onIPAccountingExit != null)
        cAccountingConfig.onIPAccountingExit();
    }



    #endregion

  }
}

