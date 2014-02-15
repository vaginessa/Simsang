using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Simsang.Contribute.Infrastructure;

namespace Simsang.Contribute.Main
{
  public partial class ContributeConfirmation : Form
  {

    #region MEMBERS

    private Settings cSettings;

    #endregion


    #region PUBLIC

    /// <summary>
    /// 
    /// </summary>
    public ContributeConfirmation()
    {
      InitializeComponent();

      StringBuilder lContributionStr = new System.Text.StringBuilder();
      lContributionStr.Append(@"{\rtf1\ansi {\b \fs30 Disclaimer}\line \line Hereby you confirm that you will use this software only for educational purpose. " +
                @"The creator of this program doesn't take any responsibility and is not liable for any damage caused " +
                @"through use of this software, be it direct, indirect, incidental or consequential damages. \line \line \line" +
                @"{\b \fs30 User contributions}\line \line This is the free version of Simsang. Hereby you confirm that by each scan " +
                @"you have conducted you contribute parts of the outcome to the user comunity and that we are permitted to use this " +
                @"data for further evaluations. The contributed parts are " +
                @"namely the {\b first three octets} of the default gateway and the {\b software version}.\line " +
                @"Only this data and nothing else, {\b \fs25 no sensitive data} that was discovered during a scan is or will be transferred to us as project contribution. " +
                @"We fully respect your privacy and appreciate your contribution.}");

      RTB_Agreement.Rtf = lContributionStr.ToString();

      cSettings = Settings.getInstance();
    }

    #endregion


    #region EVENTS

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BT_OK_Click(object sender, EventArgs e)
    {
      if (CHKB_Agree.Checked)
        cSettings.setContributeStatus("ok");
      else
        cSettings.setContributeStatus("nok");

      if (CKB_ContribSSID.Checked)
        cSettings.setSSIDStatus("ok");
      else
        cSettings.setSSIDStatus("nok");

      Dispose();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CHKB_Agree_CheckedChanged(object sender, EventArgs e)
    {
      if (CHKB_Agree.Checked)
        CKB_ContribSSID.Enabled = true;
      else
        CKB_ContribSSID.Enabled = false;
    }

    #endregion

  }
}
