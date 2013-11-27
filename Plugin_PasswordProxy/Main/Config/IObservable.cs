using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Plugin.Main.HTTPProxy
{
  public interface IObservable
  {
    void addObserver(IObserver o);
    void notifyRecords();
    void notifyRedirectURL();
    void notifyRemoteHost();
  }
}
