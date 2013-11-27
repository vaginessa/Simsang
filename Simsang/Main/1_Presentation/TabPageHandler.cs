using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;




namespace Simsang
{
  public class TabPageHandler
  {

    #region MEMBERS

    private TabControl mTB_Control;
    private TabPage[] mTabPages;
    private Hashtable mPluginPosition;

    #endregion


    #region PROPERTIES

    public Hashtable PluginPosition { get { return (mPluginPosition); } }
    public TabPage[] TabPages { get { return (mTabPages); } }

    #endregion


    #region PUBLIC

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pTBControl"></param>
    /// <param name="pPluginPosition"></param>
    public TabPageHandler(TabControl pTBControl, Hashtable pPluginPosition)
    {
      int lCount = 0;
      mTB_Control = pTBControl;
      mPluginPosition = pPluginPosition;

      if (pTBControl.TabPages.Count > 0)
      {
        mTabPages = new TabPage[pTBControl.TabPages.Count];

        foreach (TabPage lTabPage in pTBControl.TabPages)
        {
          mTabPages[lCount] = lTabPage;
          lCount++;
        } // foreach (TabPage...
      } // if (pTBControl.Ta...
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pTabName"></param>
    /// <returns></returns>
    public TabPage FindTabPage(String pTabName)
    {
      TabPage lRetVal = null;

      if (mTabPages != null)
      {
        for (int lCount = 0; lCount < mTabPages.Length; lCount++)
        {
          if (mTabPages[lCount].Text.ToLower() == pTabName.ToLower())
          {
            lRetVal = mTabPages[lCount];
            break;
          } // if (mTabPages[lCo...
        } // for (int lCoun...
      } // if (mTabPag...


      return (lRetVal);
    }




    /// <summary>
    /// 
    /// </summary>
    /// <param name="pCurrentTabPage"></param>
    public void HideTabPage(TabPage pCurrentTabPage)
    {
      if (mTB_Control.TabPages.Contains(pCurrentTabPage))
        mTB_Control.TabPages.Remove(pCurrentTabPage);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pNewTabPage"></param>
    public void ShowTabPage(TabPage pNewTabPage)
    {
      if (pNewTabPage != null)
      {
        try
        {
          String lNewTPText = pNewTabPage.Text.ToLower();

          for (int i = 0; i < mTB_Control.TabPages.Count; i++)
          {
            String lCheckText = mTB_Control.TabPages[i].Text.ToLower();

            if (lCheckText.Contains("simsang"))
            {
              mTB_Control.TabPages.Insert(i, pNewTabPage);
              break;
            } // if (lChe...



            if (lNewTPText.CompareTo(lCheckText) < 0)
            {
              mTB_Control.TabPages.Insert(i, pNewTabPage);
              break;
            } // if (lCo...                
          } // for (int i = ...              
        }
        catch (Exception)
        {
          mTB_Control.TabPages.Insert(0, pNewTabPage);
        }
      } // if (tp..
    }


    #endregion

  }
}
