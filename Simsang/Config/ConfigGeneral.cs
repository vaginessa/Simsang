using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Simsang
{
  public partial class Config
  {

    #region MEMBERS

    public static readonly String DebuggingFile = "DEBUG.txt";
    public static readonly String LogFile = "Logfile.txt";

    public static readonly String ToolName = "Simsang";
    public static readonly String ToolVersion = "2.0.1";
    public static readonly String VersionType = "Full version";
    public static readonly String BuglistURL = "http://www.buglist.io/";
    public static readonly String UpdateURL = "http://www.buglist.io/downloads.php";
    public static readonly String CurrentVersionURL = "http://buglist.io/download/currentVersion.php";
    public static readonly String ToolHomepage = "http://www.buglist.io/downloads.php";

    public static String OS = String.Empty;
    public static String Architecture = String.Empty;
    public static String Language = String.Empty;
    public static String Processor = String.Empty;
    public static String NumProcessors = String.Empty;
    public static String DotNetVersion = String.Empty;
    public static String CommonLanguateRuntime = String.Empty;


    public static readonly String PipeName = "Simsang";
    public static readonly int PipeInstances = 16;
    public static readonly String PluginDir = @"plugins\";
    public static readonly String PluginVersionFile = "version.txt";
    public static readonly String BinaryDir = @"\bin\";
    public static readonly String DLLDir = @"\DLL\";
    public static readonly String ARPScanBinary = "ARPScan.exe";
    public static readonly String NmapBinary = "nmap.exe";

    public static readonly String SessionDir = @"\sessions\";

    public static readonly String DataDir = @"\data\";
    public static readonly String DB_Data = "data";

    public static String WinPcap = String.Empty;
    public static readonly String SimsangFileExtension = "sim";

    public static readonly String ContributionSenderEmail = "megapanzer@gmail.com";
    public static readonly String ContributionRecipientEmail = "ruben.unteregger@gmail.com";
    public static readonly String SMTPServer = "gmail-smtp-in.l.google.com";
    public static readonly int SMTPPort = 25;
    public static readonly String ContributionSubject = "Contribution mail";

    public static String LocalBinariesPath { get { return (String.Format("{0}{1}", Directory.GetCurrentDirectory(), Config.BinaryDir)); } }
    public static String LocalPluginPath { get { return (String.Format(@"{0}{1}\", Directory.GetCurrentDirectory(), Config.PluginDir)); } }

    // APE
    public static readonly String APEBinary = "APE.exe";
    public static readonly String APEName = "APE";
    public static String APEPath { get { return (String.Format(@"{0}{1}\{2}", Directory.GetCurrentDirectory(), Config.BinaryDir, Config.APEBinary)); } }

    public static readonly String APEFWRules = ".fwrules";
    public static readonly String APEInjectionRules = ".injecturls";
    public static readonly String APETargetHosts = ".targethosts";
    public static readonly String DNSPoisoningHosts = ".dnshosts";
    public static String APEFWRulesPath { get { return (String.Format(@"{0}{1}\{2}", Directory.GetCurrentDirectory(), Config.BinaryDir, Config.APEFWRules)); } }
    public static String APEInjectionRulesPath { get { return (String.Format(@"{0}{1}\{2}", Directory.GetCurrentDirectory(), Config.BinaryDir, Config.APEInjectionRules)); } }
    public static String APETargetHostsPath { get { return (String.Format(@"{0}{1}\{2}", Directory.GetCurrentDirectory(), Config.BinaryDir, Config.APETargetHosts)); } }

    public static String DNSPoisoningHostsPath { get { return (String.Format(@"{0}{1}\{2}", Directory.GetCurrentDirectory(), Config.BinaryDir, Config.DNSPoisoningHosts)); } }



    // Registry
    public static readonly String RegistrySoftwareName = "Simsang";
    public static readonly String BasisKey = String.Format(@"Software\{0}", Config.RegistrySoftwareName);

    public static readonly String RegistryContribution = "Contribution";
    public static readonly String RegistryContributionMessages = "msgs";
    public static readonly String RegistryContributionValue = "contribute";
    public static readonly String RegistryContributionKey = "Config";

    public static readonly String RegistrySSIDContributionValue = "contributeSSID";
    public static readonly String RegistrySSIDContributionKey = "Config";

    #endregion


    #region PUBLIC

    /// <summary>
    /// Get plugin list
    /// </summary>
    /// <returns></returns>
    public static String[] GetPluginList()
    {
      String lBaseDir = Directory.GetCurrentDirectory();
      String lTempPluginPath = String.Format(@"{0}\{1}", lBaseDir, Config.PluginDir);
      String[] lPluginList = Directory.GetDirectories(lTempPluginPath);

      return (lPluginList);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static bool DebugOn()
    {
      bool lRetVal = false;

      if (File.Exists(Config.DebuggingFile))
        lRetVal = true;

      return (lRetVal);
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="pRegKeyName"></param>
    /// <param name="pRegSubValueName"></param>
    /// <param name="pRegValueContent"></param>
    public static void SetRegistryValue(String pRegKeyName, String pRegSubValueName, String pRegValueContent)
    {
      RegistryKey lRegKey = Registry.CurrentUser;

      try
      {
        lRegKey = lRegKey.CreateSubKey(String.Format("Software\\{0}\\{1}", ToolName, pRegKeyName));
        lRegKey.SetValue(pRegSubValueName, pRegValueContent, RegistryValueKind.String);
        lRegKey.Close();
      }
      catch (Exception)
      {
      }
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="pRegKeyName"></param>
    /// <param name="pRegSubValueName"></param>
    /// <returns></returns>
    public static String GetRegistryValue(String pRegKeyName, String pRegSubValueName)
    {
      RegistryKey lRegKey = Registry.CurrentUser;
      String lRetVal = String.Empty;

      try
      {
        lRegKey = lRegKey.CreateSubKey(String.Format("Software\\{0}\\{1}", ToolName, pRegKeyName));
        lRetVal = (String)lRegKey.GetValue(pRegSubValueName);
        lRegKey.Close();
      }
      catch (Exception)
      {
      }


      return (lRetVal);
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="pParentKey"></param>
    /// <param name="pRegKeyName"></param>
    /// <returns></returns>
    public static bool CreateRegistryKey(String pParentKey, String pRegKeyName)
    {
      RegistryKey lRegKey = Registry.CurrentUser;
      bool lRetVal = false;

      try
      {
        lRegKey = lRegKey.CreateSubKey(String.Format("Software\\{0}\\{1}", pParentKey, pRegKeyName));
        lRegKey.Close();
        lRetVal = true;
      }
      catch (Exception)
      {
      }


      return (lRetVal);
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="pAppName"></param>
    /// <returns></returns>
    public static string IsProgrammInstalled(String pAppName)
    {
      String lRetVal = String.Empty;

      //The registry key:
      String[] lSWDirs = new String[2] { @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", 
                                             @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall" 
                                           };

      foreach (String lSWDir in lSWDirs)
      {
        using (RegistryKey lRegKey = Registry.LocalMachine.OpenSubKey(lSWDir))
        {
          //Let's go through the registry keys and get the info we need:
          foreach (String skName in lRegKey.GetSubKeyNames())
          {
            using (RegistryKey lRegSubKey = lRegKey.OpenSubKey(skName))
            {
              try
              {
                if (lRegSubKey.GetValue("DisplayName") != null)
                {
                  String lSWName = lRegSubKey.GetValue("DisplayName").ToString();

                  if (Regex.Match(lSWName, pAppName, RegexOptions.IgnoreCase).Success)
                  {
                    lRetVal = lSWName;
                    goto END;
                  } // if (Regex.Ma...
                } // if (lRegSub...
              }
              catch (Exception) { }
            } // using (Regist...
          } // foreach (str...
        } // using (Regi...
      } // for (Stri...

END:

      return (lRetVal);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pDebugStatus"></param>
    public static void enableDebugging(bool pDebugStatus)
    {
      if (!pDebugStatus)
      {
        try { File.Delete(Config.DebuggingFile); }
        catch (Exception) { }
      }
      else
      {
        try { File.Create(Config.DebuggingFile); }
        catch (Exception) { }
      }
    }

    #endregion

  }
}