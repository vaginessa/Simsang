using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;


namespace Plugin.Main.Systems.ManageSystems
{
  public interface IObserver
  {
    void update(List<SystemPattern> oDict);
  }
}
