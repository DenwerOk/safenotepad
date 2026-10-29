using System;
using System.Linq;
using System.Security.Cryptography;

namespace SafeNotePad.BusinessLogic.CryptoProviders
{
    class PBKDF2CryptoProvider : ICryptoProvider
    {
        public int SaltSize { get; set; }

        public int Iterations { get; set; }

        public int HashSize { get; set; }

        public int VectorSize { get; set; }

        public PBKDF2CryptoProvider()
        {
            HashSize = 30;
            Iterations = 10000;
            SaltSize = 15;
        }

        public string Decrypt(string encryptedPassword)
        {
            throw new NotImplementedException();
        }

        public string Encrypt(string password)
        {
            var salt = new byte[SaltSize];

            using (var randomGenerator = new RNGCryptoServiceProvider())
            {
                randomGenerator.GetBytes(salt);
            }

            using (var encrypter = new Rfc2898DeriveBytes(password, salt, Iterations))
            {
                var hash = encrypter.GetBytes(HashSize);
                hash = hash.Concat(salt).ToArray();

                return Convert.ToBase64String(hash);
            }
        }

        public bool IsMatch(string password, string encryptedPassword)
        {
            var bytes = Convert.FromBase64String(encryptedPassword);
            var hashBytes = bytes.Take(HashSize).ToArray();
            var saltBytes = bytes.Skip(HashSize).Take(SaltSize).ToArray();

            using (var deriveBytes = new Rfc2898DeriveBytes(password, saltBytes, Iterations))
            {
                byte[] newKey = deriveBytes.GetBytes(HashSize);

                if (!newKey.SequenceEqual(hashBytes))
                    return false;
            }

            return true;
        }
    }
}
