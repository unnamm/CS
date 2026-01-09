using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communicate.Serial
{
    public class SerialCommunicate : IDisposable
    {
        private readonly SerialPort _serialPort = new();

        private SerialType _serialType;

        public Action<string>? DataReceived;

        public void Connect(string portName, SerialType type, int baudRate = 9600, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One, int timeoutMilli = 1000)
        {
            DataReceived = null;

            _serialType = type;

            _serialPort.PortName = portName;
            _serialPort.BaudRate = baudRate;
            _serialPort.Parity = parity;
            _serialPort.DataBits = dataBits;
            _serialPort.StopBits = stopBits;
            _serialPort.Open();

            _serialPort.WriteTimeout = timeoutMilli;
            _serialPort.ReadTimeout = timeoutMilli;

            if (type == SerialType.Event)
            {
                _serialPort.DataReceived += _serialPort_DataReceived;
            }
        }

        private void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (DataReceived == null)
                return;

            var serialPort = (SerialPort)sender;
            var data = serialPort.ReadExisting();
            DataReceived(data);
        }

        public void Write(string text)
        {
            _serialPort.Write(text);
        }

        public void Write(byte[] bytes)
        {
            _serialPort.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// When the number of bytes read is equal to readLength
        /// </summary>
        /// <param name="readLength"></param>
        /// <returns></returns>
        public async Task<byte[]> ReadAsync(int readLength)
        {
            if (_serialType == SerialType.Event)
            {
                throw new Exception("event is only DataReceived");
            }

            byte[] buffer = new byte[readLength];

            await Task.Run(() =>
            {
                int currentLength = 0;
                while (currentLength < readLength)
                {
                    currentLength += _serialPort.Read(buffer, currentLength, readLength - currentLength);
                }
            });

            return buffer;
        }

        /// <summary>
        /// When the last string read is equal to last
        /// </summary>
        /// <param name="last"></param>
        /// <param name="packetBufferSize"></param>
        /// <returns></returns>
        public async Task<string> ReadAsync(string last, int packetBufferSize = 256)
        {
            if (_serialType == SerialType.Event)
            {
                throw new Exception("event is only DataReceived");
            }

            string content = string.Empty;

            await Task.Run(() =>
            {
                while (true)
                {
                    byte[] buffer = new byte[packetBufferSize];
                    var len = _serialPort.Read(buffer, 0, buffer.Length);
                    byte[] data = new byte[len];
                    Array.Copy(buffer, data, len);

                    var temp = Encoding.UTF8.GetString(data);
                    content += temp;

                    if (temp.EndsWith(last, StringComparison.Ordinal))
                    {
                        break;
                    }
                }
            });

            return content;
        }

        public void Dispose()
        {
            _serialPort.Dispose();
            DataReceived = null;
        }
    }
}
