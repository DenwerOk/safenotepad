using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SafeNotePad.Views.Controls
{
    public class ImageButton : Button
    {
        public static readonly DependencyProperty ImageProperty =
              DependencyProperty.Register("Image", typeof(ImageSource),
              typeof(ImageButton));

        public ImageSource Image
        {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }
    }
}
