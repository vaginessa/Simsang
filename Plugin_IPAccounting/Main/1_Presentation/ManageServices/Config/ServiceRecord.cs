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



namespace Plugin.Main.IPAccounting.ManageServices
{
  public class ServiceRecord : INotifyPropertyChanged
  {

    #region MEMBERS

    private String mServiceName;
    private String mLowerPort;
    private String mUpperPort;

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion


    #region PUBLIC

    public ServiceRecord()
    {
      mServiceName = String.Empty;
      mLowerPort = String.Empty;
      mUpperPort = String.Empty;
    }

    public ServiceRecord(String pServiceName, String pLowerPort, String pUpperPort)
    {
      mServiceName = pServiceName;
      mLowerPort = pLowerPort;
      mUpperPort = pUpperPort;
    }


    public String ServiceName
    {
      get { return mServiceName; }
      set
      {
        mServiceName = value;
        this.NotifyPropertyChanged("ServiceName");
      }
    }

    #endregion


    #region PROPERTIES

    public String LowerPort
    {
      get { return mLowerPort; }
      set
      {
        mLowerPort = value;
        this.NotifyPropertyChanged("LowerPort");
      }
    }



    public String UpperPort
    {
      get { return mUpperPort; }
      set
      {
        mUpperPort = value;
        this.NotifyPropertyChanged("UpperPort");
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

