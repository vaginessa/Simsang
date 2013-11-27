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
using CertificateHandler;




namespace IMAP4SReverseProxyServer
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
     * IMAP4ReverseProxyServer.exe 80 /rhp:imap4s.google.com; /d
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
      String lCertStartDate;
      String lCertStopDate;
      String lCertificatePath;

      Console.WriteLine("Starting IMAP4S server with Certificate {0}, debugging {1} ...", RemoteHostPort, Program.cDebuggingOn);



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

        lCertificatePath = String.Format("{0}.crt", lRemoteHost);
        Console.WriteLine(String.Format("RemoteHostPort: {0} {1}", lRemoteHost, lRemotePort));
        Console.WriteLine("host: {0}, cert: {1} ", lRemoteHost, lCertificatePath);
      }
      catch (Exception lEx)
      {
        Console.WriteLine("Somethin went wrong : {0}", lEx.ToString());
        return;
      }



      /*
       * Create certificate if it doesnt exist.
       */
      if (!File.Exists(lCertificatePath))
      {
        lCertStartDate = GetTimestamp(DateTime.Now);
        lCertStopDate = DateTime.Now.AddYears(3).AddDays(-1).ToString();
        Console.WriteLine("Crt path: {0}, Rem host: {1}, Start: {2}, Stop: {3}", lCertificatePath, lRemoteHost, lCertStartDate, lCertStopDate);
        MakeCert(lCertificatePath, lRemoteHost, lCertStartDate, lCertStopDate);
      }



      /*
       * 
       */
      if (Program.cDebuggingOn)
      {
        X509Certificate2 lCert = new X509Certificate2(lCertificatePath, "");
        Console.WriteLine("Issuer name : {0}", lCert.Issuer);
        Console.WriteLine("Cert. start : {0}", lCert.GetEffectiveDateString());
        Console.WriteLine("Cert. stop  : {0}", lCert.GetExpirationDateString());
      } // if (Progra...



      /*
       * Start proxy server
       */
      try
      {

        if (IMAP4SReverseProxyServer.IMAP4SReverseProxy.Server.Start(lLocalPort, lRemoteHost, lRemotePort, lRedirectTo, lCertificatePath))
        {
          Console.WriteLine("Press enter to exit");
          Console.ReadLine();
          IMAP4SReverseProxyServer.IMAP4SReverseProxy.Server.Stop();
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








    /*
     * 
     * 
     */
    static String GetTimestamp(DateTime value)
    {
      return value.ToString("yyyy-MM-dd");
    }



    /*
     * 
     * 
     */
    public static void MakeCert(String pCertificatePath, String pHostName, String pStartDate, String pStopDate)
    {
      Console.WriteLine("Creating certificate ({0}) for {1} ...", pCertificatePath, pHostName);

      try
      {
        byte[] c = CertificateHandler.Certificate.CreateSelfSignCertificatePfx(
                String.Format("CN={0}", pHostName), //"CN=localhost", //host name
                DateTime.Parse(pStartDate), //not valid before
                DateTime.Parse(pStopDate), //not valid after
                ""); //password to encrypt key file
        using (BinaryWriter binWriter = new BinaryWriter(File.Open(pCertificatePath, FileMode.Create)))
        {
          binWriter.Write(c);
        }
      }
      catch (Exception lEx)
      {
        Console.WriteLine("Something went wrong : " + lEx.ToString());
      }
    }


  }
}
