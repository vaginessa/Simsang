using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;


namespace Plugin.Main.Systems.ManageSystems
{
  public class InfrastructureFacade
  {

    #region MEMBERS

    private static InfrastructureFacade cInstance;

    #endregion


    #region PROPERTIES

    public String SystemPatternFile { get; private set; }

    #endregion


    #region PUBLIC

    /// <summary>
    /// 
    /// </summary>
    private InfrastructureFacade()
    {
      SystemPatternFile = @"\plugins\Systems\Plugin_SystemsOS_Patterns.xml";
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
    public void saveSystemPatterns(List<SystemPattern> pSystemPatterns)
    {
      if (pSystemPatterns.Count > 0)
      {
        XmlSerializer lSerializer;
        FileStream lFS = null;

        try
        {
          lSerializer = new XmlSerializer(pSystemPatterns.GetType());
          lFS = new FileStream(SystemPatternFile, FileMode.Create);
          lSerializer.Serialize(lFS, pSystemPatterns);
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
    public List<SystemPattern> readSystemPatterns()
    {
      List<SystemPattern> lSystemPatterns = null;
      FileStream lFS = null;
      XmlSerializer lXMLSerial;

      try
      {
        lFS = new FileStream(SystemPatternFile, FileMode.Open);
        lXMLSerial = new XmlSerializer(typeof(List<SystemPattern>));
        lSystemPatterns = (List<SystemPattern>) lXMLSerial.Deserialize(lFS);
      }
      finally
      {
        if (lFS != null)
          lFS.Close();
      }

      return(lSystemPatterns);
    }

    #endregion

  }
}



