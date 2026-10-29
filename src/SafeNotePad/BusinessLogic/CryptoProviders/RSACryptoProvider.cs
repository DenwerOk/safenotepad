using System;
using System.Security.Cryptography;
using System.Text;

namespace SafeNotePad.BusinessLogic.CryptoProviders
{
    internal class RSACryptoProvider
    {
        public string RSAEncrypt(string dataString, RSAParameters RSAKeyInfo)
        {
            byte[] dataToEncrypt = Encoding.UTF8.GetBytes(dataString);

            try
            {
                byte[] encryptedData;
                //Create a new instance of RSACryptoServiceProvider. 
                using (var RSA = new RSACryptoServiceProvider(2048))
                {
                    //Import the RSA Key information. This only needs 
                    //toinclude the public key information.
                    RSA.ImportParameters(RSAKeyInfo);

                    //Encrypt the passed byte array and specify OAEP padding.   
                    //OAEP padding is only available on Microsoft Windows XP or 
                    //later.  
                    encryptedData = RSA.Encrypt(dataToEncrypt, false);
                }
                return Convert.ToBase64String(encryptedData);
            }
            //Catch and display a CryptographicException   
            //to the console. 
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return null;
            }
        }

        public string RSADecrypt(string dataStringEncrypted, RSAParameters RSAKeyInfo)
        {
            var dataToDecrypt = Convert.FromBase64String(dataStringEncrypted);
            try
            {
                byte[] decryptedData;
                //Create a new instance of RSACryptoServiceProvider. 
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(2048))
                {
                    //Import the RSA Key information. This needs 
                    //to include the private key information.
                    RSA.ImportParameters(RSAKeyInfo);

                    //Decrypt the passed byte array and specify OAEP padding.   
                    //OAEP padding is only available on Microsoft Windows XP or 
                    //later.  
                    decryptedData = RSA.Decrypt(dataToDecrypt, false);
                }
                return Encoding.UTF8.GetString(decryptedData);
            }
            //Catch and display a CryptographicException   
            //to the console. 
            catch (CryptographicException e)
            {
                Console.WriteLine(e.ToString());

                return null;
            }

        }
    }
}