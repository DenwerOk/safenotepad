using System;
using System.IO;
using SafeNotePad.BusinessLogic.CryptoProviders.Engine;
using SafeNotePad.Models.Crypto;

namespace SafeNotePad.BusinessLogic.CryptoProviders
{
    internal class AESCryptoProvider
    {
        private static readonly byte[] Key =
        {
            0, 2, 5, 8, 6, 9, 11, 15, 17,
            25, 5, 7, 8, 96, 4, 15, 25,
            18, 25, 36, 32, 65, 6, 9,
            5, 8, 7, 5, 8, 6, 10, 21
        };

        public string Decrypt(string encryptedContent, IDecryptionData data)
        {
            try
            {
                if (data is PasswordDecryptionData)
                {
                    return AESThenHMAC.SimpleDecryptWithPassword(encryptedContent, ((PasswordDecryptionData)data).Password);
                }

                if (data is SecurityFileDecryptionData)
                {
                    return AESThenHMAC.SimpleDecryptWithPassword(encryptedContent, File.ReadAllText(((SecurityFileDecryptionData)data).FilePath));
                }
            }
            catch (Exception ex)
            {
                if (data is PasswordDecryptionData)
                {
                    throw new Exception("Invalid password", ex);
                }

                if (data is SecurityFileDecryptionData)
                {
                    throw new Exception("Invalid security file", ex);
                }
            }

            throw new ArgumentException();
        }

        public string Encrypt(string content, IEncryptionData data)
        {
            if (data is PasswordEncryptionData)
            {
                return AESThenHMAC.SimpleEncryptWithPassword(content, ((PasswordEncryptionData)data).Password);
            }

            if (data is SecurityFileEncryptionData)
            {
                return AESThenHMAC.SimpleEncryptWithPassword(content, File.ReadAllText(((SecurityFileEncryptionData)data).FilePath));
            }

            throw new ArgumentException();
        }
    }
}