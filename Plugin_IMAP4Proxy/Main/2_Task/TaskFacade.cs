using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Simsang.Plugin;
using Plugin.Main.IMAP4Proxy.Config;


namespace Plugin.Main.IMAP4Proxy
{
  public class TaskFacade
  {

    #region MEMBERS

    private static TaskFacade cInstance;
    private DomainFacade cDomain;
    private IPlugin cPlugin;

    #endregion


    #region PUBLIC

    public TaskFacade(ProxyConfig pProxyConfig, IPlugin pPlugin)
    {
      cPlugin = pPlugin;
      cDomain = DomainFacade.getInstance(pProxyConfig, pPlugin);
    }


    /// <summary>
    /// Create single instance
    /// </summary>
    /// <param name="pProxyConfig"></param>
    /// <returns></returns>
    public static TaskFacade getInstance(ProxyConfig pProxyConfig, IPlugin pPlugin)
    {
      return cInstance ?? (cInstance = new TaskFacade(pProxyConfig, pPlugin));
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
    public void addRecord(IMAP4Account pRecord)
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


    /*
     * 
     * 
     */
    public String getSessionData(String pSessionFilePath)
    {
      return (cDomain.getSessionData(pSessionFilePath));
    }



    /*
     * 
     * 
     */
    public void deleteSession(String pSessionFilePath)
    {
      cDomain.deleteSession(pSessionFilePath);
    }



    /*
     * 
     * 
     */
    public void loadSessionData(String pSessionFilePath)
    {
      cDomain.loadSessionData(pSessionFilePath);
    }



    /*
     * 
     * 
     */
    public void loadSessionDataFromString(String pSessionData)
    {
      cDomain.loadSessionDataFromString(pSessionData);
    }



    /*
     * 
     * 
     */
    public void saveSession(String pSessionFilePath)
    {
      cDomain.saveSession(pSessionFilePath);
    }

    #endregion

  }
}
