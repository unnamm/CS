using System;
using System.Collections.Generic;
using System.Text;

namespace Communicate.Abstract
{
    public interface ICom : IDisposable
    {
        bool IsConnected { get; }
    }
}
