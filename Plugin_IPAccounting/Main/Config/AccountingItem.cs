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





namespace Plugin.Main.IPAccounting
{
  public class AccountingItem : INotifyPropertyChanged, IEquatable<AccountingItem>
  {

    #region MEMBERS

    private String mBasis;
    private String mPacketCounter;
    private String mDataVolume;
    private String mLastUpdate;

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion


    #region PUBLIC

    public AccountingItem()
    {
      mBasis = String.Empty;
      mPacketCounter = String.Empty;
      mDataVolume = String.Empty;
      mLastUpdate = String.Empty;
    }

    public AccountingItem(String pServiceName, String pPacketCounter, String pDataVolume, String pLastUpdate)
    {
      mBasis = pServiceName;
      mPacketCounter = pPacketCounter;
      mDataVolume = pDataVolume;
      mLastUpdate = pLastUpdate;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pObj"></param>
    /// <returns></returns>
    public bool Equals(AccountingItem pObj)
    {
      if (pObj != null && mBasis != null && mBasis == pObj.Basis)
        return true;
      else
        return false;
    }

    #endregion


    #region PROPERTIES

    public String Basis
    {
      get { return mBasis; }
      set
      {
        mBasis = value;
        this.NotifyPropertyChanged("Basis");
      }
    }

    public String PacketCounter
    {
      get { return mPacketCounter; }
      set
      {
        mPacketCounter = value;
        this.NotifyPropertyChanged("PacketCounter");
      }
    }

    public String DataVolume
    {
      get { return mDataVolume; }
      set
      {
        mDataVolume = value;
        this.NotifyPropertyChanged("DataVolume");
      }
    }

    public String LastUpdate
    {
      get { return mLastUpdate; }
      set
      {
        mLastUpdate = value;
        this.NotifyPropertyChanged("LastUpdate");
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
