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


namespace Plugin.Main.HTTPInject
{

  /// <summary>
  /// Global injection Configuration
  /// </summary>
  public class InjectionConfig
  {
    public bool isDebuggingOn;
    public SetHTTPInjectionOnStoppedDelegate onWebServerExit;
    public String BasisDirectory;
    public String InjectionRulesPath;
  }

  public delegate void SetHTTPInjectionOnStoppedDelegate();

  /// <summary>
  /// Injection exceptions
  /// </summary>
  public class InjWarningException : System.Exception
  {
    public InjWarningException(String pMessage)
      : base(pMessage)
    {
    }
  }

  public class InjErrorException : System.Exception
  {
    public InjErrorException(String pMessage)
      : base(pMessage)
    {
    }
  }


  /// <summary>
  /// Injection record definition
  /// </summary>
  public class InjectedURLRecord : INotifyPropertyChanged
  {

    #region MEMBERS

    private String cType;
    private String cRequestedHost;
    private String cRequestedURL;
    private String cInjectedHost;
    private String cInjectedURL;
    private String cInjectedFileFullPath;

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion


    #region PUBLIC

    public InjectedURLRecord()
    {
      cType = String.Empty;
      cRequestedHost = String.Empty;
      cRequestedURL = String.Empty;
      cInjectedURL = String.Empty;
    }

    public InjectedURLRecord(String pType, String pRequestedHost, String pRequestedURL, String pInjectedHost, String pInjectedURL, String pInjectedFileFullPath)
    {
      cType = pType;
      cRequestedHost = pRequestedHost;
      cRequestedURL = pRequestedURL;
      cInjectedHost = pInjectedHost;
      cInjectedURL = pInjectedURL;
      cInjectedFileFullPath = pInjectedFileFullPath;
    }

    #endregion


    #region PROPERTIES

    public String Type
    {
      get { return cType; }
      set
      {
        cType = value;
        this.NotifyPropertyChanged("Type");
      }
    }


    public String RequestedHost
    {
      get { return cRequestedHost; }
      set
      {
        cRequestedHost = value;
        this.NotifyPropertyChanged("RequestedHost");
      }
    }


    public String RequestedURL
    {
      get { return cRequestedURL; }
      set
      {
        cRequestedURL = value;
        this.NotifyPropertyChanged("RequestedURL");
      }
    }


    public String InjectedHost
    {
      get { return cInjectedHost; }
      set
      {
        cInjectedHost = value;
        this.NotifyPropertyChanged("InjectedHost");
      }
    }


    public String InjectedURL
    {
      get { return cInjectedURL; }
      set
      {
        cInjectedURL = value;
        this.NotifyPropertyChanged("InjectedURL");
      }
    }


    public String InjectedFileFullPath
    {
      get { return cInjectedFileFullPath; }
      set
      {
        cInjectedFileFullPath = value;
        this.NotifyPropertyChanged("InjectedFileFullPath");
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
