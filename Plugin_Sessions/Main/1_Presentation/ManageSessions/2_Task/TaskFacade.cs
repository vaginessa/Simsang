using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using System.IO;

using Plugin.Main.Session.ManageSessions.Config;


namespace Plugin.Main.Session.ManageSessions
{
  public class TaskFacade : IObservable
  {

    #region MEMBERS

    private static TaskFacade cInstance;
    private InfrastructureFacade cInfrastructure;
    private List<SessionPattern> cSessionPatterns;
    private List<IObserver> cObservers;

    #endregion


    #region PROPERTIES

    public List<SessionPattern> SessionPatterns { get { return cSessionPatterns; } private set { } }

    #endregion


    #region PUBLIC

    /// <summary>
    /// 
    /// </summary>
    private TaskFacade()
    {
      cInfrastructure = InfrastructureFacade.getInstance();
      cSessionPatterns = new List<SessionPattern>();
      cObservers = new List<IObserver>();
    }


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
    public void readSessionPatterns()
    {
      cSessionPatterns = cInfrastructure.readSessionPatterns();
      notify();
    }


    /// <summary>
    /// 
    /// </summary>
    public void saveSessionPatterns()
    {
      cInfrastructure.saveSessionPatterns(cSessionPatterns);
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="pPattern"></param>
    public void addRecord(SessionPattern pRecord)
    {
      if (pRecord == null)
        throw new Exception("Something is wrong with the record");
      else if (String.IsNullOrEmpty(pRecord.SessionName))
        throw new Exception("You didn't define a session name");
      else if (String.IsNullOrEmpty(pRecord.HTTPHost))
        throw new Exception("You didn't define a HTTP Host regex");
      else if (String.IsNullOrEmpty(pRecord.Webpage))
        throw new Exception("You didn't define a company web page");
      else if (String.IsNullOrEmpty(pRecord.SessionPatternString))
        throw new Exception("You didn't define a session cookie regex");

      /*
       * Check Host and Cookie regex
       */
      try 
      {
        new Regex(@pRecord.SessionPatternString); 
      }
      catch (ArgumentException)
      {
        throw new Exception("Session cookies regex is invalid");
      }

      foreach (SessionPattern lTmp in cSessionPatterns)
        if (lTmp.Equals(pRecord))
          throw new Exception("This record already exists");


      cSessionPatterns.Add(pRecord);
      saveSessionPatterns();
      notify();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pPattern"></param>
    public void removeRecord(SessionPattern pRecord)
    {
      cSessionPatterns.Remove(pRecord);
      saveSessionPatterns();
      notify();
    }

    #endregion


    #region PRIVATE

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionName"></param>
    /// <param name="pHostPattern"></param>
    /// <param name="pCookiesPattern"></param>
    /// <returns></returns>
    private bool patternExists(String pSessionName, String pHostPattern, String pCookiesPattern)
    {
      bool lRetVal = false;

      foreach (SessionPattern lTmp in cSessionPatterns)
      {
        if (lTmp.SessionName.ToLower() == pSessionName.ToLower())
        {
          lRetVal = true;
          break;
        }
        else if (!String.IsNullOrEmpty(pHostPattern) && !String.IsNullOrEmpty(pCookiesPattern) &&
                 lTmp.HTTPHost.ToLower() == pHostPattern.ToLower() && lTmp.SessionPatternString.ToLower() == pCookiesPattern.ToLower())
        {
          lRetVal = true;
          break;
        }
      }

      return (lRetVal);
    }

    #endregion


    #region Observable

    /// <summary>
    /// 
    /// </summary>
    /// <param name="o"></param>
    public void addObserver(IObserver o)
    {
      if (o != null)
        cObservers.Add(o);
    }


    /// <summary>
    /// 
    /// </summary>
    public void notify()
    {
      foreach (IObserver tmp in cObservers)
        tmp.update(cSessionPatterns);
    }

    #endregion

  }
}



