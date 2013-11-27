using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace Simsang.Contribute.Infrastructure
{
  public class ContributionProcessing
  {

    #region MEMBERS

    private static ContributionProcessing cInstance;

    #endregion


    #region PUBLIC

    /// <summary>
    /// 
    /// </summary>
    private ContributionProcessing()
    {
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static ContributionProcessing getInstance()
    {
      if (cInstance == null)
        cInstance = new ContributionProcessing();

      return (cInstance);
    }



    /// <summary>
    /// 
    /// </summary>
    public void processMessages()
    {
      String lMsgsFolder = Config.GetRegistryValue(Config.RegistryContribution, Config.RegistryContributionMessages);
      if (lMsgsFolder == null || lMsgsFolder.Length <= 0)
        Config.CreateRegistryKey(Config.RegistrySoftwareName, String.Format(@"{0}\{1}", Config.RegistryContribution, Config.RegistryContributionMessages));

      String lMsgsSubKey = String.Format(@"{0}\{1}\{2}", Config.BasisKey, Config.RegistryContribution, Config.RegistryContributionMessages);


      try
      {
        using (RegistryKey key = Registry.CurrentUser.OpenSubKey(lMsgsSubKey, true))
        {
          if (key != null)
          {
            foreach (String subkeyName in key.GetValueNames())
            {
              String lData = key.GetValue(subkeyName.ToString()).ToString();
              IContribute lContr = ContributeHTTP.getInstance();

              if (lContr.sendContribution(lData))
              {
                LogConsole.Main.LogConsole.pushMsg(String.Format("Contribution msg sent ({0}) : {1}", lContr.getMethod(), subkeyName.ToString()));
                key.DeleteValue(subkeyName.ToString());
              }
              else
              {
                LogConsole.Main.LogConsole.pushMsg(String.Format("Contribution msg not sent ({0}) : {1}", lContr.getMethod(), subkeyName.ToString()));
              }

            } // foreach (Stri...
          } // if (key...
        } // using (Regi...
      }
      catch (Exception lEx)
      {
        LogConsole.Main.LogConsole.pushMsg("Error sending contribution message : " + lEx.StackTrace);
      }
      //Config.SetRegistryValue(Config.RegistryContributionKey, Config.RegistryContributionValue, pStatus);

    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="pMsg"></param>
    /// <returns></returns>
    public bool createContributionMessage(String pMsg)
    {
      Boolean lRetVal = false;
      String lTimeStamp = String.Empty;
      RegistryKey lRegKey = Registry.CurrentUser;

      String lRegKeyString = String.Format(@"{0}\{1}\{2}", Config.BasisKey, Config.RegistryContribution, Config.RegistryContributionMessages);

      if (pMsg != null && pMsg.Length > 0)
      {
        try
        {
          lTimeStamp = DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss zz");

          lRegKey = lRegKey.CreateSubKey(lRegKeyString);
          lRegKey.SetValue(lTimeStamp, pMsg, RegistryValueKind.String);
          lRegKey.Close();

          lRetVal = true;
        }
        catch (Exception lEx)
        {
          LogConsole.Main.LogConsole.pushMsg("createContributionMessage() : " + lEx.StackTrace);
        }
      } // if (pMsg !=...

      return (lRetVal);
    }

    #endregion

  }
}
