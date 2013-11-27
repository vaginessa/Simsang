using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simsang.NativeWifi;


namespace Simsang.Wifi
{
  public class Wifi
  {

    #region MEMBERS

    private static Wifi cInstance;

    #endregion


    #region PUBLIC

    private Wifi()
    {
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static Wifi getInstance()
    {
      if (cInstance == null)
        cInstance = new Wifi();

      return (cInstance);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public String getCurrentWifiName()
    {
      String lRetVal = String.Empty;
      WlanClient lClient = new WlanClient();
      List<String> lConnectedSsids = new List<String>();

      foreach (WlanClient.WlanInterface lTmpWifiIfc in lClient.Interfaces)
      {
        Wlan.Dot11Ssid lSsid = lTmpWifiIfc.CurrentConnection.wlanAssociationAttributes.dot11Ssid;
        lConnectedSsids.Add(new String(Encoding.ASCII.GetChars(lSsid.SSID, 0, (int)lSsid.SSIDLength)));
      }

      foreach (String lTmp in lConnectedSsids)
        lRetVal += lTmp + " ";

      return (lRetVal);
    }


    #endregion

  }
}
