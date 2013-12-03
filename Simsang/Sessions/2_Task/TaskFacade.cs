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
  public class TaskFacade
  {

    #region MEMBERS

    private static TaskFacade cInstance;
    private InfrastructureFacade cInfrastructure;

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
      return(cInfrastructure.getAllSessions());
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionFileName"></param>
    public void removeSession(String pSessionName)
    {
      cInfrastructure.removeSession(pSessionName);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionName"></param>
    public AttackSession loadSession(String pSessionFileName)
    {
      return(cInfrastructure.loadSession(pSessionFileName));
    }



    /// <summary>
    /// 
    /// </summary>
    public void SaveSessionData(AttackSession pAttackSession)
    {
      cInfrastructure.SaveSessionData(pAttackSession);
    }

    #endregion

  }
}
