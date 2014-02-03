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


namespace Plugin.Main.DNSPoison
{
  class InfrastructureFacade
  {

    #region MEMBERS

    private static InfrastructureFacade cInstance;
    private IPlugin cPlugin;

    #endregion


    #region PUBLIC

    private InfrastructureFacade(IPlugin pPlugin)
    {
      cPlugin = pPlugin;

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

    #endregion


    #region SESSIONS

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="pSessionFile"></param>
    /// <returns></returns>
    public BindingList<T> loadSessionData<T>(String pSessionName)
    {
      BindingList<T> lRecords = null;
      FileStream lFS = null;
      XmlSerializer lXMLSerial;
      String lSessionFilePath = String.Format(@"{0}\{1}.xml", cPlugin.Config.SessionDir, pSessionName);

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
    /// <param name="pSessionFileName"></param>
    public void deleteSession(String pSessionName)
    {
      String lSessionFilePath = String.Format(@"{0}\{1}.xml", cPlugin.Config.SessionDir, pSessionName);

      if (File.Exists(lSessionFilePath))
        File.Delete(lSessionFilePath);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionFile"></param>
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
    /// <param name="pRequests"></param>
    public void saveSessionData<T>(String pSessionName, BindingList<T> pRequests)
    {
      if (pSessionName.Length > 0)
      {
        XmlSerializer lSerializer;
        FileStream lFS = null;
        String lSessionFilePath = String.Format(@"{0}\{1}.xml", cPlugin.Config.SessionDir, pSessionName);

        try
        {
          lSerializer = new XmlSerializer(pRequests.GetType());
          lFS = new FileStream(lSessionFilePath, FileMode.Create);
          lSerializer.Serialize(lFS, pRequests);
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

  }
}

