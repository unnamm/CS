using Hosting.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hosting.Interface
{
    internal interface IEntrySink
    {
        void Add(LogEntry entry);
    }
}
