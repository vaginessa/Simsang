using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

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
        System.IO.Directory.SetCurrentDirectory(System.Windows.Forms.Application.StartupPath);

        Application.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        ACMain lSimsangGUI = new ACMain(args);

        Application.Run(lSimsangGUI);

        //try
        //{
        //  Application.Run(lSimsangGUI);
        //}
        //catch (Exception lEx)
        //{
        //  MessageBox.Show(String.Format("Msg: {0}\r\n\r\n{1}", lEx.Message, lEx.StackTrace));
        //}
      }
    }
  }
}
