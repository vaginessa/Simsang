using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Collections;
using System.Text.RegularExpressions;



namespace HTTPSReverseProxyServer
{
  class HTTPProxy
  {
    private static HTTPProxy cInstance;
    private static readonly int BUFFER_SIZE = 8192;
    private static readonly char[] cSemiSplit = new char[] { ';' };
    private static readonly Regex cCookieSplitRegEx = new Regex(@",(?! )");

    /*
     * 
     * 
     */
    private HTTPProxy()
    {
    }


    /*
     * 
     * 
     */
    public static HTTPProxy getInstance()
    {
      if (cInstance == null)
        cInstance = new HTTPProxy();

      return (cInstance);
    }


    /*
     * 
     * 
     */
    public void processRequest(StreamReader pClientStreamReader, Stream pOutStream, String pMethod, String pURL, Hashtable pHeaders, int pContentLen, String pHTTPCommand, out String pHTTPData)
    {
      HttpWebRequest lWebReq = null;
      HttpWebResponse lResponse = null;
      char[] lPostBuffer;
      int lPOSTBytesRead = 0;
      int lTotalBytesRead = 0;
      StreamWriter lSW = null;
      List<Tuple<String, String>> lResponseHeaders;
      StreamWriter lMyResponseWriter = null;
      Stream lResponseStream = null;
      Byte[] lBuffer;
      int lBytesRead = 0;
      String lTmpStr = String.Empty;
      String lTmpBuf = String.Empty;
      String lPOSTData = String.Empty;
      /*
       * Construct the web request that we are going to issue on behalf of the client.
       */
      lWebReq = (HttpWebRequest)HttpWebRequest.Create(pURL);
      lWebReq.Method = pMethod;
      lWebReq.ProtocolVersion = System.Net.HttpVersion.Version10;

      SetHTTPHeaders(pHeaders, ref lWebReq);

      lWebReq.Proxy = null;
      lWebReq.KeepAlive = false;
      lWebReq.AllowAutoRedirect = false;
      lWebReq.AutomaticDecompression = DecompressionMethods.None;


      if (pMethod.ToUpper() == "POST")
      {
        lPostBuffer = new char[pContentLen];
        lTotalBytesRead = 0;
        lSW = new StreamWriter(lWebReq.GetRequestStream());

        while (lTotalBytesRead < pContentLen && (lPOSTBytesRead = pClientStreamReader.ReadBlock(lPostBuffer, 0, pContentLen)) > 0)
        {
          lTmpStr = new string(lPostBuffer);

          lTotalBytesRead += lPOSTBytesRead;
          lSW.Write(lPostBuffer, 0, lPOSTBytesRead);

          lTmpBuf += lTmpStr;
        } // while (lTotalBytes...

        if (lSW != null)
          lSW.Close();

        if (lTmpBuf != null && lTmpBuf.Length > 0)
          lPOSTData = lTmpBuf.ToString();
      } // if (lMethod.To...


      /*
       * Build HTTP request data string
       */
      pHTTPData = pHTTPCommand.Trim();
      foreach (String lKey in lWebReq.Headers.Keys)
        pHTTPData += String.Format("..{0}: {1}", lKey, lWebReq.Headers[lKey]);

      pHTTPData += String.Format("....{0}", lPOSTData);





      /*
       * Send data to client
       */
      //lWebReq.Timeout = 15000;

      try
      {
        lResponse = (HttpWebResponse) lWebReq.GetResponse();
      }
      catch (WebException lEx)
      {
        lResponse = lEx.Response as HttpWebResponse;

        if (Program.cDebuggingOn)
          Console.WriteLine(lEx.Message);
      }

      if (lResponse != null)
      {
        lResponseHeaders = ProcessResponse(lResponse);
        lMyResponseWriter = new StreamWriter(pOutStream);
        lResponseStream = lResponse.GetResponseStream();

        /*
         * Send back HTTP response
         */
        try
        {
          //send the response status and response headers
          WriteResponseStatus(lResponse.StatusCode, lResponse.StatusDescription, lMyResponseWriter);
          WriteResponseHeaders(lMyResponseWriter, lResponseHeaders);


          if (lResponse.ContentLength > 0)
            lBuffer = new Byte[lResponse.ContentLength];
          else
            lBuffer = new Byte[BUFFER_SIZE];



          while ((lBytesRead = lResponseStream.Read(lBuffer, 0, lBuffer.Length)) > 0)
          {
            pOutStream.Write(lBuffer, 0, lBytesRead);
          }

          lResponseStream.Close();
          pOutStream.Flush();
        }
        catch (Exception lEx)
        {
          if (Program.cDebuggingOn)
            Console.WriteLine("Error: {0}", lEx.Message);
        }
        finally
        {
          if (lResponseStream != null)
            lResponseStream.Close();

          if (lResponse != null)
            lResponse.Close();

          if (lMyResponseWriter != null)
            lMyResponseWriter.Close();
        }
      } // if (lResponse !...
    }







    /*
     * 
     * 
     */
    private static void SetHTTPHeaders(Hashtable pHeaders, ref HttpWebRequest pWebReq)
    {
      if (pHeaders != null && pHeaders.Count > 0)
      {
        foreach (String lKey in pHeaders.Keys)
        {
          switch (lKey.ToLower())
          {
            case "host":
              pWebReq.Host = pHeaders[lKey].ToString();
              break;
            case "user-agent":
              pWebReq.UserAgent = pHeaders[lKey].ToString();
              break;
            case "accept":
              pWebReq.Accept = pHeaders[lKey].ToString();
              break;
            case "referer":
              pWebReq.Referer = pHeaders[lKey].ToString();
              break;
            case "cookie":
              pWebReq.Headers["Cookie"] = pHeaders[lKey].ToString();
              break;
            case "proxy-connection":
            case "connection":
            case "keep-alive":
              //ignore these
              break;
            //            case "content-length":
            //              int.TryParse(header[1], out lContentLen);
            //              break;
            case "content-type":
              pWebReq.ContentType = pHeaders[lKey].ToString();
              break;
            case "if-modified-since":
              String[] sb = pHeaders[lKey].ToString().Trim().Split(cSemiSplit);
              DateTime d;
              if (DateTime.TryParse(sb[0], out d))
              {
                pWebReq.IfModifiedSince = d;
              }
              break;
            default:
              try
              {
                pWebReq.Headers.Add(lKey, pHeaders[lKey].ToString());
              }
              catch (Exception ex)
              {
                Console.WriteLine(String.Format("Could not add header {0}.  Exception message:{1}", lKey, ex.Message));
              }
              break;
          }
        } // foreach (Strin...
      } // if (pHeaders !=...
    }





    /*
     * 
     * 
     */
    private static void WriteResponseStatus(HttpStatusCode pCode, String pDescription, StreamWriter pMyResponseWriter)
    {
      String lOut = String.Format("HTTP/1.0 {0} {1}", (Int32)pCode, pDescription);
      pMyResponseWriter.WriteLine(lOut);
    }




    /*
     * 
     * 
     */
    private static void WriteResponseHeaders(StreamWriter pMyResponseWriter, List<Tuple<String, String>> pHeaders)
    {
      if (pHeaders != null)
      {
        foreach (Tuple<String, String> header in pHeaders)
        {
          pMyResponseWriter.WriteLine(String.Format("{0}: {1}", header.Item1, header.Item2));

          if (Program.cDebuggingOn)
            Console.WriteLine(String.Format("HDR: {0}: {1}", header.Item1, header.Item2));
        }
      }
      pMyResponseWriter.WriteLine();
      pMyResponseWriter.Flush();
    }




    /*
     * 
     * 
     */
    private static List<Tuple<String, String>> ProcessResponse(HttpWebResponse pResponse)
    {
      String lValue = null;
      String header = null;
      List<Tuple<String, String>> returnHeaders = new List<Tuple<String, String>>();

      foreach (String lTmp in pResponse.Headers.Keys)
      {
        if (lTmp.ToLower() == "set-cookie")
        {
          header = lTmp;
          lValue = pResponse.Headers[lTmp];
        }
        else
          returnHeaders.Add(new Tuple<String, String>(lTmp, pResponse.Headers[lTmp]));
      }

      if (!String.IsNullOrWhiteSpace(lValue))
      {
        pResponse.Headers.Remove(header);
        String[] lCookies = cCookieSplitRegEx.Split(lValue);

        foreach (String lCookie in lCookies)
          returnHeaders.Add(new Tuple<String, String>("Set-Cookie", lCookie));
      }

      return returnHeaders;
    }




  }
}
