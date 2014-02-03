using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

using Plugin.Main.Applications.ManageApplications;
using Simsang.Plugin;


namespace Plugin.Main.Applications
{
  public class InfrastructureFacade
  {

    #region MEMBERS

    private static InfrastructureFacade cInstance;
    private IPlugin cPlugin;

    #endregion


    #region PROPERTIES

    public String ApplicationPatterns { get; private set; }

    #endregion


    #region PUBLIC

    private InfrastructureFacade(IPlugin pPlugin)
    {
      cPlugin = pPlugin;
      ApplicationPatterns = @"\plugins\UsedApps\Plugin_UsedApps_Patterns.xml";

      // Create Session directory if it doesn't exist
      try
      {
        if (!Directory.Exists(cPlugin.Config.SessionDir))
          Directory.CreateDirectory(cPlugin.Config.SessionDir);
      }
      catch (Exception lEx)
      {
      }
    }


    /// <summary>
    /// Create single instance
    /// </summary>
    /// <returns></returns>
    public static InfrastructureFacade getInstance(IPlugin pPlugin)
    {
      if (cInstance == null)
        cInstance = new InfrastructureFacade(pPlugin);

      return (cInstance);
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

    #endregion


    #region SESSION

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="pSessionName"></param>
    /// <returns></returns>
    public List<T> loadSessionData<T>(String pSessionName)
    {
      List<T> lRetVal = null;
      FileStream lFS = null;
      XmlSerializer lXMLSerial;
      String lSessionFilePath = String.Format(@"{0}\{1}.xml", cPlugin.Config.SessionDir, pSessionName);

      try
      {
        lFS = new FileStream(lSessionFilePath, FileMode.Open);
        lXMLSerial = new XmlSerializer(typeof(List<T>));
        lRetVal = (List<T>)lXMLSerial.Deserialize(lFS);
      }
      catch (Exception lEx)
      {
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
    /// <param name="pSessionName"></param>
    public void deleteSession(String pSessionName)
    {
      String lSessionFilePath = String.Format(@"{0}\{1}.xml", cPlugin.Config.SessionDir, pSessionName);

      if (File.Exists(lSessionFilePath))
        File.Delete(lSessionFilePath);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionName"></param>
    /// <returns></returns>
    public String getSessionData(String pSessionName)
    {
      String lRetVal = String.Empty;
      String lSessionFilePath = String.Format(@"{0}\{1}.xml", cPlugin.Config.SessionDir, pSessionName);

      try
      {
        lRetVal = File.ReadAllText(lSessionFilePath);
      }
      catch (Exception)
      { }

      return (lRetVal);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="pSessionName"></param>
    /// <param name="pRecords"></param>
    public void saveSessionData<T>(String pSessionName, List<T> pRecords)
    {
      String lSessionFilePath = String.Format(@"{0}\{1}.xml", cPlugin.Config.SessionDir, pSessionName);

      if (pSessionName.Length > 0)
      {
        XmlSerializer lSerializer;
        FileStream lFS = null;

        try
        {
          lSerializer = new XmlSerializer(pRecords.GetType());
          lFS = new FileStream(lSessionFilePath, FileMode.Create);
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
    /// <typeparam name="T"></typeparam>
    /// <param name="pSessionData"></param>
    /// <returns></returns>
    public List<T> loadSessionDataFromString<T>(String pSessionData)
    {
      List<T> lRecords = new List<T>();
      var lSerializer = new XmlSerializer(typeof(List<T>));

      using (TextReader lTextReader = new StringReader(pSessionData))
      {
        lRecords = (List<T>)lSerializer.Deserialize(lTextReader);
      } // using (TextRe...

      return (lRecords);
    }

    #endregion

  }
}

