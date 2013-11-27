using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.ComponentModel;

using Simsang.Plugin;


namespace Plugin.Main.DNSPoison
{
  public class TaskFacade
  {

    #region MEMBERS

    private static TaskFacade cInstance;
    private DomainFacade cDomain;
    private IPlugin cPlugin;

    #endregion


    #region PUBLIC

    private TaskFacade(IPlugin pPlugin)
    {
      cPlugin = pPlugin;
      cDomain = DomainFacade.getInstance(pPlugin);
    }

    /// <summary>
    /// Create single instance
    /// </summary>
    /// <returns></returns>  
    public static TaskFacade getInstance(IPlugin pPlugin)
    {
      if (cInstance == null)
        cInstance = new TaskFacade(pPlugin);

      return (cInstance);
    }

    #endregion


    #region RECORDS

    /// <summary>
    /// Add IP/Host name pair that shall be poisoned
    /// </summary>
    /// <param name="pHostName"></param>
    /// <param name="pIPAddress"></param>
    public void addRecord(String pHostName, String pIPAddress)
    {
      String lHostNamePattern = @"[\d\w\.-_]+\.[\w]{2,3}$";
      String lIPAddressPattern = @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$";

      /*
       * Check IP and host name have a correct structure
       */
      if (String.IsNullOrEmpty(pHostName))
        throw new Exception("You didn't define a host name");
      else if (String.IsNullOrEmpty(pIPAddress))
        throw new Exception("You didn't define a IP address");
      else if (!Regex.Match(pHostName, lHostNamePattern, RegexOptions.IgnoreCase).Success)
        throw new Exception("Something is wrong with the host name!");
      else if (!Regex.Match(pIPAddress, lIPAddressPattern, RegexOptions.IgnoreCase).Success)
        throw new Exception("Something is wrong with the IP address!");
      else
        cDomain.addRecord(pHostName, pIPAddress);
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="pHostName"></param>
    public void removeRecordAt(String pHostName)
    {
      cDomain.removeRecordAt(pHostName);
    }


    /// <summary>
    /// 
    /// </summary>
    public void clearRecordList()
    {
      cDomain.clearRecordList();
    }

    #endregion


    #region SESSION

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionFilePath"></param>
    public void saveSession(String pSessionName)
    {
      cDomain.saveSession(pSessionName);
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionFilePath"></param>
    public void loadSessionData(String pSessionFilePath)
    {
      cDomain.loadSessionData(pSessionFilePath);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionData"></param>
    public void loadSessionDataFromString(String pSessionData)
    {
      cDomain.loadSessionDataFromString(pSessionData);
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionFilePath"></param>
    public void deleteSession(String pSessionFilePath)
    {
      cDomain.deleteSession(pSessionFilePath);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionFilePath"></param>
    /// <returns></returns>
    public String getSessionData(String pSessionFilePath)
    {
      return (cDomain.getSessionData(pSessionFilePath));
    }

    #endregion

  }
}
