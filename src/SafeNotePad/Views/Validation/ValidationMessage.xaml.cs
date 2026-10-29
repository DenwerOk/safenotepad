using System.Windows;
using System.Windows.Controls;

namespace SafeNotePad.Views.Validation
{
    /// <summary>
    ///     Interaction logic for ValidationMessage.xaml
    /// </summary>
    public partial class ValidationMessage : UserControl
    {
        private static DependencyProperty ErrorProperty = DependencyProperty.Register(
                "Error",
                typeof(string),
                typeof(ValidationMessage),
                new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnErrorPropertyChanged))
            );

        public ValidationMessage()
        {
            InitializeComponent();
        }

        public string Error
        {
            get
            {
                return (string)GetValue(ErrorProperty);
            }
            set
            {
                SetValue(ErrorProperty, value);
            }
        }

        private static void OnErrorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var valMessage = (ValidationMessage) d;
            valMessage.errTextBlock.Text = (string)e.NewValue;
            valMessage.errTextBlock.Visibility = !string.IsNullOrWhiteSpace((string)e.NewValue) ? Visibility.Visible : Visibility.Hidden;
            valMessage.errorImg.Visibility = !string.IsNullOrWhiteSpace((string)e.NewValue) ? Visibility.Visible : Visibility.Hidden;
        }
    }
}