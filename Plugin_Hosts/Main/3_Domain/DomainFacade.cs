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
  public class DomainFacade : IRecordObservable
  {

    #region MEMBERS

    private static DomainFacade cInstance;
    private InfrastructureFacade cInfrastructure;
    private List<SystemRecord> cRecordList;
    private List<ManageSystems.SystemPattern> cSystemPatternList;
    private List<IRecordObserver> cRecordObserverList;
    private List<ISystemPatternObserver> cSystemPatternObserverList;
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
      cSystemPatternList = new List<ManageSystems.SystemPattern>();
      cRecordObserverList = new List<IRecordObserver>();
      cSystemPatternObserverList = new List<ISystemPatternObserver>();
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
    public void readSystemPatterns()
    {
      cSystemPatternList = cInfrastructure.readSystemPatterns();
      notifySystemPatterns();
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
        notifyRecords();
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
        notifyRecords();
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
            throw new RecordExistsException("System alrady exists.");

        if (!Regex.Match(pRecord.SrcMAC.Trim(), @"^[\da-f]{1,2}[\-:][\da-f]{1,2}[\-:][\da-f]{1,2}[\-:][\da-f]{1,2}[\-:][\da-f]{1,2}[\-:][\da-f]{1,2}$", RegexOptions.IgnoreCase).Success)
          throw new RecordException("Something is wrong with the MAC address");

        if (!Regex.Match(pRecord.SrcIP.Trim(), @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$", RegexOptions.IgnoreCase).Success)
          throw new RecordException("Something is wrong with the IP address");

        cRecordList.Add(pRecord);
        notifyRecords();
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
      notifyRecords();
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
    public void addRecordObserver(IRecordObserver o)
    {
      cRecordObserverList.Add(o);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="o"></param>
    public void addSystemPatternObserver(ISystemPatternObserver o)
    {
      cSystemPatternObserverList.Add(o);
    }


    /// <summary>
    /// 
    /// </summary>
    public void notifyRecords()
    {
      foreach (IRecordObserver lObs in cRecordObserverList)
        lObs.updateRecordList(cRecordList);
    }


    /// <summary>
    /// 
    /// </summary>
    public void notifySystemPatterns()
    {
      foreach (ISystemPatternObserver lObs in cSystemPatternObserverList)
        lObs.updateSystemPatternList(cSystemPatternList);      
    }

    #endregion

  }
}
