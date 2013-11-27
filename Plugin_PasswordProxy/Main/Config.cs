using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Plugin.Main.HTTPProxy.Config
{

  #region TYPE DEFINITION

  public delegate void onWebServerExitedDelegate();

  #endregion



  /// <summary>
  /// Global injection Configuration
  /// </summary>
  public class WebServerConfig
  {

    #region MEMBERS

    public bool isDebuggingOn;
    public bool isRedirect;
    public onWebServerExitedDelegate onWebServerExit;
    public String BasisDirectory;
    public String RemoteHostName;
    public String RedirectToURL;

    #endregion

  }
}
