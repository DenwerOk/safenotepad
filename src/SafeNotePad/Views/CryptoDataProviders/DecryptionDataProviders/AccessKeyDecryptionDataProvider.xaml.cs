using System;
using System.Windows;
using System.Windows.Input;
using SafeNotePad.ViewModels.CryptoDataProviders.DecryptionDataProviders;

namespace SafeNotePad.Views.CryptoDataProviders.DecryptionDataProviders
{
    /// <summary>
    /// Interaction logic for RequestAccessKey.xaml
    /// </summary>
    partial class AccessKeyDecryptionDataProvider
    {
        public AccessKeyDecryptionDataProvider()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            txtPassword.Focus();
        }
    }

    class AccessKeyDecryptionDataProviderBase : BaseChildWindow<AccessKeyDecryptionDataProviderViewModel>,
        IBaseDecryptionDataProviderChildWindow<AccessKeyDecryptionDataProviderViewModel>
    {
        
    }
}
