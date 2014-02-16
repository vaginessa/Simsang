using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using Simsang.ARPScan.Main.Config;
using Simsang.ARPScan.SystemFingerprint;

namespace Simsang.ARPScan
{
  public partial class ScanMultipleSystems : Form
  {

    #region MEMBERS

    private static ScanMultipleSystems mInstance;
    private List<ScanSystem> mTargetSystems = new List<ScanSystem>();
    private TaskFacadeFingerprint cTaskFingerprint;
    private AutoResetEvent mFingerprintingFinished = new AutoResetEvent(false);

    #endregion


    #region PROPERTIES

    public List<Tuple<String, String>> TargetSystems 
    {
      get
      { 
        List<Tuple<String, String>> lRetVal = new List<Tuple<String, String>>();
        foreach (ScanSystem lTmp in mTargetSystems)
          lRetVal.Add(new Tuple<String, String> (lTmp.TargetMAC, lTmp.TargetIP));

        return lRetVal;
      } 

      set 
      {
        mTargetSystems.Clear(); 
        foreach (Tuple<String,String> lTmp in value) 
          mTargetSystems.Add(new ScanSystem(lTmp.Item2, lTmp.Item1)); 
      }
    }

    #endregion


    #region PUBLIC

    /// <summary>
    /// 
    /// </summary>
    public ScanMultipleSystems()
    {
      InitializeComponent();

      cTaskFingerprint = TaskFacadeFingerprint.getInstance();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static ScanMultipleSystems getInstance(List<Tuple<String, String>> pSystems)
    {
      if (mInstance == null)
        mInstance = new ScanMultipleSystems();

      mInstance.TargetSystems = pSystems;
      mInstance.startFingerprintingProcess();

      return mInstance;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ScanMultipleSystems_FormClosing(object sender, FormClosingEventArgs e)
    {
      // Stopping scan process
      cTaskFingerprint.stopFingerprint();

      // Hiding form
      this.Hide();

      e.Cancel = true;
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


    #region PRIVATE

    /// <summary>
    /// 
    /// </summary>
    private void startFingerprintingProcess()
    {
      int lCounter = 0;
      PGB_Systems.Minimum = 0;
      PGB_Systems.Maximum = mTargetSystems.Count;
      
      if (mTargetSystems != null && mTargetSystems.Count > 0)
      {
        foreach (ScanSystem lTmp in mTargetSystems)
        {
          lCounter++;
          mFingerprintingFinished.Reset();
          SystemFingerprint.TaskFacadeFingerprint lTaskFacadeFingerprint = SystemFingerprint.TaskFacadeFingerprint.getInstance();

          TSSL_Title.Text = String.Format("System {0} of {1}", lCounter, mTargetSystems.Count);
          TSSL_CurrentSystem.Text = String.Format("Current system: {0}  ({1})", lTmp.TargetIP, lTmp.TargetMAC);

          FingerprintConfig lConfig = new FingerprintConfig () 
          { 
            IP = lTmp.TargetIP, 
            MAC = lTmp.TargetMAC, 
            OnScanStopped = FingerprintStoppedCallback,
            IsDebuggingOn = Config.DebugOn()
          };

          lTaskFacadeFingerprint.startFingerprint(lConfig);

          try
          {
            mFingerprintingFinished.WaitOne(30*1000);
          }
          catch (Exception lEx)
          {
//            MessageBox.Show(String.Format("scan ({0}) aborted: {1}", lTmp.TargetIP, lEx.Message));
          }

          PGB_Systems.Increment(1);

        } // foreach (Scan...
      } // if (mTarge...
    }


    /// <summary>
    /// 
    /// </summary>
    private void FingerprintStoppedCallback()
    {
      mFingerprintingFinished.Set();
    }

    #endregion

  }
}
