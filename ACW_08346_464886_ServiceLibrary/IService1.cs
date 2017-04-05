using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ACW_08346_464886_ServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        string HelloEcho(int id);

        [OperationContract]
        string[] Sort(string[] array);

        [OperationContract]
        string[] PublicKey();

        [OperationContract]
        void Decrypt(byte[] encryptedByteMessage);

        [OperationContract]
        string ComputeSHA1(string message);

        [OperationContract]
        string ComputeSHA256(string message);

        [OperationContract]
        byte[] Sign(byte[] asciiByteMessage);
    }

}
