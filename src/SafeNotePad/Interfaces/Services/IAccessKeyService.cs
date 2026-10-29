using System.Security.Cryptography;
using SafeNotePad.Models;
using SafeNotePad.Models.Crypto;
using SafeNotePad.Models.Enum;

namespace SafeNotePad.Interfaces.Services
{
    public interface IAccessKeyService
    {
        string Encrypt(string content, AccessKey accessKey);

        string Decrypt(string cipher, AccessKey accessKey, IDecryptionData data);

        RSAParameters DecryptPrivateKey(AccessKey accessKey, IDecryptionData data);

        bool IsDecryptionValid(IDecryptionData data, AccessKey accessKey);

        string GenerateSecurityFile();

        string ValidateSecurityFile(string path);
    }
}