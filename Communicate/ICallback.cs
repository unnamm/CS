using System;
using System.Collections.Generic;
using System.Text;

namespace Communicate
{
    public interface ICallback
    {
        event Action<byte[]>? DataReceived;
        event Action<Exception>? ErrorReceived;
    }
}
