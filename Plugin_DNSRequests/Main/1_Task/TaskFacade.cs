using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Simsang.Plugin;
using Plugin.Main.DNSRequest.Config;

namespace Plugin.Main.DNSRequest
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


    #region REQUEST

    /// <summary>
    ///
    /// </summary>
    /// <param name="pDNSReq"></param>
    public void addRecord(List<DNSRequestRecord> pDNSReq)
    {
      cDomain.addRecord(pDNSReq);
    }


    /// <summary>
    /// 
    /// </summary>
    public void clearRecordList()
    {
      cDomain.clearRecordList();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pIndex"></param>
    public void removeRecordAt(int pIndex)
    {
      cDomain.removeRecordAt(pIndex);
    }

    #endregion


    #region SESSION

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
    /// <param name="pSessionFile"></param>
    public void loadSessionData(String pSessionFile)
    {
      cDomain.loadSessionData(pSessionFile);
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
    /// <param name="pSessionFilePath"></param>
    /// <returns></returns>
    public String getSessionData(String pSessionFilePath)
    {
      return (cDomain.getSessionData(pSessionFilePath));
    }

    #endregion

  }
}
