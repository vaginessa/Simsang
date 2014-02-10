using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simsang.ARPScan.Main.Config
{
  public delegate void OnDataCallback(String pData);

  public class ARPScanConfig
  {
    public String InterfaceID;
    public String GatewayIP;
    public String LocalIP;
    public String StartIP;
    public String StopIP;
    public Action OnARPScanStopped;
    public OnDataCallback OnDataReceived;
    public bool IsDebuggingOn;
  }



  public class FingerprintConfig
  {
    public String MAC;
    public String IP;
    public bool IsDebuggingOn;
    public Action OnScanStopped;
  }
}
