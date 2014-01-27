using System;

namespace Plugin.Main.Systems.Config
{

  public class RecordException : System.Exception
  {
    public RecordException() : base() { }
    public RecordException(String message) : base(message) { }
  }

  public class RecordExistsException : System.Exception
  {
    public RecordExistsException() : base() { }
    public RecordExistsException(String message) : base(message) { }
  }

}