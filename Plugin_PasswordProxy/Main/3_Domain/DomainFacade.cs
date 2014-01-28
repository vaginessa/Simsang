using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Simsang.Plugin;
using Plugin.Main.HTTPProxy.Config;


namespace Plugin.Main.HTTPProxy
{
  public class DomainFacade
  {

    #region MEMBERS

    private static DomainFacade cInstance;
    private InfrastructureFacade cInfrastructure;
    private const int cMaxTableRows = 128;
    private List<IObserver> cObserverList;
    private List<Account> cRecordList;
    private String cRedirectURL;
    private String cRemoteHostName;
    private IPlugin cPlugin;

    #endregion


    #region PROPERTIES

    public List<Account> RecordList { get { return (cRecordList); } private set { } }

    #endregion


    #region PUBLIC

    private DomainFacade(WebServerConfig pWebServerConfig, IPlugin pPlugin)
    {
      cPlugin = pPlugin;
      cInfrastructure = InfrastructureFacade.getInstance(pWebServerConfig, pPlugin);
      cObserverList = new List<IObserver>();
      cRecordList = new List<Account>();

      cRedirectURL = String.Empty;
      cRemoteHostName = String.Empty;
    }


    /// <summary>
    /// Create single instance
    /// </summary>
    /// <param name="pWebServerConfig"></param>
    /// <returns></returns>
    public static DomainFacade getInstance(WebServerConfig pWebServerConfig, IPlugin pPlugin)
    {
      if (cInstance == null)
        cInstance = new DomainFacade(pWebServerConfig, pPlugin);

      return (cInstance);
    }



    /// <summary>
    /// 
    /// </summary>
    public List<ManageAuthentications.AccountPattern> readAuthenticationPatterns()
    {
      return(cInfrastructure.readAuthenticationPatterns());
    }

    #endregion


    #region RECORDS

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pRecord"></param>
    public void addRecord(List<Account> pRecords)
    {
      if (pRecords != null && pRecords.Count > 0)
      {
        foreach (Account lNewAccount in pRecords)
        {
          foreach (Account lTmp in cRecordList)
            if (lTmp.DstIP == lNewAccount.DstIP && lTmp.DstPort == lNewAccount.DstPort && lTmp.Username == lNewAccount.Username && lTmp.Password == lNewAccount.Password)
              throw new Exception("Account record already exists.");

          cRecordList.Add(lNewAccount);
        }

        // Resize the DGV to the defined maximum size.
        while (cRecordList.Count > cMaxTableRows)
          cRecordList.RemoveAt(0);

        notifyRecords();
      }
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="pIndex"></param>
    public void removeRecordAt(int pIndex)
    {
      cRecordList.RemoveAt(pIndex);
      notifyRecords();
    }


    /// <summary>
    /// 
    /// </summary>
    public void clearRecordList()
    {
      cRecordList.Clear();
      notifyRecords();
    }

    #endregion


    #region SESSION

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionFilePath"></param>
    /// <param name="pRemoteHost"></param>
    /// <param name="pRedirectURL"></param>
    public void saveSession(String pSessionName, String pRemoteHost, String pRedirectURL)
    {
      PluginData lPluginData;

      cRemoteHostName = pRemoteHost;
      cRedirectURL = pRedirectURL;

      lPluginData.RemoteHost = pRemoteHost;
      lPluginData.RedirectURL = pRedirectURL;
      lPluginData.Records = cRecordList;

      cInfrastructure.saveSessionData<PluginData>(pSessionName, lPluginData);
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionFilePath"></param>
    public void loadSessionData(String pSessionFilePath)
    {
      PluginData lSessionData = cInfrastructure.loadSessionData<PluginData>(pSessionFilePath);

      if (lSessionData.Records != null && lSessionData.Records.Count > 0)
        foreach (Account lTmp in lSessionData.Records)
          cRecordList.Add(lTmp);

      // Notify all observers
      cRedirectURL = lSessionData.RedirectURL;
      cRemoteHostName = lSessionData.RemoteHost;

      notifyRecords();
      notifyRedirectURL();
      notifyRemoteHostName();
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionData"></param>
    public void loadSessionDataFromString(String pSessionData)
    {
      PluginData lSessionData = cInfrastructure.loadSessionDataFromString<PluginData>(pSessionData);

      if (lSessionData.Records != null && lSessionData.Records.Count > 0)
      {
        cRecordList.Clear();

        foreach (Account lReq in lSessionData.Records)
          cRecordList.Add(lReq);

        cRedirectURL = lSessionData.RedirectURL;
        cRemoteHostName = lSessionData.RemoteHost;

        notifyRecords();
        notifyRedirectURL();
        notifyRemoteHostName();
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


    #region EVENTS

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pWebServerConfig"></param>
    public void onInit(WebServerConfig pWebServerConfig)
    {
      cInfrastructure.onInit(pWebServerConfig);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pConfig"></param>
    public void startProxies(WebServerConfig pConfig)
    {
      cInfrastructure.startProxies(pConfig);
    }


    /// <summary>
    /// 
    /// </summary>
    public void onStop()
    {
      cInfrastructure.onStop();
    }

    #endregion


    #region OBSERVABLE INTERFACE METHODS

    public void addObserver(IObserver o)
    {
      cObserverList.Add(o);
    }


    public void notifyRecords()
    {
      foreach (IObserver lObs in cObserverList)
        lObs.updateRecords(cRecordList);
    }

    public void notifyRemoteHostName()
    {
      foreach (IObserver lObs in cObserverList)
        lObs.updateRemoteHostName(cRemoteHostName);
    }


    void notifyRedirectURL()
    {
      foreach (IObserver lObs in cObserverList)
        lObs.updateRedirectURL(cRedirectURL);
    }

    #endregion

  }
}
