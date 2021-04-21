using HashingH4.Service;
using System;
using System.Security.Cryptography;
using System.Text;

namespace HashingH4
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {

                int function = SelectEncryptionOrVerify();
                int response = SelectEncrypter();
                Encrypter encrypter; 
                if(response == 0)
                {
                    Console.WriteLine("You need to pick an algorithm");
                    encrypter = null;
                }
                else {
                    encrypter = new Encrypter((HashingAlgo)response);
                }

                switch (function)
                {
                    case 0:
                    default:
                        break;
                    case 1:
                        Encrypt(encrypter);
                        break;
                    case 2:
                        EncryptWithHmac(encrypter);
                        break;
                    case 3:
                        VerifyHash(encrypter);
                        break;
                }


            }
        }

        

        static int SelectEncryptionOrVerify()
        {
            bool acceptedInput = false;
            int response = -1;
            Console.WriteLine("Select Encryption Algorithm");
            while (!acceptedInput)
            {
                Console.WriteLine("Press 1 for Encrypting");
                Console.WriteLine("Press 2 for Encrypting With HMAC");
                Console.WriteLine("Press 3 for Verifying");

                try
                {
                    response = int.Parse(Console.ReadLine());

                    if (response > -1 && response < 4)
                    {
                        acceptedInput = true;
                    }
                    else
                    {
                        Console.WriteLine("Input was accepted but it was not within the value");
                    }
                }
                catch
                {
                    Console.WriteLine("Type in a number");
                }


            }
            return response;
        }

        static int SelectEncrypter()
        {
            bool acceptedInput = false;
            int response = -1;
            Console.WriteLine("Select Encryption Algorithm");
            while (!acceptedInput)
            {
                Console.WriteLine("Press 0 for Back");
                Console.WriteLine("Press 1 for SHA1");
                Console.WriteLine("Press 2 for MD5");
                Console.WriteLine("Press 3 for SHA256");
                Console.WriteLine("Press 4 for SHA384");
                Console.WriteLine("Press 5 for SHA512");
                
                try
                {
                    response = int.Parse(Console.ReadLine());

                    if(response > -1 && response < 6)
                    {
                        acceptedInput = true;
                    }
                    else
                    {
                        Console.WriteLine("Input was accepted but it was not within the value");
                    }
                }
                catch
                {
                    Console.WriteLine("Type in a number");
                }


            }
            return response;
        }

        private static void VerifyHash(Encrypter encrypter)
        {
            Console.WriteLine("Verifying With the hashing Alorithm " + encrypter.HashingAlgo);
            Console.Write("Plain text message: ");
            string plainTextMessage = Console.ReadLine();

            Console.Write("Message hash In Base64: ");
            string messageHashedBase64 = Console.ReadLine();
            byte[] hashMassageByte;
            try
            {
                hashMassageByte = Convert.FromBase64String(messageHashedBase64); 
            }
            catch
            {
                return;
            }

            Console.Write("Write HMAC in Base64(Blank for no hmac): ");
            string hmacBase64 = Console.ReadLine();
            byte[] hmacBytes = null;
            if(hmacBase64 != "")
            {
                try
                {
                    hmacBytes = Convert.FromBase64String(hmacBase64);
                }
                catch
                {
                    return;
                }
            }

            Console.WriteLine("Message intregity result was: " + encrypter.VerifyHash(hashMassageByte, Encoding.UTF8.GetBytes(plainTextMessage), hmacBytes));
        }

        private static void EncryptWithHmac(Encrypter encrypter)
        {
            Console.WriteLine("Encrypting HMAC With the hashing Alorithm " + encrypter.HashingAlgo);
            Console.Write("Message to Encrypt: ");
            string messageToEncrypt = Console.ReadLine();

            Console.Write("Key to Encrypt with (blank for generating random key) base64 format: ");
            string keyToEncryptWith = Console.ReadLine();
            byte[] key = null;
            try
            {
                key = Convert.FromBase64String(keyToEncryptWith);
            }
            catch
            {
                Console.WriteLine("Could not convert user inputted string as Base64, generating random key");
                keyToEncryptWith = "";
            }
            
            if (keyToEncryptWith == "")
            {
                key = encrypter.GenerateKey(32);
            }

            Console.WriteLine("Message(Base64): " + Convert.ToBase64String(encrypter.ComputeMacWithKey(Encoding.UTF8.GetBytes(messageToEncrypt), key)));
            Console.WriteLine("Key(Base64): " +Convert.ToBase64String(key));
        }

        private static void Encrypt(Encrypter encrypter)
        {
            Console.WriteLine("Encrypting With the hashing Alorithm " + encrypter.HashingAlgo);

            Console.Write("Message to Encrypt:");
            string messageToEncrypt = Console.ReadLine();

            Console.WriteLine("Message(Base64): "+ Convert.ToBase64String(encrypter.ComputeHash(Encoding.UTF8.GetBytes(messageToEncrypt))));
        }
    }
}