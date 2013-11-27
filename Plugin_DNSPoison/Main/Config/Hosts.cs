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




namespace Plugin.Main.DNSPoison
{
  public class DNSPoisonRecord : INotifyPropertyChanged
  {

    #region MEMBERS

    private String mHostName;
    private String mIPAddress;

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion


    #region PUBLIC

    public DNSPoisonRecord()
    {
      mHostName = String.Empty;
      mIPAddress = String.Empty;
    }



    public DNSPoisonRecord(String pHostName, String pIPAddress)
    {
      mHostName = pHostName;
      mIPAddress = pIPAddress;
    }

    #endregion


    #region PROPERTIES

    public String HostName
    {
      get { return mHostName; }
      set
      {
        mHostName = value;
        this.NotifyPropertyChanged("HostName");
      }
    }


    public String IPAddress
    {
      get { return mIPAddress; }
      set
      {
        mIPAddress = value;
        this.NotifyPropertyChanged("IPAddress");
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
