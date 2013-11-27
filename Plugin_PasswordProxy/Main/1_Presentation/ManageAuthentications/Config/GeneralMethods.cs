using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;
using System.Net;
using System.Diagnostics;
using System.Collections;
using System.ComponentModel;
using System.IO;

namespace Plugin.Main
{
    class GeneralMethods
    {


        public static String FindProc(int pPort)
        {
            String lRetVal = String.Empty;

            if (pPort > 0 && pPort < 65536)
            {
              try
              {
                TcpTable lTcpTable = ManagedIpHelper.GetExtendedTcpTable(true);
                foreach (TcpRow tcpRow in lTcpTable)
                {
                  if (tcpRow.LocalEndPoint.Port == pPort)
                  {
                    try
                    {
                      Process lProc = Process.GetProcessById(tcpRow.ProcessId);
                      lRetVal = lProc.ProcessName;
                      break;
                    }
                    catch (Exception lEx)
                    {
                      break;
                    }
                  } // if (tcpRow.L...
                } // foreach (TcpRow tcpR..
              }
              catch (Exception lEx)
              { 
              }
            } // if (pPort > 0 ...

            return (lRetVal);
        }
    }






    public class TcpTable : IEnumerable<TcpRow>
    {
        private IEnumerable<TcpRow> tcpRows;


        public TcpTable(IEnumerable<TcpRow> tcpRows)
        {
            this.tcpRows = tcpRows;
        }


        public IEnumerable<TcpRow> Rows
        { get { return this.tcpRows; } }


        public IEnumerator<TcpRow> GetEnumerator()
        { return this.tcpRows.GetEnumerator(); }


        IEnumerator IEnumerable.GetEnumerator()
        { return this.tcpRows.GetEnumerator(); }
    }

    public class TcpRow
    {
        private IPEndPoint localEndPoint;
        private IPEndPoint remoteEndPoint;
        private TcpState state;
        private int processId;


        public TcpRow(IpHelper.TcpRow tcpRow)
        {
            this.state = tcpRow.state;
            this.processId = tcpRow.owningPid;

            int localPort = (tcpRow.localPort1 << 8) + (tcpRow.localPort2) + (tcpRow.localPort3 << 24) + (tcpRow.localPort4 << 16);
            long localAddress = tcpRow.localAddr;
            this.localEndPoint = new IPEndPoint(localAddress, localPort);

            int remotePort = (tcpRow.remotePort1 << 8) + (tcpRow.remotePort2) + (tcpRow.remotePort3 << 24) + (tcpRow.remotePort4 << 16);
            long remoteAddress = tcpRow.remoteAddr;
            this.remoteEndPoint = new IPEndPoint(remoteAddress, remotePort);
        }


        public IPEndPoint LocalEndPoint
        { get { return this.localEndPoint; } }

        public IPEndPoint RemoteEndPoint
        { get { return this.remoteEndPoint; } }

        public TcpState State
        { get { return this.state; } }

        public int ProcessId
        { get { return this.processId; } }

    }




    public static class ManagedIpHelper
    {
        public static TcpTable GetExtendedTcpTable(bool sorted)
        {
            List<TcpRow> tcpRows = new List<TcpRow>();

            IntPtr tcpTable = IntPtr.Zero;
            int tcpTableLength = 0;

            if (IpHelper.GetExtendedTcpTable(tcpTable, ref tcpTableLength, sorted, IpHelper.AfInet, IpHelper.TcpTableType.OwnerPidAll, 0) != 0)
            {
                try
                {
                    tcpTable = Marshal.AllocHGlobal(tcpTableLength);
                    if (IpHelper.GetExtendedTcpTable(tcpTable, ref tcpTableLength, true, IpHelper.AfInet, IpHelper.TcpTableType.OwnerPidAll, 0) == 0)
                    {
                        IpHelper.TcpTable table = (IpHelper.TcpTable)Marshal.PtrToStructure(tcpTable, typeof(IpHelper.TcpTable));

                        IntPtr rowPtr = (IntPtr)((long)tcpTable + Marshal.SizeOf(table.length));
                        for (int i = 0; i < table.length; ++i)
                        {
                            tcpRows.Add(new TcpRow((IpHelper.TcpRow)Marshal.PtrToStructure(rowPtr, typeof(IpHelper.TcpRow))));
                            rowPtr = (IntPtr)((long)rowPtr + Marshal.SizeOf(typeof(IpHelper.TcpRow)));

                        }
                    }
                }
                finally
                {
                    if (tcpTable != IntPtr.Zero)
                        Marshal.FreeHGlobal(tcpTable);
                }
            }

            return new TcpTable(tcpRows);
        }

    }



    public static class IpHelper
    {
        public const String DllName = "iphlpapi.dll";
        public const int AfInet = 2;




        [DllImport(IpHelper.DllName, SetLastError = true)]
        public static extern uint GetExtendedTcpTable(IntPtr tcpTable, ref int tcpTableLength, bool sort, int ipVersion, TcpTableType tcpTableType, int reserved);



        public enum TcpTableType
        {
            BasicListener,
            BasicConnections,
            BasicAll,
            OwnerPidListener,
            OwnerPidConnections,
            OwnerPidAll,
            OwnerModuleListener,
            OwnerModuleConnections,
            OwnerModuleAll,
        }



        [StructLayout(LayoutKind.Sequential)]
        public struct TcpTable
        {
            public uint length;
            public TcpRow row;
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct TcpRow
        {
            public TcpState state;
            public uint localAddr;
            public byte localPort1;
            public byte localPort2;
            public byte localPort3;
            public byte localPort4;
            public uint remoteAddr;
            public byte remotePort1;
            public byte remotePort2;
            public byte remotePort3;
            public byte remotePort4;
            public int owningPid;
        }

    }

}
