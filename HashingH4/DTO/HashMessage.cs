using HashingH4.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace HashingH4.DTO
{

    class HashMessage
    {
        public HashingAlgo HashingAlgo { get; private set; }


        public byte[] HashedMessage { get; private set; }

        public byte[] Salt { get; private set; }


        public HashMessage(HashingAlgo algo, byte[] hashedMessage, byte[] salt)
        {
            HashingAlgo = algo;

            HashedMessage = hashedMessage;

            Salt = salt;
        }
    }
}
