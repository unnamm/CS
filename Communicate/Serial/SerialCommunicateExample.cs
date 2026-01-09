using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communicate.Serial
{
    public class SerialCommunicateExample
    {
        public static async Task RunQuery()
        {
            try
            {
                SerialCommunicate sc = new();
                sc.Connect("COM4", SerialType.Query, timeoutMilli: 1000);
                var v = await sc.ReadAsync(8);
                //var v = await sc.ReadAsync("\r\n");
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

        public static void RunEvent()
        {
            try
            {
                SerialCommunicate sc = new();
                sc.Connect("COM4", SerialType.Event);
                sc.DataReceived += Receive;
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

        public static void Receive(string data)
        {

        }
    }
}
