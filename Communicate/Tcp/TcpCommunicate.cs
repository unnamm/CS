using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Communicate.Tcp
{
    public class TcpCommunicate : IDisposable
    {
        private readonly TcpClient _client = new();

        private int _timeoutMilli;
        private NetworkStream _stream;

        public async Task ConnectAsync(string ip, int port, int timeoutMilli = 1000)
        {
            _timeoutMilli = timeoutMilli;
            using CancellationTokenSource cts = new(_timeoutMilli);
            await _client.ConnectAsync(ip, port, cts.Token);
            _stream = _client.GetStream();
        }

        public ValueTask WriteAsync(byte[] buffer)
        {
            CancellationTokenSource cts = new(_timeoutMilli);
            return _stream.WriteAsync(buffer, cts.Token);
        }

        public async Task<byte[]> ReadAsync(int packetbufferSize = 256)
        {
            byte[] buffer = new byte[packetbufferSize];
            CancellationTokenSource cts = new(_timeoutMilli);
            var len = await _stream.ReadAsync(buffer, cts.Token);
            byte[] data = new byte[len];
            Array.Copy(buffer, data, len);
            return data;
        }

        public void Dispose()
        {
            _stream.Dispose();
            _client.Dispose();
        }
    }
}
