using System;
using System.Collections.Generic;
using System.Text;

namespace Communicate.Abstract
{
    internal interface ICom : IDisposable
    {
        bool IsConnected { get; }
    }
}
