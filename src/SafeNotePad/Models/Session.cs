using System.Security.Cryptography;

namespace SafeNotePad.Models
{
    class Session
    {
        private static Session _sessionInstance;

        public static Session CurrentSession
        {
            get
            {
                if (_sessionInstance == null)
                {
                    _sessionInstance = new Session();
                }

                return _sessionInstance;
            }
        }

        public RSAParameters? BasicKey { get; set; }
    }
}
