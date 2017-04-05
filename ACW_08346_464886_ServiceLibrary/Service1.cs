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
            for (int i = 0; i < sortedArray.Length; i++)
            {
                Console.Write("{0}", sortedArray[i]);
                if ((i + 1) % 2 == 0 && (i + 1) < sortedArray.Length)
                    Console.Write(" "); // separate each pair of values
            }
            //foreach (string s in sortedArray)
            //{
            //    Console.WriteLine("{0} ", s);
            //}
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

        public void Decrypt(byte[] encryptedByteMessage)
        {
            byte[] decryptedByteMessage;
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                decryptedByteMessage = RSADecrypt(encryptedByteMessage, privKey);
                Console.Write("Decrypted message is: {0}.\r\n",
                    System.Text.Encoding.ASCII.GetString(decryptedByteMessage));
            }
            return;
        }

        public string ComputeSHA1(string message)
        {
            string hexSHA1 = "";
            byte[] asciiByteMessage = System.Text.Encoding.ASCII.GetBytes(message);
            byte[] sha1ByteMessage;
            SHA1 sha1Provider = new SHA1CryptoServiceProvider();
            sha1ByteMessage = sha1Provider.ComputeHash(asciiByteMessage);
            hexSHA1 = ByteArrayToHexString(sha1ByteMessage);
            Console.Write("SHA-1 hash of {0} is {1}.\r\n", message, hexSHA1);
            return hexSHA1;
        }

        public string ComputeSHA256(string message)
        {
            string hexSHA256 = "";
            byte[] asciiByteMessage = System.Text.Encoding.ASCII.GetBytes(message);
            byte[] sha1ByteMessage;
            SHA256 sha256Provider = new SHA256CryptoServiceProvider();
            sha1ByteMessage = sha256Provider.ComputeHash(asciiByteMessage);
            hexSHA256 = ByteArrayToHexString(sha1ByteMessage);
            Console.Write("SHA-1 hash of {0} is {1}.\r\n", message, hexSHA256);
            return hexSHA256;
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

        static public byte[] RSADecrypt(byte[] DataToDecrypt, RSAParameters RSAKeyInfo)
        {
            try
            {
                byte[] decryptedData;
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.ImportParameters(RSAKeyInfo);
                    decryptedData = RSA.Decrypt(DataToDecrypt, false);
                }
                return decryptedData;
            }
            catch (CryptographicException e)
            {
                Console.Write(e.Message);
                return null;
            }
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
