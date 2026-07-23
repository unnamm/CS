using Communicate.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Communicate.Tcp
{
    public abstract class TcpBase : IComAsync
    {
        protected readonly TcpClient _client = new();

        private readonly string _ip;
        private readonly int _port;

        public TcpBase(string ip, int port)
        {
            _ip = ip;
            _port = port;
        }

        public bool IsConnected => _client.Connected;
        public virtual void Close() => _client.Close();
        public virtual void Dispose()
        {
            _client.Dispose();
            GC.SuppressFinalize(this);
        }

        public virtual Task ConnectAsync(CancellationToken token) => _client.ConnectAsync(_ip, _port, token).AsTask();
    }
}
