using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Plugin.Main.IMAP4Proxy.Config;

namespace Plugin.Main.IMAP4Proxy
{
  public interface IObserver
  {
    void update(List<IMAP4Account> oDict);
  }
}
