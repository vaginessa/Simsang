using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;
using System.Windows.Forms;

using Simsang.ARPScan.Main.Config;


namespace Simsang.ARPScan.SystemFingerprint
{
  public partial class SystemFingerprint : Form
  {

    #region MEMBERS

    private TaskFacadeFingerprint cTaskFingerprint;
    private String mOperatingSystem;
    private String mScanDate;
    private String mMACAddress;
    private String mIPAddress;
    private String mMACHardwareVendor;

    #endregion


    #region PUBLIC

    public SystemFingerprint(String pMAC, String pIP, String pHWVendor)
    {
      InitializeComponent();

      mMACAddress = pMAC;
      mMACHardwareVendor = pHWVendor;
      mIPAddress = pIP;
      cTaskFingerprint = TaskFacadeFingerprint.getInstance();

      this.Text = String.Format("{0} / {1}", pIP, pMAC);
      loadSystemDetails();
    }

    #endregion


    #region EVENTS

    /// <summary>
    /// Close Sessions GUI on Escape.
    /// </summary>
    /// <param name="keyData"></param>
    /// <returns></returns>
    protected override bool ProcessDialogKey(Keys keyData)
    {
      if (keyData == Keys.Escape)
      {
        this.Close();
        return true;
      }
      else
        return base.ProcessDialogKey(keyData);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SystemFingerprint_FormClosing(object sender, FormClosingEventArgs e)
    {
      // Save note
      if (TB_Note.Text.Length > 0)
        cTaskFingerprint.setSystemNote(mMACAddress, TB_Note.Text);

      // Hiding form
      this.Hide();

      e.Cancel = true;
    }


    #endregion


    #region PRIVATE

    /// <summary>
    /// 
    /// </summary>
    public delegate void loadSystemDetailsDelegate();
    private void loadSystemDetails()
    {
      if (InvokeRequired)
      {
        BeginInvoke(new loadSystemDetailsDelegate(loadSystemDetails), new object[] { });
        return; 
      }

      SystemDetails lSysDetails;
      int lCount = 0;
      String lPorts = "\r\n";
      String lOSGuess = "\r\n";
      String lNote = String.Empty;

      lSysDetails = cTaskFingerprint.getSystemDetails(mMACAddress);
      lNote = cTaskFingerprint.getFingerprintNote(mMACAddress);

      TB_HWVendor.Text = mMACHardwareVendor;
      TB_MAC.Text = mMACAddress;
      TB_ScanDate.Text = lSysDetails.ScanDate;

      /*
       * Listing ports
       */
      foreach (var entry in lSysDetails.OpenPorts)
      {
        if (lCount >= 7)
          break;

        lPorts += String.Format("   {0}/{1,-5} {2}\r\n", entry.Protocol, entry.PortNo, entry.ServiceName);
        lCount++;
      } // foreach(va...

      /*
       * OS guess
       */
      foreach (var entry in lSysDetails.OSGuess)
        lOSGuess += String.Format(" {0} ({1}%)\r\n", entry.OSName, entry.Accuracy);


      TB_OSGuess.Text = lOSGuess;
      TB_OpenPorts.Text = lPorts;
      TB_Note.Text = lNote;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pMAC"></param>
    /// <returns></returns>
    private String getFileNameByMAC(String pMAC)
    {
      String lOutputFileName = String.Empty;
      String lFingerprintDir = String.Empty;
      String lMACAddr = String.Empty;

      if (!String.IsNullOrEmpty(pMAC))
      {
        lFingerprintDir = String.Format(@"{0}\{1}", Directory.GetCurrentDirectory(), Simsang.Config.FingerprintDir);
        lMACAddr = Regex.Replace(pMAC, @"[^\d\w]", "");
        lOutputFileName = String.Format(@"{0}\{1}.xml", lFingerprintDir, lMACAddr);
      } // if (!Strin...

      return (lOutputFileName);
    }

    #endregion

  }
}
