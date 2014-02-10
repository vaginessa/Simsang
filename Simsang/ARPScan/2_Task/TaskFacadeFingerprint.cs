using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

using Simsang.ARPScan.Main.Config;


namespace Simsang.ARPScan.Main
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
      // check input and throw error messages if necessary
      cInfrastructure.startFingerprint(pConfig);
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
    public void killAllRunningFingerprints()
    {
      cInfrastructure.killAllRunningFingerprints();
    }

    #endregion

  }

}
