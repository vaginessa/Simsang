using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Plugin.Main.Session.ManageSessions.Config;


namespace Plugin.Main.Session.ManageSessions
{
  public interface IObserver
  {
    void update(List<SessionPattern> oDict);
  }
}
