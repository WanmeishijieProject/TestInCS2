using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRC12
{
    class Program
    {
        const int CHAR_SIZE = 8;
        const int CRC_SIZE = 12;  
        static UInt32 PATTERN = 0x180F; //X12+x11 + x3 + x2 + x + 1  // 180F
         
        static void Main(string[] args)
        {
            var res= crc12(new byte[] { 0x32});
            
            Console.WriteLine(Add(1,2,3,4));
            Console.WriteLine(Add(1, 2, 3, 4,4,6,7,8,8,9));
            Console.WriteLine(Test(56789,(x,y,z)=> { return (x*y*z).ToString();}));
            Console.WriteLine(res);
            Console.ReadKey();
        }
        static UInt32 crc12(byte[] data)
            {
                int len = data.Length;
                UInt32 remainder = 0;
                UInt32 nremainder = 0;
                for (int i = 0; i < len; i++)
                {
                    for (int j = CHAR_SIZE - 1; j >= 0; j--)
                    {
                        bool bit = (data[i] & (1 << j)) > 0;
                        bool lb = (remainder & (1 << (CRC_SIZE - 1))) > 0;
                        int fb = (lb ^ bit)? 1:0;
                        nremainder = (uint)fb;
                        for (int k = 1; k < CRC_SIZE; k++)
                        {
                            int cb = (int)((remainder & (1 << (k - 1))) >> k - 1);
                            if ((PATTERN & (1 << k))!=0)
                            {
                                nremainder |= (uint)((cb ^ fb) << k);
                            }
                            else
                            {
                                nremainder |= (uint)(cb << k);
                            }
                        }
                        remainder = nremainder;
                    }
                }
                return remainder;
            }
        private static int Add(params int[] Args)
        {
            int sum = 0;
            foreach (var it in Args)
                sum += it;
            return sum;
        }
        private static string Test(int x, Func<int,double,double,string> func)
        {
            return x.ToString() + "    " + func(1, 2, 4);
        }
    }
}
