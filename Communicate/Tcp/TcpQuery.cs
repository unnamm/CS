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
        private readonly SemaphoreSlim _lock = new SemaphoreSlim(0, 1);

        public TcpQuery(string ip, int port) : base(ip, port) { }

        public async Task<byte[]> QueryAsync(byte[] sendData, CancellationToken token = default)
        {
            await _lock.WaitAsync(token);

            try
            {
                var readBuffer = await ReadAsync(token);
                return readBuffer;
            }
            catch
            {
                throw;
            }
            finally
            {
                _lock.Release();
            }
        }
        public async Task<byte[]> QueryExactlyAsync(byte[] sendData, uint exactlyLength, CancellationToken token = default)
        {
            var stream = base.GetStream();

            await _lock.WaitAsync(token);
            try
            {
                await stream.WriteAsync(sendData, token);

                var buffer = new byte[exactlyLength];
                await stream.ReadExactlyAsync(buffer, token);
                return buffer;
            }
            catch
            {
                throw;
            }
            finally
            {
                _lock.Release();
            }
        }
    }
}
