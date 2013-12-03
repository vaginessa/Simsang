using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NUnit.Framework;
using NUnitTests;

using Simsang.LogConsole.Main;


namespace NU_Simsang
{
  [TestFixture]
  public class NU_Logconsole
  {

    #region MEMBERS
    #endregion


    #region NUNIT

    [SetUp]
    public void init()
    {
    }


    [TearDown]
    public void dispose()
    {
    }


    [Test]
    public void PRESENTATION_showLogConsole()
    {
      LogConsole lConsole = LogConsole.getInstance(true);
      LogConsole.showLogConsole();
    }

    [Test]
    public void PRESENTATION_write_log_record()
    {
      LogConsole lConsole = LogConsole.getInstance(true);
      LogConsole.pushMsg("first log record");
    }


    [Test]
    public void PRESENTATION_get_log_content()
    {
      LogConsole lConsole = LogConsole.getInstance(true);
      LogConsole.showLogConsole();

      LogConsole.pushMsg("first log record");
      String lLogContent = lConsole.getLogContent();

      Assert.IsFalse(String.IsNullOrEmpty(lLogContent));
      Assert.IsTrue(lLogContent.Length > 0);
      Assert.IsTrue(lLogContent.Contains("first log record"));      
    }

    #endregion


    #region PRIVATE
    #endregion

  }
}
