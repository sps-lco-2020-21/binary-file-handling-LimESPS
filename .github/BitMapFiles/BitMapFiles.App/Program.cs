using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BitMapFiles.App
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] logo = File.ReadAllBytes("sps.bmp");
            Console.WriteLine($"Is file a BM file?: {BM(logo)}");
            Console.ReadKey();
        }
        static bool BM(byte[] logo)
        {
            byte first = logo[0];
            byte second = logo[1];
            char b = 'B';
            char m = 'M';
            int res1 = first ^ Convert.ToByte(b);
            int res2 = second ^ (byte)m;
            if (res1 == 0 && res2 == 0)
                return true;
            else
                return false;
        }
    }
}
