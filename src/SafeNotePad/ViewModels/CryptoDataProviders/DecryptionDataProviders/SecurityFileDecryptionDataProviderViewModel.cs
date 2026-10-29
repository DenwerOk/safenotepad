using System;
using System.IO;
using System.Windows.Forms;
using GalaSoft.MvvmLight.Command;
using SafeNotePad.Models.Crypto;

namespace SafeNotePad.ViewModels.CryptoDataProviders.DecryptionDataProviders
{
    class SecurityFileDecryptionDataProviderViewModel : BaseViewModel, IDecryptionDataProvider
    {
        private string _filePath;
        private string _accessKeyName;
        public Func<IDecryptionData, bool> ValidateData { get; set; }

        public IDecryptionData DecryptionData { get; private set; }

        public RelayCommand BrowseCommand { get; set; }

        public string FilePath
        {
            get
            {
                return _filePath;
            }
            set
            {
                _filePath = value;
                RaisePropertyChanged(() => FilePath);
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

        public SecurityFileDecryptionDataProviderViewModel()
        {
            BrowseCommand = new RelayCommand(OnBrowse);
        }

        private void OnBrowse()
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                FilePath = openFileDialog.FileName;
            }
        }

        protected override void OnOk()
        {
            if (string.IsNullOrEmpty(FilePath))
            {
                ValidationSummaryError = "File path can not be empty";
                return;
            }

            var error = ServiceContext.ValidateSecurityFile(FilePath);
            if (error.Value != null)
            {
                ValidationSummaryError = error.Value;
                return;
            }

            DecryptionData = new SecurityFileDecryptionData {FilePath = FilePath};
            if (ValidateData(DecryptionData))
            {
                CloseOk();
            }
            else
            {
                ValidationSummaryError = "Invalid File";
            }
        }
    }
}
