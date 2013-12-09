using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO;
using System.Configuration;
using System.Text.RegularExpressions;

using Simsang.Session.Config;


namespace Simsang.Session
{
  public class InfrastructureFacade
  {

    #region MEMBERS

    private static InfrastructureFacade cInstance;
    private String mSessionDir;

    #endregion


    #region PUBLIC

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
    /// <param name="pSessionFileName"></param>
    public void removeSession(String pSessionFileName)
    {
      String lFileName = String.Empty;
      try
      {
        lFileName = String.Format(@"{0}\{1}.xml", mSessionDir, pSessionFileName);
        if (File.Exists(lFileName))
          File.Delete(lFileName);
      }
      catch (Exception lEx)
      {
        LogConsole.Main.LogConsole.pushMsg(lEx.StackTrace);
      }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionName"></param>
    public AttackSession loadSession(String pSessionName)
    {
      String lSessionFilePath = String.Empty;

      try
      {
        lSessionFilePath = String.Format(@"{0}\{1}.xml", mSessionDir, pSessionName);
      }
      catch (Exception lEx)
      {
        LogConsole.Main.LogConsole.pushMsg(lEx.StackTrace);
      }

      /*
       * Load main GUI session data.
       */
      FileStream lFS = null;
      XmlSerializer lXMLSerial;
      AttackSession lAttackSession = new AttackSession();
      String lStartIP = String.Empty;
      String lStopIP = String.Empty;

      try
      {
        lFS = new FileStream(lSessionFilePath, FileMode.Open);
        lXMLSerial = new XmlSerializer(typeof(AttackSession));
        lAttackSession = (AttackSession) lXMLSerial.Deserialize(lFS);
      }
      catch (Exception lEx)
      {
        LogConsole.Main.LogConsole.pushMsg(lEx.StackTrace);
      }
      finally
      {
        if (lFS != null)
          lFS.Close();
      }

      return(lAttackSession);
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionDir"></param>
    /// <returns></returns>
    public List<AttackSession> getAllSessions()
    {
      List<AttackSession> lRetVal = new List<AttackSession>();
      DirectoryInfo lDirInfo = new DirectoryInfo(mSessionDir);
      FileInfo[] lSessionFiles = lDirInfo.GetFiles("*.xml");


      /*
       * 
       */
      foreach (FileInfo lSessionFile in lSessionFiles)
      {
        FileStream lFS = null;
        XmlSerializer lXMLSerial;

        try
        {
          lFS = new FileStream(lSessionFile.FullName, FileMode.Open);
          lXMLSerial = new XmlSerializer(typeof(AttackSession));
          AttackSession lAttackSession = (AttackSession) lXMLSerial.Deserialize(lFS);
          lRetVal.Add(lAttackSession);
        }
        catch (Exception lEx)
        {
          LogConsole.Main.LogConsole.pushMsg(String.Format("TaskFacade.GetAllSessions(): {0} ({1})", lEx.Message, lSessionFile.FullName));
        }
        finally
        {
          if (lFS != null)
            lFS.Close();
        }
      } // foreach (FileIn...

      return (lRetVal);
    }


    /// <summary>
    /// 
    /// </summary>
    public void SaveSessionData(AttackSession pAttackSession)
    {
      String lSessionFile;
      XmlSerializer lSerializer;
      FileStream lFS = null;


      lSessionFile = String.Format(@"{0}\{1}.xml", mSessionDir, Path.GetFileNameWithoutExtension(pAttackSession.SessionFileName));
      if (File.Exists(lSessionFile))
        throw new Exception("A session with this name already exists");

      try
      {
        lSerializer = new XmlSerializer(typeof(AttackSession));
        lFS = new FileStream(lSessionFile, FileMode.Create);
        lSerializer.Serialize(lFS, pAttackSession);
      }
      catch (Exception lEx)
      {
//MessageBox.Show("Can't save session data : " + lEx.ToString());
      }
      finally
      {
        if (lFS != null)
          lFS.Close();
      }
    }


    /// <summary>
    /// 
    /// </summary>
    public String readMainSessionData(String pSessionFileName)
    {
      String lSessionFilePath = String.Format(@"{0}\{1}.xml", mSessionDir, pSessionFileName);
      String lSessionData = String.Empty;

      try
      {
        if (File.Exists(lSessionFilePath))
          lSessionData = readFileData(lSessionFilePath);
      }
      catch (Exception)
      {
      }

      return (lSessionData);
    }


    /// <summary>
    /// 
    /// </summary>
    public String readFileData(String pFilePath)
    {
      String lSessionData = String.Empty;

      try
      {
        if (!String.IsNullOrEmpty(pFilePath))
          if (File.Exists(pFilePath))
            lSessionData = File.ReadAllText(pFilePath);
      }
      catch (Exception)
      {
      }

      return (lSessionData);
    }


    /// <summary>
    /// 
    /// </summary>
    public void writeSessionExportFile(String pPathSessionFile, String pDataString)
    {
      try
      {
        if (!String.IsNullOrEmpty(pDataString))
        {
          if (Regex.Match(pDataString, @"<\s*\?\s*xml.*?>", RegexOptions.IgnoreCase).Success)
            pDataString = Regex.Replace(pDataString, @"<\s*\?\s*xml.*?>", String.Empty, RegexOptions.IgnoreCase);

          using (StreamWriter lSW = new StreamWriter(pPathSessionFile))
          {
            lSW.Write(pDataString);
          } // using (Strea...
        } // if (!String....
      }
      catch (Exception lEx)
      {
        LogConsole.Main.LogConsole.pushMsg(String.Format("Export session: ", lEx.Message));
        throw new Exception(String.Format("Unable to export session : {0}", lEx.Message));
      }
    }


    /// <summary>
    /// 
    /// </summary>
    public void writeFileData(String pFilePath, String pData)
    {

      try
      {
        if (!String.IsNullOrEmpty(pFilePath) && !String.IsNullOrEmpty(pData))
          if (File.Exists(pFilePath))
            File.WriteAllText(pFilePath, pData);
      }
      catch (Exception)
      {
      }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionFileName"></param>
    /// <param name="pOverwriteExistingSession"></param>
    public void importSessionFile(String pSessionFileName, bool pImportEnforced = false)
    {
      if (!String.IsNullOrEmpty(pSessionFileName))
      {
        String lSessionData = String.Empty;
        String lSessionFileName = String.Empty;
        XDocument lXMLDoc = null;

        lSessionData = Simsang.Session.TaskFacade.getInstance().readFileData(pSessionFileName);

        if (!String.IsNullOrEmpty(lSessionData))
          lXMLDoc = XDocument.Parse(lSessionData);

        /*
         * Process session data
         */
        try
        {
          XElement lSession = lXMLDoc.Element("SessionExport").Element("Session");
          lSessionFileName = lSession.Attribute("filename").Value.ToString();

          foreach (XElement lTmp in lSession.Elements())
          {
            if (lTmp.Name.LocalName.ToString() == "AttackSession")
            {
              String lSessionFile = String.Format(@"{0}\{1}.xml", mSessionDir, lSessionFileName);
              if (File.Exists(lSessionFile))
              {
                if (pImportEnforced == false)
                  throw new Exception("A session with this name already exists");

                File.Delete(lSessionFile);
              } // if (lTmp...

              writeSessionExportFile(lSessionFile, lTmp.ToString());
              break;
            } // if (lTmp.Name...
          } // foreach (XElement...

        }
        catch (NullReferenceException lEx)
        {
          throw new Exception("Something is wrong with the file structure");
        }


        /*
         * Process plugin data
         */
        String lPluginName = String.Empty;
        XElement lAllPlugins = null;
        String lOccurredErrors = String.Empty;

        var query = from plugin in lXMLDoc.Descendants("SessionExport")
                    select plugin.Element("Plugins");


        if ((lAllPlugins = query.SingleOrDefault()) != null)
        {
          XDocument lPluginsInSession = XDocument.Parse(lAllPlugins.ToString());

          foreach (XElement lTmpElement in lPluginsInSession.Descendants("Plugin"))
          {
            if (lTmpElement.HasElements)
            {
              String lName = lTmpElement.Attribute("name").Value.ToString();
              String lPluginDirName = lTmpElement.Attribute("dirname").Value.ToString();
              String lSessionFile = String.Format("{0}\\{1}{2}{3}{4}.xml", Directory.GetCurrentDirectory(), Simsang.Config.PluginDir, lPluginDirName, Simsang.Config.SessionDir, lSessionFileName);
              String lData = String.Empty;

              try
              {
                if (File.Exists(lSessionFile))
                  File.Delete(lSessionFile);
              }
              catch (Exception lEx)
              {
                String lErrorMsg = String.Format("Error occurred while deleting {0}\r\n{1}", lSessionFileName, lEx.Message);
                LogConsole.Main.LogConsole.pushMsg(lErrorMsg);
              }

              try
              {
                lData = lTmpElement.FirstNode.ToString();
                Simsang.Session.TaskFacade.getInstance().writeFileData(lSessionFile, lData);
              }
              catch (Exception lEx)
              {
                String lErrorMsg = String.Format("Error occurred while creating {0}\r\n{1}", lSessionFileName, lEx.Message);
                LogConsole.Main.LogConsole.pushMsg(lErrorMsg);
                lOccurredErrors = String.Format("\r\n{0}\r\n", lOccurredErrors);
              }
            } // if (lTmpEle...
          } // foreach (XElem...
        } // if ((lAllPlug...
      } // if (!String...      
    }

    #endregion


    #region PRIVATE

    /// <summary>
    /// 
    /// </summary>
    private InfrastructureFacade()
    {
      mSessionDir = String.Format(@"{0}\{1}", Directory.GetCurrentDirectory(), ConfigurationManager.AppSettings["sessiondir"] ?? @"Sessions\");
    }

    #endregion

  }
}
