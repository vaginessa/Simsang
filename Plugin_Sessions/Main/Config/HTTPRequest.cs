using System;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using System.Collections;
using System.Threading;



namespace Plugin
{
  public class HTTPReq
  {

    #region PUBLIC

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pURL"></param>
    /// <param name="pUserAgent"></param>
    /// <param name="pCookies"></param>
    /// <returns></returns>
    public static string sendGetRequest(string pURL, string pUserAgent, string pCookies)
    {
      string lRetVal = String.Empty;
      HttpWebRequest lRequest = null;
      StreamReader lReader = null;

      try
      {
        lRequest = (HttpWebRequest)WebRequest.Create(pURL);
        lRequest.UserAgent = pUserAgent;
        lRequest.Method = "GET";
        lRequest.KeepAlive = false;
        lRequest.Timeout = 5000;
        lRequest.Headers.Add("Cookie", pCookies);
      }
      catch (Exception lEx)
      {
        // MessageBox.Show("sendGetRequest(0) : " + lEx.ToString() + "\r\n" + pURL);
      }


      try
      {
        using (HttpWebResponse lWebResponse = (HttpWebResponse)lRequest.GetResponse())
        {
          lReader = new StreamReader(lWebResponse.GetResponseStream());
          lRetVal = lReader.ReadToEnd();
        }
      }
      catch (WebException lEx)
      {
        //MessageBox.Show("sendGetRequest(1) : " + lEx.ToString() + "\r\n" + pURL);
      }
      finally
      {
        if (lReader != null)
          lReader.Close();
      }

      return (lRetVal);
    }

    #endregion

  }
}