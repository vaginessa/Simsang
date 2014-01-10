using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

using Plugin.Main.Session.ManageSessions.Config;


namespace Plugin.Main.Session.ManageSessions
{
  public class InfrastructureFacade
  {

    #region MEMBERS

    private static InfrastructureFacade cInstance;

    #endregion


    #region PROPERTIES

    public String SessionPatternsFile { get; private set; }

    #endregion


    #region PUBLIC

    /// <summary>
    /// 
    /// </summary>
    private InfrastructureFacade()
    {
      SessionPatternsFile = @"\plugins\Sessions\Plugin_Session_Patterns.xml";
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
    public List<SessionPattern> readSessionPatterns()
    {
      FileStream lFS = null;
      XmlSerializer lXMLSerial;
      List<SessionPattern> lSessionPatterns = new List<SessionPattern>();

      try
      {
        String lPatternFilePath = String.Format(@"{0}{1}", Directory.GetCurrentDirectory(), SessionPatternsFile);
        lFS = new FileStream(lPatternFilePath, FileMode.Open);
        lXMLSerial = new XmlSerializer(typeof(List<SessionPattern>));
        lSessionPatterns = (List<SessionPattern>) lXMLSerial.Deserialize(lFS);
      }
      finally
      {
        if (lFS != null)
          lFS.Close();
      }

      return (lSessionPatterns);
    }


    /// <summary>
    /// 
    /// </summary>
    public void saveSessionPatterns(List<SessionPattern> pRecords)
    {
      if (pRecords.Count > 0)
      {
        XmlSerializer lSerializer;
        FileStream lFS = null;

        try
        {
          String lPatternFile = String.Format("{0}{1}", Directory.GetCurrentDirectory(), SessionPatternsFile);

          lSerializer = new XmlSerializer(pRecords.GetType());
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



