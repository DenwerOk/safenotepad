using System.Windows;
using SafeNotePad.ViewModels;
using UserControl = System.Windows.Controls.UserControl;

namespace SafeNotePad.Views
{
    class BaseUserControl<T> : UserControl, IBaseUserControl<T> where T : BaseViewModel, new()
    {
        public T ViewModel
        {
            get
            {
                return (T)DataContext;
            }
            set
            {
                DataContext = value;
            }
        }

        public BaseUserControl()
        {
            Initialize<T>();
            Loaded += OnLoaded;
        }

        public T2 FindPage<T2>() where T2 : DependencyObject
        {
            return this.ParentOfType<T2>();
        }

        public void Initialize<TParam>() where TParam : T, new()
        {
            ViewModel = new TParam();
            ViewModel.View = this;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            ViewModel.EnableValidationHandlers();
        }
    }
}
