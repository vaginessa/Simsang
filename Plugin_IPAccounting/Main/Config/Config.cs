using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Plugin.Main.IPAccounting.Config
{

  #region TYPE DEFINITION

  public delegate void onAccountingExitDelegate();
  public delegate void onUpdateListDelegate(List<AccountingItem> lRecords);

  #endregion



  /// <summary>
  /// Global injection Configuration
  /// </summary>
  public class IPAccountingConfig
  {
    public bool isDebuggingOn;
    public onAccountingExitDelegate onIPAccountingExit;
    public onUpdateListDelegate onUpdateList;
    public String BasisDirectory;
    public String Interface;
    public String StructureParameter;
  }

}
