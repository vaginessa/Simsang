using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;


namespace Simsang.ARPScan.Main.Infrastructure
{
  class ProcessARPScan
  {

    #region MEMBERS

    private static ProcessARPScan cInstance;
    private String cARPScanProcName = "ARPScan";
    private Process cARPScanProc;
    private String cBaseDir;
    private String cARPScanBin;
    private String cData = String.Empty;
    private Action<String> cOnDataFunc;
    private Action cOnStop;

    #endregion


    #region PUBLIC

    /// <summary>
    /// 
    /// </summary>
    private ProcessARPScan()
    {
      cBaseDir = Directory.GetCurrentDirectory();
      cARPScanBin = String.Format(@"{0}\bin\{1}", cBaseDir, Config.ARPScanBinary);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static ProcessARPScan getInstance()
    {
      if (cInstance == null)
        cInstance = new ProcessARPScan();

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
    public void startARPScan(String pIfcID, String pStartIP, String pStopIP, Action<String> pOnDataFunc, Action pOnStop)
    {
      cOnDataFunc = pOnDataFunc;
      cOnStop = pOnStop;

      if (File.Exists(cARPScanBin))
      {
        cARPScanProc = new Process();

        cARPScanProc.StartInfo.FileName = cARPScanBin;
        cARPScanProc.StartInfo.Arguments = String.Format("{0} {1} {2}", pIfcID, pStartIP, pStopIP);
        cARPScanProc.StartInfo.UseShellExecute = false;
        //cARPScanProc.StartInfo.CreateNoWindow = true;
        cARPScanProc.StartInfo.CreateNoWindow = Config.DebugOn() ? false : true;
        cARPScanProc.StartInfo.WindowStyle = Config.DebugOn() ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden;

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
      else
        throw new Exception(String.Format("Error: {0} not found.", Config.ARPScanBinary));
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

    #endregion


    #region EVENTS

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void onARPScanExited(object sender, System.EventArgs e)
    {
      cOnStop();
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
          Match lTrafficMatch = Regex.Match(cData, @"(<arp>.*?</arp>)", RegexOptions.Singleline | RegexOptions.IgnoreCase);

          if (lTrafficMatch.Success)
          {
            cOnDataFunc(cData);
            cData = String.Empty;
          } // if (Regex.M...
        } // if (e.Data...
      } // if (e.Data..
    }

    #endregion

  }
}
