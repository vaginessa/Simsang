using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.NetworkInformation;
using System.Net;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;

using Simsang.Updates.Config;


namespace Simsang.Updates
{
  class InfrastructureFacade
  {

    #region MEMBERS

    private static InfrastructureFacade mInstance;
    private UpdateConfig mUpdates = new UpdateConfig();
    
    #endregion


    #region PUBLIC

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static InfrastructureFacade getInstance()
    {
      if (mInstance == null)
        mInstance = new InfrastructureFacade();

      mInstance.getUpdateInformation();

      return mInstance;
    }


    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public UpdateConfig getUpdateConfig()
    {
      return mUpdates;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool IsUpdateAvailable()
    {
      bool lRetVal = false;


      /*
       * Parse and compare current and newest version.
       */
      try
      {
        if (Regex.Match(Simsang.Config.ToolVersion, @"^\d+\.\d+\.\d+$").Success && Regex.Match(mUpdates.AvailableVersionStr, @"^\d+\.\d+\.\d+$").Success)
        {
          int lAvailableVersionInt = Int32.Parse(Regex.Replace(mUpdates.AvailableVersionStr, @"[^\d]+", ""));
          int lToolVersionInt = Int32.Parse(Regex.Replace(Simsang.Config.ToolVersion, @"[^\d]+", ""));

          if (lAvailableVersionInt.CompareTo(lToolVersionInt) > 0)
            lRetVal = true;
        } // if (Rege...
      }
      catch (Exception)
      {
      }

      return (lRetVal);
    }

    #endregion


    #region PRIVATE

    /// <summary>
    /// 
    /// </summary>
    private InfrastructureFacade()
    {
    }


    /// <summary>
    /// 
    /// </summary>
    private void getUpdateInformation()
    {
      HttpWebRequest lWebRequest = null;
      WebResponse lWebResponse = null;
      Stream lDataStream = null;
      StreamReader lReader = null;
      String lCurrentVersionXML = String.Empty;


      if (NetworkInterface.GetAllNetworkInterfaces().Any(x => x.OperationalStatus == OperationalStatus.Up))
      {
        try
        {
          lWebRequest = (HttpWebRequest)WebRequest.Create(Simsang.Config.CurrentVersionURL);
          lWebResponse = lWebRequest.GetResponse();

          lDataStream = lWebResponse.GetResponseStream();
          lReader = new StreamReader(lDataStream);
          lCurrentVersionXML = lReader.ReadToEnd();

          if (!String.IsNullOrEmpty(lCurrentVersionXML) && !String.IsNullOrEmpty(Simsang.Config.ToolVersion))
          {
            XmlDocument lXMLDoc = new XmlDocument();
            lXMLDoc.LoadXml(lCurrentVersionXML);

            /*
             * Load version from XML 
             */
            try
            {
              var lData = lXMLDoc.SelectNodes("/simsang");
              mUpdates.AvailableVersionStr = lData.Item(0)["version"].InnerText;

              /*
               * Load messages from XML 
               */
              mUpdates.Messages.Clear();
              XmlNodeList lMessages = lXMLDoc.SelectNodes("/simsang/message");
              foreach (XmlNode xn in lMessages)
                mUpdates.Messages.Add(xn.InnerText);
            }
            catch (Exception lEx)
            {
              String lMessage = lEx.Message;
            }
          } // if (!St...
        }
        catch (Exception lEx)
        {
          LogConsole.Main.LogConsole.pushMsg(String.Format("IsUpdateAvailable(): {0}", lEx.Message));
        }
        finally
        {
          if (lReader != null)
            lReader.Close();

          if (lWebResponse != null)
            lWebResponse.Close();
        }
      } // if (NetworkInterface...
    }

    #endregion

  }
}
