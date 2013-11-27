using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Plugin
{
  public partial class Main_Notes : Form
  {
    public Main_Notes()
    {
      InitializeComponent();
    }

    public void setText(string pData)
    {
      TB_Data.Text = pData;
      TB_Data.Select(0, 0);
    }

    public void appendText(string pData)
    {
      TB_Data.Text += pData;
      TB_Data.Select(0, 0);

      HighlightPhrase(TB_Data, "Cookies");
      HighlightPhrase(TB_Data, "System");
      HighlightPhrase(TB_Data, "Website");
    }





    void HighlightPhrase(RichTextBox pRTB, string pSearch)
    {
      int pos = pRTB.SelectionStart;
      string s = pRTB.Text;
      Font lFont = new Font("Verdana", 8, FontStyle.Bold);
      for (int ix = 0; ; )
      {
        int jx = s.IndexOf(pSearch, ix, StringComparison.CurrentCultureIgnoreCase);
        if (jx < 0)
          break;



        pRTB.SelectionStart = jx;
        pRTB.SelectionLength = pSearch.Length;
        pRTB.SelectionFont = lFont;


        ix = jx + 1;
      } // for (int ...

      pRTB.SelectionStart = pos;
      pRTB.SelectionLength = 0;
    }

  }
}
