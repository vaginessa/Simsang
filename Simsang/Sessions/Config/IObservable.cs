using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Simsang.Session.Config
{
  public interface IObservable
  {
    void addObserver(IObserver o);
    void notify();
  }
}
