using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using ACW_08346_464886_ServiceLibrary;
using System.Security.Cryptography;

namespace ACW_08346_464886_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            RSAParameters publicKey;
            RSAParameters privateKey;

            // start the server using the service library
            Service1 serviceInstance = new Service1();
            ServiceHost myHost = new ServiceHost(serviceInstance);
            myHost.Open();
            Console.WriteLine("Server running...");

            // read the public and private keys parameters from the 8 lines input
            string[] inputLines = new string[8];
            for (int i = 0; i < 8; i++)
            {
                inputLines[i] = Console.ReadLine();
            }
            
            // assign public and private keys
            publicKey = new RSAParameters()
            {
                Exponent = StringToByteArray(inputLines[3]),
                Modulus = StringToByteArray(inputLines[5])
            };
            serviceInstance.PubKey = publicKey;
            privateKey = new RSAParameters()
            {
                D = StringToByteArray(inputLines[0]),
                DP = StringToByteArray(inputLines[1]),
                DQ = StringToByteArray(inputLines[2]),
                Exponent = StringToByteArray(inputLines[3]),
                InverseQ = StringToByteArray(inputLines[4]),
                Modulus = StringToByteArray(inputLines[5]),
                P = StringToByteArray(inputLines[6]),
                Q = StringToByteArray(inputLines[7])
            };
            serviceInstance.PrivKey = privateKey;

            Console.ReadLine(); // should close in 10 seconds instead of waiting for input
            myHost.Close();
        }

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
            .Where(x => x % 2 == 0)
            .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
            .ToArray();
        }
    }
}
