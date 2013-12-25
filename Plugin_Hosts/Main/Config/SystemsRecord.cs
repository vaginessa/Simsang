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





namespace Plugin.Main.Systems.Config
{
  public class SystemRecord : INotifyPropertyChanged
  {

    #region MEMBERS

    private String mSrcMAC;
    private String mSrcIP;
    private String mUserAgent;
    private String mOperatingSystem;
    private String mHWVendor;
    private String mLastSeen;
    private String mID;

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion


    public SystemRecord()
    {
      mSrcMAC = String.Empty;
      mSrcIP = String.Empty;
      mUserAgent = String.Empty;
      mOperatingSystem = String.Empty;
      mHWVendor = String.Empty;
      mLastSeen = String.Empty;
      mID = String.Empty;
    }

    public SystemRecord(String pSrcMAC, String pSrcIP, String pUserAgentString, String pHWVendor, String pOperatingSystem, String pLastSeen)
    {
      mSrcMAC = pSrcMAC;
      mSrcIP = pSrcIP;
      mUserAgent = pUserAgentString;
      mOperatingSystem = pOperatingSystem;
      mHWVendor = pHWVendor;
      mLastSeen = !String.IsNullOrEmpty(pLastSeen) ? pLastSeen : DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
////      mID = String.Format("{0}{1}{2}", pSrcMAC, pSrcMAC, pUserAgentString);
//      mID = String.Format("{0}{1}{2}", pSrcMAC, pSrcIP, pUserAgentString);      

      mSrcMAC = Regex.Replace(mSrcMAC, @"-", ":");
      mID = String.Format("{0}{1}", pSrcMAC.ToLower(), pSrcIP);
    }


    public String SrcMAC
    {
      get { return mSrcMAC; }
      set
      {
        mSrcMAC = value;
        this.NotifyPropertyChanged("SrcMAC");
      }
    }


    public String SrcIP
    {
      get { return mSrcIP; }
      set
      {
        mSrcIP = value;
        this.NotifyPropertyChanged("SrcIP");
      }
    }



    public String OperatingSystem
    {
      get { return mOperatingSystem; }
      set
      {
        mOperatingSystem = value;
        this.NotifyPropertyChanged("OperatingSystem");
      }
    }



    public String UserAgent
    {
      get { return mUserAgent; }
      set
      {
        mUserAgent = value;
        this.NotifyPropertyChanged("UserAgent");
      }
    }



    public String LastSeen
    {
      get { return mLastSeen; }
      set
      {
        mLastSeen = value;
        this.NotifyPropertyChanged("LastSeen");
      }
    }


    public String HWVendor
    {
      get { return mHWVendor; }
      set
      {
        mHWVendor = value;
        this.NotifyPropertyChanged("HWVendor");
      }
    }


    public String ID
    {
      get { return mID; }
      set
      {
        mID = value;
        this.NotifyPropertyChanged("ID");
      }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pName"></param>
    private void NotifyPropertyChanged(String pName)
    {
      if (PropertyChanged != null)
        PropertyChanged(this, new PropertyChangedEventArgs(pName));
    }
  }
}
