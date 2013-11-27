using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Plugin.Main.POP3Proxy.Config;

namespace Plugin.Main.POP3Proxy
{
  public interface IObserver
  {
    void update(List<POP3Account> oDict);
  }
}
