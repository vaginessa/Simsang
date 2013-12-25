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
          String lPatternFile = String.Format("{0}{1}", Directory.GetCurrentDirectory(), SystemPatternFile);

//          lFS = new FileStream(lPatternFile, FileMode.Open);
          lSerializer = new XmlSerializer(pSystemPatterns.GetType());
          lFS = new FileStream(lPatternFile, FileMode.Create);
          lSerializer.Serialize(lFS, pSystemPatterns);
        }
        catch (Exception lEx)
        {
          String lErrorMsg = lEx.Message;
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
        String lPatternFile = String.Format("{0}{1}", Directory.GetCurrentDirectory(), SystemPatternFile);

        lFS = new FileStream(lPatternFile, FileMode.Open);
        lXMLSerial = new XmlSerializer(typeof(List<SystemPattern>));
        lSystemPatterns = (List<SystemPattern>)lXMLSerial.Deserialize(lFS);
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



