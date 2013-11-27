using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Plugin.Main.Applications.Config
{
  public interface IObservable
  {
    void addObserver(IObserver o);
    void notify();
  }
}
