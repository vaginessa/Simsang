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





namespace Plugin.Main.HTTPProxy
{
  public class Account : INotifyPropertyChanged
  {

    #region MEMBERS

    private String mSrcMAC;
    private String mSrcIP;
    private String mDstIP;
    private String mDstPort;
    private String mUsername;
    private String mPassword;

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion


    #region PUBLIC

    public Account()
    {
      mSrcMAC = String.Empty;
      mSrcIP = String.Empty;
      mDstIP = String.Empty;
      mDstPort = String.Empty;
      mUsername = String.Empty;
      mPassword = String.Empty;
    }

    public Account(String pSrcMAC, String pSrcIP, String pDstIP, String pDstPort, String pUsername, String pPassword)
    {
      mSrcMAC = pSrcMAC;
      mSrcIP = pSrcIP;
      mDstIP = pDstIP;
      mDstPort = pDstPort;
      mUsername = pUsername;
      mPassword = pPassword;
    }

    #endregion


    #region PROPERTIES

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


    public String DstIP
    {
      get { return mDstIP; }
      set
      {
        mDstIP = value;
        this.NotifyPropertyChanged("DstIP");
      }
    }


    public String DstPort
    {
      get { return mDstPort; }
      set
      {
        mDstPort = value;
        this.NotifyPropertyChanged("DstPort");
      }
    }

    public String Username
    {
      get { return mUsername; }
      set
      {
        mUsername = value;
        this.NotifyPropertyChanged("Username");
      }
    }


    public String Password
    {
      get { return mPassword; }
      set
      {
        mPassword = value;
        this.NotifyPropertyChanged("Password");
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
