using System;
using System.Collections.Generic;
using System.Text;

namespace Hosting.FileLog
{
    internal class ScopeDisposable : IDisposable
    {
        private readonly Stack<object> _stack;
        public ScopeDisposable(Stack<object> stack) => _stack = stack;
        public void Dispose()
        {
            if (_stack.Count > 0) _stack.Pop();
        }
    }
}
