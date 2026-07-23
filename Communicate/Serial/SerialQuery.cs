using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communicate.Serial
{
    public class SerialQuery : SerialBase
    {
        private readonly object _lock = new();

        public SerialQuery(string comport, int timeoutMilli = 1000, int baudRate = 9600) : base(comport, baudRate)
        {
            _client.ReadTimeout = timeoutMilli;
            _client.WriteTimeout = timeoutMilli;
        }

        public byte[] Query(string data, char end)
        {
            lock (_lock)
            {
                _client.Write(data);
                return base.ReadPacket(end);
            }
        }
        public byte[] Query(string data, int length)
        {
            lock (_lock)
            {
                _client.Write(data);
                return base.ReadPacket(length);
            }
        }
        public byte[] Query(byte[] data, char end)
        {
            lock (_lock)
            {
                _client.Write(data, 0, data.Length);
                return base.ReadPacket(end);
            }
        }
        public byte[] Query(byte[] data, int length)
        {
            lock (_lock)
            {
                _client.Write(data, 0, data.Length);
                return base.ReadPacket(length);
            }
        }
    }
}
