using System.Net.Sockets;
using System.Net;

namespace Lib.CS
{
    /// <summary>
    /// Use StreamCommunicateProcess
    /// </summary>
    internal class TcpProcess
    {
        const int PORT = 2222;

        public void F()
        {
            Task.Run(f1);
            Task.Run(f2);
        }
        private async void f1()
        {
            Server server = new Server();

            await server.Listen();
            await server.Listen();

            await Task.Delay(500); // 2 client all connect wait

            server.SendAll();
        }
        private async void f2()
        {
            Client client = new Client();
            await client.Connect();
            Client client2 = new Client();
            await client2.Connect();

            var data1 = await client.Read(); //data1, data2 same message
            var data2 = await client2.Read();
        }

        public struct ClientInfo
        {
            public TcpClient Client;
            public Stream Stream;
            public StreamCommunicateProcess Communicate; //my code in same folder
        }

        class Server
        {
            private TcpListener _listener;
            private List<ClientInfo> _clients;

            public Server()
            {
                _clients = new List<ClientInfo>();
                _listener = new TcpListener(new IPEndPoint(IPAddress.Any, PORT));
            }

            public async Task Listen() //use other thread while
            {
                await Task.Run(() => _listener.Start()); //wait until connect client

                TcpClient client = await _listener.AcceptTcpClientAsync();

                ClientInfo info = new ClientInfo();
                info.Client = client;
                info.Stream = client.GetStream();
                info.Communicate = new StreamCommunicateProcess(info.Stream);
                _clients.Add(info);
            }

            public async void SendAll()
            {
                foreach (var v in _clients)
                {
                    await v.Communicate.WriteAsync("send all client");
                }
            }

            public async Task<string> Receive(int index)
            {
                var data = await _clients[index].Communicate.ReadAsync<string>();
                return data;
            }
        }

        class Client
        {
            private TcpClient _client;
            private Stream _stream;
            private StreamCommunicateProcess _communicate;

            public Client()
            {
                _client = new TcpClient();
            }

            public async Task Connect()
            {
                const string IP = "127.0.0.1";
                await _client.ConnectAsync(IP, PORT);
                _stream = _client.GetStream();
                _communicate = new StreamCommunicateProcess(_stream);
            }

            public async void Write()
            {
                await _communicate.WriteAsync("data");
            }

            public async Task<string> Read()
            {
                var data = await _communicate.ReadAsync<string>();
                return data;
            }
        }
    }
}
