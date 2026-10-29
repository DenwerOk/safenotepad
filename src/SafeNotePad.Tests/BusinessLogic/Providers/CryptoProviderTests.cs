using System;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SafeNotePad.BusinessLogic.CryptoProviders;
using SafeNotePad.Models.Crypto;

namespace SafeNotePad.Tests.BusinessLogic.Providers
{
    [TestFixture]
    public class CryptoProviderTests
    {
        [Test]
        public void AESCryptoStressTest()
        {
            var contents = TestLibrary.ReadAllLines("BusinessLogic\\Providers\\CryptoProviderSampleContent.txt").ToList();
            var passwords = TestLibrary.ReadAllLines("BusinessLogic\\Providers\\CryptoProviderSamplePasswords.txt");
            contents.Add("BusinessLogic\\Providers\\AESTestFiles\\Test1.txt");
            contents.Add("BusinessLogic\\Providers\\AESTestFiles\\Test2.txt");
            contents.Add("BusinessLogic\\Providers\\AESTestFiles\\Test3.txt");
            for (int i = 0; i < contents.Count; i++)
            {
                var content = contents[i];
                for (int j = 0; j < passwords.Length; j++)
                {
                    var password = passwords[j];
                    var aesProvider = new AESCryptoProvider();
                    var encrypted = aesProvider.Encrypt(content, new PasswordEncryptionData {Password = password});
                    var decrypted = aesProvider.Decrypt(encrypted, new PasswordDecryptionData {Password = password});

                    Assert.AreEqual(content, decrypted,
                        "Decrypted and encrypted won't match: encrypted is {0}, decrypted is {1}", encrypted, decrypted);
                }

                Console.WriteLine("{0}% passed", i * 100 / contents.Count);
            }
        }

        [Test]
        public void RSACryptoStressTest()
        {
            var contents = TestLibrary.ReadAllLines("BusinessLogic\\Providers\\CryptoProviderSampleContent.txt").ToList();
            var largeFile = TestLibrary.ReadAllLines("BusinessLogic\\Providers\\RSAProviderTestFile.txt");
            contents.Add(largeFile.ToString());
            for (int i = 0; i < contents.Count; i++)
            {
                var content = contents[i];
                var rsaProvider = new RSACryptoProvider();
                var param = CryptoHelper.CreateNewParameters();
                var encrypted = rsaProvider.RSAEncrypt(content, param);
                var decrypted = rsaProvider.RSADecrypt(encrypted, param);

                Assert.AreEqual(content, decrypted,
                    "Decrypted and encrypted won't match: encrypted is {0}, decrypted is {1}", encrypted, decrypted);

                Console.WriteLine("{0}% passed", i * 100 / contents.Count);
            }
        }

        [Test]
        [Ignore]
        public void Generate()
        {
            var rnd = new Random((int)DateTime.Now.Ticks);

            for (int i = 1; i < 60; i++)
            {
                for (int j = 0; j < 1; j++)
                {
                    GenerateString(i, rnd);
                }
            }
        }

        void GenerateString(int max, Random rnd)
        {
            var str = new StringBuilder();
            for (int j = 0; j < max; j++)
            {
                str.Append(Convert.ToChar(Convert.ToInt32(Math.Floor(60 * rnd.NextDouble() + 65))));
            }

            Console.WriteLine(str);
        }
    }
}
