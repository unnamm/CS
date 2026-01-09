using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communicate.Serial
{
    public class SerialCommunicate
    {
        private readonly SerialPort _serialPort = new();

        public void Connect(string portName, int baudRate = 9600, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One, int timeoutMilli = 1000)
        {
            _serialPort.PortName = portName;
            _serialPort.BaudRate = baudRate;
            _serialPort.Parity = parity;
            _serialPort.DataBits = dataBits;
            _serialPort.StopBits = stopBits;
            _serialPort.Open();

            _serialPort.WriteTimeout = timeoutMilli;
            _serialPort.ReadTimeout = timeoutMilli;
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
        /// <returns></returns>
        public async Task<string> ReadAsync(string last)
        {
            string content = string.Empty;

            await Task.Run(() =>
            {
                while (true)
                {
                    byte[] buffer = new byte[256];
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
    }
}
