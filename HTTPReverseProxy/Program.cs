using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using System.IO.Pipes;
using System.Windows.Forms;

using NConsoler;



namespace HTTPReverseProxyServer
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
      Console.WriteLine("-----------------------------------------------\n\n");
      Consolery.Run(typeof(Program), args);
    }

    /*
     * HTTPReverseProxyServer.exe 80 /ru:www.test.ch/www/adsf /d
     * HTTPReverseProxyServer.exe 80 /rhp:www.blick.ch;80 /d
     */

    [Action]
    public static void StartServer(
//            [Required(Description = "Local port")] String LocalPort,
//            [Required(Description = "Remote host")] String RemoteHost,
//            [Required(Description = "Remote port")] String RemotePort,
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

        Console.WriteLine(String.Format("RemoteHostPort: {0} {1}", lRemoteHost, lRemotePort));
        Console.WriteLine(String.Format("RedirectTo: {0}", RedirectTo));
      }
      catch (Exception lEx)
      {
        Console.WriteLine("Exception : {0}", lEx.ToString());
        return;
      }



      /*
       * Start proxy server
       */
      try
      {
        if (ProxyServer.Server.Start(lLocalPort, lRemoteHost, lRemotePort, lRedirectTo))
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
        Console.WriteLine("WriteToPipe() : " + lEx.ToString());
      }


      return (lRetVal);
    }




  }
}
