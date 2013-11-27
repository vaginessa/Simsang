using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Simsang.Plugin;
using Plugin.Main.IMAP4Proxy.Config;


namespace Plugin.Main.IMAP4Proxy
{
  public class DomainFacade : IObservable
  {

    #region MEMBERS

    private static DomainFacade cInstance;
    private InfrastructureFacade cInfrastructure;
    private List<IObserver> cObserverList;
    private List<IMAP4Account> cRecordList;
    private IPlugin cPlugin;

    #endregion


    #region PROPERTIES

    public List<IMAP4Account> RecordList { get { return (cRecordList); } private set { } }

    #endregion


    #region PUBLIC

    /// <summary>
    /// 
    /// </summary>
    public DomainFacade(ProxyConfig pProxyConfig, IPlugin pPlugin)
    {
      cPlugin = pPlugin;
      cRecordList = new List<IMAP4Account>();
      cObserverList = new List<IObserver>();
      cInfrastructure = InfrastructureFacade.getInstance(pProxyConfig, pPlugin);
    }



    /// <summary>
    /// Create single instance
    /// </summary>
    public static DomainFacade getInstance(ProxyConfig pProxyConfig, IPlugin pPlugin)
    {
      return cInstance ?? (cInstance = new DomainFacade(pProxyConfig, pPlugin));
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
    public void onStart(ProxyConfig pProxyConfig)
    {
      cInfrastructure.onStart(pProxyConfig);
    }


    /// <summary>
    /// 
    /// </summary>
    public void onStop()
    {
      cInfrastructure.onStop();
    }

    #endregion


    #region RECORDS

    /// <summary>
    /// 
    /// </summary>
    public void removeAllRecords()
    {
      if (cRecordList.Count > 0)
      {
        cRecordList.Clear();
        notify();
      }
    }


    /// <summary>
    /// 
    /// </summary>
    public void removeRecordAt(int pIndex)
    {
      if (cRecordList.Count > 0)
      {
        cRecordList.RemoveAt(pIndex);
        notify();
      }
    }



    /// <summary>
    /// 
    /// </summary>
    public void addRecord(IMAP4Account pRecord)
    {
      cRecordList.Add(pRecord);
      notify();
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
      return (cInfrastructure.getSessionData(pSessionFilePath));
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
    public void loadSessionData(String pSessionFilePath)
    {
      List<IMAP4Account> lSessionData = null;

      lSessionData = cInfrastructure.loadSessionData<IMAP4Account>(pSessionFilePath);

      if (lSessionData != null && lSessionData.Count > 0)
        foreach (IMAP4Account lTmp in lSessionData)
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
      BindingList<IMAP4Account> lRecords = cInfrastructure.loadSessionDataFromString<IMAP4Account>(pSessionData);

      if (lRecords != null && lRecords.Count > 0)
      {
        cRecordList.Clear();

        foreach (IMAP4Account lReq in lRecords)
          cRecordList.Add(lReq);
      }
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionFilePath"></param>
    public void saveSession(String pSessionFilePath)
    {
      cInfrastructure.saveSessionData<IMAP4Account>(pSessionFilePath, cRecordList);
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
