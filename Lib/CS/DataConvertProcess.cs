using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.CS
{
    internal class DataConvertProcess
    {
        public static BitArray GetBitArrayFromUshort(ushort value) => //ushort -> 16bit
            new(BitConverter.GetBytes(value).ToArray());

        public static ushort GetushortFromBitArray(BitArray bitArray) //16bit -> ushort
        {
            if (bitArray.Count != sizeof(ushort) * 8)
            {
                throw new Exception("bit array size error");
            }

            byte[] bytes = new byte[2];
            bitArray.CopyTo(bytes, 0);

            return BitConverter.ToUInt16(bytes);
        }

        public static float ConvertFloatFromUshorts(ushort[] values) //two ushort -> float
        {
            if (values.Length != 2)
                throw new Exception("values count need two");

            var final = Array.ConvertAll([values[0], values[1]], BitConverter.GetBytes);
            return BitConverter.ToSingle([final[0][0], final[0][1], final[1][0], final[1][1]], 0);
        }

        public static ushort[] ConvertUshortsFromFloat(float value) //float -> two ushort
        {
            var bytes = BitConverter.GetBytes(value);
            return [BitConverter.ToUInt16(bytes, 0), BitConverter.ToUInt16(bytes, 2)];
        }
    }
}
