using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Simsang.Plugin;
using Plugin.Main.Session;
using Plugin.Main.Session.Config;
using Plugin.Main.Session.ManageSessions.Config;

namespace Plugin.Main.Session
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


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public List<SessionPattern> readSessionPatterns()
    {
      return (cDomain.readSessionPatterns());
    }

    #endregion


    #region RECORDS

    /// <summary>
    /// 
    /// </summary>
    public void emptyRecordList()
    {
      cDomain.emptyRecordList();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pRecord"></param>
    public void addRecord(Session.Config.Session pRecord)
    {
      cDomain.addRecord(pRecord);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pIndex"></param>
    public void removeElementAt(int pIndex)
    {
      cDomain.removeElementAt(pIndex);
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
      return (cDomain.getSessionData(pSessionFilePath));
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
    public void saveSession(String pSessionFilePath)
    {
      cDomain.saveSession(pSessionFilePath);
    }

    #endregion

  }
}
