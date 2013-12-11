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
  public class UsedPlugins : INotifyPropertyChanged
  {

    #region MEMBERS

    private String mPluginName;
    private String mPluginDescription;
    private String mVersion;
    private String mActive;
    public event PropertyChangedEventHandler PropertyChanged;

    #endregion


    public UsedPlugins(String pPluginName, String pPluginDescription, String pVersion, String pActive)
    {
      mPluginName = pPluginName;
      mPluginDescription = pPluginDescription;
      mVersion = pVersion;
      mActive = pActive;
    }



    public String PluginName
    {
      get { return mPluginName; }
      set
      {
        mPluginName = value;
        this.NotifyPropertyChanged("PluginName");
      }
    }

    public String PluginDescription
    {
      get { return mPluginDescription; }
      set
      {
        mPluginDescription = value;
        this.NotifyPropertyChanged("PluginDescription");
      }
    }


    public String PluginVersion
    {
      get { return mVersion; }
      set
      {
        mVersion = value;
        this.NotifyPropertyChanged("PluginVersion");
      }
    }


    public String Active
    {
      get { return mActive; }
      set
      {
        mActive = value;
        this.NotifyPropertyChanged("Active");
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
