using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simsang.Contribute.Infrastructure
{
  interface IContribute
  {
    bool sendContribution(String pData);
    String getMethod();
  }
}
