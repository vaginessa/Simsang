using System;
using System.IO;

using Simsang.LogConsole.Main;

namespace Simsang
{
  public partial class Config
  {

    #region PUBLIC

    /// <summary>
    ///  Network interface
    /// </summary>
    public struct sInterfaces
    {
      public String sIfcID;
      public String sIfcName;
      public String sIfcDescr;
      public String sIfcIPAddr;
      public String sIfcNetAddr;
      public String sIfcBcastAddr;
      public String sIfcDefaultGW;
      public String sIfcGWMAC;
      public bool sUsed;
    }

    public static sInterfaces[] Interfaces = new sInterfaces[32];



    /// <summary>
    /// 
    /// </summary>
    /// <param name="pID"></param>
    /// <param name="pName"></param>
    /// <param name="pDescr"></param>
    /// <param name="pIPAddr"></param>
    /// <param name="pNWAddr"></param>
    /// <param name="pBCAddr"></param>
    /// <param name="pDefaultGW"></param>
    /// <param name="pGWMAC"></param>
    public static void SetInterfaceInstance(String pID, String pName, String pDescr, String pIPAddr, String pNWAddr, String pBCAddr, String pDefaultGW, String pGWMAC)
    {
      for (int lCount = 0; lCount < Interfaces.Length; lCount++)
      {
        if (Interfaces[lCount].sUsed == false)
        {
          Interfaces[lCount].sUsed = true;
          Interfaces[lCount].sIfcID = pID;
          Interfaces[lCount].sIfcName = pName;
          Interfaces[lCount].sIfcDescr = pDescr;
          Interfaces[lCount].sIfcIPAddr = pIPAddr;
          Interfaces[lCount].sIfcBcastAddr = pBCAddr;
          Interfaces[lCount].sIfcNetAddr = pNWAddr;
          Interfaces[lCount].sIfcDefaultGW = pDefaultGW;
          Interfaces[lCount].sIfcGWMAC = pGWMAC;

          break;
        } // if (lTmp...
      } // foreach (s...
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static int NumInterfaces()
    {
      int lCounter = 0;

      foreach (sInterfaces lTmpIfc in Interfaces)
        if (lTmpIfc.sUsed == false)
          lCounter++;

      return (lCounter);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pIfcID"></param>
    /// <returns></returns>
    public static sInterfaces GetIfcByID(String pIfcID)
    {
      sInterfaces lRetVal = new sInterfaces();

      foreach (sInterfaces lTmpIfc in Interfaces)
      {
        LogConsole.Main.LogConsole.pushMsg("/" + lTmpIfc.sIfcID + "/" + pIfcID + "/");
        if (lTmpIfc.sIfcID == pIfcID)
        {
          lRetVal = lTmpIfc;
          break;
        } // if (lTmpI...
      } //  foreach (sInte...

      return (lRetVal);
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="pIndex"></param>
    /// <returns></returns>
    public static String GetIDByIndex(int pIndex)
    {
      String lRetVal = String.Empty;

      if (pIndex >= 0 && pIndex < Interfaces.Length)
        lRetVal = Interfaces[pIndex].sIfcID;

      return (lRetVal);
    }

    #endregion

  }
}