using System;
using System.IO;
using System.Security.Cryptography;
using SafeNotePad.BusinessLogic.CryptoProviders;
using SafeNotePad.Interfaces.Services;
using SafeNotePad.Models;
using SafeNotePad.Models.Crypto;

namespace SafeNotePad.Services
{
    class AccessKeyService : IAccessKeyService
    {
        private readonly IDomainService _domainService;

        public AccessKeyService(IDomainService domainService)
        {
            _domainService = domainService;
        }

        public RSAParameters DecryptPrivateKey(AccessKey accessKey, IDecryptionData data)
        {
            return DecryptPrivateKeyValue(accessKey, data).ToRSAParameter(true);
        }

        public string DecryptPrivateKeyValue(AccessKey accessKey, IDecryptionData data)
        {
            if (data is BasicDecryptionData)
            {
                return ((BasicDecryptionData) data).PrivateKey.GetPrivateKey();
            }

            var provider = new AESCryptoProvider();
            return provider.Decrypt(accessKey.PrivateKeyEncrypted, data);
        }

        public string Decrypt(string cipher, AccessKey accessKey, IDecryptionData data)
        {
            var key = DecryptPrivateKey(accessKey, data);
            var rsaService = new RSACryptoProvider();
            return rsaService.RSADecrypt(cipher, key);
        }

        public AccessKey EncryptKey(AccessKey accessKey, IEncryptionData data)
        {
            return EncryptKey(accessKey, data, CryptoHelper.CreateNewParameters());
        }

        private AccessKey EncryptKey(AccessKey accessKey, IEncryptionData data, RSAParameters parameter)
        {
            var provider = new AESCryptoProvider();
            accessKey.PublicKey = parameter.GetPublicKey();
            accessKey.PrivateKeyEncrypted = provider.Encrypt(parameter.GetPrivateKey(), data);

            return accessKey;
        }

        public string Encrypt(string content, AccessKey accessKey)
        {
            var rsaService = new RSACryptoProvider();
            return rsaService.RSAEncrypt(content, accessKey.PublicKey.ToRSAParameter(false));
        }

        public bool IsDecryptionValid(IDecryptionData data, AccessKey accessKey)
        {
            try
            {
                DecryptPrivateKey(accessKey, data);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public string ValidateSecurityFile(string path)
        {
            if (!File.Exists(path))
            {
                return "File not found";
            }

            return null;
        }

        public string GenerateSecurityFile()
        {
            using (var randomProvider = new RNGCryptoServiceProvider())
            {
                var backupKeyBytes = new byte[1024];
                randomProvider.GetBytes(backupKeyBytes);
                return "<SecurityKey>" + Convert.ToBase64String(backupKeyBytes) + "</SecurityKey>";
            }
        }
    }
}
