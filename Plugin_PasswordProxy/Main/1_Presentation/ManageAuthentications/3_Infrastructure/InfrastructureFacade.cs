using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text.RegularExpressions;


namespace Plugin.Main.HTTPProxy.ManageAuthentications
{
  public class InfrastructureFacade
  {

    #region MEMBERS

    private static InfrastructureFacade cInstance;

    #endregion


    #region PROPERTIES

    public String PatternFile { get; private set; }

    #endregion


    #region PUBLIC

    /// <summary>
    /// 
    /// </summary>
    private InfrastructureFacade()
    {
      PatternFile = @"\plugins\HTTPProxy\Plugin_AccountsHTMLAuth_Patterns.xml";
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static InfrastructureFacade getInstance()
    {
      return cInstance ?? (cInstance = new InfrastructureFacade());
    }


    /// <summary>
    /// 
    /// </summary>
    public void saveAccountPatterns(List<AccountPattern> pRecords)
    {
      if (pRecords.Count > 0)
      {
        XmlSerializer lSerializer;
        FileStream lFS = null;

        try
        {
          lSerializer = new XmlSerializer(typeof(List<AccountPattern>));
          lFS = new FileStream(PatternFile, FileMode.Create);
          lSerializer.Serialize(lFS, pRecords);
        }
        finally
        {
          if (lFS != null)
            lFS.Close();
        }
      } // if (pSessi...
    }



    /// <summary>
    /// 
    /// </summary>
    public List<AccountPattern> readAccountsPatterns()
    {
      List<AccountPattern> lRetVal = null;
      FileStream lFS = null;
      XmlSerializer lXMLSerial;

      try
      {
        lFS = new FileStream(PatternFile, FileMode.Open);
        lXMLSerial = new XmlSerializer(typeof(List<AccountPattern>));
        lRetVal = (List<AccountPattern>)lXMLSerial.Deserialize(lFS);
      }
      finally
      {
        if (lFS != null)
          lFS.Close();
      }

      return (lRetVal);
    }

    #endregion

  }
}
