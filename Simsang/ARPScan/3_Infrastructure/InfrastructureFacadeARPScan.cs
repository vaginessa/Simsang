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
        } // foreach (Proc...
      } // if ((lACIn..
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
