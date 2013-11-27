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




namespace HTTPSReverseProxyServer
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
      Console.WriteLine("{0} 0.4 (www.megapanzer.com)", Application.ProductName);
      Console.WriteLine("------------------------------------------------\n\n");
      Consolery.Run(typeof(Program), args);
    }


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



    /*
     * HTTPReverseProxyServer.exe 80 /ru:www.test.ch/www/adsf /d
     * HTTPReverseProxyServer.exe 80 /rhp:www.blick.ch;80 /d
     */

    [Action]
    public static void StartServer(
//            [Required(Description = "Local port")] string LocalPort,
//            [Required(Description = "Remote host")] string RemoteHost,
//            [Required(Description = "Remote port")] string RemotePort,
//            [Optional(false, "d")] bool debug)

            [Required(Description = "Local port")] String LocalPort,
            [Optional("", "rhp", Description = "RemoteHost;port")] String RemoteHostPort,
            [Optional("", "ru", Description = "Redirect to URL")] String RedirectTo,
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

      Console.WriteLine("Starting HTTPS server with Certificate {0}, debugging {1} ...", RemoteHostPort, Program.cDebuggingOn);



      /*
       * 
       */
      try
      {
        lLocalPort = Int32.Parse(LocalPort);

        if (RemoteHostPort != null && RemoteHostPort.Length > 0)
        {
          char[] lSeparator = {':', '/', ';'};
          String[] lSplitter = RemoteHostPort.Split(lSeparator);

          lRemoteHost = lSplitter[0];
          lRemotePort = Int32.Parse(lSplitter[1]);
        }
        else if (RedirectTo != null && RedirectTo.Length > 0)
        {
          lRedirectTo = RedirectTo;
        }

        lCertificatePath = String.Format("{0}.crt", lRemoteHost);
        Console.WriteLine(String.Format("RemoteHostPort: {0} {1}", lRemoteHost, lRemotePort));
        Console.WriteLine(String.Format("RedirectTo: {0}", RedirectTo));
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
        if (ProxyServer.Server.Start(lLocalPort, lRemoteHost, lRemotePort, lRedirectTo, lCertificatePath))
        {
          Console.WriteLine("Press enter to exit");
          Console.ReadLine();
          ProxyServer.Server.Stop();
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
    static void DisplaySecurityLevel(SslStream stream)
    {
      Console.WriteLine("Cipher: {0} strength {1}", stream.CipherAlgorithm, stream.CipherStrength);
      Console.WriteLine("Hash: {0} strength {1}", stream.HashAlgorithm, stream.HashStrength);
      Console.WriteLine("Key exchange: {0} strength {1}", stream.KeyExchangeAlgorithm, stream.KeyExchangeStrength);
      Console.WriteLine("Protocol: {0}", stream.SslProtocol);
    }


    /*
     * 
     * 
     */
    static void DisplaySecurityServices(SslStream stream)
    {
      Console.WriteLine("Is authenticated: {0} as server? {1}", stream.IsAuthenticated, stream.IsServer);
      Console.WriteLine("IsSigned: {0}", stream.IsSigned);
      Console.WriteLine("Is Encrypted: {0}", stream.IsEncrypted);
    }


    /*
     * 
     * 
     */
    static void DisplayStreamProperties(SslStream stream)
    {
      Console.WriteLine("Can read: {0}, write {1}", stream.CanRead, stream.CanWrite);
      Console.WriteLine("Can timeout: {0}", stream.CanTimeout);
    }


    /*
     * 
     * 
     */
    static void DisplayCertificateInformation(SslStream stream)
    {
      Console.WriteLine("Certificate revocation list checked: {0}", stream.CheckCertRevocationStatus);

      X509Certificate localCertificate = stream.LocalCertificate;
      if (stream.LocalCertificate != null)
      {
        Console.WriteLine("Local cert was issued to {0} and is valid from {1} until {2}.",
            localCertificate.Subject,
            localCertificate.GetEffectiveDateString(),
            localCertificate.GetExpirationDateString());
      }
      else
      {
        Console.WriteLine("Local certificate is null.");
      }
      // Display the properties of the client's certificate.
      X509Certificate remoteCertificate = stream.RemoteCertificate;
      if (stream.RemoteCertificate != null)
      {
        Console.WriteLine("Remote cert was issued to {0} and is valid from {1} until {2}.",
            remoteCertificate.Subject,
            remoteCertificate.GetEffectiveDateString(),
            remoteCertificate.GetExpirationDateString());
      }
      else
      {
        Console.WriteLine("Remote certificate is null.");
      }
    }





  }
}
