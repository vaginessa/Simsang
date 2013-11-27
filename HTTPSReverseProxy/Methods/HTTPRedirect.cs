using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;


namespace HTTPSReverseProxyServer
{
  class HTTPRedirect
  {
    private static HTTPRedirect cInstance;



    /*
     * 
     * 
     */
    private HTTPRedirect()
    {
    }


    /*
     * 
     * 
     */
    public static HTTPRedirect getInstance()
    {
      if (cInstance == null)
        cInstance = new HTTPRedirect();

      return (cInstance);
    }



    /*
     * 
     * 
     */
    public void processRequest(Stream pServerStream, String pRedirectURL, Hashtable pHeaders)
    {
      String lOutput = String.Format("HTTP/1.1 301 Found\nLocation: http://{0}\r\n\r\n", pRedirectURL);
      byte[] lRedirectBytesArray = Encoding.ASCII.GetBytes(String.Format("HTTP/1.1 301 Found\nLocation: http://{0}\r\n\r\n", pRedirectURL));
      int lTotalBytes = lRedirectBytesArray.Length;


      /*
       * Print headers
       */
      if (Program.cDebuggingOn && pHeaders != null && pHeaders.Count > 0)
        foreach (String lKey in pHeaders.Keys)
          Console.WriteLine(String.Format("HDR: {0}: {1}", lKey, pHeaders[lKey]));

      /*
       * Send back redirect code
       */
      pServerStream.Write(lRedirectBytesArray, 0, lTotalBytes);
      pServerStream.Flush();


      if (Program.cDebuggingOn)
        Console.WriteLine("RedirectURL: {0}", lOutput);
    }

  }
}
