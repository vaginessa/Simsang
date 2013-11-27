using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Plugin.Main.IPAccounting.ManageServices
{
  public interface IObserver
  {
    void update(List<ServiceRecord> oList);
  }
}
