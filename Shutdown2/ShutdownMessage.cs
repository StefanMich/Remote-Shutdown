using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shutdown;

namespace Shutdown
{
    public class ShutdownMessage
    {
        private int interval;
        private ShutdownType type;
        
        public ShutdownType Type
        {
            get { return type; }
        }
        public int Interval
        {
            get { return interval; }
        }

        public ShutdownMessage(int interval, ShutdownType type)
        {
            this.interval = interval;
            this.type = type;
        }

        public byte[] GetMessage()
        {
            byte[] b = new byte[1 + sizeof(int)];
            b[0] = (byte)type;
            BitConverter.GetBytes(interval).CopyTo(b, 1);

            return b;
        }

        /// <summary>
        /// Reads an array of and creates a <see cref="ShutownMessage"/> of its content
        /// </summary>
        /// <param name="b">An array of bytes</param>
        /// <param name="k">The length of the array</param>
        /// <returns></returns>
        public static ShutdownMessage ReadMessage(byte[] b, int k)
        {
            ShutdownType t = (ShutdownType)b[0];
            int i = BitConverter.ToInt32(b, 1);

            return new ShutdownMessage(i, t);
        }
            
        public override string ToString()
        {
            return "Type: " + type.ToString() + " Interval: " + interval.ToString() + "\n";
        }
    }
}
