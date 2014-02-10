using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

using Simsang.ARPScan.Main.Config;
using Simsang.ARPScan.MainTaskFacadeARPScan;

namespace Simsang.ARPScan.Main
{
  public class TaskFacadeARPScan
  {

    #region MEMBERS

    private static TaskFacadeARPScan cInstance;
    private InfrastructureFacadeARPScan cInfrastructure;

    #endregion


    #region PUBLIC

    /// <summary>
    /// 
    /// </summary>
    public TaskFacadeARPScan()
    {
      cInfrastructure = InfrastructureFacadeARPScan.getInstance();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static TaskFacadeARPScan getInstance()
    {
      return cInstance ?? (cInstance = new TaskFacadeARPScan());
    }



    /// <summary>
    /// 
    /// </summary>
    public void stopARPScan()
    {
      cInfrastructure.stopARPScan();
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="pARPConf"></param>
    public void startARPScan(ARPScanConfig pARPConfig)
    {
      IPAddress lStartIPAddr;
      IPAddress lStopIPAddr;

      /*
       * Validate input parameters.
       */
      if (String.IsNullOrEmpty(pARPConfig.InterfaceID))
        throw new Exception("Something is wrong with the interface ID");
      else if (!IPAddress.TryParse(pARPConfig.StartIP, out lStartIPAddr))
        throw new Exception("Something is wrong with the start IP address");
      else if (!IPAddress.TryParse(pARPConfig.StopIP, out lStopIPAddr))
        throw new Exception("Something is wrong with the stop IP address");
      else if (IPHelper.Compare(lStartIPAddr, lStopIPAddr) > 0)
        throw new Exception("Start IP address is greater than stop IP address");

      cInfrastructure.startARPScan(pARPConfig);
    }



    /// <summary>
    /// 
    /// </summary>
    public void killAllRunningARPScans()
    {
      cInfrastructure.killAllRunningARPScans();
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
