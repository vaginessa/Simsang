using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Plugin.Main.Systems.Config
{
  public interface IRecordObservable
  {
    void addRecordObserver(IRecordObserver o);
    void notifyRecords();
  }

  public interface ISystemPatternObservable
  {
    void addSystemPatternObserver(ISystemPatternObserver o);
    void notifySystemPatterns();
  }

}
