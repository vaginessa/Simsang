using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;


namespace Simsang.MiniBrowser
{
  class TaskFacade
  {
    #region MEMBERS

    private static TaskFacade cInstance;

    #endregion


    #region PUBLIC 

    private TaskFacade()
    {
    }


    /// <summary>
    /// Create single instance
    /// </summary>
    /// <returns></returns>
    public static TaskFacade getInstance()
    {
      if (cInstance == null)
        cInstance = new TaskFacade();

      return (cInstance);
    }


    /// <summary>
    /// 
    /// </summary>
    public void clearIECache()
    {
      Process process = new Process();
      process.StartInfo.FileName = "cmd.exe";
      process.StartInfo.Arguments = "/c " + "del /f /s /q \"%userprofile%\\Local Settings\\Temporary Internet Files\\*.*\"";
      process.StartInfo.UseShellExecute = false;
      process.StartInfo.RedirectStandardInput = true;
      process.StartInfo.RedirectStandardOutput = true;
      process.StartInfo.RedirectStandardError = true;
      process.StartInfo.CreateNoWindow = true;
      process.Start();
      Application.DoEvents();
    }



    /// <summary>
    /// 
    /// </summary>
    public void clearCookies()
    {
      DirectoryInfo dir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Cookies));
      foreach (FileInfo info in dir.GetFiles("*.txt"))
      {
        info.Delete();
        Application.DoEvents();
      }
    }

    #endregion

  }
}
