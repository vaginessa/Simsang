using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using NUnit.Framework;
using NUnitTests;

using Simsang.ARPScan.Main;
using Simsang.ARPScan.Main.Config;

namespace NU_Simsang
{
  [TestFixture]
  public class NU_ARPScan
  {

    #region MEMBERS

//    private InfrastructureFacade cInfrastructureFacade;
    private TaskFacade cTask;
    private ARPScan cPresentation;
    private String cARPScanPathSource = @"C:\Users\run\Documents\Visual Studio 2010\Projects\Simsang\Debug\ARPScan.exe";
    private String cARPScanPathLocal = @".\bin\ARPScan.exe";

    #endregion


    #region NUNIT

    #region PARSING INPUT

    [SetUp]
    public void init()
    {
//      cInfrastructureFacade = InfrastructureFacade.getInstance();
      cTask = TaskFacade.getInstance();
      cPresentation = ARPScan.GetInstance();
    }


    [TearDown]
    public void dispose()
    {
    }


    [Test]
    [ExpectedException(typeof(Exception), ExpectedMessage = "ARPscan binary not found")]
    public void INFRASTRUCTURE_binary_not_found()
    {
      Tools.removeFile(cARPScanPathLocal);
      ARPScanConfig lConfig = new ARPScanConfig() { InterfaceID = "our-ifc-id", StartIP = "192.168.1.0", StopIP = "192.168.1.150", GatewayIP = "192.168.1.1", LocalIP = "192.168.1.123" };
      cTask.startARPScan(lConfig);
    }


    [Test]
    [ExpectedException(typeof(Exception), ExpectedMessage = "Something is wrong with the interface ID")]
    public void INFRASTRUCTURE_interface_null()
    {
      getARPScanBinary();
      ARPScanConfig lConfig = new ARPScanConfig() { InterfaceID = null, StartIP = "192.168.1.0", StopIP = "192.168.1.150", GatewayIP = "192.168.1.1", LocalIP = "192.168.1.123" };
      cTask.startARPScan(lConfig);
    }

    [Test]
    [ExpectedException(typeof(Exception), ExpectedMessage = "Something is wrong with the interface ID")]
    public void INFRASTRUCTURE_interface_empty()
    {
      getARPScanBinary();
      ARPScanConfig lConfig = new ARPScanConfig() { InterfaceID = "", StartIP = "192.168.1.0", StopIP = "192.168.1.150", GatewayIP = "192.168.1.1", LocalIP = "192.168.1.123" };
      cTask.startARPScan(lConfig);
    }




    [Test]
    [ExpectedException(typeof(Exception), ExpectedMessage = "Something is wrong with the start IP address")]
    public void INFRASTRUCTURE_startIP_null()
    {
      getARPScanBinary();
      ARPScanConfig lConfig = new ARPScanConfig() { InterfaceID = "our-ifc-id", StartIP = null, StopIP = "192.168.1.150", GatewayIP = "192.168.1.1", LocalIP = "192.168.1.123" };
      cTask.startARPScan(lConfig);
    }

    [Test]
    [ExpectedException(typeof(Exception), ExpectedMessage = "Something is wrong with the start IP address")]
    public void INFRASTRUCTURE_startIP_empty()
    {
      getARPScanBinary();
      ARPScanConfig lConfig = new ARPScanConfig() { InterfaceID = "our-ifc-id", StartIP = "", StopIP = "192.168.1.150", GatewayIP = "192.168.1.1", LocalIP = "192.168.1.123" };
      cTask.startARPScan(lConfig);
    }



    [Test]
    [ExpectedException(typeof(Exception), ExpectedMessage = "Something is wrong with the stop IP address")]
    public void INFRASTRUCTURE_stopIP_null()
    {
      getARPScanBinary();
      ARPScanConfig lConfig = new ARPScanConfig() { InterfaceID = "our-ifc-id", StartIP = "192.168.1.0", StopIP = null, GatewayIP = "192.168.1.1", LocalIP = "192.168.1.123" };
      cTask.startARPScan(lConfig);
    }

    [Test]
    [ExpectedException(typeof(Exception), ExpectedMessage = "Something is wrong with the stop IP address")]
    public void INFRASTRUCTURE_stopIP_empty()
    {
      getARPScanBinary();
      ARPScanConfig lConfig = new ARPScanConfig() { InterfaceID = "our-ifc-id", StartIP = "192.168.1.0", StopIP = "", GatewayIP = "192.168.1.1", LocalIP = "192.168.1.123" };
      cTask.startARPScan(lConfig);
    }


    [Test]
    [ExpectedException(typeof(Exception), ExpectedMessage = "Start IP address is greater than stop IP address")]
    public void INFRASTRUCTURE_stopIP_greater_than_startIP()
    {
      getARPScanBinary();
      ARPScanConfig lConfig = new ARPScanConfig() { InterfaceID = "our-ifc-id", StartIP = "192.168.1.140", StopIP = "192.168.1.15", GatewayIP = "192.168.1.1", LocalIP = "192.168.1.123" };
      cTask.startARPScan(lConfig);
    }

    #endregion

    #region RECORDS
      
    // Add record
    // Not testable. Infrastructure and presentation layer 
    // overlap on too many places... bad design has to be fixed!!
    //[Test]
    //public void PRESENTATION_add_record()
    //{
    //  InfrastructureFacade lInfrastructure = InfrastructureFacade.getInstance();
    //  BindingList<String> lTargetList = new BindingList<string>();
    //  Simsang.ACMain lACMain = new Simsang.ACMain(null);
    //  ARPScan lARPScan = ARPScan.GetInstance(lACMain, ref lTargetList);
    //  lARPScan.updateTextBox("<arp><type>mytype</type><ip>myip</ip><mac>mymac</mac></arp>");
    //}

    // Add record Twice
    // Clear records


    #endregion

    #endregion


    #region PRIVATE

    /// <summary>
    /// 
    /// </summary>
    private void getARPScanBinary()
    {
      try
      {
        if (!Directory.Exists("bin"))
          Directory.CreateDirectory(@"bin");

        Tools.removeFile(cARPScanPathLocal);
        File.Copy(cARPScanPathSource, cARPScanPathLocal);
      }
      catch (Exception) { } 
    }

    #endregion

  }
}
