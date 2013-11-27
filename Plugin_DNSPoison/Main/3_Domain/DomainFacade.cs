using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Simsang.Plugin;


namespace Plugin.Main.DNSPoison
{
  public class DomainFacade : IObservable
  {

    #region MEMBERS

    private static DomainFacade cInstance;
    private InfrastructureFacade cInfrastructure;
    private BindingList<DNSPoisonRecord> cRecordList;
    private List<IObserver> cObserverList;
    private IPlugin cPlugin;

    #endregion


    #region PROPERTIES

    public BindingList<DNSPoisonRecord> RecordList { get { return cRecordList; } private set { } }

    #endregion


    #region PUBLIC

    private DomainFacade(IPlugin pPlugin)
    {
      cPlugin = pPlugin;
      cObserverList = new List<IObserver>();
      cRecordList = new BindingList<DNSPoisonRecord>();
      cInfrastructure = InfrastructureFacade.getInstance(pPlugin);
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
    /// Add host/IP pair to list
    /// </summary>
    /// <param name="pHostName"></param>
    /// <param name="pIPAddress"></param>
    public void addRecord(String pHostName, String pIPAddress)
    {
      if (!String.IsNullOrEmpty(pHostName) && !String.IsNullOrEmpty(pIPAddress))
      {
        foreach (DNSPoisonRecord lTmp in cRecordList)
          if (lTmp.HostName.ToLower() == pHostName.ToLower())
            throw new Exception(String.Format("An entry with this hostname already exists"));

        cRecordList.Add(new DNSPoisonRecord(pHostName.ToLower(), pIPAddress));
      }
      notify();
    }



    /// <summary>
    /// Remove host/IP pair from list
    /// </summary>
    /// <param name="pHostName"></param>
    public void removeRecordAt(String pHostName)
    {
      /*
       * 1. Create unique ID for record
       */
      foreach (DNSPoisonRecord lTmp in cRecordList)
      {
        if (lTmp.HostName == pHostName)
        {
          cRecordList.Remove(lTmp);
          break;
        }
      }

      notify();
    }


    /// <summary>
    /// 
    /// </summary>
    public void clearRecordList()
    {
      if (cRecordList.Count > 0)
      {
        cRecordList.Clear();
        notify();
      }
    }

    #endregion


    #region SESSION

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionFilePath"></param>
    public void saveSession(String pSessionName)
    {
      cInfrastructure.saveSessionData<DNSPoisonRecord>(pSessionName, cRecordList);
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionFilePath"></param>
    public void loadSessionData(String pSessionFilePath)
    {
      BindingList<DNSPoisonRecord> lSessionData = null;

      lSessionData = cInfrastructure.loadSessionData<DNSPoisonRecord>(pSessionFilePath);

      if (lSessionData != null && lSessionData.Count > 0)
        foreach (DNSPoisonRecord lTmp in lSessionData)
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
      BindingList<DNSPoisonRecord> lRecords = cInfrastructure.loadSessionDataFromString<DNSPoisonRecord>(pSessionData);

      if (lRecords != null && lRecords.Count > 0)
      {
        cRecordList.Clear();

        foreach (DNSPoisonRecord lReq in lRecords)
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
    /// <param name="pSessionFilePath"></param>
    /// <returns></returns>
    public String getSessionData(String pSessionFilePath)
    {
      return (cInfrastructure.getSessionData(pSessionFilePath));
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
