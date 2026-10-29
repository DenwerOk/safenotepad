using System.Security.Cryptography;

namespace SafeNotePad.Models.Crypto
{
    class BasicDecryptionData : IDecryptionData
    {
        public RSAParameters PrivateKey { get; set; }
    }
}
