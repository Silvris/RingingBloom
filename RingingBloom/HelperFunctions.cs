using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
            byte newByte2 = br.ReadByte();
            while (newByte != 0 || newByte2 != 0)
            {
                stringC.Add(newByte);
                stringC.Add(newByte2);
                newByte = br.ReadByte();
                newByte2 = br.ReadByte();
            }
            return Encoding.Unicode.GetString(stringC.ToArray());
        }

        public static void WriteUniNullTerminatedString(BinaryWriter bw, string str)
        {
            bw.Write(Encoding.Unicode.GetBytes(str.ToCharArray()));
            bw.Write((Int16)0);
        }

        public static BinaryReader OpenFile(string filename, Encoding encoding = null)//surprised it doesn't get too mad at me here
        {
            if (encoding is null)
            {
                encoding = Encoding.ASCII;
            }

            try
            {
                BinaryReader br = new BinaryReader(new FileStream(filename, FileMode.OpenOrCreate),encoding);
                return br;
            }
            catch(IOException)
            {
                MessageBox.Show("File could not be opened, likely because it is being used by another process.");
                return null;
            }

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
