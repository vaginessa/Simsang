using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;


namespace Simsang
{
  static class Program
  {
    /// <summary>
    /// Der Haupteinstiegspunkt für die Anwendung.
    /// </summary>
    [STAThread]
    static void Main(String[] args)
    {
      OperatingSystem lOS = Environment.OSVersion;
      Version vs = lOS.Version;

      /*
       * OS checks
       */
      if (lOS.Platform != PlatformID.Win32NT || vs.Major < 6)
      {
        string lMsg = string.Format("{0} doesnt run on your Windows version!", Config.ToolName);
        MessageBox.Show(lMsg, "Error",
        MessageBoxButtons.OK, MessageBoxIcon.Error);

      /*
       * Start GUI
       */
      }
      else
      {
        Directory.SetCurrentDirectory(System.Windows.Forms.Application.StartupPath);
        DirectoryChecks(System.Windows.Forms.Application.StartupPath);

        Application.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        SimsangMain lSimsangGUI = Simsang.SimsangMain.getInstance(args);
        Application.Run(lSimsangGUI);
      }
    } // static void main ...



    /// <summary>
    /// 
    /// </summary>
    /// <param name="pBaseDir"></param>
    private static void DirectoryChecks(String pBaseDir)
    {
      String[] lDirs = new String[] { Config.PluginDir, Config.SessionDir, Config.BinaryDir, Config.DataDir, Config.DLLDir };

      foreach (String lTmpDir in lDirs)
      {
        try
        {
          String lDir = String.Format(@"{0}\{1}", pBaseDir, lTmpDir);
          if (!Directory.Exists(lDir))
            Directory.CreateDirectory(lDir);
        }
        catch (Exception lEx)
        {
          String lErrorMsg = String.Format("Error occurred while creating \"{0}\"\r\nMessage: {1}", lTmpDir, lEx.Message);
          MessageBox.Show(lErrorMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
      } // foreach...
    } // private static vo..

  }
}
