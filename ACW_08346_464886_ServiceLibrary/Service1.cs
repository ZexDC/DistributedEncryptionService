using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;

namespace ACW_08346_464886_ServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Service1 : IService1
    {
        private RSAParameters pubKey;
        public RSAParameters PubKey
        {
            set { pubKey = value; }
        }

        private RSAParameters privKey;
        public RSAParameters PrivKey
        {
            set { privKey = value; }
        }

        public string HelloEcho(int id)
        {
            Console.Write("Client No. {0} has contacted the server.\r\n", id);
            return "Hello";
        }

        public string[] Sort(string[] array)
        {
            string[] sortedArray = array;
            Array.Sort(sortedArray);
            Console.Write("Sorted values:\r\n");
            foreach(string s in sortedArray)
            {
                Console.WriteLine("{0} ", s);
            }
            return sortedArray;
        }

        public string[] PublicKey()
        {
            string[] hexPublicKey = new string[2];
            Console.Write("Sending the public key to the client.\r\n");
            hexPublicKey[0] = ByteArrayToHexString(pubKey.Exponent);
            hexPublicKey[1] = ByteArrayToHexString(pubKey.Modulus);
            return hexPublicKey;
        }

        static string ByteArrayToHexString(byte[] byteArray)
        {
            string hexString = "";
            if (byteArray != null)
            {
                foreach (byte b in byteArray)
                {
                    hexString += b.ToString("x2");
                }
            }
            return hexString;
        }

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
