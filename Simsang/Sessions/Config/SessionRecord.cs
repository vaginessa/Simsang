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


namespace Simsang
{
  class SessionRecord : INotifyPropertyChanged
  {

    #region MEMBERS

    private string mSessionName;
    private string mName;
    private string mDescription;
    private string mStart;
    private string mStop;

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion


    #region PUBLIC

    public SessionRecord(string pSessionName, string pName, string pDescription, string pStart, string pStop)
    {
      mSessionName = pSessionName;
      mName = pName;
      mDescription = pDescription;
      mStart = pStart;
      mStop = pStop;
    }

    #endregion


    #region PROPERTIES

    public string File
    {
      get { return mSessionName; }
      set
      {
        mSessionName = value;
        this.NotifyPropertyChanged("File");
      }
    }


    public string Name
    {
      get { return mName; }
      set
      {
        mName = value;
        this.NotifyPropertyChanged("Name");
      }
    }


    public string Description
    {
      get { return mDescription; }
      set
      {
        mDescription = value;
        this.NotifyPropertyChanged("Description");
      }
    }


    public string Start
    {
      get { return mStart; }
      set
      {
        mStart = value;
        this.NotifyPropertyChanged("Start");
      }
    }


    public string Stop
    {
      get { return mStop; }
      set
      {
        mStop = value;
        this.NotifyPropertyChanged("Stop");
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
