using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.ComponentModel;

using Simsang.Plugin;
using Plugin.Main.Systems.Config;


namespace Plugin.Main.Systems.ManageSystems
{
  public class TaskFacade
  {

    #region MEMBERS

    private static TaskFacade cInstance;
    private InfrastructureFacade cInfrastructure;
    private List<IObserver> cObservers;
    private List<SystemPattern> cSystemPattern;

    #endregion


    #region PROPERTIES

    public List<SystemPattern> SystemRecords { get { return cSystemPattern; } }

    #endregion


    #region PUBLIC

    /// <summary>
    /// 
    /// </summary>
    private TaskFacade()
    {
      cInfrastructure = InfrastructureFacade.getInstance();
      cSystemPattern = new List<SystemPattern>();
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
    public void readSystemPatterns()
    {
      cSystemPattern = cInfrastructure.readSystemPatterns();
      notify();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pRecord"></param>
    public void addRecord(SystemPattern pRecord)
    {
      if (String.IsNullOrEmpty(pRecord.SystemName))
        throw new Exception("You didn't define a system name");
      else if (String.IsNullOrEmpty(pRecord.SystemPatternString))
        throw new Exception("You didn't define a system pattern");

      /*
       * Throw exception if record already exists
       */
      if (cSystemPattern != null)      
        foreach (SystemPattern lTmp in cSystemPattern)
          if (lTmp.Equals(pRecord))
            throw new Exception("An entry with this pattern already exists");

      try { Regex.Match("", pRecord.SystemPatternString); }
      catch (ArgumentException)
      {
        throw new Exception("URL pattern is invalid"); 
      }

      cSystemPattern.Add(pRecord);
      saveSystemRecords();
      notify();
    }


    /// <summary>
    /// 
    /// </summary>
    public void saveSystemRecords()
    {
      cInfrastructure.saveSystemPatterns(cSystemPattern);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pRecord"></param>
    public void removeRecord(SystemPattern pRecord)
    {
      cSystemPattern.Remove(pRecord);
      saveSystemRecords();
      notify();
    }

    #endregion


    #region OBSERVABLE

    public void addObserver(IObserver o)
    {
      if (o != null)
        cObservers.Add(o);
    }

    public void notify()
    {
      foreach (IObserver tmp in cObservers)
        tmp.update(cSystemPattern);
    }

    #endregion

  }
}
