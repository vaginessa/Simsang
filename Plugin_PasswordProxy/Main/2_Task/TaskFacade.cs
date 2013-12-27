using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.ComponentModel;

using Simsang.Plugin;
using Plugin.Main.HTTPProxy.Config;


namespace Plugin.Main.HTTPProxy
{
  public class TaskFacade
  {

    #region MEMBERS

    private static TaskFacade cInstance;
    private DomainFacade cDomain;
    private IPlugin cPlugin;

    #endregion


    #region PUBLIC

    private TaskFacade(WebServerConfig pWebServerConfig, IPlugin pPlugin)
    {
      cPlugin = pPlugin;
      cDomain = DomainFacade.getInstance(pWebServerConfig, pPlugin);
    }


    /// <summary>
    /// Create single instance
    /// </summary>
    /// <param name="pWebServerConfig"></param>
    /// <returns></returns>
    public static TaskFacade getInstance(WebServerConfig pWebServerConfig, IPlugin pPlugin)
    {
      if (cInstance == null)
        cInstance = new TaskFacade(pWebServerConfig, pPlugin);

      return (cInstance);
    }


    /// <summary>
    /// 
    /// </summary>
    public List<ManageAuthentications.AccountPattern> readAuthenticationPatterns()
    {
      return(cDomain.readAuthenticationPatterns());
    }

    #endregion


    #region RECORDS

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pRecord"></param>
    public void addRecord(Account pRecord)
    {
      int lDstPort;
      String lRemSystem = pRecord.DstIP.Trim();

      if (!Int32.TryParse(pRecord.DstPort, out lDstPort))
        throw new Exception("Something is wrong with the remote port.");
      else if (!Regex.Match(lRemSystem, @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$").Success && !Regex.Match(lRemSystem, @"\.[\d\w]+").Success)
        throw new Exception("Something is wrong with the remote system.");

      cDomain.addRecord(pRecord);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pIndex"></param>
    public void removeRecordAt(int pIndex)
    {
      cDomain.removeRecordAt(pIndex);
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
    /// <param name="pRemoteHost"></param>
    /// <param name="pRedirectURL"></param>
    public void saveSession(String pSessionFilePath, String pRemoteHost, String pRedirectURL)
    {
      cDomain.saveSession(pSessionFilePath, pRemoteHost, pRedirectURL);
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


    #region EVENTS

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pWebServerConfig"></param>
    public void onInit(WebServerConfig pWebServerConfig)
    {
      cDomain.onInit(pWebServerConfig);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pConfig"></param>
    public void startProxies(WebServerConfig pConfig)
    {
      cDomain.startProxies(pConfig);
    }


    /// <summary>
    /// 
    /// </summary>
    public void onStop()
    {
      cDomain.onStop();
    }

    #endregion

  }
}
