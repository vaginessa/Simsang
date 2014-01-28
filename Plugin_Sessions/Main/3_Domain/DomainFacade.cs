using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using Plugin.Main.Session.ManageSessions.Config;
using Simsang.Plugin;


namespace Plugin.Main.Session
{
  public class DomainFacade : IObservable
  {

    #region MEMBERS

    private static DomainFacade cInstance;
    private InfrastructureFacade cInfrastructure;
    private const int cMaxTableRows = 128;
    private List<Session.Config.Session> cRecordList;
    private List<IObserver> cObserverList;
    private IPlugin cPlugin;

    #endregion


    #region PROPERTIES

    public List<Session.Config.Session> RecordList { get { return (cRecordList); } private set { } }

    #endregion


    #region PUBLIC

    private DomainFacade(IPlugin pPlugin)
    {
      cPlugin = pPlugin;
      cInfrastructure = InfrastructureFacade.getInstance(pPlugin);
      cRecordList = new List<Session.Config.Session>();
      cObserverList = new List<IObserver>();
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


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public List<SessionPattern> readSessionPatterns()
    {
      return (cInfrastructure.readSessionPatterns());
    }

    #endregion


    #region RECORDS

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
    /// <param name="pRecord"></param>
    public void addRecord(Session.Config.Session pRecord)
    {
      Int32 lDstPort = 0;

      if (pRecord != null)
      {
        // Check if record already exists
        foreach (Session.Config.Session lTmp in cRecordList)
          if (lTmp.ID == pRecord.ID)
            throw new Exception("Session alrady exists.");

        // Check if URL is correct
        if (!Regex.Match(pRecord.URL, @"[\w\d\-_\.:/]+/.+").Success)
          throw new Exception("Something is wrong with the URL.");

        // Check if destination port is correct
        if (!Int32.TryParse(pRecord.DstPort, out lDstPort))
          throw new Exception("Something is wrong with the destination port.");

        cRecordList.Add(pRecord);

        // Resize the DGV to the defined maximum size. \
        while (cRecordList.Count > cMaxTableRows)
          cRecordList.RemoveAt(0);

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
      List<Session.Config.Session> lSessionData = null;

      lSessionData = cInfrastructure.loadSessionData<Session.Config.Session>(pSessionFilePath);

      if (lSessionData != null && lSessionData.Count > 0)
        foreach (Session.Config.Session lTmp in lSessionData)
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
      BindingList<Session.Config.Session> lRecords = cInfrastructure.loadSessionDataFromString<Session.Config.Session>(pSessionData);

      if (lRecords != null && lRecords.Count > 0)
      {
        cRecordList.Clear();

        foreach (Session.Config.Session lReq in lRecords)
          cRecordList.Add(lReq);
      }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionFilePath"></param>
    public void saveSession(String pSessionFilePath)
    {
      cInfrastructure.saveSessionData<Session.Config.Session>(pSessionFilePath, cRecordList);
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
