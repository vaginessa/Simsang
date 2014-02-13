using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

using Simsang.ARPScan.Main.Config;


namespace Simsang.ARPScan.SystemFingerprint
{
  public class TaskFacadeFingerprint
  {

    #region MEMBERS

    private static TaskFacadeFingerprint cInstance;
    private InfrastructureFacadeFingerprint cInfrastructure;

    #endregion


    #region PUBLIC

    /// <summary>
    /// 
    /// </summary>
    public TaskFacadeFingerprint()
    {
      cInfrastructure = InfrastructureFacadeFingerprint.getInstance();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static TaskFacadeFingerprint getInstance()
    {
      return cInstance ?? (cInstance = new TaskFacadeFingerprint());
    }



    /// <summary>
    /// 
    /// </summary>
    public void startFingerprint(FingerprintConfig pConfig)
    {
      cInfrastructure.startFingerprint(pConfig);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pMAC"></param>
    /// <returns></returns>
    public String getFingerprintNote(String pMAC)
    {
      return cInfrastructure.getSystemNote(pMAC);
    }


    /// <summary>
    /// 
    /// </summary>
    public void stopFingerprint()
    {
      cInfrastructure.stopFingerprint();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pMACAddress"></param>
    public SystemDetails getSystemDetails(String pMACAddress)
    {
      return cInfrastructure.getSystemDetails(pMACAddress);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pMACAddress"></param>
    public String getSystemNote(String pMACAddress)
    {
      return cInfrastructure.getSystemNote(pMACAddress);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pMACAddress"></param>
    public void setSystemNote(String pMACAddress, String pNote)
    {
      cInfrastructure.setSystemNote(pMACAddress, pNote);
    }

    #endregion

  }

}
