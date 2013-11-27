using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Simsang
{
  [Serializable, XmlRoot(Namespace = "http://www.megapanzer.com/aycarrumba")]
  public class AttackSession
  {

    #region MEMBERS

    private String mSessionDir = String.Empty;
    private String mStartTime = String.Empty;
    private String mStartIP = String.Empty;
    private String mStopIP = String.Empty;
    private String mStopTime = String.Empty;
    private String mName = String.Empty;
    private String mDescription = String.Empty;
    //        private String mFileName = String.Empty;
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
    [XmlElementAttribute()]
    //public String FileName
    //{
    //    set { mFileName = value; }
    //    get { return (mFileName); }
    //}
    public String SessionFileName
    {
      set { mSessionFileName = value; }
      get { return (mSessionFileName); }
    }

    #endregion


    #region PUBLIC

    public AttackSession()
    {
    }

    public AttackSession(String pSessionDir)
    {
      mSessionDir = pSessionDir.Length > 0 ? pSessionDir : "";
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionDir"></param>
    /// <returns></returns>
    public static List<AttackSession> GetAllSessions(String pSessionDir)
    {
      List<AttackSession> lRetVal = new List<AttackSession>();
      DirectoryInfo lDirInfo = new DirectoryInfo(pSessionDir);
      FileInfo[] lSessionFiles = lDirInfo.GetFiles("*.xml");


      /*
       * 
       */
      foreach (FileInfo lSessionFile in lSessionFiles)
      {
        FileStream lFS = null;
        XmlSerializer lXMLSerial;

        try
        {
          lFS = new FileStream(lSessionFile.FullName, FileMode.Open);
          lXMLSerial = new XmlSerializer(typeof(AttackSession));
          AttackSession lAttackSession = (AttackSession)lXMLSerial.Deserialize(lFS);
          lRetVal.Add(lAttackSession);
        }
        catch (Exception lEx)
        {
          LogConsole.Main.LogConsole.pushMsg(String.Format("AttackSession.GetAllSessions(): {0} ({1})", lEx.Message, lSessionFile.FullName));
        }
        finally
        {
          if (lFS != null)
            lFS.Close();
        }
      } // foreach (FileIn...

      return (lRetVal);
    }


    /// <summary>
    /// 
    /// </summary>
    public void SaveSessionData()
    {
      String lTimeStamp = DateTime.Now.ToString("yyyyMMdd_hhmmssff");
      String lSessionFile;
      XmlSerializer lSerializer;
      FileStream lFS = null;

      //            mFileName = string.Format("{0}.xml", lTimeStamp);
      mSessionFileName = string.Format("{0}.xml", lTimeStamp);

      try
      {
        lSessionFile = String.Format("{0}{1}.xml", mSessionDir, lTimeStamp);
        lSerializer = new XmlSerializer(typeof(AttackSession));
        lFS = new FileStream(lSessionFile, FileMode.Create);
        lSerializer.Serialize(lFS, this);
      }
      catch (Exception lEx)
      {
        MessageBox.Show("Can't save session data : " + lEx.ToString());
      }
      finally
      {
        if (lFS != null)
          lFS.Close();
      }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionName"></param>
    /// <returns></returns>
    public AttackSession GetSessionByName(String pSessionName)
    {
      AttackSession lRetVal = null;

      foreach (AttackSession lSess in AttackSession.GetAllSessions(mSessionDir))
      {
        if (pSessionName == lSess.Name)
        {
          lRetVal = lSess;
          break;
        } // if (pSessio...
      } // foreach (Atta...

      return (lRetVal);
    }

    #endregion

  }
}
