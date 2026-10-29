using System.Windows;
using SafeNotePad.ViewModels.CryptoDataProviders.DecryptionDataProviders;

namespace SafeNotePad.Views.CryptoDataProviders.DecryptionDataProviders
{
    /// <summary>
    /// Interaction logic for RequestAccessKey.xaml
    /// </summary>
    partial class SecurityFileDecryptionDataProvider
    {
        public SecurityFileDecryptionDataProvider()
        {
            InitializeComponent();
        }

        private void File_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Note that you can have more than one file.
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                // Assuming you have one file that you care about, pass it off to whatever
                // handling code you have defined.
                ViewModel.FilePath = files[0];
            }
        }
    }

    class SecurityFileDecryptionDataProviderBase : BaseChildWindow<SecurityFileDecryptionDataProviderViewModel>,
        IBaseDecryptionDataProviderChildWindow<SecurityFileDecryptionDataProviderViewModel>
    {
        
    }
}
