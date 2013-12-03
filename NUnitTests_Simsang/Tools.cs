using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Security.Principal;

namespace NUnitTests
{



  /*
   * 
   * 
   */
  static public class TopMostMessageBox
  {
    static public DialogResult Show(string message)
    {
      return Show(message, string.Empty, MessageBoxButtons.OK);
    }

    static public DialogResult Show(string message, string title)
    {
      return Show(message, title, MessageBoxButtons.OK);
    }

    static public DialogResult Show(string message, string title, MessageBoxButtons buttons)
    {
      // Create a host form that is a TopMost window which will be the 
      // parent of the MessageBox.
      Form topmostForm = new Form();
      // We do not want anyone to see this window so position it off the 
      // visible screen and make it as small as possible
      topmostForm.Size = new System.Drawing.Size(1, 1);
      topmostForm.StartPosition = FormStartPosition.Manual;
      System.Drawing.Rectangle rect = SystemInformation.VirtualScreen;
      topmostForm.Location = new System.Drawing.Point(rect.Bottom + 10, rect.Right + 10);
      topmostForm.Show();
      // Make this form the active form and make it TopMost
      topmostForm.Focus();
      topmostForm.BringToFront();
      topmostForm.TopMost = true;
      // Finally show the MessageBox with the form just created as its owner
      DialogResult result = MessageBox.Show(topmostForm, message, title, buttons);
      topmostForm.Dispose(); // clean it up all the way

      return result;
    }
  }




  public static class Tools
  {

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pFileName"></param>
    public static void removeFile(String pFileName)
    {
      try
      {
        if (!String.IsNullOrEmpty(pFileName) && File.Exists(pFileName))
          File.Delete(pFileName);
      }
      catch (Exception) { }
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="pProcName"></param>
    /// <returns></returns>
    public static bool processExists(String pProcName)
    {
      bool lRetVal = false;
      if (!String.IsNullOrEmpty(pProcName))
      {
        try
        {
          if (Process.GetProcessesByName(pProcName).Count() > 0)
            lRetVal = true;
        }
        catch (Exception)
        { }
      } // if (!String...

      return (lRetVal);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pCWD"></param>
    /// <param name="pCommand"></param>
    /// <param name="pArguments"></param>
    /// <returns></returns>
    public static bool startProcess(String pCWD, String pCommand, String pArguments)
    {
      bool lRetVal = false;
      ProcessStartInfo lSI = new ProcessStartInfo();

	     lSI.FileName = pCommand;
	     lSI.Arguments = pArguments;
      lSI.WorkingDirectory = pCWD;
      lSI.CreateNoWindow = false;
      lSI.UseShellExecute = true;

      try
      {
        if (!String.IsNullOrEmpty(pCommand))
          System.Diagnostics.Process.Start(lSI);
      }
      catch (Exception lEx)
      {
        MessageBox.Show(String.Format("{0}\r\n{1}", lEx.Message, lEx.StackTrace));
      }

      return(lRetVal);
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="pProcName"></param>
    public static void killProcessByName(String pProcName)
    {
      if (!String.IsNullOrEmpty(pProcName))
      {
        foreach (Process process in Process.GetProcessesByName(pProcName))
        {
          try
          {
            process.Kill();
          }
          catch (Exception)
          { }
        } // foreach (...
      } // if (!String...
    }




    /// <summary>
    /// 
    /// </summary>
    /// <param name="pRemoteIPAddres"></param>
    /// <returns></returns>
    public static String GetRandomNICIdentifier(String pRemoteIPAddres)
    {
      String lRetVal = String.Empty;

      UdpClient u = new UdpClient(pRemoteIPAddres, 1);
      IPAddress localAddr = ((IPEndPoint)u.Client.LocalEndPoint).Address;


      foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
      {
        if (nic.OperationalStatus == OperationalStatus.Up && !String.IsNullOrEmpty(nic.Id))
        {
          lRetVal = nic.Id;
          break;
        } // if (!Str...
      } // foreach (Net...

      return (lRetVal);
    }




    /// <summary>
    /// 
    /// </summary>
    /// <param name="pDstIP"></param>
    /// <param name="pDstPort"></param>
    /// <param name="pData"></param>
    public static void sendUdpPacket(String pDstIP, int pDstPort, String pData)
    {
      byte[] lData;
      UdpClient lSender = null;
      
      try
      {
        lData = Encoding.ASCII.GetBytes(pData);
        lSender = new UdpClient();
        lSender.Connect(new IPEndPoint(IPAddress.Parse(pDstIP), pDstPort));
        int bytesSent = lSender.Send(lData, lData.Length);
      }
      finally
      {
        if (lSender != null)
          lSender.Close();
      }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pRemoteIP"></param>
    /// <returns></returns>
    public static bool isSystemAlive(String pRemoteIP)
    {
      bool lRetVal = false;
      Ping pingSender = new Ping();
      PingOptions options = new PingOptions();
      string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
      byte[] buffer = Encoding.ASCII.GetBytes(data);
      int timeout = 120;

      options.DontFragment = true;

      if (((PingReply)pingSender.Send(pRemoteIP, timeout, buffer, options)).Status == IPStatus.Success)
        lRetVal = true;

      return(lRetVal);
    }



    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static bool IsAdmin()
    {
      WindowsIdentity wi = WindowsIdentity.GetCurrent();
      WindowsPrincipal wp = new WindowsPrincipal(wi);

      return (wp.IsInRole(WindowsBuiltInRole.Administrator));
    }
  }
}
