using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Simsang.Plugin;


namespace Plugin.Main.HTTPRequest
{
  public class DomainFacade : IObservable
  {

    #region MEMBERS

    private static DomainFacade cInstance;
    private InfrastructureFacade cInfrastructure;
    private List<HTTPRequests> cRecordList;
    private List<IObserver> cObserverList;
    private IPlugin cPlugin;

    #endregion


    #region PROPERTIES

    public List<HTTPRequests> HTTPRequestList { get { return (cRecordList); } private set { } }

    #endregion


    #region PUBLIC

    private DomainFacade(IPlugin pPlugin)
    {
      cPlugin = pPlugin;
      cObserverList = new List<IObserver>();
      cInfrastructure = InfrastructureFacade.getInstance(pPlugin);
      cRecordList = new List<HTTPRequests>();
    }


    /// <summary>
    /// Create single instance
    /// </summary>
    /// <returns></returns>
    public static DomainFacade getInstance(IPlugin pPlugin)
    {
      if (cInstance == null)
        cInstance = new DomainFacade(pPlugin);

      return (cInstance);
    }

    #endregion


    #region RECORDS

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pRecords"></param>
    public void addRecords(List<HTTPRequests> pRecords)
    {
      if (pRecords != null && pRecords.Count > 0)
      {
        foreach (HTTPRequests lTmp in pRecords)
          cRecordList.Add(lTmp);

        notify();
      }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pIndex"></param>
    public void removeElementAt(int pIndex)
    {
      cRecordList.RemoveAt(pIndex);
      notify();
    }



    /// <summary>
    /// 
    /// </summary>
    public void emptyRequestList()
    {
      cRecordList.Clear();
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
      cInfrastructure.saveSession<HTTPRequests>(pSessionFilePath, cRecordList);
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionFilePath"></param>
    public void loadSessionData(String pSessionFilePath)
    {
      BindingList<HTTPRequests> lSessionData = null;

      lSessionData = cInfrastructure.loadSessionData<HTTPRequests>(pSessionFilePath);

      if (lSessionData != null && lSessionData.Count > 0)
        foreach (HTTPRequests lTmp in lSessionData)
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
      BindingList<HTTPRequests> lRecords = cInfrastructure.loadSessionDataFromString<HTTPRequests>(pSessionData);

      if (lRecords != null && lRecords.Count > 0)
      {
        cRecordList.Clear();

        foreach (HTTPRequests lReq in lRecords)
          cRecordList.Add(lReq);
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
    /// <param name="pSessionName"></param>
    /// <returns></returns>
    public String getSessionData(String pSessionName)
    {
      return (cInfrastructure.getSessionData(pSessionName));
    }

    #endregion


    #region IOBSERVABLE INTERFACE METHODS

    /// <summary>
    /// 
    /// </summary>
    /// <param name="o"></param>
    public void addObserver(IObserver o)
    {
      cObserverList.Add(o);
    }

    /// <summary>
    /// 
    /// </summary>
    public void notify()
    {
      foreach (IObserver lObs in cObserverList)
        lObs.update(cRecordList);
    }

    #endregion

  }
}
