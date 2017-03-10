using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using ACW_08346_464886_ServiceLibrary;

namespace ACW_08346_464886_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost myHost = new ServiceHost(typeof(Service1));
            myHost.Open();
            Console.WriteLine("Server running...");
            Console.ReadLine();
            myHost.Close();
        }
    }
}
