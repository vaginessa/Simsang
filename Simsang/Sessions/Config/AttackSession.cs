using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Simsang.Session.Config
{
  [Serializable, XmlRoot(Namespace = "http://www.megapanzer.com/aycarrumba")]
  public class AttackSession
  {

    #region MEMBERS

    private String mStartTime = String.Empty;
    private String mStartIP = String.Empty;
    private String mStopIP = String.Empty;
    private String mStopTime = String.Empty;
    private String mName = String.Empty;
    private String mDescription = String.Empty;
    private String mSessionFileName = String.Empty;

    #endregion


    #region PROPERTIES

    [XmlElementAttribute()]
    public String StartTime
    {
      set { mStartTime = value; }
      get { return (mStartTime); }
    }
    [XmlElementAttribute()]
    public String StopTime
    {
      set { mStopTime = value; }
      get { return (mStopTime); }
    }
    [XmlElementAttribute()]
    public String Name
    {
      set { mName = value; }
      get { return (mName); }
    }
    [XmlElementAttribute()]
    public String Description
    {
      set { mDescription = value; }
      get { return (mDescription); }
    }
    [XmlElementAttribute()]
    public String StartIP
    {
      set { mStartIP = value; }
      get { return (mStartIP); }
    }
    [XmlElementAttribute()]
    public String StopIP
    {
      set { mStopIP = value; }
      get { return (mStopIP); }
    }
    
    //public String FileName
    //{
    //    set { mFileName = value; }
    //    get { return (mFileName); }
    //}
    [XmlElementAttribute()]
    public String SessionFileName
    {
      set { mSessionFileName = value; }
      get { return (mSessionFileName); }
    }

    #endregion
  }
}
