using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Communicate.Tcp
{
    public class TcpQuery : TcpBase
    {
        public TcpQuery(string ip, int port) : base(ip, port) { }

        public async Task<byte[]> QueryAsync(byte[] sendData, CancellationToken token = default)
        {
            var stream = base.GetStream();
            await stream.WriteAsync(sendData, token);

            var buffer = new byte[byte.MaxValue];
            var readLength = await stream.ReadAsync(buffer, token);

            byte[] readBuffer = new byte[readLength];
            Array.Copy(buffer, readBuffer, readLength);

            return readBuffer;
        }
        public async Task<byte[]> QueryExactlyAsync(byte[] sendData, uint exactlyLength, CancellationToken token = default)
        {
            var stream = base.GetStream();
            await stream.WriteAsync(sendData, token);

            var buffer = new byte[exactlyLength];
            await stream.ReadExactlyAsync(buffer, token);
            return buffer;
        }
    }
}
