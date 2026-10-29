using System;
using System.Security.Cryptography;
using Moq;
using NUnit.Framework;
using SafeNotePad.Interfaces.Services;
using SafeNotePad.Models;
using SafeNotePad.Models.Crypto;
using SafeNotePad.Models.Enum;
using SafeNotePad.Services;

namespace SafeNotePad.Tests.Services
{
    [TestFixture]
    class AccessKeyServiceTests
    {
        private Mock<IDomainService> _domainServiceMock;

        private AccessKeyService GetAccessKeyService()
        {
            _domainServiceMock = new Mock<IDomainService>();
            return new AccessKeyService(_domainServiceMock.Object);
        }

        [Test]
        public void EncryptDecryptKey()
        {
            const string content = @"k|tghjOO\_PPzi_XbJi`MAoYJ`BbTfry";
            var service = GetAccessKeyService();

            var accessKey = new AccessKey
            {
                EncryptionTypeID = (int)EncryptionType.Password,
                EncryptionData = new PasswordEncryptionData { Password = "x" },
                PublicKey = Constants.TestPublicKey,
                PrivateKeyEncrypted = Constants.TestPrivateKeyEncrypted
            };
            var encryptedContent = service.Encrypt(content, accessKey);
            Assert.IsNotEmpty(
                encryptedContent, 
                "Content encrypted by Access Key is empty");

            Assert.AreNotEqual(content, encryptedContent, "Encrypted content must not be equal to decrypted content!");
            Assert.IsNotEmpty(accessKey.PrivateKeyEncrypted, "Private key encrypted must not be empty!");
            Assert.IsFalse(accessKey.PrivateKeyEncrypted.Contains("RSAKeyValue"), "Private key must be encrypted!!!");

            var decrypted = service.Decrypt(encryptedContent, accessKey, new PasswordDecryptionData {Password = "x"});

            Assert.AreEqual(content, decrypted, "Encryption or decryption is corrupt");
        }

        [Test]
        public void DecryptPrivateKeyValue()
        {
            const string decryptedPrivateKeyValue = Constants.TestPrivateKey;

            var accessKey = new AccessKey
            {
                AccessKeyID = 25,
                EncryptionTypeID = (int)EncryptionType.Password,
                PublicKey = Constants.TestPublicKey,
                PrivateKeyEncrypted = Constants.TestPrivateKeyEncrypted
            };

            var service = GetAccessKeyService();

            // check if z password works
            var decryptedValue = service.DecryptPrivateKeyValue(accessKey, new PasswordDecryptionData { Password = "x" });
            Assert.AreEqual(decryptedPrivateKeyValue, decryptedValue, "Decryption doesn't work.");
        }

        [Test]
        public void DecryptionValidationTest()
        {
            var accessKey = new AccessKey
            {
                AccessKeyID = 25,
                EncryptionTypeID = (int)EncryptionType.Password,
                PublicKey = Constants.TestPublicKey,
                PrivateKeyEncrypted = Constants.TestPrivateKeyEncrypted,
            };

            var service = GetAccessKeyService();

            var isValid = service.IsDecryptionValid(new PasswordDecryptionData { Password = "x" }, accessKey);
            Assert.IsTrue(isValid, "Decryption validation returned wrong result");

            isValid = service.IsDecryptionValid(new PasswordDecryptionData { Password = "y" }, accessKey);
            Assert.IsFalse(isValid, "Decryption validation returned wrong result");
        }
    }
}
