using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communicate.Serial
{
    public enum SerialType
    {
        /// <summary>
        /// write and read
        /// </summary>
        Query,

        /// <summary>
        /// wait read event
        /// </summary>
        Event,
    }
}
