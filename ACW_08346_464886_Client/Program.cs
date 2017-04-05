using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ACW_08346_464886_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            // declare empty public key for later use in ENC
            string[] hexPublicKey = null;

            string[] inputLines = null;
            int lines = Convert.ToInt32(Console.ReadLine()); // read number of lines that will follow
            inputLines = new string[lines];
            for (int i = 0; i < lines; i++)
            {
                inputLines[i] = Console.ReadLine(); // put console input into string array
            }
            foreach (string s in inputLines)
            {
                // get the instruction of line s
                int firstSpaceIndex = s.IndexOf(" ");
                string instruction;
                if (firstSpaceIndex == -1) // no spaces found
                    instruction = s;
                else
                    instruction = s.Substring(0, firstSpaceIndex);
                // create reference to the service library
                ServiceReference1.Service1Client Service = new ServiceReference1.Service1Client();
                // execute action based on instruction
                switch (instruction.ToUpper())
                {
                    case "HELLO":
                        // get id from input line s
                        int id = Convert.ToInt32(s.Substring(firstSpaceIndex + 1));
                        // send message to server containing id
                        string response = Service.HelloEcho(id);
                        Console.Write(response + "\r\n");
                        break;
                    case "SORT":
                        List<string> splitList = s.Split(' ').ToList();
                        splitList.RemoveRange(0, 2); // remove the instruction and the first value (array length)
                        string[] arrayToSort = splitList.ToArray(); // array containing only values to sort
                        string[] sortedArray = Service.Sort(arrayToSort);
                        Console.Write("Sorted values:\r\n");
                        for (int i = 0; i < sortedArray.Length; i++)
                        {
                            Console.Write("{0}", sortedArray[i]);
                            if ((i + 1) % 2 == 0 && (i + 1) < sortedArray.Length)
                            {
                                Console.Write(" "); // separate each pair of values
                            }
                        }
                        break;
                    case "PUBKEY":
                        hexPublicKey = Service.PublicKey();
                        Console.WriteLine(hexPublicKey[0]);
                        Console.WriteLine(hexPublicKey[1]);
                        break;
                    case "ENC":
                        if (hexPublicKey == null)
                        {
                            Console.Write("No public key.\r\n");
                        }
                        else
                        {
                            // get message from input line s
                            string message = s.Substring(firstSpaceIndex + 1);
                            // convert msg to byte array
                            byte[] asciiByteMessage = System.Text.Encoding.ASCII.GetBytes(message);
                            byte[] encryptedByteMessage;
                            // recreate the public key as RSAParameters object
                            RSAParameters publicKey = new RSAParameters()
                            {
                                Exponent = StringToByteArray(hexPublicKey[0]),
                                Modulus = StringToByteArray(hexPublicKey[1])
                            };
                            // encrypt the message
                            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                            {
                                encryptedByteMessage = RSAEncrypt(asciiByteMessage, publicKey);
                            }
                            Service.Decrypt(encryptedByteMessage);
                            Console.Write("Encrypted message sent.\r\n");
                        }
                        break;
                    case "SHA1":
                        // get message from input line s
                        string messageSHA1 = s.Substring(firstSpaceIndex + 1);
                        // get the hexadecimal format of the computed SHA1
                        string hexSHA1 = Service.ComputeSHA1(messageSHA1);
                        Console.WriteLine(hexSHA1);
                        break;
                    case "SHA256":
                        // get message from input line s
                        string messageSHA256 = s.Substring(firstSpaceIndex + 1);
                        // get the hexadecimal format of the computed SHA256
                        string hexSHA256 = Service.ComputeSHA256(messageSHA256);
                        Console.WriteLine(hexSHA256);
                        break;
                    case "SIGN":
                        if (hexPublicKey == null)
                        {
                            Console.Write("No public key.\r\n");
                        }
                        else
                        {
                            // get message from input line s
                            string message = s.Substring(firstSpaceIndex + 1);
                            // convert msg to byte array
                            byte[] asciiByteMessage = System.Text.Encoding.ASCII.GetBytes(message);
                            // recreate the public key as RSAParameters object
                            RSAParameters publicKey = new RSAParameters()
                            {
                                Exponent = StringToByteArray(hexPublicKey[0]),
                                Modulus = StringToByteArray(hexPublicKey[1])
                            };
                            // get the signed hex from the server anc check if it is correct
                            byte[] signedAsciiByteMessage = Service.Sign(asciiByteMessage);
                            if (VerifySignedHash(asciiByteMessage, signedAsciiByteMessage, publicKey))
                            {
                                Console.Write("Signature successfully verified.\r\n");
                            }
                            else
                            {
                                Console.Write("Data not signed.\r\n");
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
#if DEBUG
            Console.ReadLine();
#endif
        }

        static public byte[] RSAEncrypt(byte[] DataToEncrypt, RSAParameters RSAKeyInfo)
        {
            try
            {
                byte[] encryptedData;
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.ImportParameters(RSAKeyInfo);
                    encryptedData = RSA.Encrypt(DataToEncrypt, false);
                }
                return encryptedData;
            }
            catch (CryptographicException e)
            {
                Console.Write(e.Message);
                return null;
            }
        }

        public static bool VerifySignedHash(byte[] DataToVerify, byte[] SignedData, RSAParameters Key)
        {
            if (SignedData == null) return false;

            try
            {
                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();
                RSAalg.ImportParameters(Key);
                // Verify the data using the signature.
                return RSAalg.VerifyData(DataToVerify, new SHA256CryptoServiceProvider(), SignedData);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
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
