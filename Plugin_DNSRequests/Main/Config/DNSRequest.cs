using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;



namespace Plugin.Main.DNSRequest.Config
{
  [Serializable, XmlRoot(Namespace = "http://www.buglist.io/simsang")]
  public class DNSRequestRecord : INotifyPropertyChanged
  {

    #region MEMBERS

    private String mSrcMAC;
    private String mSrcIP;
    private String mTimestamp;
    private String mDNSHostname;
    private String mPacketType;

    #endregion


    #region PUBLIC

    public DNSRequestRecord()
    {
      mSrcMAC = String.Empty;
      mSrcIP = String.Empty;
      mTimestamp = DateTime.Now.ToString("yyyy.MM.dd hh:mm:ss");
      mDNSHostname = String.Empty;
      mPacketType = String.Empty;
    }


    public DNSRequestRecord(String pSrcMAC, String pSrcIP, String pDNSHostname, String pType)
    {
      mSrcMAC = pSrcMAC;
      mSrcIP = pSrcIP;
      mTimestamp = DateTime.Now.ToString("yyyy.MM.dd hh:mm:ss");
      mDNSHostname = pDNSHostname;
      mPacketType = pType;
    }

    #endregion


    #region PROPERTIES

    [XmlElementAttribute()]
    public String SrcMAC
    {
      get { return mSrcMAC; }
      set
      {
        mSrcMAC = value;
        this.NotifyPropertyChanged("SrcMAC");
      }
    }


    [XmlElementAttribute()]
    public String SrcIP
    {
      get { return mSrcIP; }
      set
      {
        mSrcIP = value;
        this.NotifyPropertyChanged("SrcIP");
      }
    }



    [XmlElementAttribute()]
    public String Timestamp
    {
      get { return mTimestamp; }
      set
      {
        mTimestamp = value;
        this.NotifyPropertyChanged("Timestamp");
      }
    }

    [XmlElementAttribute()]
    public String DNSHostname
    {
      get { return mDNSHostname; }
      set
      {
        mDNSHostname = value;
        this.NotifyPropertyChanged("DNSHostname");
      }
    }

    [XmlElementAttribute()]
    public String PacketType
    {
      get { return mPacketType; }
      set
      {
        mPacketType = value;
        this.NotifyPropertyChanged("PacketType");
      }
    }

    #endregion


    #region PRIVATE

    /// <summary>
    /// 
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;
    private void NotifyPropertyChanged(String pName)
    {
      if (PropertyChanged != null)
        PropertyChanged(this, new PropertyChangedEventArgs(pName));
    }

    #endregion

  }
}
