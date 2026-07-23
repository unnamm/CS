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
        private readonly TcpClient _client = new();
        private readonly string _ip;
        private readonly int _port;

        public TcpBase(string ip, int port)
        {
            _ip = ip;
            _port = port;
        }

        public bool IsConnected => _client.Connected;
        protected Stream GetStream() => _client.GetStream();
        public virtual void Close() => _client.Close();
        public virtual void Dispose() => _client.Dispose();

        public virtual Task ConnectAsync(CancellationToken token) => _client.ConnectAsync(_ip, _port, token).AsTask();

        protected ValueTask WriteAsync(byte[] data, CancellationToken token)
        {
            var stream = _client.GetStream();
            return stream.WriteAsync(data, token);
        }

        protected async Task<byte[]> ReadAsync(CancellationToken token)
        {
            var stream = _client.GetStream();

            var buffer = new byte[byte.MaxValue];
            var readLength = await stream.ReadAsync(buffer, token);
            var readBuffer = new byte[readLength];
            Array.Copy(buffer, readBuffer, readLength);

            return readBuffer;
        }
    }
}
