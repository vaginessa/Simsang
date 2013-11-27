using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Plugin.Main.HTTPRequest
{
  public interface IObserver
  {
    void update(List<HTTPRequests> oDict);
  }
}
