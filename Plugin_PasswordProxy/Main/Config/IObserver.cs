using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Plugin.Main.HTTPProxy
{
  public interface IObserver
  {
    void updateRecords(List<Account> oDict);
    void updateRedirectURL(String pRedirectURL);
    void updateRemoteHostName(String pRemoteHostName);
  }
}
