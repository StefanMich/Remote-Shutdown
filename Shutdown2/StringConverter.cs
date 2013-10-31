using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shutdown
{
    public static class StringConverter
    {
        public static byte[] GetBytes(this string str)
        {
            byte[] bytes = new byte[4 + Encoding.UTF8.GetByteCount(str)];
            BitConverter.GetBytes(str.Length).CopyTo(bytes, 0);
            Encoding.UTF8.GetBytes(str).CopyTo(bytes, 4);

            return bytes;
            /*
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;*/
        }

        public static string GetString(this byte[] bytes)
        {
            int size = BitConverter.ToInt32(bytes, 0);
            return Encoding.UTF8.GetString(bytes, 4, size);
        }
    }
}
