using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{
    internal class DataConvertProcess
    {
        //1byte -> 8bit
        public static BitArray GetBitArrayFromByte(byte value) => new BitArray((byte[])[value]);

        //8bit -> 1byte
        public static byte GetByteFromBitArray(BitArray bitArray)
        {
            if (bitArray.Count != sizeof(byte) * 8)
            {
                throw new Exception("bit array size error");
            }

            byte[] bytes = new byte[1];
            bitArray.CopyTo(bytes, 0);

            return bytes[0];
        }

        //1ushort -> 16bit
        public static BitArray GetBitArrayFromUshort(ushort value) => new(BitConverter.GetBytes(value).ToArray());

        //16bit -> 1ushort
        public static ushort GetushortFromBitArray(BitArray bitArray)
        {
            if (bitArray.Count != sizeof(ushort) * 8)
            {
                throw new Exception("bit array size error");
            }

            byte[] bytes = new byte[2];
            bitArray.CopyTo(bytes, 0);

            return BitConverter.ToUInt16(bytes);
        }

        //2ushort -> 1float
        public static float ConvertFloatFromUshorts(ushort[] values)
        {
            if (values.Length != 2)
                throw new Exception("values count need two");

            var final = Array.ConvertAll([values[0], values[1]], BitConverter.GetBytes);
            return BitConverter.ToSingle([final[0][0], final[0][1], final[1][0], final[1][1]], 0);
        }

        //1float -> 2ushort
        public static ushort[] ConvertUshortsFromFloat(float value)
        {
            var bytes = BitConverter.GetBytes(value);
            return [BitConverter.ToUInt16(bytes, 0), BitConverter.ToUInt16(bytes, 2)];
        }
    }
}
