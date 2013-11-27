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
      ServicesFile = @"\plugins\IPAccounting\Service_Definitions.xml";
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
        XmlSerializer lSerializer;
        FileStream lFS = null;

        try
        {
          lSerializer = new XmlSerializer(typeof(List<ServiceRecord>));
          lFS = new FileStream(ServicesFile, FileMode.Create);
          lSerializer.Serialize(lFS, pRecords);
        }
        finally
        {
          if (lFS != null)
            lFS.Close();
        }
      } // if (pSessi...
    }


    /// <summary>
    /// 
    /// </summary>
    public List<ServiceRecord> readServicesPatterns()
    {
      List<ServiceRecord> lRetVal = null;
      FileStream lFS = null;
      XmlSerializer lXMLSerial;

      try
      {
        lFS = new FileStream(ServicesFile, FileMode.Open);
        lXMLSerial = new XmlSerializer(typeof(List<ServiceRecord>));
        lRetVal = (List<ServiceRecord>)lXMLSerial.Deserialize(lFS);
      }
      finally
      {
        if (lFS != null)
          lFS.Close();
      }

      return (lRetVal);
    }

    #endregion

  }
}
