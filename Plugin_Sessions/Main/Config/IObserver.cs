using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Plugin.Main.Session.Config;

namespace Plugin.Main.Session
{
  public interface IObserver
  {
    void update(List<Session.Config.Session> oDict);
  }
}
