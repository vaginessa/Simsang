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

namespace Plugin.Main.Applications.ManageApplications
{
  public class ApplicationPattern : INotifyPropertyChanged
  {

    #region MEMBERS

    private String mID;
    private String mApplicationPatternString;
    private String mApplicationname;
    private String mCompanyURL;

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion


    #region PUBLIC

    public ApplicationPattern()
    {
      mID = String.Empty;
      mApplicationPatternString = String.Empty;
      mApplicationname = String.Empty;
      mCompanyURL = String.Empty;
    }


    public ApplicationPattern(String pApplicationPatternString, String pApplicationName, String pCompanyURL)
    {
      mID = String.Format("{0}{1}{2}", pApplicationPatternString, pApplicationName, pCompanyURL);
      mApplicationPatternString = pApplicationPatternString;
      mApplicationname = pApplicationName;
      mCompanyURL = pCompanyURL;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pObj"></param>
    /// <returns></returns>
    public override bool Equals(object pObj)
    {
      bool lRetVal = false;

      if (pObj is ApplicationPattern)
      {
        ApplicationPattern lTmp = (ApplicationPattern) pObj;
        if (lTmp.ApplicationPatternString == this.ApplicationPatternString && lTmp.ApplicationName == this.ApplicationName)
          lRetVal = true;
      }

      return(lRetVal);
    }

    #endregion


    #region PROPERTIES

    public string ApplicationPatternString 
    {
      get { return mApplicationPatternString; }
      set
      {
        mApplicationPatternString = value;
        this.NotifyPropertyChanged("applicationpatternstring");
      }
    }



    public string ApplicationName
    {
      get { return mApplicationname; }
      set
      {
        mApplicationname = value;
        this.NotifyPropertyChanged("applicationname");
      }
    }



    public string CompanyURL
    {
      get { return mCompanyURL; }
      set
      {
        mCompanyURL = value;
        this.NotifyPropertyChanged("companyurl");
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
