using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Communicate
{
    public class TcpCommunicate : IDisposable
    {
        private readonly TcpClient _client = new();

        private int _timeoutMilli;

        public ValueTask ConnectAsync(string ip, int port, int timeoutMilli = 1000)
        {
            _timeoutMilli = timeoutMilli;

            using CancellationTokenSource cts = new(_timeoutMilli);

            return _client.ConnectAsync(ip, port, cts.Token);
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
