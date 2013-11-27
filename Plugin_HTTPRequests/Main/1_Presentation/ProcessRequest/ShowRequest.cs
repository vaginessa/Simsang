using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;




namespace Plugin.Main
{
  public partial class ShowRequest : Form
  {

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pContent"></param>
    public ShowRequest(String pContent)
    {
      InitializeComponent();

      TB_Request.Text = pContent;
      TB_Request.Select(0, 0);

      this.KeyUp += PluginHTTPProxyUC_KeyUp;
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void PluginHTTPProxyUC_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Escape)
        this.Close();
    }
  }
}
