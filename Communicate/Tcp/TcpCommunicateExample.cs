using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communicate.Tcp
{
    public class TcpCommunicateExample
    {
        public static async Task Run()
        {
            try
            {
                TcpCommunicate tcp = new();
                await tcp.ConnectAsync("127.0.0.1", 1234);
                var data = await tcp.ReadAsync();
            }
            catch (OperationCanceledException ex)
            {
                //communication fail
            }
            catch (Exception ex)
            {
                //other fail
            }
        }
    }
}
