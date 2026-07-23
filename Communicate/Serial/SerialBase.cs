using Communicate.Abstract;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communicate.Serial
{
    public abstract class SerialBase : IComSync
    {
        protected readonly SerialPort _client;

        public SerialBase(string comport, int baudRate) => _client = new(comport, baudRate);

        public bool IsConnected => _client != null && _client.IsOpen;
        public void Connect() => _client.Open();
        public void Close() => _client.Close();
        public virtual void Dispose() => _client.Dispose();

        protected byte[] ReadPacket(char end) => ReadPacket((byte)end);

        protected byte[] ReadPacket(byte end)
        {
            List<byte> readList = [];

            while (true)
            {
                var buffer = new byte[byte.MaxValue];
                var readLength = _client.Read(buffer, 0, byte.MaxValue);

                var endIndex = Array.IndexOf(buffer, end, 0, readLength);
                if (endIndex != -1) //마지막 문자가 있으면
                {
                    var readBuffer = new byte[endIndex];
                    Array.Copy(buffer, readBuffer, endIndex);
                    readList.AddRange(readBuffer);
                    return readList.ToArray();
                }
                else //읽었는데 마지막 문자가 없으면 읽은만큼 더하고 다시 읽기
                {
                    var readBuffer = new byte[readLength];
                    Array.Copy(buffer, readBuffer, readLength);
                    readList.AddRange(readBuffer);
                }
            }
        }

        protected byte[] ReadPacket(int length)
        {
            byte[] buffer = new byte[length];
            var readIndex = 0;
            while (true)
            {
                readIndex += _client.Read(buffer, readIndex, length - readIndex);
                if (length == readIndex)
                {
                    return buffer;
                }
            }
        }
    }
}
