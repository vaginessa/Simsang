using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Windows.Forms;
using System.Configuration;
using System.Collections.Generic;
using System.Diagnostics;

using NConsoler;




namespace IMAP4ReverseProxyServer
{
  class Program
  {
    public static Boolean cDebuggingOn = false;
    public static String cPipeName = "Simsang";
    private static NamedPipeClientStream cPipeClient = null;
    private static StreamWriter cPipeWriter = null;


    static void Main(string[] args)
    {
      Console.Clear();
      Console.WriteLine("{0} 0.1 (www.megapanzer.com)", Application.ProductName);
      Console.WriteLine("------------------------------------------------\n\n");
      Consolery.Run(typeof(Program), args);
    }





    /*
     * IMAP4ReverseProxyServer.exe 80 /rhp:imap.google.com; /d
     */

    [Action]
    public static void StartServer(
            [Required(Description = "Local port")] String LocalPort,
            [Optional("", "rhp", Description = "RemoteHost;port")] String RemoteHostPort,
            [Optional(false, "d")] bool debug)
    {
      int lLocalPort = 0;
      int lRemotePort = 0;
      String lRemoteHost = String.Empty;
      String lRedirectTo = String.Empty;
      Program.cDebuggingOn = debug;

      Console.WriteLine("Starting IMAP4 server {0}, debugging {1} ...", RemoteHostPort, Program.cDebuggingOn);



      /*
       * 
       */
      try
      {
        lLocalPort = Int32.Parse(LocalPort);

        if (RemoteHostPort != null && RemoteHostPort.Length > 0)
        {
          char[] lSeparator = { ':', '/', ';' };
          String[] lSplitter = RemoteHostPort.Split(lSeparator);

          lRemoteHost = lSplitter[0];
          lRemotePort = Int32.Parse(lSplitter[1]);
        }

        Console.WriteLine(String.Format("RemoteHostPort: {0} {1}", lRemoteHost, lRemotePort));
        Console.WriteLine("host: {0} ", lRemoteHost);
      }
      catch (Exception lEx)
      {
        Console.WriteLine("Somethin went wrong : {0}", lEx.ToString());
        return;
      }


      /*
       * Start proxy server
       */
      try
      {

        if (IMAP4ReverseProxyServer.IMAP4SReverseProxy.Server.Start(lLocalPort, lRemoteHost, lRemotePort, lRedirectTo))
        {
          Console.WriteLine("Press enter to exit");
          Console.ReadLine();
          IMAP4ReverseProxyServer.IMAP4SReverseProxy.Server.Stop();
        }
        else
          Console.WriteLine("Something went wrong");

      }
      catch (Exception lEx)
      {
        Console.WriteLine("Something went wrong : {0}", lEx.ToString());
      }
    }



    /*
     * 
     * 
     */
    public static bool WriteToPipe(String pData)
    {
      bool lRetVal = false;

      /*
       * Create Pipe 
       */
      try
      {
        if (cPipeClient == null)
          if ((cPipeClient = new NamedPipeClientStream(cPipeName)) != null)
            cPipeClient.Connect(500);

        if (cPipeClient != null && (cPipeWriter = new StreamWriter(cPipeClient)) != null)
          cPipeWriter.AutoFlush = true;

        if (cPipeClient != null && cPipeWriter != null && cPipeClient.IsConnected && cPipeClient.CanWrite)
        {
          if (pData.Length > 0)
          {
            String lTemp = pData.Trim();
            cPipeWriter.WriteLine(lTemp);
          } // if (pDat...

          lRetVal = true;
        }
        else
        {
          cPipeClient = null;
          cPipeWriter = null;
        } // if (cPipe...

      }
      catch (Exception lEx)
      {
        Console.WriteLine("WriteToPipe() : " + lEx.Message);
      }


      return (lRetVal);
    }

  }
}

