using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Plugin.Main.DNSRequest.Config;

namespace Plugin.Main.DNSRequest
{
  public interface IObserver
  {
    void update(BindingList<DNSRequestRecord> oDict);
  }
}
