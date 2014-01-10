using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;


namespace Plugin.Main.Applications.ManageApplications
{
  public class InfrastructureFacade
  {

    #region MEMBERS

    private static InfrastructureFacade cInstance;

    #endregion


    #region PROPERTIES

    public String ApplicationPatterns { get; private set; }

    #endregion


    #region PUBLIC

    /// <summary>
    /// 
    /// </summary>
    private InfrastructureFacade()
    {
      ApplicationPatterns = @"\plugins\UsedApps\Plugin_UsedApps_Patterns.xml";
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
    public List<ApplicationPattern> readApplicationPatterns()
    {
      List<ApplicationPattern> lRetVal = null;
      FileStream lFS = null;
      XmlSerializer lXMLSerial;

      try
      {
        String lPatternFile = String.Format("{0}{1}", Directory.GetCurrentDirectory(), ApplicationPatterns);

        lFS = new FileStream(lPatternFile, FileMode.Open);
        lXMLSerial = new XmlSerializer(typeof(List<ApplicationPattern>));
        lRetVal = (List<ApplicationPattern>)lXMLSerial.Deserialize(lFS);
      }
      finally
      {
        if (lFS != null)
          lFS.Close();
      }

      return (lRetVal);
    }

    /// <summary>
    /// 
    /// </summary>
    public void saveApplicationPatterns(List<ApplicationPattern> pRecords)
    {
      if (pRecords.Count > 0)
      {
        XmlSerializer lSerializer;
        FileStream lFS = null;
        String lPatternFile = String.Format("{0}{1}", Directory.GetCurrentDirectory(), ApplicationPatterns);

        try
        {
          lSerializer = new XmlSerializer(typeof(List<ApplicationPattern>));
          lFS = new FileStream(lPatternFile, FileMode.Create);
          lSerializer.Serialize(lFS, pRecords);
        }
        finally
        {
          if (lFS != null)
            lFS.Close();
        }
      } // if (pSessi...
    }

    #endregion

  }
}