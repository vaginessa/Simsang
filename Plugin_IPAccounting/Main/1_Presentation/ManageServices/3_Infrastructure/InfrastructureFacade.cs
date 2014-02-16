using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text.RegularExpressions;


namespace Plugin.Main.IPAccounting.ManageServices
{
  public class InfrastructureFacade
  {

    #region MEMBERS

    private static InfrastructureFacade cInstance;

    #endregion


    #region PROPERTIES

    public String ServicesFile { get; private set; }

    #endregion


    #region PUBLIC

    /// <summary>
    /// 
    /// </summary>
    private InfrastructureFacade()
    {
      ServicesFile = String.Format(@"{0}\{1}", Directory.GetCurrentDirectory(), @"\plugins\IPAccounting\Service_Definitions.txt");
    }



    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static InfrastructureFacade getInstance()
    {
      return cInstance ?? (cInstance = new InfrastructureFacade());
    }



    /// <summary>
    /// 
    /// </summary>
    public void saveServiceRecords(List<ServiceRecord> pRecords)
    {
      if (pRecords.Count > 0)
      {
        String[] lServices = new String[pRecords.Count];
        for (int i = 0; i < pRecords.Count; i++)
        { 
          ServiceRecord lTmp = pRecords.ElementAt(i);
          lServices[i] = String.Format("{0},{1},{2}", lTmp.LowerPort, lTmp.UpperPort, lTmp.ServiceName);
        } // foreach (Serv...

        File.WriteAllLines(ServicesFile, lServices);

      } // if (pSessi...
    }


    /// <summary>
    /// 
    /// </summary>
    public List<ServiceRecord> readServicesPatterns()
    {
      List<ServiceRecord> lRetVal = new List<ServiceRecord>();

      try
      {
        foreach (String lLine in File.ReadAllLines(ServicesFile))
        {
          if (lLine.Contains(','))
          {
            String[] lSplit = lLine.Split(new char[] { ',' }, 3);
            lRetVal.Add(new ServiceRecord() { LowerPort = lSplit[0], UpperPort = lSplit[1], ServiceName = lSplit[2] });
          } // if (lLi...
        } // foreach (St...
      }
      catch (Exception lEx)
      {
        String lErr = lEx.Message;
      }

      return (lRetVal);
    }

    #endregion

  }
}
