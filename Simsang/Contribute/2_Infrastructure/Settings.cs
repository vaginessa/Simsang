using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simsang.Contribute.Infrastructure
{
  public class Settings
  {

    #region MEMBERS

    private static Settings cInstance;

    #endregion


    #region PUBLIC

    /// <summary>
    /// 
    /// </summary>
    private Settings()
    {
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static Settings getInstance()
    {
      if (cInstance == null)
        cInstance = new Settings();

      return (cInstance);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pStatus"></param>
    public void setContributeStatus(String pStatus)
    {
      if (!String.IsNullOrEmpty(pStatus))
        Config.SetRegistryValue(Config.RegistryContributionKey, Config.RegistryContributionValue, pStatus);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool isContributeSet()
    {
      bool lRetVal = false;
      String lStatus = String.Empty;

      try
      {
        lStatus = Config.GetRegistryValue(Config.RegistryContributionKey, Config.RegistryContributionValue);
        if (!String.IsNullOrEmpty(lStatus) && lStatus.ToLower() == "ok")
          lRetVal = true;
      }
      catch (Exception)
      { }

      return (lRetVal);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pStatus"></param>
    public void setSSIDStatus(String pStatus)
    {
      if (!String.IsNullOrEmpty(pStatus))
        Config.SetRegistryValue(Config.RegistrySSIDContributionKey, Config.RegistrySSIDContributionValue, pStatus);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool isSSIDStatusSet()
    {
      bool lRetVal = false;
      String lStatus = String.Empty;

      try
      {
        lStatus = Config.GetRegistryValue(Config.RegistrySSIDContributionKey, Config.RegistrySSIDContributionValue);
        if (!String.IsNullOrEmpty(lStatus) && lStatus.ToLower() == "ok")
          lRetVal = true;
      }
      catch (Exception)
      { }

      return (lRetVal);
    }

    #endregion

  }
}
