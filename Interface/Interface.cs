using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Simsang.Plugin
{
  public class PluginProperties
  {
    public String PluginName { get; set; }
    public String PluginDescription { get; set; }
    public String PluginVersion { get; set; }
    public String Ports { get; set; }
    public String BaseDir { set; get; }
    public String SessionDir { set; get; }
    public bool IsActive { get; set; }
  }

  public interface IPlugin
  {
    Control PluginControl { get; }
    PluginProperties Config { set; get; }
    IPluginHost Host { get; set; }


    String getData();
    String onGetSessionData(String pSessionID);
    void SetTargets(List<String> pTargetList);
    void onNewData(String pData);
    void onSaveSessionData(String pSessionID);
    void onLoadSessionDataFromFile(String pSessionID);
    void onLoadSessionDataFromString(String pSessionData);
    void onDeleteSessionData(String pSessionID);
    void onResetPlugin();
    void onInit();
    void onShutDown();
    void onStartAttack();
    void onStopAttack();
  }


  /// <summary>
  /// The host
  /// </summary>
  public interface IPluginHost
  {
    bool Register(IPlugin ipi);
    bool IsDebuggingOn();
    String GetInterface();
    String GetStartIP();
    String GetStopIP();
    String GetSessionName();
    String GetCurrentIP();
    List<Tuple<String, String, String>> GetAllReachableSystems();// MAC, IP, Vendor
    String GetWorkingDirectory();
    String GetAPEWorkingDirectory();
    String GetAPEFWRulesFile();
    String GetAPEInjectionRulesFile();
    String GetDNSPoisoningHostsFile();
    void PluginSetStatus(Object pPluginObj, String pStatus);
    void LogMessage(String pMsg);
    void StopAllPlugins();
  }
}
