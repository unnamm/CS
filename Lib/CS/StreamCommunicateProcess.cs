using System.Text;

namespace Lib.CS
{
    internal class StreamCommunicateProcess
    {
        private const int TIMEOUT = 5000;

        private Stream _stream;

        public StreamCommunicateProcess(Stream s)
        {
            _stream = s;
        }

        public async void F() //async func
        {
            await WriteAsync(new byte[] { 1, 2 }); //write byte[]
            byte[] data = await ReadAsync<byte[]>(); //read byte[]

            Console.WriteLine(data[0]);

            await WriteAsync("123"); //write string
            string data2 = await ReadAsync<string>(); //read string

            Console.WriteLine(data2);
        }

        public static ushort CRC16(byte[] data) //make crc16
        {
            var size = data.Length;

            int cr = 0xFFFF;
            for (int i = 0; i < size; i++)
            {
                cr = cr ^ data[i];
                for (int j = 0; j < 8; j++)
                {
                    if ((cr & 0x0001) == 0x0001)
                    {
                        cr >>= 1;
                        cr ^= 0xA001;
                    }
                    else
                    {
                        cr >>= 1;
                    }
                }
            }
            return (ushort)cr;
        }

        public Task WriteAsync<T>(T data)
        {
            object write = null;

            if (typeof(T) == typeof(byte[]))
            {
                write = data;
            }
            if (typeof(T) == typeof(string))
            {
                write = Encoding.UTF8.GetBytes(data.ToString());
            }
            if (write == null)
            {
                throw new Exception();
            }

            return writeAsync((byte[])write);
        }

        private async Task writeAsync(byte[] data)
        {
            try
            {
                //await _stream.WriteAsync(data); // timeout no work //infinity wait
                await timeout(_stream.WriteAsync(data).AsTask(), TIMEOUT);
            }
            catch (Exception ex)
            {
                throw new Exception("write error: ", ex);
            }
        }

        //auto byte[] string
        public async Task<T> ReadAsync<T>()
        {
            var data = await readAsync();
            object o = null;

            if (typeof(T) == typeof(byte[]))
            {
                o = data;
            }
            if (typeof(T) == typeof(string))
            {
                o = Encoding.UTF8.GetString(data);
            }
            if (o is null)
            {
                throw new Exception("receive fail");
            }

            return (T)o;
        }

        private async Task<byte[]> readAsync()
        {
            const int BUF_SIZE = 512;
            var buffer = new byte[BUF_SIZE];
            int len = 0;
            try
            {
                len = await timeout(_stream.ReadAsync(buffer, 0, BUF_SIZE), TIMEOUT);
            }
            catch (Exception e)
            {
                throw new Exception("read error: ", e);
            }

            var data = new byte[len];
            Array.Copy(buffer, data, len);
            return data;
        }

        private async Task timeout(Task t, int millisecs) //return void
        {
            var v = await Task.WhenAny(t, Task.Delay(millisecs));

            if (v != t)
            {
                throw new TimeoutException("timeout");
            }
        }

        private async Task<T> timeout<T>(Task<T> t, int millisecs) //return <T>
        {
            var v = await Task.WhenAny(t, Task.Delay(millisecs));

            if (v != t)
            {
                throw new TimeoutException("timeout");
            }

            return t.Result;
        }
    }
}
