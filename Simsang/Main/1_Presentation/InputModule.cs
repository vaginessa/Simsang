using System;
using System.Threading;
using System.IO;
using System.IO.Pipes;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Text;
using System.ComponentModel;
using Simsang.Plugin;




namespace Simsang
{
  class InputModule
  {

    #region MEMBERS

    private ACMain mACMain;
    private Thread[] mInputWorkerThread = new Thread[Config.PipeInstances];
    private static bool mStopThreads = false;
    private NamedPipeServerStream[] mPipeStream = new NamedPipeServerStream[Config.PipeInstances];
    private StreamReader[] mStreamReader = new StreamReader[Config.PipeInstances];

    #endregion


    #region PUBLIC

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pACMain"></param>
    public InputModule(ACMain pACMain)
    {
      mACMain = pACMain;
    }


    /// <summary>
    /// 
    /// </summary>
    public void startInputThread()
    {
      mStopThreads = false;


      try
      {
        /*
         * We have several concurrently running NamedPipes reading
         * input data. Start them all.
         */
        for (int i = 0; i < Config.PipeInstances; i++)
        {

          try
          {
            if (mPipeStream[i] != null)
            {
              mPipeStream[i].Close();
              mPipeStream[i] = null;
            }
          }
          catch (Exception lEx)
          {
            LogConsole.Main.LogConsole.pushMsg("Can't start named pipe - " + lEx.StackTrace + "\n" + lEx.ToString());
            MessageBox.Show("Can't start named pipe : " + lEx.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
          }


          mInputWorkerThread[i] = new Thread(new ParameterizedThreadStart(dataInputThread));
          mInputWorkerThread[i].Start(i);
        } // for (int i ...
      }
      catch (Exception lEx)
      {
        LogConsole.Main.LogConsole.pushMsg("An error occurred while starting the sniffer : " + lEx.StackTrace + "\n" + lEx.ToString());
        MessageBox.Show("An error occurred while starting the sniffer : " + lEx.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }



    /// <summary>
    /// 
    /// </summary>
    public void stopInputThreads()
    {
      NamedPipeClientStream lPipeClient = null;
      StreamWriter lStreamWriter = null;


      mStopThreads = true;

      ///*
      if (mPipeStream != null)
      {
        for (int i = 0; i < Config.PipeInstances; i++)
        {
          try
          {
            ClosePipeStream(mPipeStream[i], mStreamReader[i]);

            lPipeClient = new NamedPipeClientStream(".", Config.PipeName, PipeDirection.InOut);
            lStreamWriter = new StreamWriter(lPipeClient);

            lPipeClient.Connect(500);
            lStreamWriter.AutoFlush = true;
            lStreamWriter.WriteLine("QUIT\r\n");
            //                    lStreamWriter.Close();

            //                    lPipeClient.Close();
            //                    lPipeClient = null;                    
            /*
                                if (mInputWorkerThread != null)
                                {
                                    //System.Threading.Thread.Sleep(500);
                                  try { mInputWorkerThread[i].Abort(); } catch { }
                                    //mInputWorkerThread.Join();
                                  try { mInputWorkerThread[i].Interrupt();  } catch { }
                                }
            */
          }
          catch (TimeoutException lTEx)
          {
            LogConsole.Main.LogConsole.pushMsg(lTEx.StackTrace + "\n" + lTEx.ToString());
          }
          catch (Exception lEx)
          {
            LogConsole.Main.LogConsole.pushMsg("An error occurred while starting the sniffer : " + lEx.StackTrace + "\n" + lEx.ToString());
          }
          finally
          {
            if (lStreamWriter != null)
              try { lStreamWriter.Close(); }
              catch { }

            if (lPipeClient != null)
            {
              try { lPipeClient.Close(); }
              catch { }
              lPipeClient = null;
            }
          }
        } // for (int i ...


        for (int i = 0; i < Config.PipeInstances; i++)
        {
          if (mPipeStream != null && mPipeStream[i] != null)
          {
            try
            {
              mPipeStream[i].Disconnect();
              mPipeStream[i].Close();
              mPipeStream[i].Dispose();
            }
            catch { }
          }
        }



      } // if (mPipeStream !...
      //*/            
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pData"></param>
    public void dataInputThread(object pData)
    //        public static void dataInputThread(object pData)
    {
      int lThreadNo = (int)pData;
      string lTmpLine = String.Empty;



      while (mStopThreads == false)
      {
        try
        {
          ClosePipeStream(mPipeStream[lThreadNo], mStreamReader[lThreadNo]);

          mPipeStream[lThreadNo] = new NamedPipeServerStream(Config.PipeName, PipeDirection.InOut, Config.PipeInstances); //, PipeTransmissionMode.Byte, PipeOptions.None);
          mPipeStream[lThreadNo].WaitForConnection();
          mStreamReader[lThreadNo] = new StreamReader(mPipeStream[lThreadNo]);
        }
        catch (Exception lEx)
        {
          LogConsole.Main.LogConsole.pushMsg(lEx.StackTrace);

          try { ClosePipeStream(mPipeStream[lThreadNo], mStreamReader[lThreadNo]); }
          catch { }
          mPipeStream = null;
          mStreamReader = null;
          mStopThreads = true;
          break;
        }



        /*
         * 
         */
        try
        {
          while (mPipeStream[lThreadNo] != null &&
                 mStreamReader[lThreadNo] != null &&
                 mPipeStream[lThreadNo].IsConnected)
          {
            try
            {

              if ((lTmpLine = mStreamReader[lThreadNo].ReadLine()) != null && lTmpLine.Length > 0)
              {
                if (Regex.Match(lTmpLine, "QUIT", RegexOptions.IgnoreCase).Success)
                {
                  ClosePipeStream(mPipeStream[lThreadNo], mStreamReader[lThreadNo]);
                  mPipeStream = null;
                  mStreamReader = null;
                  mStopThreads = true;
                  break;
                } // if (Regex.Matc...

                mACMain.UpdateMainTB(lTmpLine);
              }
              else
              {
                break;
              } // if (lTmpLine != nul..
            }
            catch (ObjectDisposedException lEx)
            {
              LogConsole.Main.LogConsole.pushMsg(lEx.StackTrace + "\n" + lEx.ToString());
              break;
            }
            catch (Exception lEx)
            {
              LogConsole.Main.LogConsole.pushMsg(lEx.StackTrace + "\n" + lEx.ToString());
              break;
            }
          } // while (mStopT...
        }
        catch (Exception lEx)
        {
          LogConsole.Main.LogConsole.pushMsg(lEx.StackTrace + "\n" + lEx.ToString());
        }

      } // while (1 == 1
    }

    #endregion


    #region PRIVATE

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pPipeStream"></param>
    /// <param name="pStreamReader"></param>
    private static void ClosePipeStream(NamedPipeServerStream pPipeStream, StreamReader pStreamReader)
    {
      if (pPipeStream != null)
      {
        pPipeStream.Close();
        pPipeStream = null;
      }
      if (pStreamReader != null)
      {
        pStreamReader.Close();
        pStreamReader = null;
      }
    }


    #endregion

  }
}
