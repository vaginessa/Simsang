//
//class InfrastructureFacade : IObservable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text.RegularExpressions;


namespace Plugin.Main.Applications.ManageApplications
{
  public class TaskFacade : IObservable
  {

    #region MEMBERS

    private static TaskFacade cInstance;
    private List<ApplicationPattern> cApplicationPatterns;
    private List<IObserver> cObservers;
    private InfrastructureFacade cInfrastructure;

    #endregion


    #region PROPERTIES

    public List<ApplicationPattern> ApplicationRecords { get { return cApplicationPatterns; } }

    #endregion


    #region PUBLIC

    /// <summary>
    /// 
    /// </summary>
    private TaskFacade()
    {
      cApplicationPatterns = new List<ApplicationPattern>();
      cObservers = new List<IObserver>();
      cInfrastructure = InfrastructureFacade.getInstance();
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
    public void readApplicationPatterns()
    {
      List<ApplicationPattern> lApplicationPatterns = null;


      lApplicationPatterns = cInfrastructure.readApplicationPatterns();

      if (lApplicationPatterns != null && lApplicationPatterns.Count > 0)
      {
        cApplicationPatterns.Clear();
        foreach (ApplicationPattern lReq in lApplicationPatterns)
          cApplicationPatterns.Add(lReq);
      }

      notify();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pPattern"></param>
    public void addRecord(ApplicationPattern pRecord)
    {

      if (String.IsNullOrEmpty(pRecord.ApplicationPatternString))
        throw new Exception("You didn't define an application pattern");
      else if (String.IsNullOrEmpty(pRecord.ApplicationName))
        throw new Exception("You didn't define a company name");
      else if (String.IsNullOrEmpty(pRecord.CompanyURL))
        throw new Exception("You didn't define a company web page");
      else
      {
        try { Regex.Match("", pRecord.ApplicationPatternString); }
        catch (ArgumentException lEx) { throw new Exception("URL pattern is invalid"); }
      }

      foreach (ApplicationPattern lTmp in cApplicationPatterns)
        if (lTmp.Equals(pRecord))
          throw new Exception("An entry with this pattern already exists");

      cApplicationPatterns.Add(pRecord);
      cInfrastructure.saveApplicationPatterns(cApplicationPatterns);
      notify();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pPattern"></param>
    public void removeRecord(ApplicationPattern pRecord)
    {
      cApplicationPatterns.Remove(pRecord);
      cInfrastructure.saveApplicationPatterns(cApplicationPatterns);
      notify();
    }

    /// <summary>
    /// 
    /// </summary>
    public void saveApplicationPatterns()
    {
      cInfrastructure.saveApplicationPatterns(cApplicationPatterns);
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
        tmp.update(cApplicationPatterns);
    }

    #endregion

  }
}