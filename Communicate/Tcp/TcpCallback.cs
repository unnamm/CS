using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communicate.Tcp
{
    public class TcpCallback : TcpBase, ICallback
    {
        public event Action<byte[]>? DataReceived;
        public event Action<Exception>? ErrorReceived;

        private readonly uint? _readLength;

        private CancellationTokenSource _cancelSource = new();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="readLength">fix read length</param>
        public TcpCallback(string ip, int port, uint? readLength = null) : base(ip, port)
        {
            _readLength = readLength;
        }

        public override void Close()
        {
            _cancelSource.Cancel();
            base.Close();
        }

        public override void Dispose()
        {
            Close();
            _cancelSource.Dispose();
            base.Dispose();
        }

        public override async Task ConnectAsync(CancellationToken token)
        {
            await base.ConnectAsync(token);

            _cancelSource.Cancel();
            _cancelSource = new();

            _ = RunAsync(_cancelSource.Token);
        }

        private async Task RunAsync(CancellationToken token)
        {
            try
            {
                while (true)
                {
                    byte[] buffer;
                    if (_readLength == null)
                    {
                        buffer = await base.ReadAsync(token);
                    }
                    else
                    {
                        buffer = await base.ReadExactlyAsync(_readLength.Value, token);
                    }
                    DataReceived?.Invoke(buffer);
                }
            }
            catch (Exception ex)
            {
                Close();
                ErrorReceived?.Invoke(ex);
            }
        }
    }
}
