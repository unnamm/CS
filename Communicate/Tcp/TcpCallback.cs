using Communicate.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Communicate.Tcp
{
    public class TcpCallback : TcpBase, ICallback
    {
        public event Action<byte[]>? DataReceived;
        public event Action<Exception>? ErrorReceived;

        private readonly int? _readLength;

        private CancellationTokenSource _cancelSource = new();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="readLength">수신 데이터의 길이 고정, 이 데이터의 길이가 와야 패킷 완성</param>
        public TcpCallback(string ip, int port, int? readLength = null) : base(ip, port)
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
            GC.SuppressFinalize(this);
        }

        public override async Task ConnectAsync(CancellationToken token)
        {
            await base.ConnectAsync(token);

            _cancelSource.Cancel();
            _cancelSource = new();

            if (_readLength == null)
            {
                _ = RunAsync(_cancelSource.Token);
            }
            else
            {
                _ = ReadExactlyAsync(_cancelSource.Token);
            }
        }

        private async Task RunAsync(CancellationToken token)
        {
            try
            {
                var stream = base.GetStream();

                while (true)
                {
                    var readBuffer = await base.ReadAsync(token);
                    DataReceived?.Invoke(readBuffer);
                }
            }
            catch (Exception ex)
            {
                Close();
                ErrorReceived?.Invoke(ex);
            }
        }

        private async Task ReadExactlyAsync(CancellationToken token)
        {
            try
            {
                var stream = base.GetStream();

                while (true)
                {
                    var buffer = new byte[_readLength!.Value];
                    await stream.ReadExactlyAsync(buffer, token);
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
