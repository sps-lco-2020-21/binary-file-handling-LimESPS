﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Net.Http.Headers;

namespace BitMapFiles.App
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] logo = File.ReadAllBytes("sps.bmp");
            Console.WriteLine($"Is file a BM file?: {BM(logo)}");
            Console.WriteLine("Size in bytes is {0}",sizeinbytes(logo));
            Console.WriteLine($"Offset to start of image data is: {size_calc(list_create("13,12,11,10"))} bytes");
            Console.WriteLine($"Size of bitmapinfoheader is: {size_calc(list_create("17,16,15,14"))}");
            Console.WriteLine($"Image width in pixels is: {size_calc(list_create("21,20,19,18"))}");
            Console.WriteLine($"Image height in pixels is: {size_calc(list_create("25,24,23,22"))}");
            Console.WriteLine($"Number of bits per pixel is: {size_calc(list_create("29,28"))}");
            Console.WriteLine($"Compression type is: {compression_type_check(size_calc(list_create("33,32,31,30")))}");
            stenography();
            Console.ReadKey();
        }
        static bool BM(byte[] logo)
        {
            byte first = logo[0];
            byte second = logo[1];
            char b = 'B';
            char m = 'M';
            int res1 = first ^ (byte)b;
            int res2 = second ^ (byte)m;
            if (res1 == 0 && res2 == 0)
                return true;
            else
                return false;
        }

        static int sizeinbytes(byte[] logo)
        {
            List<byte> logovalr = logo.Where(x => Array.IndexOf(logo, x) >= 2 && Array.IndexOf(logo, x) <= 5).ToList(); //contains 57 items, does not work
            IEnumerable<byte> logovals = from thing in logo where Array.IndexOf(logo, thing) == 1 select thing; //contains 57, also does not work

            List<byte> logoval = new List<byte> { logo[5], logo[4], logo[3], logo[2] };
            Console.WriteLine("logo3 is {0}", logo[3].ToString());
            IEnumerable<string> bits = from byte item in logoval select Convert.ToString(item, 2);
            //Console.WriteLine(string.Join("", bits));
            return Convert.ToInt32(string.Join("", bits), 2);
        }
        static List<byte> list_create(string a)
        {
            byte[] logo = File.ReadAllBytes("sps.bmp");
            IEnumerable<byte> result_enum = from item in a.Split(',') select logo[int.Parse(item)];
            List<byte> result_list = result_enum.ToList();
            return result_list;
        }
        
        static int size_calc(List<byte> logoval) //very similar to size in bytes
        {

            IEnumerable<string> bits = from byte item in logoval select Convert.ToString(item, 2);
            //Console.WriteLine(string.Join("", bits));
            return Convert.ToInt32(string.Join("", bits), 2);
        }
        static string compression_type_check(int a)
        {
            if (a == 0)
                return "none";
            else if (a == 1)
                return "RLE-8";
            else if (a == 2)
                return "RLE-4";
            else
                return "error";
        }
        static void stenography()
        {
            Console.WriteLine("Hide word or find word (h/f)?\n\n");
            string entry = Console.ReadLine();
            if (entry == "h")
                hide_word();
            else if (entry == "f")
                find_word();
           
        }
        static void hide_word()
        {
            List<string> input = entries();
            byte[] logo = File.ReadAllBytes(input[0]);
            int x = 54;
            char[] letters_array = input[1].ToCharArray();
            List<char> letters = letters_array.ToList();
            foreach(char item in letters)
            {
                byte val = Convert.ToByte(item);
                logo[x] = val;
                ++x;
                Console.WriteLine("x is {0}", x);
            }
            string path = Environment.CurrentDirectory;
            string document = Path.Combine(path, "outputfile.bmp");
            StreamWriter outputfile = new StreamWriter(document, true);
            outputfile.Write(logo);
            outputfile.Close();
        }
        static void find_word()
        {
            List<string> input = entries();
            byte[] logo = File.ReadAllBytes(input[0]);
            int x = 54;
            List<char> letters_found = new List<char>();
            IEnumerable<char> letters = from item in logo where char.IsLetter(Convert.ToChar(item)) && Array.IndexOf(logo,item) > 2 select Convert.ToChar(item);
            
            string hidden_word2 = string.Join("", letters);
            Console.WriteLine($"Hidden word is {hidden_word2}"); //hidden word of outputfile.bmp is always tmbte * number of times I have added a hidden word, 
        }
        static List<string> entries()
        {
            Console.WriteLine("Enter file: ");
            string file = Console.ReadLine();
            Console.WriteLine("Enter hidden word (not needed when finding word)");
            string word = Console.ReadLine();
            return new List<string> { file, word };
        }
    }
}
