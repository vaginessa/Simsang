using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Plugin.Main.IMAP4Proxy.Config
{

  public class IMAP4Account : INotifyPropertyChanged
  {

    #region MEMBERS

    private String mSrcMAC;
    private String mSrcIP;
    private String mDstIP;
    private String mDstPort;
    private String mUsername;
    private String mPassword;
    private String mServer;

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion


    #region PUBLIC

    public IMAP4Account()
    {
      mSrcMAC = String.Empty;
      mSrcIP = String.Empty;
      mDstIP = String.Empty;
      mDstPort = String.Empty;
      mUsername = String.Empty;
      mPassword = String.Empty;
      mServer = String.Empty;
    }

    public IMAP4Account(String pSrcMAC, String pSrcIP, String pDstIP, String pDstPort, String pUsername, String pPassword, String pServer)
    {
      mSrcMAC = pSrcMAC;
      mSrcIP = pSrcIP;
      mDstIP = pDstIP;
      mDstPort = pDstPort;
      mUsername = pUsername;
      mPassword = pPassword;
      mServer = pServer;
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

    public String Server
    {
      get { return mServer; }
      set
      {
        mServer = value;
        this.NotifyPropertyChanged("Server");
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

