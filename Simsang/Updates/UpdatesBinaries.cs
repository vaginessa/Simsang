using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Net.NetworkInformation;
using System.Net;
using System.Windows.Forms;


namespace Simsang
{
  public class UpdatesBinaries
  {

    #region MEMBERS

    private static UpdatesBinaries mInstance;
    private int mCurrentVersionInt;

    #endregion


    #region PROPERTIES

    private int CurrentVersion { get { return mCurrentVersionInt; } set { mCurrentVersionInt = value; } }

    #endregion


    #region PUBLIC

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static UpdatesBinaries getInstance()
    {
      return mInstance ?? (mInstance = new UpdatesBinaries());
    }


    /// <summary>
    /// 
    /// </summary>
    private UpdatesBinaries()
    { 
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool IsUpdateAvailable()
    {
      bool lRetVal = false;
      HttpWebRequest lWebRequest = null;
      WebResponse lWebResponse = null;
      Stream lDataStream = null;
      StreamReader lReader = null;
      String lCurrentVersion = String.Empty;


      if (NetworkInterface.GetAllNetworkInterfaces().Any(x => x.OperationalStatus == OperationalStatus.Up))
      {
        try
        {
          lWebRequest = (HttpWebRequest) WebRequest.Create(Config.CurrentVersionURL);
          lWebResponse = lWebRequest.GetResponse();

          lDataStream = lWebResponse.GetResponseStream();
          lReader = new StreamReader(lDataStream);
          lCurrentVersion = lReader.ReadToEnd();

          if (!String.IsNullOrEmpty(lCurrentVersion) && !String.IsNullOrEmpty(Config.ToolVersion))
          {
            if (Regex.Match(Config.ToolVersion, @"^\d+\.\d+\.\d+$").Success && Regex.Match(lCurrentVersion, @"^\d+\.\d+\.\d+$").Success)
            {
              int lCurrentVersionInt = Int32.Parse(Regex.Replace(lCurrentVersion, @"[^\d]+", ""));
              int lToolVersionInt = Int32.Parse(Regex.Replace(Config.ToolVersion, @"[^\d]+", ""));

              if (lCurrentVersion.CompareTo(lToolVersionInt) > 0)
                lRetVal = true;
            } // if (Rege...
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

      return (lRetVal);
    }

    #endregion

  }
}
