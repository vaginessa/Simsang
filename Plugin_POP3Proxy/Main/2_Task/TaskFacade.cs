using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Simsang.Plugin;
using Plugin.Main.POP3Proxy.Config;


namespace Plugin.Main.POP3Proxy
{
  public class TaskFacade
  {

    #region MEMBERS

    private static TaskFacade cInstance;
    private DomainFacade cDomain;
    private IPlugin cPlugin;

    #endregion


    #region PUBLIC

    /// <summary>
    /// 
    /// </summary>
    private TaskFacade(ProxyConfig pConfig, IPlugin pPlugin)
    {
      cPlugin = pPlugin;
      cDomain = DomainFacade.getInstance(pConfig, pPlugin);
    }


    /// <summary>
    /// Create single instance
    /// </summary>
    /// <returns></returns>
    public static TaskFacade getInstance(ProxyConfig pConfig, IPlugin pPlugin)
    {
      if (cInstance == null)
        cInstance = new TaskFacade(pConfig, pPlugin);

      return (cInstance);
    }

    #endregion


    #region EVENTS

    /// <summary>
    /// 
    /// </summary>
    public void onInit()
    {
      cDomain.onInit();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pProxyConfig"></param>
    public void onStart(ProxyConfig pProxyConfig)
    {
      cDomain.onStart(pProxyConfig);
    }


    /// <summary>
    /// 
    /// </summary>
    public void onStop()
    {
      cDomain.onStop();
    }

    #endregion


    #region RECORDS

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pRecord"></param>
    public void addRecord(POP3Account pRecord)
    {
      cDomain.addRecord(pRecord);
    }


    /// <summary>
    /// 
    /// </summary>
    public void removeAllRecords()
    {
      cDomain.removeAllRecords();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pIndex"></param>
    public void removeRecordAt(int pIndex)
    {
      cDomain.removeRecordAt(pIndex);
    }

    #endregion


    #region SESSIION

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionFilePath"></param>
    /// <returns></returns>
    public String getSessionData(String pSessionFilePath)
    {
      return (cDomain.getSessionData(pSessionFilePath));
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
    public void saveSession(String pSessionFilePath)
    {
      cDomain.saveSession(pSessionFilePath);
    }

    #endregion

  }
}
