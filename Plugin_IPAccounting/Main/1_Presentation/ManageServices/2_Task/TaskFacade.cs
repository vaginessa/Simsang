using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.ComponentModel;

using Simsang.Plugin;
using Plugin.Main.IPAccounting.Config;


namespace Plugin.Main.IPAccounting.ManageServices
{
  public class TaskFacade
  {

    #region MEMBERS

    private static TaskFacade cInstance;
    private InfrastructureFacade cInfrastructure;
    private List<IObserver> cObservers;
    private List<ServiceRecord> cServiceRecords;

    #endregion


    #region PROPERTIES

    public List<ServiceRecord> ServiceRecords { get { return cServiceRecords; } }

    #endregion


    #region PUBLIC

    /// <summary>
    /// 
    /// </summary>
    private TaskFacade()
    {
      cInfrastructure = InfrastructureFacade.getInstance();
      cServiceRecords = new List<ServiceRecord>();
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
    public void readServicesPatterns()
    {
      cServiceRecords = cInfrastructure.readServicesPatterns();
      notify();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pRecord"></param>
    public void addRecord(ServiceRecord pRecord)
    {
      int lLowerPort;
      int lUpperPort;

      /*
       * Check if parameters are valid
       */
      if (pRecord == null)
        throw new Exception("Something is wrong with the record");
      if (!checkPort(pRecord.LowerPort))
        throw new Exception("The lower port is invalid");
      else if (!checkPort(pRecord.UpperPort))
        throw new Exception("The upper port is invalid");
      else if (String.IsNullOrEmpty(pRecord.ServiceName))
        throw new Exception("The service name is invalid");


      try
      {
        lLowerPort = Int32.Parse(pRecord.LowerPort);
        lUpperPort = Int32.Parse(pRecord.UpperPort);
      }
      catch (Exception)
      {
        throw new Exception("Something is wrong with the lower/upper port definition");
      }

      if (lLowerPort > lUpperPort)
      {
        throw new Exception("Lower port can't be greater than upper port");
      }

      /*
       * Check if service wasnt defined already.
       */
      if (isOverlappingWith(lLowerPort, lUpperPort) != null)
        throw new Exception(String.Format("The port definition is overlapping with an other service"));


      cServiceRecords.Add(pRecord);
      saveServiceRecords();
      notify();
    }


    /// <summary>
    /// 
    /// </summary>
    public void saveServiceRecords()
    {
      cInfrastructure.saveServiceRecords(cServiceRecords);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pRecord"></param>
    public void removeRecord(ServiceRecord pRecord)
    {
      cServiceRecords.Remove(pRecord);
      saveServiceRecords();
      notify();
    }

    #endregion


    #region PRIVATE


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pPort"></param>
    /// <returns></returns>
    private Boolean checkPort(String pPort)
    {
      Boolean lRetVal = true;
      int lPort = 0;

      if (pPort == null || !Regex.Match(pPort, @"^\d+$", RegexOptions.IgnoreCase).Success)
      {
        lRetVal = false;
        goto END;
      }

      try
      {
        lPort = Int32.Parse(pPort);
      }
      catch (Exception)
      {
        lRetVal = false;
        goto END;
      }

      if (lPort < 1 || lPort > 65534)
      {
        lRetVal = false;
        goto END;
      }


    END:

      return (lRetVal);
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="pLowerPort"></param>
    /// <param name="pUpperPort"></param>
    /// <returns></returns>
    private ServiceRecord isOverlappingWith(int pLowerPort, int pUpperPort)
    {
      ManageServices.ServiceRecord lRetVal = null;
      int lTmpLowerPort;
      int lTmpUpperPort;



      if (pLowerPort > 0 && pUpperPort < 65536)
      {
        foreach (ManageServices.ServiceRecord lTmp in cServiceRecords)
        {
          try
          {
            lTmpLowerPort = Int32.Parse(lTmp.LowerPort);
            lTmpUpperPort = Int32.Parse(lTmp.UpperPort);
          }
          catch (Exception)
          {
            continue;
          }


          if (lTmpLowerPort <= pLowerPort && lTmpUpperPort >= pLowerPort)
          {
            lRetVal = lTmp;
            break;
          }
          else if (lTmpLowerPort <= pUpperPort && lTmpUpperPort >= pUpperPort)
          {
            lRetVal = lTmp;
            break;

            // Hiding
          }
          else if (pLowerPort <= lTmpLowerPort && pUpperPort >= lTmpUpperPort)
          {
            lRetVal = lTmp;
            break;
          }
        } // foreach (Servic...
      } // if (pLowerPo...

      return (lRetVal);
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
        tmp.update(cServiceRecords);
    }

    #endregion

  }
}
