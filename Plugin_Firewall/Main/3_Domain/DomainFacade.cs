using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Simsang.Plugin;


namespace Plugin.Main.Firewall
{
  public class DomainFacade : IObservable
  {

    #region MEMBERS

    private static DomainFacade cInstance;
    private BindingList<FWRule> cRecordList;
    private List<IObserver> cObservers;
    private InfrastructureFacade cInfrastructure;
    private IPlugin cPlugin;

    #endregion


    #region PROPERTIES

    public BindingList<FWRule> FirewallRuleList { get { return cRecordList; } private set { } }

    #endregion


    #region PUBLIC 

    private DomainFacade(IPlugin pPlugin)
    {
      cPlugin = pPlugin;
      cRecordList = new BindingList<FWRule>();
      cObservers = new List<IObserver>();
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
    /// Add firewall rule to list
    /// </summary>
    /// <param name="pFWRule"></param>
    public void addRecord(FWRule pFWRule)
    {
      if (pFWRule != null)
      {
        foreach (FWRule lTmp in cRecordList)
          if (lTmp.ID == pFWRule.ID)
            throw new Exception("Firewall rule alrady exists.");

        cRecordList.Add(pFWRule);
      }
      notify();
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="pFWRuleID"></param>
    public void removeRecordAt(String pFWRuleID)
    {
      try
      {
        if (cRecordList != null && cRecordList.Count > 0)
          foreach (FWRule lTmp in cRecordList.ToList())
            if (lTmp.ID == pFWRuleID)
              cRecordList.Remove(lTmp);

        notify();
      }
      catch (Exception)
      {
      }
    }



    /// <summary>
    /// 
    /// </summary>
    public void clearRecordList()
    {
      cRecordList.Clear();
      notify();
    }

    #endregion


    #region SESSION

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionName"></param>
    /// <returns></returns>
    public String getSessionData(String pSessionName)
    {
      return (cInfrastructure.getSessionData(pSessionName));
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionName"></param>
    public void deleteSession(String pSessionName)
    {
      cInfrastructure.deleteSession(pSessionName);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionName"></param>
    public void loadSessionData(String pSessionName)
    {
      BindingList<FWRule> lSessionData = null;

      lSessionData = cInfrastructure.loadSessionData<FWRule>(pSessionName);

      if (lSessionData != null && lSessionData.Count > 0)
        foreach (FWRule lTmp in lSessionData)
          cRecordList.Add(lTmp);

      notify();
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionData"></param>
    public void loadSessionDataFromString(String pSessionData)
    {
      BindingList<FWRule> lRecords = cInfrastructure.loadSessionDataFromString<FWRule>(pSessionData);

      if (lRecords != null && lRecords.Count > 0)
      {
        cRecordList.Clear();

        foreach (FWRule lReq in lRecords)
          cRecordList.Add(lReq);
      }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionName"></param>
    public void saveSession(String pSessionName)
    {
      cInfrastructure.saveSessionData<FWRule>(pSessionName, cRecordList);
    }

    #endregion


    #region EVENTS

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pFWRulesPath"></param>
    public void onStart(String pFWRulesPath)
    {
      cInfrastructure.onStart(cRecordList.ToList(), pFWRulesPath);
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="pFWRulesPath"></param>
    public void onStop(String pFWRulesPath)
    {
      cInfrastructure.onStop(pFWRulesPath);
    }

    #endregion


    #region OBSERVABLE INTERFACE METHODS

    public void addObserver(IObserver o)
    {
      cObservers.Add(o);
    }

    public void notify()
    {
      foreach (IObserver lTmp in cObservers)
        lTmp.update(cRecordList);
    }

    #endregion

  }
}
