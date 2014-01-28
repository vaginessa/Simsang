using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Simsang.Plugin;
using Plugin.Main.POP3Proxy.Config;


namespace Plugin.Main.POP3Proxy
{
  public class DomainFacade : IObservable
  {

    #region MEMBERS

    private static DomainFacade cInstance;
    private InfrastructureFacade cInfrastructure;
    private const int cMaxTableRows = 128;
    private List<IObserver> cObserverList;
    private List<POP3Account> cRecordList;
    private IPlugin cPlugin;

    #endregion


    #region PROPERTIES

    public List<POP3Account> RecordList { get { return (cRecordList); } private set { } }

    #endregion


    #region PUBLIC

    /// <summary>
    /// 
    /// </summary>
    private DomainFacade(ProxyConfig pConfig, IPlugin pPlugin)
    {
      cPlugin = pPlugin;
      cObserverList = new List<IObserver>();
      cInfrastructure = InfrastructureFacade.getInstance(pConfig, pPlugin);
      cRecordList = new List<POP3Account>();
    }


    /// <summary>
    /// Create single instance
    /// </summary>
    /// <returns></returns>
    public static DomainFacade getInstance(ProxyConfig pConfig, IPlugin pPlugin)
    {
      if (cInstance == null)
        cInstance = new DomainFacade(pConfig, pPlugin);

      return (cInstance);
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
    public void addRecord(List<POP3Account> pRecords)
    {
      if (pRecords != null && pRecords.Count > 0)
      {
        foreach (POP3Account lTmpRecord in pRecords)
          cRecordList.Add(lTmpRecord);

        // Resize the DGV to the defined maximum size. \
        while (cRecordList.Count > cMaxTableRows)
          cRecordList.RemoveAt(0);

        notify();
      }
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
      List<POP3Account> lSessionData = null;

      lSessionData = cInfrastructure.loadSessionData<POP3Account>(pSessionFilePath);

      if (lSessionData != null && lSessionData.Count > 0)
        foreach (POP3Account lTmp in lSessionData)
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
      List<POP3Account> lRecords = cInfrastructure.loadSessionDataFromString<POP3Account>(pSessionData);

      if (lRecords != null && lRecords.Count > 0)
      {
        cRecordList.Clear();

        foreach (POP3Account lReq in lRecords)
          cRecordList.Add(lReq);
      }
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionFilePath"></param>
    public void saveSession(String pSessionFilePath)
    {
      cInfrastructure.saveSessionData<POP3Account>(pSessionFilePath, cRecordList);
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
