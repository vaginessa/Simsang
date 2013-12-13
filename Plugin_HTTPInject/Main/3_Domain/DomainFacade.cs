using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Simsang.Plugin;


namespace Plugin.Main.HTTPInject
{
  public class DomainFacade : IObservable
  {

    #region MEMBERS

    private static DomainFacade cInstance;
    private InfrastructureFacade cInfrastructure;
    private List<InjectedURLRecord> cRecordList;
    private List<IObserver> cObservers;
    private IPlugin cPlugin;

    #endregion


    #region PROPERTIES

    public List<InjectedURLRecord> InjectedURLList { get { return cRecordList; } private set { } }

    #endregion


    #region PUBLIC

    private DomainFacade(InjectionConfig pProxyConfig, IPlugin pPlugin)
    {
      cPlugin = pPlugin;
      cInfrastructure = InfrastructureFacade.getInstance(pProxyConfig, pPlugin);
      cRecordList = new List<InjectedURLRecord>();
      cObservers = new List<IObserver>();
    }


    /// <summary>
    /// Create single instance
    /// </summary>
    /// <param name="pFWConfig"></param>
    /// <returns></returns>
    public static DomainFacade getInstance(InjectionConfig pFWConfig, IPlugin pPlugin)
    {
      if (cInstance == null)
        cInstance = new DomainFacade(pFWConfig, pPlugin);

      return (cInstance);
    }

    #endregion


    #region RECORDS

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pRecord"></param>
    public void addRecord(InjectedURLRecord pRecord)
    {
      cRecordList.Add(pRecord);
      notify();
    }


    /// <summary>
    /// 
    /// </summary>
    public void clearRecordList()
    {
      cRecordList.Clear();
      notify();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pRequestedHost"></param>
    /// <param name="pRequestedURL"></param>
    public void removeRecordAt(String pRequestedHost, String pRequestedURL)
    {
      if (cRecordList != null && cRecordList.Count > 0)
      {
        for (int i = 0; i < cRecordList.Count; i++)
          if (cRecordList.ElementAt(i).RequestedHost == pRequestedHost && cRecordList.ElementAt(i).RequestedURL == pRequestedURL)
            cRecordList.RemoveAt(i);
      }
        //foreach (InjectedURLRecord lTmp in cRecordList)
        //  if (lTmp.RequestedHost == pRequestedHost && lTmp.RequestedURL == pRequestedURL)
        //    cRecordList.Remove(lTmp);

      notify();
    }


    #endregion


    #region SESSION

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionFilePath"></param>
    public void saveSession(String pSessionName)
    {
      cInfrastructure.saveSessionData<InjectedURLRecord>(pSessionName, cRecordList);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionFilePath"></param>
    public void loadSessionData(String pSessionName)
    {
      List<InjectedURLRecord> lSessionData = null;

      lSessionData = cInfrastructure.loadSessionData<InjectedURLRecord>(pSessionName);

      if (lSessionData != null && lSessionData.Count > 0)
        foreach (InjectedURLRecord lTmp in lSessionData)
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
      List<InjectedURLRecord> lRecords = cInfrastructure.loadSessionDataFromString<InjectedURLRecord>(pSessionData);

      if (lRecords != null && lRecords.Count > 0)
      {
        cRecordList.Clear();

        foreach (InjectedURLRecord lReq in lRecords)
          cRecordList.Add(lReq);
      }
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionFilePath"></param>
    public void deleteSession(String pSessionName)
    {
      cInfrastructure.deleteSession(pSessionName);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionFilePath"></param>
    /// <returns></returns>
    public String getSessionData(String pSessionName)
    {
      return (cInfrastructure.getSessionData(pSessionName));
    }

    #endregion


    #region EVENTS

    /// <summary>
    /// 
    /// </summary>
    public void onStart()
    {
      cInfrastructure.onStart(cRecordList.ToList());
    }


    /// <summary>
    /// 
    /// </summary>
    public void onStop()
    {
      cInfrastructure.onStop();
    }


    /// <summary>
    /// 
    /// </summary>
    public void onInit()
    {
      cInfrastructure.onInit();
    }

    #endregion


    #region METHODS IObservable

    public void addObserver(IObserver pObserver)
    {
      if (pObserver != null)
        cObservers.Add(pObserver);
    }


    public void notify()
    {
      foreach (IObserver lTmp in cObservers)
        lTmp.update(cRecordList.ToList());
    }

    #endregion

  }
}
