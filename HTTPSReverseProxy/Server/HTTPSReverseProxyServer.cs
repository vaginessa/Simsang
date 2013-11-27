using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Collections;

using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;



namespace HTTPSReverseProxyServer
{
  public sealed class ProxyServer
  {
    [System.Runtime.InteropServices.DllImport("Iphlpapi.dll", EntryPoint = "SendARP")]
    internal extern static Int32 SendArp(Int32 destIpAddress, Int32 srcIpAddress, byte[] macAddress, ref Int32 macAddressLength);


    private static readonly ProxyServer cServer = new ProxyServer();

    private static readonly int BUFFER_SIZE = 8192;
    private static readonly char[] cSemiSplit = new char[] { ';' };
    private static readonly char[] cEqualSplit = new char[] { '=' };
    private static readonly String[] cColonSpaceSplit = new string[] { ": " };
    private static readonly char[] lSpaceSplit = new char[] { ' ' };
    private static readonly char[] lCommaSplit = new char[] { ',' };
    private static readonly Regex lCookieSplitRegEx = new Regex(@",(?! )");

    private TcpListener cListener;
    private Thread cListenerThread;

    private static int cLocalPort;
    private static int cRemotePort;
    private static String cRemoteHost;
    private static String cRemoteIP;
    private static String cRedirectURL;
    private static X509Certificate cServerCertificate = null;
    private static Hashtable cTargetMACs = new Hashtable();




    /*
     * 
     * 
     */
    public IPAddress ListeningIPInterface
    {
      get { return IPAddress.Any; }
    }
    public int ListeningPort
    {
      get { return cLocalPort; }
    }




    /*
     * 
     * 
     */
    public static ProxyServer Server
    {
      get { return cServer; }
    }


    /*
     * 
     * 
     */
    public bool Start(int pLocalPort, String pRemoteHost, int pRemotePort, String pRedirectURL, String pCertificatePath)
    {
      cLocalPort = pLocalPort;
      cRemoteHost = pRemoteHost;
      cRemotePort = pRemotePort;
      cRedirectURL = pRedirectURL;

      /*
       * Determine remote IP
       */
      IPHostEntry lHostEntry;
      lHostEntry = Dns.GetHostEntry(cRemoteHost);

      if (lHostEntry.AddressList.Length > 0)
        cRemoteIP = lHostEntry.AddressList[0].ToString();
      else
        cRemoteIP = "0.0.0.0";

if (Program.cDebuggingOn)
  Console.WriteLine("Start(0) : remote system {0}/{1}, redirect to: \"{2}\"", cRemoteHost, cRemoteIP, cRedirectURL);


      /*
       * Start listener
       */
      cListener = new TcpListener(ListeningIPInterface, cLocalPort);

      try
      {
        cServerCertificate = new X509Certificate2(pCertificatePath, "");
        cListener.Start();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        return(false);
      }

      cListenerThread = new Thread(new ParameterizedThreadStart(HandleHTTPSClient));
      cListenerThread.Start(cListener);

      return(true);
    }




    /*
     * 
     * 
     */
    public void Stop()
    {
      cListener.Stop();

      //wait for server to finish processing current connections...
      cListenerThread.Abort();
      cListenerThread.Join();
      cListenerThread.Join();
    }




    /*
     * 
     * 
     */
    private static void HandleHTTPSClient(Object obj)
    {
      TcpListener lListener = (TcpListener) obj;
      try
      {
        while (true)
        {
          TcpClient lClient = lListener.AcceptTcpClient();
          while (!ThreadPool.QueueUserWorkItem(new WaitCallback(ProxyServer.ProcessClient), lClient)) ;
        }
      }
      catch (ThreadAbortException) { }
      catch (SocketException) { }
    }



    /*
     * 
     * 
     */
    private static void ProcessClient(Object pObj)
    {
      TcpClient lTCPClient = (TcpClient) pObj;
      Stream lClientStream = lTCPClient.GetStream();
      SslStream lSSLStream = new SslStream(lClientStream, true);
      String lClientIP = String.Empty;
      String lClientPort = String.Empty;
      String lClientMAC = String.Empty;


      // Set timeouts for the read and write to 5 seconds.
      lSSLStream.ReadTimeout = 5000;
      lSSLStream.WriteTimeout = 5000;

      /*
       * Determine client IP and MAC address.
       */
      try
      {
        char lDelim = new char();
        lDelim = ':';
        String[] lWords = lTCPClient.Client.RemoteEndPoint.ToString().Split(lDelim);
        lClientIP = lWords[0];
        lClientPort = lWords[1];
      }
      catch (Exception lEx) 
      { 
        if (Program.cDebuggingOn)
          Console.WriteLine("ProcessClient() : {0}\r\n{1}", lEx.Message, lEx.StackTrace);
      }


      try
      {
        lClientMAC = GetMACFromNetworkComputer(lClientIP);
      }
      catch (Exception)
      {
        lClientMAC = "00:00:00:00:00:00";
      }


      try
      {
        lSSLStream.AuthenticateAsServer(cServerCertificate, false, SslProtocols.Tls | SslProtocols.Ssl3 | SslProtocols.Ssl2, false);
        DoHttpsProcessing(lClientMAC, lClientIP, lClientPort, lSSLStream);
      }
      catch (Exception lEx)
      {
        Console.WriteLine(String.Format("ERROR: {0}\r\n{1}", lEx.Message, lEx.StackTrace));
        return;
      }
      finally
      {
        lTCPClient.Close();
      }
    }




    /*
     * 
     * 
     */
    private static void DoHttpsProcessing(String pSrcMAC, String pSrcIP, String pSrcPort, Stream pClientStream)
    {
      //      Stream lClientStream = client.GetStream();
      Stream lOutStream = pClientStream;
      StreamReader lClientStreamReader = new StreamReader(pClientStream);
      String[] lReqSplitBuffer;
      String lMethod = String.Empty;
      String lRemoteURI = String.Empty;
      String lURL = String.Empty;
      Version lVersion = new Version(1, 0);
      int lContentLen = 0;
      String lHTTPData = String.Empty;
      String lHTTPCommand = String.Empty;
      String lLine = String.Empty;
      String lTmpStr = String.Empty;
      String lTmpBuf = String.Empty;
      Hashtable lHeaders = new Hashtable();

      try
      {
        // Read the first line HTTP command
        lHTTPCommand = lClientStreamReader.ReadLine();

        /*
         * Header corrupted. Stop thread execution.
         */
        if (String.IsNullOrEmpty(lHTTPCommand))
        {
          lClientStreamReader.Close();
          pClientStream.Close();

          return;
        }


        /*
         * Break up the line into three components
         */
        lReqSplitBuffer = lHTTPCommand.Split(lSpaceSplit, 3);
        lMethod = lReqSplitBuffer[0];
        lRemoteURI = lReqSplitBuffer[1];
        lURL = String.Empty;

        // Read the request headers from the client and copy them to our request
        lContentLen = ReadRequestHeaders(lClientStreamReader, ref lHeaders);

        // Build the URI
        if (lHeaders != null && lHeaders.ContainsKey("host") && lHeaders["host"].ToString().Length > 0)
          lURL = string.Format("https://{0}:{1}{2}", lHeaders["host"].ToString(), cRemotePort, lRemoteURI);
        else
          lURL = string.Format("https://{0}:{1}{2}", cRemoteHost, cRemotePort, lRemoteURI);


        /*
         * Redirect URL
         */
        if (!String.IsNullOrEmpty(cRedirectURL) && lMethod.ToLower() == "get")
        {
          HTTPRedirect.getInstance().processRequest(lOutStream, cRedirectURL, lHeaders);


        /*
         * Process HTTP request
         */
        }
        else
        {
          // StreamReader pClientStreamReader, Stream pOutStream, String pMethod, String pURL, Hashtable pHeaders, int pContentLen, String pHTTPCommand, String pPOSTData
          HTTPProxy.getInstance().processRequest(lClientStreamReader, lOutStream, lMethod, lURL, lHeaders, lContentLen, lHTTPCommand, out lHTTPData);

          String lPipeData = String.Format("TCP||{0}||{1}||{2}||{3}||{4}||{5}\r\n", pSrcMAC, pSrcIP, pSrcPort, cRemoteIP, cRemotePort, lHTTPData);
          Program.WriteToPipe(lPipeData);

          if (Program.cDebuggingOn)
            Console.WriteLine(lPipeData.TrimEnd());
        } // if (!String.IsNullOrEmpty(cR...         
      }
      catch (Exception lEx)
      {
        if (Program.cDebuggingOn) 
          Console.WriteLine(String.Format("ERROR: {0}\r\n{1}", lEx.Message, lEx.StackTrace));

        return;
      }
      finally
      {
        if (lClientStreamReader != null)
          lClientStreamReader.Close();

        if (pClientStream != null)
          pClientStream.Close();

        if (lOutStream != null)
          lOutStream.Close();
      }
    }





    /*
     * 
     * 
     */
    private static int ReadRequestHeaders(StreamReader pSR, /*ref HttpWebRequest pWebReq,*/ ref Hashtable pHeaders)
    {
      String lHTTPCmd;
      int lContentLen = 0;

      do
      {
        lHTTPCmd = pSR.ReadLine();
        if (String.IsNullOrEmpty(lHTTPCmd))
          return lContentLen;

        String[] header = lHTTPCmd.Split(cColonSpaceSplit, 2, StringSplitOptions.None);
        switch (header[0].ToLower())
        {
          case "host":
//            pWebReq.Host = header[1];
            pHeaders.Add("host", header[1]);
            break;
          case "user-agent":
//            pWebReq.UserAgent = header[1];
            pHeaders.Add("user-agent", header[1]);
            break;
          case "accept":
//            pWebReq.Accept = header[1];
            pHeaders.Add("accept", header[1]);
            break; 
          case "referer":
//            pWebReq.Referer = header[1];
            pHeaders.Add("referer", header[1]);
            break;
          case "cookie":
//            pWebReq.Headers["Cookie"] = header[1];
            pHeaders.Add("cookie", header[1]);
            break;
          case "proxy-connection":
          case "connection":
          case "keep-alive":
            //ignore these
            break;
          case "content-length":
            int.TryParse(header[1], out lContentLen);
            pHeaders.Add("content-length", header[1]);
            break;
          case "content-type":
//            pWebReq.ContentType = header[1];
            pHeaders.Add("content-type", header[1]);
            break;
          case "if-modified-since":
            String[] sb = header[1].Trim().Split(cSemiSplit);
            DateTime d;
            if (DateTime.TryParse(sb[0], out d))
            {
//              pWebReq.IfModifiedSince = d;
              pHeaders.Add("if-modified-since", header[1]);
            }
            break;
          default:
            try
            {
//              pWebReq.Headers.Add(header[0], header[1]);
              pHeaders.Add(header[0], header[1]);
            }
            catch (Exception ex)
            {
              Console.WriteLine(String.Format("Could not add header {0}.  Exception message:{1}", header[0], ex.Message));
            }
            break;
        }
      }
      while (!String.IsNullOrWhiteSpace(lHTTPCmd));


      // It is ALWAYS host cRemoteHost
      /*
      if (pWebReq.Host.Length <= 0)
        pWebReq.Host = cRemoteHost;
      */

      return(lContentLen);
    }



    /*
     * 
     * 
     */
    private static String GetMACFromNetworkComputer(String pClientIP)
    {
      String lRetVal = String.Empty;
      Int32 lConvertedIPAddr = 0;
      byte[] lMACArray;
      int lByteArrayLen = 0;
      int lARPReply = 0;
      IPAddress lIPAddress = null;


      /*
       * First check the local cache if IP/MAC entry exists.
       */
      if (cTargetMACs.ContainsKey(pClientIP))
        return (cTargetMACs[pClientIP].ToString());


      /*
       * Target MAC is not in local cache. Send ARP request. 
       */
      lIPAddress = IPAddress.Parse(pClientIP);

      if (lIPAddress.AddressFamily != AddressFamily.InterNetwork)
        throw new ArgumentException("The remote system only supports IPv4 addresses");

      lConvertedIPAddr = ConvertIPToInt32(lIPAddress);
      lMACArray = new byte[6]; // 48 bit
      lByteArrayLen = lMACArray.Length;

      if ((lARPReply = SendArp(lConvertedIPAddr, 0, lMACArray, ref lByteArrayLen)) != 0)
        throw new Exception(String.Format("Error no. {0} occured while resolving MAC address of system {1}",
                                           lARPReply, pClientIP));


      //return the MAC address in a PhysicalAddress format
      for (int i = 0; i < lMACArray.Length; i++)
      {
        lRetVal += String.Format("{0}", lMACArray[i].ToString("X2"));
        lRetVal += (i != lMACArray.Length - 1) ? "-" : "";
      } // for (in...

      // Add IP/MAC to cache.
      cTargetMACs.Add(pClientIP, lRetVal);

      return (lRetVal);
    }




    /*
     * 
     * 
     */
    private static Int32 ConvertIPToInt32(IPAddress pIPAddr)
    {
      byte[] lByteAddress = pIPAddr.GetAddressBytes();
      return BitConverter.ToInt32(lByteAddress, 0);
    }



  }
}
