using System.Security.Cryptography;

namespace SafeNotePad.BusinessLogic.CryptoProviders
{
    public static class CryptoExtensions
    {
        public static RSAParameters ToRSAParameter(this string xml, bool includePrivateKey)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(xml);
                return rsa.ExportParameters(includePrivateKey);
            }
        }

        public static string GetPublicKey(this RSAParameters param)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(param);
                return rsa.ToXmlString(false);
            }
        }

        public static string GetPrivateKey(this RSAParameters param)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(param);
                return rsa.ToXmlString(true);
            }
        }
    }
}
