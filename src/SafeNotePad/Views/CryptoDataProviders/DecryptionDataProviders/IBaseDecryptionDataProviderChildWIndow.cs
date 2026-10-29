using SafeNotePad.ViewModels.CryptoDataProviders.DecryptionDataProviders;

namespace SafeNotePad.Views.CryptoDataProviders.DecryptionDataProviders
{
    interface IBaseDecryptionDataProviderChildWindow<out T> : IBaseChildWindow<T> where T: IDecryptionDataProvider
    {
    }
}
