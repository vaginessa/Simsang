using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Simsang.Plugin;
using Plugin.Main.IPAccounting.Config;


namespace Plugin.Main.IPAccounting
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
    private TaskFacade(IPAccountingConfig pConfig, IPlugin pPlugin)
    {
      cPlugin = pPlugin;
      cDomain = DomainFacade.getInstance(pConfig, pPlugin);
    }


    /// <summary>
    /// Create single instance
    /// </summary>
    /// <returns></returns>
    public static TaskFacade getInstance(IPAccountingConfig pConfig, IPlugin pPlugin)
    {
      if (cInstance == null)
        cInstance = new TaskFacade(pConfig, pPlugin);

      return (cInstance);
    }

    #endregion


    #region SESSION


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionFilePath"></param>
    public void saveSession(String pSessionFilePath)
    {
      cDomain.saveSession(pSessionFilePath);
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


    #region RECORDS

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pRecord"></param>
    public void addRecord(AccountingItem pRecord)
    {
      cDomain.addRecord(pRecord);
    }


    /// <summary>
    /// 
    /// </summary>
    public void emptyRecordList()
    {
      cDomain.emptyRecordList();
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
    public void onStart(IPAccountingConfig pConfig)
    {
      cDomain.onStart(pConfig);
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
