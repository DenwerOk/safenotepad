using System;
using SafeNotePad.Models.Crypto;

namespace SafeNotePad.ViewModels.CryptoDataProviders.DecryptionDataProviders
{
    interface IDecryptionDataProvider : IBaseViewModel
    {
        Func<IDecryptionData, bool> ValidateData { get; set; }

        IDecryptionData DecryptionData { get; }

        string AccessKeyName { get; set; }
    }
}
