using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using ManageSystems = Plugin.Main.Systems.ManageSystems;


namespace Plugin.Main.Systems.Config
{
  public interface IRecordObserver
  {
    void updateRecordList(List<SystemRecord> oDict);
  }

  public interface ISystemPatternObserver
  {
    void updateSystemPatternList(List<ManageSystems.SystemPattern> oDict);
  }
}
