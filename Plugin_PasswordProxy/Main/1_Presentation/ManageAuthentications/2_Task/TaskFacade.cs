using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;


namespace Plugin.Main.HTTPProxy.ManageAuthentications
{
  public class TaskFacade : IObservable
  {

    #region MEMBERS

    private static TaskFacade cInstance;
    private InfrastructureFacade cInfrastructure;
    private List<IObserver> cObservers;
    private List<AccountPattern> cAccountPatterns;

    #endregion


    #region PROPERTIES

    public List<AccountPattern> AccountPatterns { get { return cAccountPatterns; } }

    #endregion


    #region PUBLIC

    /// <summary>
    /// 
    /// </summary>
    private TaskFacade()
    {
      cInfrastructure = InfrastructureFacade.getInstance();
      cAccountPatterns = new List<AccountPattern>();
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
    public void readAccountsPatterns()
    {
      cAccountPatterns = cInfrastructure.readAccountsPatterns();
      notify();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pRecord"></param>
    public void addRecord(AccountPattern pRecord)
    {
      if (pRecord == null)
        throw new Exception("Something is wrong with the account pattern record.");
      else if (pRecord.Method == null || pRecord.Method.Length <= 0)
        throw new Exception("You didn't define a request method");
      else if (pRecord.Host == null || pRecord.Host.Length <= 0)
        throw new Exception("You didn't define a host pattern");
      else if (pRecord.Path == null || pRecord.Path.Length <= 0)
        throw new Exception("You didn't define a path pattern");
      else if (pRecord.DataPattern == null || pRecord.DataPattern.Length <= 0)
        throw new Exception("You didn't define a data pattern");
      else if (pRecord.Company == null || pRecord.Company.Length <= 0)
        throw new Exception("You didn't define a company name");
      else if (pRecord.WebPage == null || pRecord.WebPage.Length <= 0)
        throw new Exception("You didn't define a company web page");
      else
      {
        try { Regex.Match("", pRecord.Host); }
        catch (ArgumentException)
        {
          throw new Exception("Host pattern is invalid");
        }

        try { Regex.Match("", pRecord.Path); }
        catch (ArgumentException)
        {
          throw new Exception("Path pattern is invalid");
        }

        try { Regex.Match("", pRecord.DataPattern); }
        catch (ArgumentException)
        {
          throw new Exception("Data pattern is invalid");
        }
      }

      if (cAccountPatterns.Count > 0)
        foreach (AccountPattern lTmp in cAccountPatterns)
          if (lTmp.Equals(pRecord))
            throw new Exception("This record already exists.");

      cAccountPatterns.Add(pRecord);
      notify();
    }


    /// <summary>
    /// 
    /// </summary>
    public void saveAccountPatterns()
    {
      cInfrastructure.saveAccountPatterns(cAccountPatterns);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pRecord"></param>
    public void removeRecord(AccountPattern pRecord)
    {
      cAccountPatterns.Remove(pRecord);
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
        tmp.update(cAccountPatterns);
    }

    #endregion

  }
}
