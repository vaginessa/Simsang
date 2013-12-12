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


    /// <summary>
    /// 
    /// </summary>
    private void initLogConsole()
    {
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
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static LogConsole getInstance(bool enforce=false)
    {
      if (mLogConsole == null || enforce == true)
      {
        mLogConsole = new LogConsole();
        mLogConsole.initLogConsole();
      }

      return (mLogConsole);
    }


    /// <summary>
    /// /
    /// </summary>
    public delegate void showLogConsoleDelegate();
    public static void showLogConsole()
    {
//      if (mLogConsole != null && mLogConsole.InvokeRequired)
//      {
////        mLogConsole.BeginInvoke(new showLogConsoleDelegate(), new object[] { });
//        mLogConsole.Invoke(new Action(showLogConsole));
//        return;
//      } // if (InvokeRequired)

      if (mLogConsole == null)
        mLogConsole = getInstance();

      mLogConsole.Show();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pMsg"></param>
    public delegate void pushMsgDelegate(String pMsg);
    public static void pushMsg(String pMsg)
    {

      /*
       * Create log console instance if not done yet.
       */
      if (mLogConsole == null)
        getInstance();

      if (mLogConsole != null && mLogConsole.InvokeRequired)
      {
        mLogConsole.BeginInvoke(new pushMsgDelegate(pushMsg), new object[] { pMsg });
        return;
      } // if (InvokeRequired)



      String lTimeStamp = String.Empty;
      DateTime lTime = DateTime.Now;

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
          mLogConsole.TB_LogContent.AppendText(String.Format("{0} - {1}\r\n", lTimeStamp, pMsg));

          mLogConsole.TB_LogContent.SelectionStart = mLogConsole.TB_LogContent.Text.Length;
          mLogConsole.TB_LogContent.ScrollToCaret();
        }
        catch (Exception lEx)
        {
          String lMsg = lEx.Message;
        }
      } // if (!Stri
    }

    public delegate String getLogContentDelegate();
    public String getLogContent()
    {
      //if (mLogConsole != null && mLogConsole.InvokeRequired)
      //{
      //  mLogConsole.BeginInvoke(new getLogContentDelegate(getLogContent), new object[] { });
      //  return null;
      //} // if (InvokeRequired)

      return mLogConsole.TB_LogContent.Text;
    }

    #endregion


    #region EVENTS

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
    /// Close Sessions GUI on Escape.
    /// </summary>
    /// <param name="keyData"></param>
    /// <returns></returns>
    protected override bool ProcessDialogKey(Keys keyData)
    {
      if (keyData == Keys.Escape)
      {
        this.Hide();
        return false;
        //this.Close();
        //return true;
      }
      else
        return base.ProcessDialogKey(keyData);
    }

    #endregion


    #region PRIVATE

    /// <summary>
    /// 
    /// </summary>
    public LogConsole()
    {
      InitializeComponent();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pMsg"></param>
    private static void pushTitle(String pMsg)
    {
      String lTimeStamp = String.Empty;
      DateTime lTime = DateTime.Now;

      try { lTimeStamp = lTime.ToString("yyyy-MM-dd-HH:mm:ss"); }
      catch (Exception) { }

      if (!String.IsNullOrEmpty(pMsg))
      {
        pMsg = pMsg.Trim();
        mLogConsole.TB_LogContent.AppendText(String.Format("{0} - {1}\r\n", lTimeStamp, pMsg));

        mLogConsole.TB_LogContent.SelectionStart = mLogConsole.TB_LogContent.Text.Length;
        mLogConsole.TB_LogContent.ScrollToCaret();
      }
    }

    #endregion

  }
}
