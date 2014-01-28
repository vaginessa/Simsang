using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Simsang.Plugin;
using Plugin.Main.DNSRequest.Config;

namespace Plugin.Main.DNSRequest
{
  public class DomainFacade : IObservable
  {

    #region MEMBERS

    private static DomainFacade cInstance;
    private List<IObserver> cObserverList;
    private BindingList<DNSRequestRecord> cRecordList;
    private InfrastructureFacade cInfrastructure;
    private const int cMaxTableRows = 128;
    private IPlugin cPlugin;

    #endregion


    #region PROPERTIES

    public BindingList<DNSRequestRecord> RecordList { get { return cRecordList; } private set { } }

    #endregion


    #region PUBLIC

    private DomainFacade(IPlugin pPlugin)
    {
      cPlugin = pPlugin;
      cObserverList = new List<IObserver>();
      cRecordList = new BindingList<DNSRequestRecord>();
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
    /// 
    /// </summary>
    /// <param name="pDNSReq"></param>
    public void addRecord(List<DNSRequestRecord> pDNSReq)
    {
      if (pDNSReq != null && pDNSReq.Count > 0)
      {
        foreach (DNSRequestRecord lTmpReq in pDNSReq)
        {
          if (String.IsNullOrEmpty(lTmpReq.SrcIP))
            throw new Exception("Something is wrong with the source IP.");
          else if (String.IsNullOrEmpty(lTmpReq.DNSHostname))
            throw new Exception("Something is wrong with the source host name.");
          else if (String.IsNullOrEmpty(lTmpReq.PacketType))
            throw new Exception("Something is wrong with the source request type.");

          cRecordList.Add(lTmpReq);
        } // foreach ...


        // Resize the DGV to the defined maximum size. \
        while (cRecordList.Count > cMaxTableRows)
          cRecordList.RemoveAt(0);

        notify();
      } // if (pDNSR
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
    public void loadSessionData(String pSessionFilePath)
    {
      BindingList<DNSRequestRecord> lSessionData = null;

      lSessionData = cInfrastructure.loadSessionData<DNSRequestRecord>(pSessionFilePath);

      if (lSessionData != null && lSessionData.Count > 0)
        foreach (DNSRequestRecord lTmp in lSessionData)
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
      BindingList<DNSRequestRecord> lRecords = cInfrastructure.loadSessionDataFromString<DNSRequestRecord>(pSessionData);

      if (lRecords != null && lRecords.Count > 0)
      {
        cRecordList.Clear();

        foreach (DNSRequestRecord lReq in lRecords)
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


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionFilePath"></param>
    public void saveSession(String pSessionFilePath)
    {
      cInfrastructure.saveSessionData<DNSRequestRecord>(pSessionFilePath, cRecordList);
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
