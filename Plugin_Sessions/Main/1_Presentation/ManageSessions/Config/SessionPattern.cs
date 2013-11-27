using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Plugin.Main.Session.ManageSessions.Config
{

  #region TYPE DEFINITION
  #endregion


  public class SessionPattern : INotifyPropertyChanged
  {

    #region MEMBERS

    private String cSessionPatternString;
    private String cSessionName;
    private String cWebpage;
    private String cHTTPHost;

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion


    #region PUBLIC

    public SessionPattern()
    {
      cSessionPatternString = String.Empty;
      cSessionName = String.Empty;
      cWebpage = String.Empty;
      cHTTPHost = String.Empty;
    }

    public SessionPattern(String pSessionPatternString, String pSessionName, String pHTTPHost, String pWebpage)
    {
      cSessionPatternString = pSessionPatternString;
      cSessionName = pSessionName;
      cWebpage = pWebpage;
      cHTTPHost = pHTTPHost;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object pObj)
    {
      bool lRetVal = false;

      if (pObj is SessionPattern)
      {
        SessionPattern lTmp = (SessionPattern)pObj;

        if (lTmp.cSessionPatternString == this.cSessionPatternString && lTmp.cHTTPHost == this.cHTTPHost)
          lRetVal = true;
      }

      return (lRetVal);
    }

    #endregion


    #region PROPERTIES

    public string SessionPatternString
    {
      get { return cSessionPatternString; }
      set
      {
        cSessionPatternString = value;
        this.NotifyPropertyChanged("sessionpatternstring");
      }
    }

    public string SessionName
    {
      get { return cSessionName; }
      set
      {
        cSessionName = value;
        this.NotifyPropertyChanged("sessionname");
      }
    }


    public string HTTPHost
    {
      get { return cHTTPHost; }
      set
      {
        cHTTPHost = value;
        this.NotifyPropertyChanged("httphost");
      }
    }


    public string Webpage
    {
      get { return cWebpage; }
      set
      {
        cWebpage = value;
        this.NotifyPropertyChanged("webpage");
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
