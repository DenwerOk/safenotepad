using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;
using SafeNotePad.Utils;

namespace SafeNotePad.Views
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : Window
    {
        public About()
        {
            InitializeComponent();
            txtVersion.Text = Settings.Version;
        }

        private void Hyperlink_OnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
        }
    }
}
