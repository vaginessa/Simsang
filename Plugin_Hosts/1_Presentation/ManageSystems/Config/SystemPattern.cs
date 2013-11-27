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


namespace Plugin.Main.Systems.ManageSystems
{
  public class SystemPattern : INotifyPropertyChanged
  {

    #region MEMBERS

    private String cSystemPatternString;
    private String cSystemName;

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion


    #region PUBLIC

    public SystemPattern()
    {
      cSystemPatternString = String.Empty;
      cSystemName = String.Empty;
    }

    public SystemPattern(String pSystemPatternString, String pSystemName)
    {
      cSystemPatternString = pSystemPatternString;
      cSystemName = pSystemName;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object pObj)
    {
      bool lRetVal = false;

      if (pObj is SystemPattern)
        if (((SystemPattern)pObj).SystemPatternString.ToLower() == this.SystemPatternString.ToLower())
          lRetVal = true;
      
      return(lRetVal);
    }

    #endregion


    #region PROPERTIES

    public string SystemPatternString
    {
      get { return cSystemPatternString; }
      set
      {
        cSystemPatternString = value;
        this.NotifyPropertyChanged("systempatternstring");
      }
    }


    public string SystemName
    {
      get { return cSystemName; }
      set
      {
        cSystemName = value;
        this.NotifyPropertyChanged("systemname");
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
