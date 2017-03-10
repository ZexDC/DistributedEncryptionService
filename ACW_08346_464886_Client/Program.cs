using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACW_08346_464886_Client
{
    class Program
    {
        static void Main(string[] args)
        {
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
                string instruction = s.Substring(0, firstSpaceIndex);
                // execute action based on instruction
                ServiceReference1.Service1Client Service = new ServiceReference1.Service1Client();
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
                        foreach (string element in sortedArray)
                        {
                            Console.WriteLine("{0} ", element);
                        }
                        break;
                    case "PUBKEY":
                        break;
                    case "ENC":
                        break;
                    case "SHA1":
                        break;
                    case "SHA256":
                        break;
                    case "SIGN":
                        break;
                    default:
                        break;
                }
            }
            Console.ReadLine();
        }
    }
}
