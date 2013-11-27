using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Web;


namespace Simsang.Contribute.Infrastructure
{
  class ContributeHTTP : IContribute
  {

    #region MEMBERS

    private static IContribute cInstance;

    #endregion


    #region PUBLIC

    /// <summary>
    /// 
    /// </summary>
    private ContributeHTTP()
    {
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static IContribute getInstance()
    {
      if (cInstance == null)
        cInstance = new ContributeHTTP();

      return (cInstance);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pData"></param>
    /// <returns></returns>
    public bool sendContribution(String pData)
    {
      bool lRetVal = false;
      HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create("http://buglist.io/c/contribute.php");
      ASCIIEncoding encoding = new ASCIIEncoding();
      String lUserAgent = String.Format("Mozilla/4.0 (compatible; MSIE 10.0; Windows NT {0}; .NET CLR {1})", Config.OS, Config.DotNetVersion);
      try
      {
        byte[] data = encoding.GetBytes(pData);
        String lEncodedData = String.Format("data={0}", System.Web.HttpUtility.UrlEncode(data));


        httpWReq.Method = "POST";
        httpWReq.ContentType = "application/x-www-form-urlencoded";
        httpWReq.ContentLength = lEncodedData.Length;
        httpWReq.UserAgent = lUserAgent;


        using (Stream stream = httpWReq.GetRequestStream())
        {
          stream.Write(encoding.GetBytes(lEncodedData), 0, lEncodedData.Length);
        }

        HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();
        String responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
        LogConsole.Main.LogConsole.pushMsg(String.Format("Contribution sent ({0}) : {1}", getMethod(), String.Format("{0} ...", lEncodedData.Substring(0, 30))));

        lRetVal = true;
      }
      catch (Exception lEx)
      {
        LogConsole.Main.LogConsole.pushMsg("Error sending contribution message : " + lEx.Message);
      }


      return (lRetVal);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public String getMethod()
    {
      return ("HTTP");
    }

    #endregion

  }
}
