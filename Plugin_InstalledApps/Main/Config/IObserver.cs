using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;


namespace Plugin.Main.Applications.Config
{
  public interface IObserver
  {
    void update(List<ApplicationRecord> oDict);
  }
}
