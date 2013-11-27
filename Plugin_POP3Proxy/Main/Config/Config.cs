using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Plugin.Main.POP3Proxy.Config
{

  #region TYPE DEFINITION

  public delegate void OnProxyExitDelegate();

  #endregion


  /// <summary>
  /// 
  /// </summary>
  public class ProxyConfig
  {
    public bool isDebuggingOn;
    public OnProxyExitDelegate onProxyExit;
    public String BasisDirectory;
    public String RemoteHostName;
  }


  /// <summary>
  /// 
  /// </summary>
  public class ExceptionWarning : Exception
  {
    public ExceptionWarning(String pMsg)
      : base(pMsg)
    {
    }
  }


  /// <summary>
  /// 
  /// </summary>
  public class ExceptionError : Exception
  {
    public ExceptionError(String pMsg)
      : base(pMsg)
    {
    }
  }

}
