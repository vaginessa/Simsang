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





namespace Plugin.Main.Firewall
{
  public class FWRule : INotifyPropertyChanged
  {

    #region MEMBERS

    private String mID;
    private String mProtocol;
    private String mSrcIP;
    private String mDstIP;
    private String mSrcPortLower;
    private String mSrcPortUpper;
    private String mDstPortLower;
    private String mDstPortUpper;

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion


    #region PUBLIC

    public FWRule()
    {
      mID = String.Empty;
      mProtocol = String.Empty;
      mSrcIP = String.Empty;
      mDstIP = String.Empty;
      mSrcPortLower = String.Empty;
      mSrcPortUpper = String.Empty;
      mDstPortLower = String.Empty;
      mDstPortUpper = String.Empty;
    }

    public FWRule(String pProtocol, String pSrcIP, String pSrcPortLower, String pSrcPortUpper, String pDstIP, String pDstPortLower, String pDstPortUpper)
    {
      mID = String.Format("{0}{1}{2}{3}{4}{5}{6}", pProtocol, pDstIP, pDstPortLower, pDstPortUpper, pSrcIP, pSrcPortLower, pSrcPortUpper);
      mProtocol = pProtocol;
      mSrcIP = pSrcIP;
      mDstIP = pDstIP;
      mSrcPortLower = pSrcPortLower;
      mSrcPortUpper = pSrcPortUpper;
      mDstPortLower = pDstPortLower;
      mDstPortUpper = pDstPortUpper;
    }

    #endregion


    #region PROPERTIES

    public String Protocol
    {
      get { return mProtocol; }
      set
      {
        mProtocol = value;
        this.NotifyPropertyChanged("Protocol");
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



    public String SrcPortLower
    {
      get { return mSrcPortLower; }
      set
      {
        mSrcPortLower = value;
        this.NotifyPropertyChanged("SrcPortLower");
      }
    }



    public String SrcPortUpper
    {
      get { return mSrcPortUpper; }
      set
      {
        mSrcPortUpper = value;
        this.NotifyPropertyChanged("SrcPortUpper");
      }
    }





    public String DstPortLower
    {
      get { return mDstPortLower; }
      set
      {
        mDstPortLower = value;
        this.NotifyPropertyChanged("DstPortLower");
      }
    }



    public String DstPortUpper
    {
      get { return mDstPortUpper; }
      set
      {
        mDstPortUpper = value;
        this.NotifyPropertyChanged("DstPortUpper");
      }
    }

    #endregion


    #region PRIVATE

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pName"></param>
    private void NotifyPropertyChanged(string pName)
    {
      if (PropertyChanged != null)
        PropertyChanged(this, new PropertyChangedEventArgs(pName));
    }

    #endregion

  }
}
