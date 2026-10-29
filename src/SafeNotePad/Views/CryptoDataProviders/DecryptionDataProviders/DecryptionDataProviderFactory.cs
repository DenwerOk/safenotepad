using SafeNotePad.Models.Enum;
using SafeNotePad.ViewModels.CryptoDataProviders.DecryptionDataProviders;

namespace SafeNotePad.Views.CryptoDataProviders.DecryptionDataProviders
{
    class DecryptionDataProviderFactory
    {
        public static IBaseDecryptionDataProviderChildWindow<IDecryptionDataProvider> Create(EncryptionType type)
        {
            if (type == EncryptionType.Password)
            {
                return new AccessKeyDecryptionDataProvider();
            }

            if (type == EncryptionType.SecurityFile)
            {
                return new SecurityFileDecryptionDataProvider();
            }

            return null;
        }
    }
}
