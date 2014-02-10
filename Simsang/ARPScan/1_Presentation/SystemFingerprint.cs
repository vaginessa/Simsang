using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Simsang.ARPScan._1_Presentation
{
  public partial class SystemFingerprint : Form
  {

    #region MEMBERS

    private String mOperatingSystem;
    private String mScanDate;
    private String mMACAddress;
    private String mMACHardwareVendor;
    private String madsf;

    #endregion


    #region PUBLIC

    public SystemFingerprint(String pFile)
    {
      InitializeComponent();

      
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

    #endregion




  }
}
