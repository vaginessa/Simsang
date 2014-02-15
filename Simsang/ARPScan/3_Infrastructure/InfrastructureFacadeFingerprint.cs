using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

using Simsang;
using Simsang.ARPScan.Main.Config;


namespace Simsang.ARPScan.SystemFingerprint
{
  public class InfrastructureFacadeFingerprint
  {

    #region MEMBERS

    private static InfrastructureFacadeFingerprint cInstance;
    private String cBaseDir;
    private FingerprintConfig cFingerprintConf;

    private String cNmapProcName = "nmap";
////    private String cNmapParameters = "-T4 -F -O {0} -oX {1}"; // Fast scan
//    private String cNmapParameters = "--host-timeout 15s -T4 -O {0} -oX {1}";
////    private String cNmapParameters = "--host-timeout 15s -sV -T4 -O -F --version-light {0} -oX {1}";
    private String cNmapParameters = "--host-timeout 15s -T5 -O --osscan-guess -F {0} -oX {1}";
    private Process cNmapProc;
    private String cNmapBin;
    private bool cProcStopRequested;

    private String cXMLOutputFile;
    private String cXMLMACAddress;

    #endregion


    #region PUBLIC

    /// <summary>
    /// 
    /// </summary>
    private InfrastructureFacadeFingerprint()
    {
      cBaseDir = Directory.GetCurrentDirectory();
      cNmapBin = String.Format(@"{0}\bin\nmap\{1}", cBaseDir, Simsang.Config.NmapBinary);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static InfrastructureFacadeFingerprint getInstance()
    {
      if (cInstance == null)
        cInstance = new InfrastructureFacadeFingerprint();

      return (cInstance);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pMACAddress"></param>
    public SystemDetails getSystemDetails(String pMAC)
    {
      var lXMLDoc = new XDocument();
      String lFilePath = getFingerprintNmapFile(pMAC);
      SystemDetails lSysDetails = new SystemDetails();

      /*
       * Parse scan date
       */
      try
      {
        lXMLDoc = XDocument.Load(lFilePath);
        lSysDetails.ScanDate = lXMLDoc.Descendants().Elements("runstats").First().Element("finished").Attribute("timestr").Value;
      }
      catch (Exception lEx)
      {
        String lMessage = lEx.Message;
      }



      /*
       * Parse Note
       */
      try
      {
        lXMLDoc = XDocument.Load(lFilePath);
        lSysDetails.ScanDate = lXMLDoc.Descendants().Elements("runstats").First().Element("finished").Attribute("timestr").Value;
      }
      catch (Exception lEx)
      {
        String lMessage = lEx.Message;
      }


      /*
       * Parse open ports
       */
      try
      {
        var ports = lXMLDoc.Descendants().Elements("port")
                       .Select(y => new OpenService
                       { 
                         Protocol = y.Attribute("protocol").Value,
                         PortNo = y.Attribute("portid").Value,
                         ServiceName = y.Element("service").Attribute("name").Value
                       });
        lSysDetails.OpenPorts = ports.ToList<OpenService>();
      }
      catch (Exception) { }


      /*
       * Extract OS guess from XML file
       */
      try
      {
        var OSGuess = lXMLDoc.Descendants().Elements("os").First().Elements("osmatch")
                       .Select(y => new OS
                       {
                         Accuracy = y.Attribute("accuracy").Value,
                         OSName = y.Attribute("name").Value
                       });
        lSysDetails.OSGuess = OSGuess.ToList<OS>();
      }
      catch (Exception lEx)
      {
        String lMsg = lEx.Message;
      }

      return lSysDetails;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pMACAddress"></param>
    public String getSystemNote(String pMACAddress)
    {
      String lRetVal = String.Empty;
      String lFilePath = getFingerprintNoteFile(pMACAddress);

      try
      {
        if (File.Exists(lFilePath))
          lRetVal = File.ReadAllText(lFilePath);
      }
      catch (Exception)
      { 
      }

      return lRetVal;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pMACAddress"></param>
    public void setSystemNote(String pMAC, String pNote)
    {
      if (!String.IsNullOrEmpty(pMAC) && !String.IsNullOrEmpty(pNote))
      {
        String lFilePath = getFingerprintNoteFile(pMAC);
        try
        {
          File.WriteAllText(lFilePath, pNote);
        }
        catch (Exception)
        {}
      }
    }


    /// <summary>
    /// 
    /// </summary>
    public void stopFingerprint()
    {
      // Stop running process.
      if (cNmapProc != null)
      {
        cProcStopRequested = true;
        cNmapProc.EnableRaisingEvents = false;
        cNmapProc.Exited += null;
        cNmapProc.Disposed += null;

        try
        {
          if (cNmapProc != null && cNmapProc.HasExited == false && cNmapProc.Responding)
            cNmapProc.Kill();
        }
        catch (Exception lEx)
        { }
      }

      // Kill everything that looks like our fingerprint process
      killAllRunningFingerprints();
    }


    /// <summary>
    /// 
    /// </summary>
    public void startFingerprint(FingerprintConfig pConfig)
    {
      if (!File.Exists(cNmapBin))
        throw new Exception("Nmap binary not found");

      if (pConfig == null)
        throw new Exception("Something is wrong with the configuration parameters");

      if (String.IsNullOrEmpty(pConfig.IP))
        throw new Exception("Something is wrong with the target IP address");

      cProcStopRequested = false;
      cFingerprintConf = pConfig;
      cXMLMACAddress = pConfig.MAC;
      cXMLOutputFile = Path.GetTempFileName();

      cNmapProc = new Process();
      cNmapProc.StartInfo.FileName = cNmapBin;
      cNmapProc.StartInfo.Arguments = String.Format(cNmapParameters, pConfig.IP, cXMLOutputFile);
      cNmapProc.StartInfo.UseShellExecute = false;
      cNmapProc.StartInfo.CreateNoWindow = pConfig.IsDebuggingOn ? false : true;
      cNmapProc.StartInfo.WindowStyle = pConfig.IsDebuggingOn ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden;
      cNmapProc.EnableRaisingEvents = true;

      // Configure the process exited event
      cNmapProc.Exited += onNmapScanExited;
      cNmapProc.Disposed += onNmapScanExited;

      cNmapProc.Start();
    }

    #endregion


    #region EVENTS

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void onNmapScanExited(object sender, System.EventArgs e)
    {
      if (cProcStopRequested == false)
      {
        String lFingerprintDir = String.Format(@"{0}\{1}", Directory.GetCurrentDirectory(), Simsang.Config.FingerprintDir);

        // 1. Create fingerprint directory if necessary     
        if (!Directory.Exists(lFingerprintDir))
        {
          try
          {
            Directory.CreateDirectory(lFingerprintDir);
          }
          catch (Exception)
          {
          }
        } // if (!Direc...

        // 2. Save fingerprint file    
        try
        {
          if (File.Exists(cXMLOutputFile))
          {
            String lOutputFileName = getFingerprintNmapFile(cXMLMACAddress);

            File.Copy(cXMLOutputFile, lOutputFileName, true);
            File.Delete(cXMLOutputFile);
          } // if File.Ex...
        }
        catch (Exception)
        { }


        // 3. Tell the main process we're done
        if (cFingerprintConf.OnScanStopped != null)
          cFingerprintConf.OnScanStopped();
      } // if (cProcSto...
    }

    #endregion


    #region PRIVATE

    /// <summary>
    /// 
    /// </summary>
    private void killAllRunningFingerprints()
    {
      Process[] lACInstances;

      if ((lACInstances = Process.GetProcessesByName(cNmapProcName)) != null && lACInstances.Length > 0)
      {
        foreach (Process lProc in lACInstances)
        {
          try { lProc.Kill(); }
          catch (Exception) { }
        } // foreach (Pro...
      } // if ((lA...
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pMAC"></param>
    /// <returns></returns>
    private String getFingerprintNmapFile(String pMAC)
    {
      String lOutputFileName = String.Empty;
      String lFingerprintDir = String.Empty;
      String lMACAddr = String.Empty;

      if (!String.IsNullOrEmpty(pMAC))
      {
        lFingerprintDir = String.Format(@"{0}\{1}", Directory.GetCurrentDirectory(), Simsang.Config.FingerprintDir);
        lMACAddr = Regex.Replace(pMAC, @"[^\d\w]", "");
        lOutputFileName = String.Format(@"{0}\{1}.nmap", lFingerprintDir, lMACAddr);
      } // if (!Strin...

      return (lOutputFileName);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pMAC"></param>
    /// <returns></returns>
    private String getFingerprintNoteFile(String pMAC)
    {
      String lOutputFileName = String.Empty;
      String lFingerprintDir = String.Empty;
      String lMACAddr = String.Empty;

      if (!String.IsNullOrEmpty(pMAC))
      {
        lFingerprintDir = String.Format(@"{0}\{1}", Directory.GetCurrentDirectory(), Simsang.Config.FingerprintDir);
        lMACAddr = Regex.Replace(pMAC, @"[^\d\w]", "");
        lOutputFileName = String.Format(@"{0}\{1}.note", lFingerprintDir, lMACAddr);
      } // if (!Strin...

      return (lOutputFileName);
    }

    #endregion

  }
}
