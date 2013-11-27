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




namespace Plugin.Main.HTTPRequest
{
  public class HTTPRequests : INotifyPropertyChanged
  {

    #region MEMBERS

    private String mSrcMAC;
    private String mSrcIP;
    private String mTimestamp;
    private String mMethod;
    private String mRemoteHost;
    private String mRemoteFile;
    private String mURL;
    private String mSessionCookies;
    private String mRequest;

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion


    #region PUBLIC

    public HTTPRequests()
    {
      mSrcMAC = String.Empty;
      mSrcIP = String.Empty;
      mTimestamp = DateTime.Now.ToString("yyyy.MM.dd hh:mm:ss");
      mMethod = String.Empty;
      mRemoteHost = String.Empty;
      mRemoteFile = String.Empty;
      mURL = String.Empty;
      mSessionCookies = String.Empty;
      mRequest = String.Empty;
    }


    public HTTPRequests(String pSrcMAC, String pSrcIP, String pMethod, String pRemoteHost, String pRemoteFile, String pCookies, String pRequest)
    {
      mSrcMAC = pSrcMAC;
      mSrcIP = pSrcIP;
      mTimestamp = DateTime.Now.ToString("yyyy.MM.dd hh:mm:ss");
      mMethod = pMethod;
      mRemoteHost = pRemoteHost;
      mRemoteFile = pRemoteFile;
      mURL = String.Format("{0}{1}", mRemoteHost, mRemoteFile);
      mSessionCookies = pCookies;
      mRequest = pRequest;
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


    public String Timestamp
    {
      get { return mTimestamp; }
      set
      {
        mTimestamp = value;
        this.NotifyPropertyChanged("Timestamp");
      }
    }


    public String Method
    {
      get { return mMethod; }
      set
      {
        mMethod = value;
        this.NotifyPropertyChanged("Method");
      }
    }


    public String RemoteHost
    {
      get { return mRemoteHost; }
      set
      {
        mRemoteHost = value;
        this.NotifyPropertyChanged("RemoteHost");
      }
    }


    public String RemoteFile
    {
      get { return mRemoteFile; }
      set
      {
        mRemoteFile = value;
        this.NotifyPropertyChanged("RemoteFile");
      }
    }



    public String URL
    {
      get { return mURL; }
      set
      {
        mURL = value;
        this.NotifyPropertyChanged("URL");
      }
    }


    public String SessionCookies
    {
      get { return mSessionCookies; }
      set
      {
        mSessionCookies = value;
        this.NotifyPropertyChanged("SessionCookies");
      }
    }


    public String Request
    {
      get { return mRequest; }
      set
      {
        mRequest = value;
        this.NotifyPropertyChanged("Request");
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
