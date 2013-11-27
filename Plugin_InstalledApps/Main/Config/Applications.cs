using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Text.RegularExpressions;





namespace Plugin.Main.Applications.Config
{
  public class ApplicationRecord : INotifyPropertyChanged
  {

    #region MEMBERS

    private String mSrcMAC;
    private String mSrcIP;
    private String mDstPort;
    private String mHTTPHost;
    private String mHTTPURI;
    private String mAppName;
    private String mAppURL;
    private String mID;

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion


    public override bool Equals(object pObj)
    {
      Boolean lRetVal = false;

      if (pObj != null)
      {
        ApplicationRecord lTmp = (ApplicationRecord)pObj;
        if (this.mAppName == lTmp.AppName && this.mSrcIP == lTmp.mSrcIP)
          lRetVal = true;
      }


      return (lRetVal);
    }



    public ApplicationRecord()
    {
      mSrcMAC = String.Empty;
      mSrcIP = String.Empty;
      mDstPort = String.Empty;
      mHTTPHost = String.Empty;
      mHTTPURI = String.Empty;
      mAppName = String.Empty;
      mAppURL = String.Empty;
      mID = String.Empty;
    }


    public ApplicationRecord(String pSrcMAC, String pSrcIP, String pDstPort, String pHTTPHost, String pHTTPURI, String pAppName, String pAppURL)
    {
      mSrcMAC = pSrcMAC;
      mSrcIP = pSrcIP;
      mDstPort = pDstPort;
      mHTTPHost = pHTTPHost;
      mHTTPURI = pHTTPURI;
      mAppName = pAppName;
      mAppURL = pAppURL;
      mID = String.Format("{0}{1}{2}{3}{4}", pSrcMAC, pSrcIP, pDstPort, pHTTPHost, pHTTPURI);
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


    public String HTTPHost
    {
      get { return (mHTTPHost); }
      set
      {
        mHTTPHost = value;
        this.NotifyPropertyChanged("HTTPHost");
      }
    }


    public String HTTPURI
    {
      get { return (mHTTPURI); }
      set
      {
        mHTTPURI = value;
        this.NotifyPropertyChanged("HTTPURI");
      }
    }

    public String AppName
    {
      get { return (mAppName); }
      set
      {
        mAppName = value;
        this.NotifyPropertyChanged("AppName");
      }
    }


    public String AppURL
    {
      get { return (mAppURL); }
      set
      {
        mAppURL = value;
        this.NotifyPropertyChanged("AppURL");
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
