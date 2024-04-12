using Lib.CS;
using System.Net.Sockets;

namespace Lib
{
    //main run call public
    //lib test run
    public class LibMain
    {
        public void Run()
        {
            TcpClient tc = new TcpClient("127.0.0.1", 12345);
            StreamCommunicateProcess pro = new(tc.GetStream());
            //pro.WriteAsync(new byte[] {55,56,57,58,59,60,61 });
            //pro.WriteAsync("345hgy54");

            pro.F();
        }
    }
}
