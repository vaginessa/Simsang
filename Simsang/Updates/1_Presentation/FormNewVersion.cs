using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Simsang;

namespace Simsang.Updates
{
  public partial class FormNewVersion : Form
  {

    #region PUBLIC

    public FormNewVersion()
    {
      InitializeComponent();

      Config.UpdateConfig lConf = InfrastructureFacade.getInstance().getUpdateConfig();
      StringBuilder lContributionStr = new System.Text.StringBuilder();

      try
      {
        lContributionStr.Append(@"{\rtf1\ansi There is a new Simsang version (" + lConf.AvailableVersionStr + @") available.\line Click on the link below to download it.");
        lContributionStr.Append(@"\line \line \line");
        lContributionStr.Append(@"{\b \fs20 Changes } \line ");
        if (lConf != null && lConf.Messages != null)
          foreach (String lTmp in lConf.Messages)
            lContributionStr.Append(@"\line \bullet  " + lTmp);

        lContributionStr.Append("}");
      }
      catch (Exception lEx)
      { }

      RTB_SimsangUpdate.Rtf = lContributionStr.ToString();
    }

    #endregion


    #region EVENTS

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BT_Close_Click(object sender, EventArgs e)
    {
      Dispose();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void LL_DownloadURL_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      System.Diagnostics.Process.Start(Simsang.Config.ToolHomepage);
      Dispose();
    }



    /// <summary>
    /// Close Sessions GUI on Escape.
    /// </summary>
    /// <param name="keyData"></param>
    /// <returns></returns>
    protected override bool ProcessDialogKey(Keys keyData)
    {
      if (keyData == Keys.Escape)
      {
        this.Close();
        return true;
      }
      else
        return base.ProcessDialogKey(keyData);
    }

    #endregion

  }
}
