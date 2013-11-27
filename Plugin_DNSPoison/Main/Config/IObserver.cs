using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Plugin.Main.DNSPoison
{
  public interface IObserver
  {
    void update(BindingList<DNSPoisonRecord> oDict);
  }
}
