using Communicate.Abstract;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communicate.Serial
{
    public class SerialCallback : SerialBase, ICallback
    {
        public event Action<byte[]>? DataReceived;
        public event Action<Exception>? ErrorReceived;

        private readonly char? _end;
        private readonly int? _readLength;

        public SerialCallback(char end, string comport, int baudRate = 9600) : base(comport, baudRate)
        {
            _client.DataReceived += Client_DataReceived;
            _end = end;
        }
        public SerialCallback(int readLength, string comport, int baudRate = 9600) : base(comport, baudRate)
        {
            _client.DataReceived += Client_DataReceived;
            _readLength = readLength;
        }

        public override void Dispose()
        {
            _client.DataReceived -= Client_DataReceived;
            base.Dispose();
        }

        private void Client_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                var serial = (SerialPort)sender;
                byte[] receiveBuffer;
                if (_end != null)
                {
                    receiveBuffer = ReadPacket(serial, _end.Value);
                }
                else if (_readLength != null)
                {
                    receiveBuffer = ReadPacket(serial, _readLength.Value);
                }
                else
                {
                    throw new NotImplementedException();
                }

                DataReceived?.Invoke(receiveBuffer);
            }
            catch (Exception ex)
            {
                ErrorReceived?.Invoke(ex);
            }
        }
    }
}
