using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Plugin.Main.Firewall.Config
{

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
