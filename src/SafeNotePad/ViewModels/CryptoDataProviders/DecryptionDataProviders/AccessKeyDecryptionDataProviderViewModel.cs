using System;
using System.ComponentModel.DataAnnotations;
using SafeNotePad.Models.Crypto;

namespace SafeNotePad.ViewModels.CryptoDataProviders.DecryptionDataProviders
{
    class AccessKeyDecryptionDataProviderViewModel : BaseViewModel, IDecryptionDataProvider
    {
        private string _accessKey;
        private string _accessKeyName;

        [Required(ErrorMessage = "Password is required")]
        public string AccessKey
        {
            get
            {
                return _accessKey;
            }
            set
            {
                _accessKey = value;
                RaisePropertyChanged(() => AccessKey);
            }
        }

        public Func<IDecryptionData, bool> ValidateData { get; set; }

        public IDecryptionData DecryptionData
        {
            get
            {
                return new PasswordDecryptionData {Password = AccessKey};
            }
        }

        public string AccessKeyName
        {
            get
            {
                return _accessKeyName;
            }
            set
            {
                _accessKeyName = value;
                RaisePropertyChanged(() => AccessKeyName);
            }
        }

        protected override void OnOk()
        {
            bool result = true;
            if (ValidateData != null)
            {
                result = ValidateData(DecryptionData);
            }

            if (result)
            {
                CloseOk();
            }
            else
            {
                SetValidationSummaryError("Invalid password");
            }
        }
    }
}
