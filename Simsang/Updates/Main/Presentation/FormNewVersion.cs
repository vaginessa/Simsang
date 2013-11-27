using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Simsang;

namespace SimsangUpdates
{
  public partial class FormNewVersion : Form
  {

    #region PUBLIC

    public FormNewVersion()
    {
      InitializeComponent();
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

    private void LL_DownloadURL_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      System.Diagnostics.Process.Start(Config.ToolHomepage);
      Dispose();
    }

    #endregion

  }
}
