using System.Windows;
using System.Windows.Media;
using SafeNotePad.ViewModels;

namespace SafeNotePad.Views
{
    interface IBaseChildWindow<out T> : IBaseControl where T : IBaseViewModel
    {
        bool IsClosed { get; }

        bool IsLoaded { get; }

        bool? DialogResult { get; set; }

        void Hide();

        void Show();

        void ShowErrorMessage(string message);

        void Show(IBaseChildWindow<IBaseViewModel> dialog);

        bool? ShowDialog();

        bool? ShowDialog(IBaseChildWindow<IBaseViewModel> window);

        T ViewModel { get; }

        bool Topmost { get; set; }

        ImageSource Icon { get; set; }

        ResizeMode ResizeMode { get; set; }

        bool ShowInTaskbar { get; set; }

        WindowStartupLocation WindowStartupLocation { get; set; }

        Window Owner { get; set; }

        WindowState WindowState { get; set; }
    }
}
