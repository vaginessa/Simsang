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
    private SystemFingerprintStatus mFingerprintStatus;

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
      mFingerprintStatus = new SystemFingerprintStatus();
      TSSL_CurrentSystem.Text = String.Empty;
      TSSL_Title.Text = String.Empty;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static ScanMultipleSystems getInstance(List<Tuple<String, String>> pSystems)
    {
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
      // Setting cancelation in BGW
      this.stopFingerprintingProcess();

      // Hiding form
      this.Hide();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BGW_Scanner_DoWork(object sender, DoWorkEventArgs e)
    {
      PGB_Systems.Minimum = 0;
      PGB_Systems.Maximum = mTargetSystems.Count;

      mFingerprintStatus.CurrentIndexNo = 0;
      mFingerprintStatus.MaxIndex = mTargetSystems.Count;


      foreach (ScanSystem lTmp in mTargetSystems)
      {                 
        mFingerprintingFinished.Reset();
        SystemFingerprint.TaskFacadeFingerprint lTaskFacadeFingerprint = SystemFingerprint.TaskFacadeFingerprint.getInstance();

        mFingerprintStatus.CurrentIndexNo++;
        mFingerprintStatus.CurrentSystemIP = lTmp.TargetIP;
        mFingerprintStatus.CurrentSystemMAC = lTmp.TargetMAC;

        incrementProgress(mFingerprintStatus);


        if (!BGW_Scanner.CancellationPending)
        {
          FingerprintConfig lConfig = new FingerprintConfig()
          {
            IP = lTmp.TargetIP,
            MAC = lTmp.TargetMAC,
            OnScanStopped = FingerprintStoppedCallback,
            IsDebuggingOn = Config.DebugOn()
          };

          lTaskFacadeFingerprint.startFingerprint(lConfig);

          try
          {
            mFingerprintingFinished.WaitOne(30 * 1000);
          }
          catch (Exception lEx)
          {
            MessageBox.Show(String.Format("scan ({0}) aborted: {1}", lTmp.TargetIP, lEx.Message));
          }
        } // if (BGW_Sca...
      } // foreach (Scan...

      mFingerprintStatus.CurrentSystemIP = String.Empty;
      mFingerprintStatus.CurrentSystemMAC = String.Empty;
      incrementProgress(mFingerprintStatus);

      if (!BGW_Scanner.CancellationPending)
        stopFingerprintingProcess();
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
        TSSL_CurrentSystem.Text = "Stopping processes ...";
        stopFingerprintingProcess();
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
      if (mTargetSystems != null) // && mTargetSystems.Count > 0)
      {
        this.Cursor = Cursors.WaitCursor;
        BGW_Scanner.RunWorkerAsync();
      } // if (mTarge...
    }


    /// <summary>
    /// 
    /// </summary>
    delegate void stopFingerprintingProcessDelegate();
    private void stopFingerprintingProcess()
    {
      if (InvokeRequired)
      {
        BeginInvoke(new stopFingerprintingProcessDelegate(stopFingerprintingProcess), new object[] { });
        return; 
      }


      try
      {
        BGW_Scanner.CancelAsync();
      }
      catch (Exception lEx)
      {
        String lMsg = lEx.Message;
      }



      try
      {
        cTaskFingerprint.stopFingerprint();
      }
      catch (Exception lEx)
      {
        String lMsg = lEx.Message;
      }
   

      try
      {
        this.Cursor = Cursors.Default;
      }
      catch (Exception lEx)
      {
        String lMsg = lEx.Message;
      }

    }


    /// <summary>
    /// 
    /// </summary>
    delegate void incrementProgressDelegate(SystemFingerprintStatus pFingerprintStatus);
    private void incrementProgress(SystemFingerprintStatus pFingerprintStatus)
    {
      if (InvokeRequired)
      {
        BeginInvoke(new incrementProgressDelegate(incrementProgress), new object[] { pFingerprintStatus });
        return;
      }

      TSSL_Title.Text = String.Format("System {0} of {1}", pFingerprintStatus.CurrentIndexNo, pFingerprintStatus.MaxIndex);
      PGB_Systems.Increment(1);

      if (String.IsNullOrEmpty(pFingerprintStatus.CurrentSystemIP) && String.IsNullOrEmpty(pFingerprintStatus.CurrentSystemIP))
        TSSL_CurrentSystem.Text = "Done";
      else
        TSSL_CurrentSystem.Text = String.Format("Current system: {0}  ({1})", pFingerprintStatus.CurrentSystemIP, pFingerprintStatus.CurrentSystemMAC);  
    }


    /// <summary>
    /// 
    /// </summary>
    private void FingerprintStoppedCallback()
    {
      mFingerprintingFinished.Set();
    }

    #endregion


    #region DATATYPES

    public struct SystemFingerprintStatus
    {
      public int MaxIndex;
      public int CurrentIndexNo;
      public String CurrentSystemIP;
      public String CurrentSystemMAC;
    }

    #endregion

    private void BGW_Scanner_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
    }

  }
}
