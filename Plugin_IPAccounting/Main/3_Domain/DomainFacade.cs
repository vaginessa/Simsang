using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Simsang.Plugin;
using Plugin.Main.IPAccounting.Config;

namespace Plugin.Main.IPAccounting
{
  public class DomainFacade : IObservable
  {

    #region MEMBERS

    private static DomainFacade cInstance;
    private InfrastructureFacade cInfrastructure;
    private List<IObserver> cObserverList;
    private List<AccountingItem> cRecordList;
    private IPlugin cPlugin;

    #endregion


    #region PROPERTIES

    public List<AccountingItem> RecordList { get { return (cRecordList); } private set { } }

    #endregion


    #region PUBLIC

    /// <summary>
    /// 
    /// </summary>
    private DomainFacade(IPAccountingConfig pConfig, IPlugin pPlugin)
    {
      cPlugin = pPlugin;
      cInfrastructure = InfrastructureFacade.getInstance(pConfig, pPlugin);
      cRecordList = new List<AccountingItem>();
      cObserverList = new List<IObserver>();
    }


    /// <summary>
    /// Create single instance
    /// </summary>
    /// <returns></returns>
    public static DomainFacade getInstance(IPAccountingConfig pConfig, IPlugin pPlugin)
    {
      if (cInstance == null)
        cInstance = new DomainFacade(pConfig, pPlugin);

      return (cInstance);
    }

    #endregion


    #region RECORDS

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pRecord"></param>
    public void addRecord(AccountingItem pRecord)
    {
      cRecordList.Add(pRecord);
    }


    /// <summary>
    /// 
    /// </summary>
    public void emptyRecordList()
    {
      cRecordList.Clear();
      notify();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pIndex"></param>
    public void removeRecordAt(int pIndex)
    {
      cRecordList.RemoveAt(pIndex);
      notify();
    }

    #endregion


    #region SESSION

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionFilePath"></param>
    public void saveSession(String pSessionFilePath)
    {
      cInfrastructure.saveSessionData<AccountingItem>(pSessionFilePath, cRecordList);
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionFilePath"></param>
    public void loadSessionData(String pSessionFilePath)
    {
      List<AccountingItem> lSessionData = null;

      lSessionData = cInfrastructure.loadSessionData<AccountingItem>(pSessionFilePath);

      if (lSessionData != null && lSessionData.Count > 0)
        foreach (AccountingItem lTmp in lSessionData)
          cRecordList.Add(lTmp);

      // Notify all observers
      notify();
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionData"></param>
    public void loadSessionDataFromString(String pSessionData)
    {
      List<AccountingItem> lRecords = cInfrastructure.loadSessionDataFromString<AccountingItem>(pSessionData);

      if (lRecords != null && lRecords.Count > 0)
      {
        cRecordList.Clear();

        foreach (AccountingItem lReq in lRecords)
          cRecordList.Add(lReq);

        notify();
      }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionFilePath"></param>
    public void deleteSession(String pSessionFilePath)
    {
      cInfrastructure.deleteSession(pSessionFilePath);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionFilePath"></param>
    /// <returns></returns>
    public String getSessionData(String pSessionFilePath)
    {
      return (cInfrastructure.getSessionData(pSessionFilePath));
    }

    #endregion


    #region EVENTS

    /// <summary>
    /// 
    /// </summary>
    public void onInit()
    {
      cInfrastructure.onInit();
      cRecordList.Clear();
      notify();
    }

    /// <summary>
    /// 
    /// </summary>
    public void onStart(IPAccountingConfig pConfig)
    {
      cInfrastructure.onStart(pConfig);
    }

    /// <summary>
    /// 
    /// </summary>
    public void onStop()
    {
      cInfrastructure.onStop();
    }

    #endregion


    #region OBSERVABLE INTERFACE METHODS

    public void addObserver(IObserver o)
    {
      cObserverList.Add(o);
    }


    public void notify()
    {
      foreach (IObserver lObs in cObserverList)
        lObs.update(cRecordList);
    }

    #endregion

  }
}
