using System;
using System.Collections.Generic;
using System.Text;

namespace Communicate.Abstract
{
    internal interface IComAsync : ICom
    {
        Task ConnectAsync(CancellationToken token = default);
    }
}
