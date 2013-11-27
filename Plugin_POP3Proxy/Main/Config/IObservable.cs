using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Plugin.Main.POP3Proxy
{
  public interface IObservable
  {
    void addObserver(IObserver o);
    void notify();
  }
}
