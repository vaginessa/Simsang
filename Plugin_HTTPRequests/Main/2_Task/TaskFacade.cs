using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Simsang.Plugin;


namespace Plugin.Main.HTTPRequest
{
  public class TaskFacade
  {

    #region MEMBERS

    private static TaskFacade cInstance;
    private DomainFacade cDomain;
    private IPlugin cPlugin;

    #endregion


    #region PUBLIC

    private TaskFacade(IPlugin pPlugin)
    {
      cPlugin = pPlugin;
      cDomain = DomainFacade.getInstance(pPlugin);
    }


    /// <summary>
    /// Create single instance
    /// </summary>
    /// <returns></returns>
    public static TaskFacade getInstance(IPlugin pPlugin)
    {
      if (cInstance == null)
        cInstance = new TaskFacade(pPlugin);

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
      cDomain.addRecords(pRecords);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pIndex"></param>
    public void removeElementAt(int pIndex)
    {
      cDomain.removeElementAt(pIndex);
    }


    /// <summary>
    /// 
    /// </summary>
    public void emptyRequestList()
    {
      cDomain.emptyRequestList();
    }

    #endregion


    #region SESSIONS


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionFilePath"></param>
    public void saveSession(String pSessionFilePath)
    {
      cDomain.saveSession(pSessionFilePath);
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionFilePath"></param>
    public void loadSessionData(String pSessionFilePath)
    {
      cDomain.loadSessionData(pSessionFilePath);
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
    /// <param name="pSessionFilePath"></param>
    public void deleteSession(String pSessionFilePath)
    {
      cDomain.deleteSession(pSessionFilePath);
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

  }
}
