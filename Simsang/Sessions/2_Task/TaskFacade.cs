using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

using Simsang.Session.Config;


namespace Simsang.Session
{
  public class TaskFacade : IObservable
  {

    #region MEMBERS

    private static TaskFacade cInstance;
    private InfrastructureFacade cInfrastructure;
    private List<AttackSession> cRecords;
    private List<IObserver> cObservers;

    #endregion


    #region PUBLIC

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static TaskFacade getInstance()
    {
      return cInstance ?? (cInstance = new TaskFacade());
    }


    /// <summary>
    /// 
    /// </summary>
    private TaskFacade()
    {
      cRecords = new List<AttackSession>();
      cObservers = new List<IObserver>();
      cInfrastructure = InfrastructureFacade.getInstance();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionName"></param>
    /// <returns></returns>
    public AttackSession getSessionByName(String pSessionName)
    {
      AttackSession lRetVal = null;

      foreach (AttackSession lSess in cInfrastructure.getAllSessions())
      {
        if (pSessionName == lSess.Name)
        {
          lRetVal = lSess;
          break;
        } // if (pSessio...
      } // foreach (Atta...

      return (lRetVal);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionDir"></param>
    /// <returns></returns>
    public List<AttackSession> getAllSessions()
    {
      return (cInfrastructure.getAllSessions());
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionFileName"></param>
    public void removeSession(String pSessionName)
    {
      cInfrastructure.removeSession(pSessionName);
      findAllSessions();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionName"></param>
    public AttackSession loadSession(String pSessionFileName)
    {
      return(cInfrastructure.loadSession(pSessionFileName));
    }


    public void findAllSessions()
    {
      cRecords.Clear();
      cRecords = cInfrastructure.getAllSessions();
      notify();
    }


    /// <summary>
    /// 
    /// </summary>
    public void SaveSessionData(AttackSession pAttackSession)
    {
      cInfrastructure.SaveSessionData(pAttackSession);
      findAllSessions();
    }


    /// <summary>
    /// 
    /// </summary>
    public String readMainSessionData(String pSessionFileName)
    {
      return(cInfrastructure.readMainSessionData(pSessionFileName));
    }


    /// <summary>
    /// 
    /// </summary>
    public void writeSessionExportFile(String pPathSessionFile, String pDataString)
    {
      cInfrastructure.writeSessionExportFile(pPathSessionFile, pDataString);
    }

    #endregion


    #region IOBSERVABLE

    /// <summary>
    /// 
    /// </summary>
    /// <param name="o"></param>
    public void addObserver(IObserver pObserver)
    {
      cObservers.Add(pObserver);
    }


    /// <summary>
    /// 
    /// </summary>
    public void notify()
    {
      foreach (IObserver lTmp in cObservers)
        lTmp.update(cRecords);
    }

    #endregion

  }
}
