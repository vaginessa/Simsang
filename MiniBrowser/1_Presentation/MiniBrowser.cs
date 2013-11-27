using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.IO;
using System.Net;
using SHDocVw;
using System.Threading;



namespace Simsang.MiniBrowser
{
  public partial class Browser : Form
  {

    #region IMPORTS

    [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern bool InternetSetCookie(String lpszUrlName, String lbszCookieName, String lpszCookieData);

    [DllImport("wininet.dll", SetLastError = true, CharSet = CharSet.Auto, EntryPoint = "DeleteUrlCacheEntryA", CallingConvention = CallingConvention.StdCall)]
    public static extern bool DeleteUrlCacheEntry(String lpszUrlName);

    [DllImport("urlmon.dll", CharSet = CharSet.Ansi)]
    private static extern int UrlMkSetSessionOption(int dwOption, String pBuffer, int dwBufferLength, int dwReserved); const int URLMON_OPTION_USERAGENT = 0x10000001;

    #endregion


    #region MEMBERS

    private const String mDefaultUserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)";
    private static String mHeaderData;
    private String mCookies;
    private String mUserAgent;
    private TaskFacade cTask;

    #endregion


    #region PUBLIC

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pURL"></param>
    /// <param name="pCookie"></param>
    /// <param name="pSrcIP"></param>
    /// <param name="pUserAgent"></param>
    public Browser(String pURL, String pCookie, String pSrcIP, String pUserAgent)
    {
      InitializeComponent();

      cTask = TaskFacade.getInstance();

      TB_URL.Text = pURL;
      TB_Cookies.Text = pCookie;
      mCookies = pCookie;

      TB_UserAgent.Text = pUserAgent;
      mUserAgent = pUserAgent;

      this.Text = "MiniBrowser 0.1";


      if (!String.IsNullOrEmpty(pURL))
      {
        if (!pURL.ToLower().StartsWith("http"))
        {
          pURL = String.Format("http://{0}", pURL);
          this.TB_URL.Text = pURL;
        } // if (!lURL....
      } // if (!stri...



      /*
       * 
       */
      String lURL = this.TB_URL.Text;

      mHeaderData = String.Empty;

      cTask.clearIECache();
      cTask.clearCookies();


      if (CB_Cookies.Checked && TB_Cookies.Text.Length > 0)
      {
        try
        {
          foreach (String lCookie in TB_Cookies.Text.ToString().Split(';'))
          {
            if (lCookie.Length > 0 && lCookie.Contains("="))
            {
              Regex regex = new Regex("=");
              String[] substrings = regex.Split(lCookie, 2);

              InternetSetCookie(lURL, substrings[0], substrings[1]);
            } // if (lCookie....
          } // foreach (s...
        }
        catch (Exception lEx)
        {
        }
      } // if (CB_Cookie...
      //                     

      if (CB_UserAgent.Checked && TB_UserAgent.Text.Length > 0)
        mHeaderData = "User-Agent: " + TB_UserAgent.Text + "\r\n";
      else
        mHeaderData = String.Format("User-Agent: {0}\r\n", mDefaultUserAgent);

      if (CB_Cookies.Checked && TB_Cookies.Text.Length > 0)
        mHeaderData += "Cookie: " + TB_Cookies.Text + "\r\n";


      DeleteUrlCacheEntry(lURL);
      cTask.clearIECache();
      cTask.clearCookies();

      UrlMkSetSessionOption(URLMON_OPTION_USERAGENT, TB_UserAgent.Text, TB_UserAgent.Text.Length, 0);
    }

    #endregion


    #region EVENTS

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BT_Open_Click(object sender, EventArgs e)
    {
      String lURL = this.TB_URL.Text;

      mHeaderData = String.Empty;

      cTask.clearIECache();
      cTask.clearCookies();


      if (CB_Cookies.Checked && TB_Cookies.Text.Length > 0)
      {
        try
        {
          foreach (String lCookie in TB_Cookies.Text.ToString().Split(';'))
          {
            if (lCookie.Length > 0 && lCookie.Contains("="))
            {
              Regex regex = new Regex("=");
              String[] substrings = regex.Split(lCookie, 2);

              InternetSetCookie(lURL, substrings[0], substrings[1]);
            } // if (lCookie....
          } // foreach (s...
        }
        catch (Exception)
        {
        }
      } // if (CB_Cookie...         

      if (CB_UserAgent.Checked && TB_UserAgent.Text.Length > 0)
        mHeaderData = "User-Agent: " + TB_UserAgent.Text + "\r\n";
      else
        mHeaderData = String.Format("User-Agent: {0}\r\n", mDefaultUserAgent);

      if (CB_Cookies.Checked && TB_Cookies.Text.Length > 0)
        mHeaderData += "Cookie: " + TB_Cookies.Text + "\r\n";



      DeleteUrlCacheEntry(lURL);
      cTask.clearIECache();
      cTask.clearCookies();


      UrlMkSetSessionOption(URLMON_OPTION_USERAGENT, TB_UserAgent.Text, TB_UserAgent.Text.Length, 0);
      WB_MiniBrowser.ScriptErrorsSuppressed = true;
      WB_MiniBrowser.Navigate(lURL, "", null, mHeaderData);
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="pEnabled"></param>
    public delegate void ActivateGBDetailsDelegate(bool pEnabled);
    public void ActivateGBDetails(bool pEnabled)
    {
      if (InvokeRequired)
      {
        BeginInvoke(new ActivateGBDetailsDelegate(ActivateGBDetails), new object[] { pEnabled });
        return;
      } // if (InvokeRequired)

      if (pEnabled == false)
      {
        GB_Details.Enabled = false;
        //                GB_WebPage.Enabled = false;
        //                Cursor = Cursors.WaitCursor;
      }
      else
      {
        GB_Details.Enabled = true;
        //                GB_WebPage.Enabled = true;
        //                Cursor = Cursors.Default;
      }
    }




    /// <summary>
    ///  HTTP request Access token.
    ///  This is the tricky part! If somebody knows an easier way to get an AccessToken
    ///  -> let me know.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BGW_GetAccessToken_DoWork(object sender, DoWorkEventArgs e)
    {
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TB_URL_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Enter)
      {
        e.SuppressKeyPress = true;
        BT_Open_Click(null, null);
      }
    }

    #endregion

  }
}
