using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace HashingH4.Service
{
    class Encrypter
    {
        public HashingAlgo HashingAlgo { private set; get; }
        HashAlgorithm hashAlgorithm;
        HMAC macInUse;
        RNGCryptoServiceProvider cryptoServiceProvider = new RNGCryptoServiceProvider();


        public Encrypter(HashingAlgo hashingAlgo)
        {
            HashingAlgo = hashingAlgo;
            switch (hashingAlgo)
            {
                case HashingAlgo.SHA1:
                    hashAlgorithm = new SHA1CryptoServiceProvider();
                    macInUse = new HMACSHA1();
                    break;
                default:
                case HashingAlgo.MD5:
                    hashAlgorithm = new MD5CryptoServiceProvider();
                    macInUse = new HMACMD5();
                    break;
                case HashingAlgo.SHA256:
                    hashAlgorithm = new SHA256CryptoServiceProvider();
                    macInUse = new HMACSHA256();
                    break;
                case HashingAlgo.SHA384:
                    hashAlgorithm = new SHA384CryptoServiceProvider();
                    macInUse = new HMACSHA384();
                    break;
                case HashingAlgo.SHA512:
                    hashAlgorithm = new SHA512CryptoServiceProvider();
                    macInUse = new HMACSHA512();
                    break;
            }
        }

        public byte[] GenerateKey(int keySize)
        {
            byte[] key = new byte[keySize];

            cryptoServiceProvider.GetBytes(key);

            return key;
        }

        public byte[] ComputeHash(byte[] message)
        {
            return hashAlgorithm.ComputeHash(message);
        }
        public byte[] ComputeMacWithKey(byte[] message, byte[] key)
        {
            macInUse.Key = key;
            return macInUse.ComputeHash(message);
        }

        public bool VerifyHash(byte[] hashedMessage, byte[] plaintextMessage, byte[] key = null)
        {
            byte[] resultHash;
            if (key == null)
            {
                resultHash = ComputeHash(plaintextMessage);
            }
            else
            {
                resultHash = ComputeMacWithKey(plaintextMessage, key);
            }

            return hashedMessage.SequenceEqual(resultHash);
        }

    }
}
