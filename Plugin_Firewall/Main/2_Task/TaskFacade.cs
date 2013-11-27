using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Text.RegularExpressions;

using Simsang.Plugin;


namespace Plugin.Main.Firewall
{
  public class TaskFacade
  {

    #region MEMBERS

    private static TaskFacade cInstance;
    private DomainFacade cDomain;
    private IPlugin cPlugin;

    #endregion


    #region PUBLIC

    private TaskFacade(IPlugin pPlugin)
    {
      cPlugin = pPlugin;
      cDomain = DomainFacade.getInstance(pPlugin);
    }


    /// <summary>
    /// Create single instance
    /// </summary>
    /// <returns></returns>
    public static TaskFacade getInstance(IPlugin pPlugin)
    {
      if (cInstance == null)
        cInstance = new TaskFacade(pPlugin);

      return (cInstance);
    }

    #endregion


    #region RECORDS

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pProtocol"></param>
    /// <param name="pSrcIP"></param>
    /// <param name="pDstIP"></param>
    /// <param name="pSrcPortLowerStr"></param>
    /// <param name="pSrcPortUpperStr"></param>
    /// <param name="pDstPortLowerStr"></param>
    /// <param name="pDstPortUpperStr"></param>
    public void addRecord(String pProtocol, String pSrcIP, String pDstIP, String pSrcPortLowerStr, String pSrcPortUpperStr, String pDstPortLowerStr, String pDstPortUpperStr)
    {
      int lSrcPortLower = 0;
      int lSrcPortUpper = 0;
      int lDstPortLower = 0;
      int lDstPortUpper = 0;
      String lID = String.Empty;
      String lErrorMsg = String.Empty;

      /*
       * Set default values where necessary
       */
      if (String.IsNullOrEmpty(pSrcIP))
        pSrcIP = "0.0.0.0";

      if (String.IsNullOrEmpty(pDstIP))
        pDstIP = "0.0.0.0";

      if (String.IsNullOrEmpty(pSrcPortLowerStr))
        pSrcPortLowerStr = "0";

      if (String.IsNullOrEmpty(pSrcPortUpperStr))
        pSrcPortUpperStr = "0";

      if (String.IsNullOrEmpty(pDstPortLowerStr))
        pDstPortLowerStr = "0";

      if (String.IsNullOrEmpty(pDstPortUpperStr))
        pDstPortUpperStr = "0";

      /*
       * Parse ports
       */
      try
      {
        lSrcPortLower = Int32.Parse(pSrcPortLowerStr);
        lSrcPortUpper = Int32.Parse(pSrcPortUpperStr);
        lDstPortLower = Int32.Parse(pDstPortLowerStr);
        lDstPortUpper = Int32.Parse(pDstPortUpperStr);
      }
      catch (Exception)
      {
        throw new Exception("Check the firewall rule port settings.");
      }

      /*
       * Arrange port settings
       */
      if (lSrcPortLower == 0 && lSrcPortUpper > 0)
      {
        lSrcPortLower = lSrcPortUpper;
        pSrcPortLowerStr = pSrcPortUpperStr;
      }

      if (lSrcPortUpper == 0 && lSrcPortLower > 0)
      {
        lSrcPortUpper = lSrcPortLower;
        pSrcPortUpperStr = pSrcPortLowerStr;
      }

      if (lDstPortLower == 0 && lDstPortUpper > 0)
      {
        lDstPortLower = lDstPortUpper;
        pDstPortLowerStr = pDstPortUpperStr;
      }

      if (lDstPortUpper == 0 && lDstPortLower > 0)
      {
        lDstPortUpper = lDstPortLower;
        pDstPortUpperStr = pDstPortLowerStr;
      }

      /*
       * Create firewall rule ID
       */
      lID = String.Format("{0}{1}{2}{3}{4}{5}{6}", pProtocol, pDstIP, pDstPortLowerStr, pDstPortUpperStr, pSrcIP, pSrcPortLowerStr, pSrcPortUpperStr);



      /*
       * Check IP addresses/port format
       */
      if (!Regex.Match(pSrcIP, @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$").Success)
        lErrorMsg = "Something is wrong with the source IP";
      else if (!Regex.Match(pDstIP, @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$").Success)
        lErrorMsg = "Something is wrong with the destination IP";
      else if (!Regex.Match(pSrcPortLowerStr, @"^\d{1,5}$").Success || Int32.Parse(pSrcPortLowerStr) < 0 || Int32.Parse(pSrcPortLowerStr) > 65535)
        lErrorMsg = "Something is wrong with the source port (lower)";
      else if (!Regex.Match(pSrcPortUpperStr, @"^\d{1,5}$").Success || Int32.Parse(pSrcPortUpperStr) < 0 || Int32.Parse(pSrcPortUpperStr) > 65535)
        lErrorMsg = "Something is wrong with the source port (upper)";
      else if (!Regex.Match(pDstPortLowerStr, @"^\d{1,5}$").Success || Int32.Parse(pDstPortLowerStr) < 0 || Int32.Parse(pDstPortLowerStr) > 65535)
        lErrorMsg = "Something is wrong with the destination port (lower)";
      else if (!Regex.Match(pDstPortUpperStr, @"^\d{1,5}$").Success || Int32.Parse(pDstPortUpperStr) < 0 || Int32.Parse(pDstPortUpperStr) > 65535)
        lErrorMsg = "Something is wrong with the destination port (upper)";
      else if (lDstPortLower > lDstPortUpper)
        lErrorMsg = "Lower destination port is greater than the upper port";
      else if (lSrcPortLower > lSrcPortUpper)
        lErrorMsg = "Lower source port is greater than the upper port";


      /*
       * 
       */
      if (lErrorMsg.Length > 0)
        throw new Exception(lErrorMsg);
      else
        cDomain.addRecord(new FWRule(pProtocol, pSrcIP, pSrcPortLowerStr, pSrcPortUpperStr, pDstIP, pDstPortLowerStr, pDstPortUpperStr));
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pID"></param>
    public void removeRuleFromList(String pID)
    {
      cDomain.removeRecordAt(pID);
    }


    /// <summary>
    /// 
    /// </summary>
    public void emptyRuleList()
    {
      cDomain.clearRecordList();
    }

    #endregion


    #region SESSION

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionName"></param>
    public void saveSession(String pSessionName)
    {
      cDomain.saveSession(pSessionName);
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionName"></param>
    public void loadSessionData(String pSessionName)
    {
      cDomain.loadSessionData(pSessionName);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionData"></param>
    public void loadSessionDataFromString(String pSessionData)
    {
      cDomain.loadSessionDataFromString(pSessionData);
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionName"></param>
    public void deleteSession(String pSessionName)
    {
      cDomain.deleteSession(pSessionName);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pSessionName"></param>
    /// <returns></returns>
    public String getSessionData(String pSessionName)
    {
      return (cDomain.getSessionData(pSessionName));
    }

    #endregion


    #region EVENTS

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pFWRulesPath"></param>
    public void onStart(String pFWRulesPath)
    {
      cDomain.onStart(pFWRulesPath);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pFWRulesPath"></param>
    public void onStop(String pFWRulesPath)
    {
      cDomain.onStop(pFWRulesPath);
    }

    #endregion

  }
}
