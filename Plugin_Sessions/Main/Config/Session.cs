using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Plugin.Main.Session.Config
{


  public class Session : INotifyPropertyChanged
  {

    #region MEMBERS

    private String mTimeStamp;
    private String mSrcMAC;
    private String mSrcIP;
    private String mURL;
    private String mDstPort;
    private String mSessionCookies;
    private String mBrowser;
    private String mGroup;
    private String mID;
    public event PropertyChangedEventHandler PropertyChanged;

    #endregion


    #region PUBLIC

    public Session()
    {
      mTimeStamp = String.Empty;
      mSrcMAC = String.Empty;
      mSrcIP = String.Empty;
      mURL = String.Empty;
      mDstPort = String.Empty;
      mSessionCookies = String.Empty;
      mBrowser = String.Empty;
      mGroup = String.Empty;
      mID = String.Empty;
    }

    public Session(String pSrcMAC, String pSrcIP, String pURL, String pDstPort, String pSessionCookies, String pBrowser, String pGroup)
    {
      mID = String.Format("{0}{1}{2}", pURL.Trim(), pDstPort.Trim(), pSessionCookies.Trim());
      mTimeStamp = DateTime.Now.ToString("dd.MM.yyyy HH:mm");
      mSrcMAC = pSrcMAC;
      mSrcIP = pSrcIP;
      mURL = pURL;
      mDstPort = pDstPort;
      mSessionCookies = pSessionCookies;
      mBrowser = pBrowser;
      mGroup = pGroup;
    }

    #endregion


    #region PROPERTIES

    public String TimeStamp
    {
      get { return (mTimeStamp); }
      set
      {
        mTimeStamp = value;
        this.NotifyPropertyChanged("TimeStamp");
      }
    }


    public String SrcMAC
    {
      get { return (mSrcMAC); }
      set
      {
        mSrcMAC = value;
        this.NotifyPropertyChanged("SrcMAC");
      }
    }


    public String SrcIP
    {
      get { return (mSrcIP); }
      set
      {
        mSrcIP = value;
        this.NotifyPropertyChanged("SrcIP");
      }
    }


    public String URL
    {
      get { return (mURL); }
      set
      {
        mURL = value;
        this.NotifyPropertyChanged("URL");
      }
    }


    public String DstPort
    {
      get { return (mDstPort); }
      set
      {
        mDstPort = value;
        this.NotifyPropertyChanged("DstPort");
      }
    }


    public String SessionCookies
    {
      get { return (mSessionCookies); }
      set
      {
        mSessionCookies = value;
        this.NotifyPropertyChanged("SessionCookies");
      }
    }


    public String Browser
    {
      get { return (mBrowser); }
      set
      {
        mBrowser = value;
        this.NotifyPropertyChanged("Browser");
      }
    }


    public String Group
    {
      get { return (mGroup); }
      set
      {
        mGroup = value;
        this.NotifyPropertyChanged("Group");
      }
    }


    public String ID
    {
      get { return (mID); }
      set
      {
        mID = value;
        this.NotifyPropertyChanged("ID");
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