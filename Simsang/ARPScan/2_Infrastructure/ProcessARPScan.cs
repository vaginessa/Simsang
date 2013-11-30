using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;


namespace Simsang.ARPScan.Main.Infrastructure
{
  public class ProcessARPScan
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
      IPAddress lStartIPAddr;
      IPAddress lStopIPAddr;
      cOnDataFunc = pOnDataFunc;
      cOnStop = pOnStop;

      /*
       * Validate input parameters.
       */
      if (!File.Exists(cARPScanBin))
        throw new Exception("ARPscan binary not found");
      else if (String.IsNullOrEmpty(pIfcID))
        throw new Exception("Something is wrong with the interface ID");
      else if (!IPAddress.TryParse(pStartIP, out lStartIPAddr))
        throw new Exception("Something is wrong with the start IP address");
      else if (!IPAddress.TryParse(pStopIP, out lStopIPAddr))
          throw new Exception("Something is wrong with the stop IP address");
      else if (IPHelper.Compare(lStartIPAddr, lStopIPAddr) > 0)
        throw new Exception("Start IP address is greater than stop IP address");

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


  public static class IPHelper
  {

    #region PUBLIC

    /// <summary>
    /// 
    /// </summary>
    /// <param name="IP"></param>
    /// <returns></returns>
    public static int ToInteger(IPAddress IP)
    {
      int result = 0;

      byte[] bytes = IP.GetAddressBytes();
      result = (int)(bytes[0] << 24 | bytes[1] << 16 | bytes[2] << 8 | bytes[3]);

      return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="IP1"></param>
    /// <param name="IP2"></param>
    /// <returns></returns>
    public static int Compare(this IPAddress IP1, IPAddress IP2)
    {
      int ip1 = ToInteger(IP1);
      int ip2 = ToInteger(IP2);
      return (((ip1 - ip2) >> 0x1F) | (int)((uint)(-(ip1 - ip2)) >> 0x1F));
    }

    #endregion 

  }

}
