using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;


namespace Simsang.Session.Config
{
  public interface IObserver
  {
    void update(List<AttackSession> oDict);
  }
}
