using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communicate.Serial
{
    public class SerialCommunicateExample
    {
        public static async Task Run()
        {
            try
            {
                SerialCommunicate sc = new();
                sc.Connect("COM4", timeoutMilli: 1000);
                //var v = await sc.ReadAsync("\r\n");
                var v = await sc.ReadAsync(8);
            }
            catch (TimeoutException ex)
            {
                //Read Fail
            }
            catch (Exception ex)
            {
                //other
            }
        }
    }
}
