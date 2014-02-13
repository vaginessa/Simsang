using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Text.RegularExpressions;

namespace Simsang.ARPScan.Main
{
  public class TargetRecord : INotifyPropertyChanged
  {

    #region MEMBERS

    private String mIP;
    private String mMAC;
    private bool mStatus;
    private String mVendor;
    private Button mButton;
    private String mLastScanDate;
    private String mNote;
    public event PropertyChangedEventHandler PropertyChanged;

    #endregion


    #region PUBLIC

    public TargetRecord(String pIP, String pMAC, String pVendor, String pLastScanDate, String pNote)
    {
      mIP = pIP;
      mMAC = pMAC;
      mVendor = pVendor;
      mStatus = false;
      mButton = new Button() { Text = "Scan", Width=40, Height= 20 };
      mButton.Text = "Scan";
      mLastScanDate = pLastScanDate;
      mNote = pNote;
    }

    #endregion


    #region PROPERTIES

    public String IP
    {
      get { return mIP; }
      set
      {
        mIP = value;
        this.NotifyPropertyChanged("IP");
      }
    }


    public String MAC
    {
      get { return mMAC; }
      set
      {
        mMAC = value;
        this.NotifyPropertyChanged("MAC");
      }
    }


    public String Vendor
    {
      get { return mVendor; }
      set
      {
        mVendor = value;
      }
    }



    public bool Status
    {
      get { return mStatus; }
      set
      {
        mStatus = value;
        this.NotifyPropertyChanged("Status");
      }
    }


    public Button Fingerprint
    {
      get { return mButton; }
      set
      {
        mButton = value;
        this.NotifyPropertyChanged("Fingerprint");
      }
    }


    public String LastScanDate
    {
      get { return mLastScanDate; }
      set
      {
        mLastScanDate = value;
      }
    }


    public String Note
    {
      get { return mNote; }
      set
      {
        mNote = value;
      }
    }

    #endregion


    #region PRIVATE

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pName"></param>
    private void NotifyPropertyChanged(String pName)
    {
      if (PropertyChanged != null)
        PropertyChanged(this, new PropertyChangedEventArgs(pName));
    }

    #endregion

  }
}
