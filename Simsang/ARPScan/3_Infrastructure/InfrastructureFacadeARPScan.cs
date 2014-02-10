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


namespace Simsang.ARPScan.MainTaskFacadeARPScan
{
  public class InfrastructureFacadeARPScan
  {

    #region MEMBERS

    private static InfrastructureFacadeARPScan cInstance;
    private String cARPScanProcName = "ARPScan";
    private Process cARPScanProc;
    private String cBaseDir;
    private String cARPScanBin;
    private String cData = String.Empty;
    private Action<String> cOnDataFunc;
    private ARPScanConfig cARPScanConf;

    private String cNmapProcName = "nmap";
    private String cNmapParameters = "-T4 -F --open {0} -O -oX {1}";
    private Process cNmapProc;
    private String cNmapBin;

    private String cXMLOutputFile;
    private String cXMLMACAddress;

    #endregion


    #region PUBLIC

    /// <summary>
    /// 
    /// </summary>
    private InfrastructureFacadeARPScan()
    {
      cBaseDir = Directory.GetCurrentDirectory();
      cARPScanBin = String.Format(@"{0}\bin\{1}", cBaseDir, Simsang.Config.ARPScanBinary);
      cNmapBin = String.Format(@"{0}\bin\nmap\{1}", cBaseDir, Simsang.Config.NmapBinary);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static InfrastructureFacadeARPScan getInstance()
    {
      if (cInstance == null)
        cInstance = new InfrastructureFacadeARPScan();

      return (cInstance);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pIfcID"></param>
    /// <param name="pStartIP"></param>
    /// <param name="pStopIP"></param>
    /// <param name="pOnDataFunc"></param>
    /// <param name="pOnStop"></param>
    public void startARPScan(ARPScanConfig pARPConfig)
    {
      cARPScanConf = pARPConfig;

      if (!File.Exists(cARPScanBin))
        throw new Exception("ARPscan binary not found");

      cARPScanProc = new Process();
      cARPScanProc.StartInfo.FileName = cARPScanBin;
      cARPScanProc.StartInfo.Arguments = String.Format("{0} {1} {2}", cARPScanConf.InterfaceID, cARPScanConf.StartIP, cARPScanConf.StopIP);
      cARPScanProc.StartInfo.UseShellExecute = false;
      //cARPScanProc.StartInfo.CreateNoWindow = true;
      cARPScanProc.StartInfo.CreateNoWindow = cARPScanConf.IsDebuggingOn ? false : true;
      cARPScanProc.StartInfo.WindowStyle = cARPScanConf.IsDebuggingOn ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden;

      // set up output redirection
      cARPScanProc.StartInfo.RedirectStandardOutput = true;
      //cARPScanProc.StartInfo.RedirectStandardError = true;
      cARPScanProc.EnableRaisingEvents = true;

      // Set the data received handlers
      //cARPScanProc.ErrorDataReceived += onDataRecived;
      cARPScanProc.OutputDataReceived += onDataRecived;

      // Configure the process exited event
      cARPScanProc.Exited += onARPScanExited;
      cARPScanProc.Disposed += onARPScanExited;

      cARPScanProc.Start();
      //cARPScanProc.BeginErrorReadLine();
      cARPScanProc.BeginOutputReadLine();

      Thread.Sleep(100);
    }


    /// <summary>
    /// 
    /// </summary>
    public void stopARPScan()
    {
      if (cARPScanProc != null && cARPScanProc.HasExited == false && cARPScanProc.Responding)
        cARPScanProc.Kill();
    }


    /// <summary>
    /// 
    /// </summary>
    public void killAllRunningARPScans()
    {
      Process[] lACInstances;
      if ((lACInstances = Process.GetProcessesByName(cARPScanProcName)) != null && lACInstances.Length > 0)
      {
        foreach (Process lProc in lACInstances)
        {
          try { lProc.Kill(); }
          catch (Exception) { }
        }
      }
    }



    /// <summary>
    /// 
    /// </summary>
    public void startFingerprint(FingerprintConfig pConfig)
    {
      if (String.IsNullOrEmpty(pConfig.IP))
        throw new Exception("Something is wrong with the target IP address");

      if (!File.Exists(cNmapBin))
        throw new Exception("ARPscan binary not found");

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
//      cNmapProc.BeginOutputReadLine();
    }

    #endregion


    #region EVENTS

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void onARPScanExited(object sender, System.EventArgs e)
    {
      if (cARPScanConf.OnARPScanStopped != null)
        cARPScanConf.OnARPScanStopped();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void onNmapScanExited(object sender, System.EventArgs e)
    {
      String lFingerprintDir = String.Format(@"{0}\{1}", Directory.GetCurrentDirectory(), Simsang.Config.FingerprintDir);
//      System.Windows.Forms.MessageBox.Show(cXMLOutputFile);

      // 1. Save fingerprint file      
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
      
      try
      {
        if (File.Exists(cXMLOutputFile))
        {
          String lMACAddr = Regex.Replace(cXMLMACAddress, @"[^\d\w]", "");
          String lOutputFileName = String.Format(@"{0}\{1}.xml", lFingerprintDir, lMACAddr);

          File.Copy(cXMLOutputFile, lOutputFileName);
          File.Delete(cXMLOutputFile);
        }
      }
      catch (Exception)
      { }

//      if (pConfig != null 

    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void onDataRecived(object sender, DataReceivedEventArgs e)
    {
      if (e.Data != null && e.Data.Length > 0)
      {
        if (e.Data != "<EOF>")
          cData += e.Data + "\n";
        else if (e.Data == "<EOF>")
        {
          Match lTrafficMatch = Regex.Match(cData, @"(<arp>.*?</arp>)", RegexOptions.Singleline | RegexOptions.IgnoreCase);

          if (lTrafficMatch.Success)
          {
            cARPScanConf.OnDataReceived(cData);
            cData = String.Empty;
          } // if (Regex.M...
        } // if (e.Data...
      } // if (e.Data..
    }

    #endregion

  }
}
