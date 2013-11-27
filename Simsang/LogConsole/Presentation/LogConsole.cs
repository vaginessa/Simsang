using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Simsang.LogConsole.Main
{
  public partial class LogConsole : Form
  {

    #region MEMBERS

    private static LogConsole mLogConsole;

    #endregion


    #region PUBLIC

    public LogConsole()
    {
      InitializeComponent();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void LogConsole_FormClosing(object sender, FormClosingEventArgs e)
    {
      this.Hide();
      e.Cancel = true;
    }


    /// <summary>
    /// 
    /// </summary>
    public static void initLogConsole()
    {
      if (mLogConsole == null)
      {
        mLogConsole = new LogConsole();

        pushTitle("Starting Log console");
        pushTitle(String.Format("Simsang version : {0}", Config.ToolVersion));
        pushTitle(String.Format("OS : {0}", Config.OS));
        pushTitle(String.Format("Architecture : {0}", Config.Architecture));
        pushTitle(String.Format("Language : {0}", Config.Language));
        pushTitle(String.Format("Processor : {0}", Config.Processor));
        pushTitle(String.Format("Num. processors : {0}", Config.NumProcessors));
        pushTitle(String.Format(".Net version : {0}", Config.DotNetVersion));
        pushTitle(String.Format("CLR version : {0}", Config.CommonLanguateRuntime));
        pushTitle(String.Format("WinPcap version : {0}", Config.WinPcap));
      } // if (mLogC
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static LogConsole getInstance()
    {
      initLogConsole();
      return (mLogConsole);
    }


    /// <summary>
    /// /
    /// </summary>
    public static void showLogConsole()
    {
      initLogConsole();
      mLogConsole.Show();
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="pMsg"></param>
    private static void pushTitle(string pMsg)
    {
      String lTimeStamp = String.Empty;
      DateTime lTime = DateTime.Now;


      initLogConsole();



      try { lTimeStamp = lTime.ToString("yyyy-MM-dd-HH:mm:ss"); }
      catch (Exception) { }

      if (!String.IsNullOrEmpty(pMsg))
      {
        pMsg = pMsg.Trim();
        mLogConsole.TB_LogContent.Text += string.Format("{0} - {1}\r\n", lTimeStamp, pMsg);

        mLogConsole.TB_LogContent.SelectionStart = mLogConsole.TB_LogContent.Text.Length;
        mLogConsole.TB_LogContent.ScrollToCaret();
      }
    }




    /// <summary>
    /// 
    /// </summary>
    /// <param name="pMsg"></param>
    public delegate void pushMsgDelegate(string pMsg);
    public static void pushMsg(string pMsg)
    {
      if (mLogConsole != null && mLogConsole.InvokeRequired)
      {
        mLogConsole.BeginInvoke(new pushMsgDelegate(pushMsg), new object[] { pMsg });
        return;
      } // if (InvokeRequired)





      String lTimeStamp = String.Empty;
      DateTime lTime = DateTime.Now;


      try { initLogConsole(); }
      catch { }



      try { lTimeStamp = lTime.ToString("yyyy-MM-dd-HH:mm:ss"); }
      catch { }

      if (!String.IsNullOrEmpty(pMsg))
      {
        try
        {
          /*
           * Write to log console
           */
          pMsg = pMsg.Trim();
          mLogConsole.TB_LogContent.Text += string.Format("{0} - {1}\r\n", lTimeStamp, pMsg);

          mLogConsole.TB_LogContent.SelectionStart = mLogConsole.TB_LogContent.Text.Length;
          mLogConsole.TB_LogContent.ScrollToCaret();
        }
        catch { }
      } // if (!Stri
    }

    #endregion


    #region EVENTS

    /// <summary>
    /// Close Sessions GUI on Escape.
    /// </summary>
    /// <param name="keyData"></param>
    /// <returns></returns>
    protected override bool ProcessDialogKey(Keys keyData)
    {
      if (keyData == Keys.Escape)
      {
        this.Close();
        return true;
      }
      else
        return base.ProcessDialogKey(keyData);
    }

    #endregion

  }
}
