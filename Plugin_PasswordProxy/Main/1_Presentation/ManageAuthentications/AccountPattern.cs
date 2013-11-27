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

namespace Plugin.Main.HTTPProxy.ManageAuthentications
{

  public class AccountPattern : INotifyPropertyChanged
  {

    #region MEMBERS

    private String cMethod;
    private String cHost;
    private String cPath;
    private String cDataPattern;
    private String cCompany;
    private String cWebPage;

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion


    #region PUBLIC

    /// <summary>
    /// 
    /// </summary>
    public AccountPattern()
    {
      cMethod = String.Empty;
      cHost = String.Empty;
      cPath = String.Empty;
      cDataPattern = String.Empty;
      cCompany = String.Empty;
      cWebPage = String.Empty;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pMethod"></param>
    /// <param name="pHost"></param>
    /// <param name="pPath"></param>
    /// <param name="pDataPattern"></param>
    /// <param name="pCompany"></param>
    /// <param name="pWebPage"></param>
    public AccountPattern(String pMethod, String pHost, String pPath, String pDataPattern, String pCompany, String pWebPage)
    {
      cMethod = pMethod;
      cHost = pHost;
      cPath = pPath;
      cDataPattern = pDataPattern;
      cCompany = pCompany;
      cWebPage = pWebPage;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object lObj)
    {
      bool lRetVal = false;

      if (lObj is AccountPattern)
      {
        AccountPattern lTmp = (AccountPattern) lObj;

        if (lTmp.Method == this.Method && lTmp.Host == this.Host && lTmp.Path == this.Path)
        {
          lRetVal = true;
        } // if (lObj...
      } // if (lObj...

      return(lRetVal);
    }

    #endregion


    #region PROPERTIES

    public string Method
    {
      get { return cMethod; }
      set
      {
        cMethod = value;
        this.NotifyPropertyChanged("method");
      }
    }

    public string Host
    {
      get { return cHost; }
      set
      {
        cHost = value;
        this.NotifyPropertyChanged("host");
      }
    }


    public string Path
    {
      get { return cPath; }
      set
      {
        cPath = value;
        this.NotifyPropertyChanged("path");
      }
    }

    public string DataPattern
    {
      get { return cDataPattern; }
      set
      {
        cDataPattern = value;
        this.NotifyPropertyChanged("datapattern");
      }
    }


    public string Company
    {
      get { return cCompany; }
      set
      {
        cCompany = value;
        this.NotifyPropertyChanged("company");
      }
    }


    public string WebPage
    {
      get { return cWebPage; }
      set
      {
        cWebPage = value;
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
