using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Plugin.Main.Systems;
using Plugin.Main.Systems.Config;

using Simsang.Plugin;


namespace Plugin.Main.Systems
{
  public class DomainFacade : IObservable
  {

    #region MEMBERS

    private static DomainFacade cInstance;
    private InfrastructureFacade cInfrastructure;
    private List<SystemRecord> cRecordList;
    private List<IObserver> cObserverList;
    private IPlugin cPlugin;

    #endregion


    #region PROPERTIES

    public List<SystemRecord> RecordList { get { return (cRecordList); } private set { } }

    #endregion


    #region PUBLIC

    private DomainFacade(IPlugin pPlugin)
    {
      cPlugin = pPlugin;
      cInfrastructure = InfrastructureFacade.getInstance(pPlugin);
      cRecordList = new List<SystemRecord>();
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
    public void addRecord(SystemRecord pRecord)
    {
      if (pRecord != null)
      {
        // Check if record already exists
        foreach (SystemRecord lTmp in cRecordList)
          if (lTmp.ID == pRecord.ID)
            throw new RecordException("System alrady exists.");

        if (!Regex.Match(pRecord.SrcMAC.Trim(), @"^[\da-f]{1,2}[\-:][\da-f]{1,2}[\-:][\da-f]{1,2}[\-:][\da-f]{1,2}[\-:][\da-f]{1,2}[\-:][\da-f]{1,2}$", RegexOptions.IgnoreCase).Success)
          throw new RecordException("Something is wrong with the MAC address");

        if (!Regex.Match(pRecord.SrcIP.Trim(), @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$", RegexOptions.IgnoreCase).Success)
          throw new RecordException("Something is wrong with the IP address");

        cRecordList.Add(pRecord);
        notify();
      }
    }

    #endregion


    #region SESSION

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionFilePath"></param>
    public void saveSession(String pSessionFilePath)
    {
      cInfrastructure.saveSessionData<SystemRecord>(pSessionFilePath, cRecordList);
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionFilePath"></param>
    public void loadSessionData(String pSessionFilePath)
    {
      List<SystemRecord> lSessionData = null;

      lSessionData = cInfrastructure.loadSessionData<SystemRecord>(pSessionFilePath);

      if (lSessionData != null && lSessionData.Count > 0)
        foreach (SystemRecord lTmp in lSessionData)
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
      BindingList<SystemRecord> lRecords = cInfrastructure.loadSessionDataFromString<SystemRecord>(pSessionData);

      if (lRecords != null && lRecords.Count > 0)
      {
        cRecordList.Clear();

        foreach (SystemRecord lReq in lRecords)
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
