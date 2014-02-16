using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Simsang.ARPScan.Main.Config
{
  public class ScanSystem : INotifyPropertyChanged
  {

    #region MEMBERS

    private String mTargetIP;
    private String mTargetMAC;
    public event PropertyChangedEventHandler PropertyChanged;

    #endregion


    #region PUBLIC

    public ScanSystem(String pTargetIP, String pTargetMAC)
    {
      mTargetIP = pTargetIP;
      mTargetMAC = pTargetMAC;
    }

    #endregion


    #region PROPERTIES

    public String TargetIP
    {
      get { return mTargetIP; }
      set
      {
        mTargetIP = value;
        this.NotifyPropertyChanged("TargetIP");
      }
    }

    public String TargetMAC
    {
      get { return mTargetMAC; }
      set
      {
        mTargetMAC = value;
        this.NotifyPropertyChanged("TargetMAC");
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
