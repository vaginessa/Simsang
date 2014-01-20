using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.IO;

using Simsang.Plugin;


namespace Plugin.Main.HTTPInject
{
  public class TaskFacade
  {

    #region MEMBERS

    private static TaskFacade cInstance;
    private DomainFacade cDomain;
    private IPlugin cPlugin;

    #endregion


    #region PUBLIC

    private TaskFacade(InjectionConfig pProxyConfig, IPlugin pPlugin)
    {
      cPlugin = pPlugin;
      cDomain = DomainFacade.getInstance(pProxyConfig, pPlugin);
    }


    /// <summary>
    /// Create single instance
    /// </summary>
    /// <param name="pFWConfig"></param>
    /// <returns></returns>
    public static TaskFacade getInstance(InjectionConfig pProxyConfig, IPlugin pPlugin)
    {
      if (cInstance == null)
        cInstance = new TaskFacade(pProxyConfig, pPlugin);

      return (cInstance);
    }

    #endregion


    #region RECORDS

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pType"></param>
    /// <param name="pRequestedHost"></param>
    /// <param name="pRequestedURL"></param>
    /// <param name="pReplacementHost"></param>
    /// <param name="pReplacementURL"></param>
    /// <param name="pReplacementFullPath"></param>
    public void addRecord(String pType, String pRequestedHost, String pRequestedURL, String pReplacementHost, String pReplacementURL, String pReplacementFullPath)
    {

      if (String.IsNullOrEmpty(pType) || (pType.ToLower() != "inject" && pType.ToLower() != "redirect"))
        throw new Exception("Something is wrong with the injection type.\r\nPossible values are : inject|redirect");
      else if (cDomain.InjectedURLList.Where(s => s.RequestedHost == pRequestedHost && s.RequestedURL == pRequestedURL).Count() > 0)
        throw new Exception(String.Format("A record for this URL already exists."));

      // Redirect 
      else if (pType.ToLower().Contains("redirect") && (String.IsNullOrEmpty(pRequestedHost) || String.IsNullOrWhiteSpace(pRequestedHost) || !Regex.Match(pRequestedHost, @"^[\d\w\-_\.]*[\d\w\-_]+\.[\w]{2,10}$").Success))
        throw new Exception("Something is wrong with the requested URL.\r\nFormat is : www.facebook.com");
      else if (pType.ToLower().Contains("redirect") && (String.IsNullOrEmpty(pRequestedURL) || String.IsNullOrWhiteSpace(pRequestedURL) || !Regex.Match(pRequestedURL, @"^/.*$").Success))
        throw new Exception("Something is wrong with the requested URL.\r\nFormat is : /login.php?user=user1");
      else if (pType.ToLower().Contains("redirect") && (String.IsNullOrEmpty(pReplacementHost) || String.IsNullOrWhiteSpace(pReplacementHost) || !Regex.Match(pReplacementHost, @"^[\d\w\-_\.]*[\d\w\-_]+\.[\w]{2,10}$").Success))
        throw new Exception("Something is wrong with the redirect host name.");
      else if (pType.ToLower().Contains("redirect") && (String.IsNullOrEmpty(pReplacementURL) || String.IsNullOrWhiteSpace(pReplacementURL) || !Regex.Match(pReplacementURL, @"^/.*$").Success))
        throw new Exception("Something is wrong with the redirected URL.\r\nFormat is : /login.php?user=user1");

      // Inject
      else if (pType.ToLower().Contains("inject") && pType.ToLower().Contains("inject") && ((String.IsNullOrEmpty(pReplacementURL) || String.IsNullOrWhiteSpace(pReplacementURL))))
        throw new Exception("Something is wrong with the injected URL.\r\nFormat is : Your_File_Name.pdf");
      else if (pType.ToLower().Contains("inject") && pType.ToLower().Contains("inject") && (!File.Exists(pReplacementFullPath)))
        throw new Exception(String.Format("Injection file doesn't exist"));

      cDomain.addRecord(new InjectedURLRecord(pType, pRequestedHost, pRequestedURL, pReplacementHost, pReplacementURL, pReplacementFullPath));
    }


    /// <summary>
    /// 
    /// </summary>
    public void emptyInjectionList()
    {
      cDomain.clearRecordList();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pRequestedHost"></param>
    /// <param name="pRequestedURL"></param>
    public void removeItemFromList(String pRequestedHost, String pRequestedURL)
    {
      cDomain.removeRecordAt(pRequestedHost, pRequestedURL);
    }

    #endregion


    #region SESSION

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionName"></param>
    public void saveSession(String pSessionName)
    {
      cDomain.saveSession(pSessionName);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionName"></param>
    public void loadSessionData(String pSessionName)
    {
      cDomain.loadSessionData(pSessionName);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionData"></param>
    public void loadSessionDataFromString(String pSessionData)
    {
      cDomain.loadSessionDataFromString(pSessionData);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionName"></param>
    public void deleteSession(String pSessionName)
    {
      cDomain.deleteSession(pSessionName);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionName"></param>
    /// <returns></returns>
    public String getSessionData(String pSessionName)
    {
      return (cDomain.getSessionData(pSessionName));
    }

    #endregion


    #region EVENTS

    /// <summary>
    /// 
    /// </summary>
    public void onStart()
    {
      cDomain.onStart();
    }


    /// <summary>
    /// 
    /// </summary>
    public void onStop()
    {
      cDomain.onStop();
    }


    /// <summary>
    /// 
    /// </summary>
    public void onInit()
    {
      cDomain.onInit();
    }

    #endregion

  }
}
