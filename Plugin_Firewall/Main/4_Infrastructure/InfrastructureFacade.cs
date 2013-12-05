using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

using Simsang.Plugin;

namespace Plugin.Main.Firewall
{
  public class InfrastructureFacade
  {

    #region MEMBERS

    private static InfrastructureFacade cInstance;
    private IPlugin cPlugin;

    #endregion


    #region PUBLIC

    private InfrastructureFacade(IPlugin pPlugin)
    {
      cPlugin = pPlugin;
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

    #endregion


    #region PRIVATE

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private String GetSessionDir()
    {
      String lRetVal = String.Empty;

      lRetVal = String.Format("{0}{1}", cPlugin.Config.BaseDir, cPlugin.Config.SessionDir);

      try
      {
        if (!Directory.Exists(lRetVal))
          Directory.CreateDirectory(lRetVal);
      }
      catch (Exception lEx)
      {
        cPlugin.Host.LogMessage(lEx.StackTrace);
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
    public BindingList<T> loadSessionData<T>(String pSessionName)
    {
      BindingList<T> lRecords = null;
      FileStream lFS = null;
      XmlSerializer lXMLSerial;
      String lSessionFilePath = String.Format(@"{0}\{1}.xml", GetSessionDir(), pSessionName);

      try
      {
        lFS = new FileStream(lSessionFilePath, FileMode.Open);
        lXMLSerial = new XmlSerializer(typeof(BindingList<T>));
        lRecords = (BindingList<T>)lXMLSerial.Deserialize(lFS);
      }
      finally
      {
        if (lFS != null)
          lFS.Close();
      }

      return (lRecords);
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionName"></param>
    public void deleteSession(String pSessionName)
    {
      String lSessionFilePath = String.Format(@"{0}\{1}.xml", GetSessionDir(), pSessionName);

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
      String lSessionFilePath = String.Format(@"{0}\{1}.xml", GetSessionDir(), pSessionName);

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
    public void saveSessionData<T>(String pSessionName, BindingList<T> pRecords)
    {
      if (pSessionName.Length > 0)
      {
        XmlSerializer lSerializer;
        FileStream lFS = null;
        String lSessionFilePath = String.Format(@"{0}\{1}.xml", GetSessionDir(), pSessionName);

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
    public BindingList<T> loadSessionDataFromString<T>(String pSessionData)
    {
      BindingList<T> lRecords = new BindingList<T>();
      var lSerializer = new XmlSerializer(typeof(BindingList<T>));

      using (TextReader lTextReader = new StringReader(pSessionData))
      {
        lRecords = (BindingList<T>)lSerializer.Deserialize(lTextReader);
      } // using (TextRe...

      return (lRecords);
    }

    #endregion


    #region EVENTS

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pFWrules"></param>
    /// <param name="pFWRulesPath"></param>
    public void onStart(List<FWRule> pFWrules, String pFWRulesPath)
    {
      String lFWRules = String.Empty;

      /*
       * Write APE firewall rules file
       */
      if (!String.IsNullOrEmpty(pFWRulesPath))
      {
        if (File.Exists(pFWRulesPath))
          File.Delete(pFWRulesPath);


        foreach (FWRule lTmp in pFWrules)
          lFWRules += String.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}\r\n", lTmp.Protocol, lTmp.SrcIP, lTmp.SrcPortLower, lTmp.SrcPortUpper, lTmp.DstIP, lTmp.DstPortLower, lTmp.DstPortUpper);

        using (StreamWriter outfile = new StreamWriter(pFWRulesPath))
        {
          outfile.Write(lFWRules);
        } // using (Strea...
      } // if (lFWRulesPath... 
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pFWRulesPath"></param>
    public void onStop(String pFWRulesPath)
    {
      try
      {
        if (!String.IsNullOrEmpty(pFWRulesPath) && File.Exists(pFWRulesPath))
          File.Delete(pFWRulesPath);
      }
      catch (Exception) { }
    }

    #endregion

  }
}

