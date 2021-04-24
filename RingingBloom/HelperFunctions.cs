using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using RingingBloom.Common;

namespace RingingBloom
{
    class HelperFunctions
    {
        public static string ReadNullTerminatedString(BinaryReader br)
        {
            List<byte> stringC = new List<byte>();
            byte newByte = br.ReadByte();
            while(newByte != 0)
            {
                stringC.Add(newByte);
                newByte = br.ReadByte();
            }
            return Encoding.ASCII.GetString(stringC.ToArray());
        }

        public static void WriteNullTerminatedString(BinaryWriter bw, string str)
        {
            bw.Write(str.ToCharArray());
            bw.Write((byte)0);
        }

        public static string ReadUniNullTerminatedString(BinaryReader br)
        {
            List<byte> stringC = new List<byte>();
            byte newByte = br.ReadByte();
            byte newByteNull = br.ReadByte();
            while (newByte != 0)
            {
                stringC.Add(newByte);
                newByte = br.ReadByte();
                newByteNull = br.ReadByte();
            }
            return Encoding.ASCII.GetString(stringC.ToArray());
        }

        public static void WriteUniNullTerminatedString(BinaryWriter bw, string str)
        {
            bw.Write(Encoding.Unicode.GetBytes(str.ToCharArray()));
            bw.Write((Int16)0);
        }

        public static Wem MakeWems(string name, BinaryReader fs)
        {
            Wem newWem = new Wem(name, "0", fs);
            fs.Close();
            return newWem;
        }

        public static Brush GetBrushFromHex(string hexColor)
        {
            BrushConverter bc = new BrushConverter();
            Brush newBrush = (Brush)bc.ConvertFrom(hexColor);
            return newBrush;
        }

        public static byte[] Combine(byte[] first, byte[] second)
        {
            byte[] bytes = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, bytes, 0, first.Length);
            Buffer.BlockCopy(second, 0, bytes, first.Length, second.Length);
            return bytes;
        }
    }
}
