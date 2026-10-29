using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using SafeNotePad.Logging;
using SafeNotePad.ViewModels;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace SafeNotePad.Views
{
    class BaseChildWindow<T> : Window, IBaseChildWindow<T> where T : BaseViewModel, new()
    {
        private static ILogger _logger;

        public bool IsClosed { get; set; }

        public virtual bool AutoInitialization
        {
            get
            {
                return true;
            }
        }

        public T ViewModel
        {
            get
            {
                return (T) DataContext;
            }
            set
            {
                DataContext = value;
            }
        }

        public BaseChildWindow()
        {
            if (AutoInitialization)
            {
                Initialize<T>();
            }

            Loaded += OnLoaded;
            _logger = LogManager.GetLogger(GetType().Name);
        }

        public T2 FindPage<T2>() where T2 : DependencyObject
        {
            return this.ParentOfType<T2>();
        }

        public void Initialize<TParam>() where TParam : T, new()
        {
            ViewModel = new TParam();
            ViewModel.View = this;
            ViewModel.OnClose += OnClose;
            ViewModel.OnCancel += OnCancel;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _logger.Info("Loaded");
            ViewModel.EnableValidationHandlers();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ViewModel.OkCommand.Execute(null);
                return;
            }

            if (e.Key == Key.Escape)
            {
                ViewModel.CancelCommand.Execute(null);
                return;
            }

            base.OnKeyDown(e);
        }

        public void CloseInstantly()
        {
            ViewModel.CloseToExit = true;
            Close();
        }

        public bool? ShowDialog(IBaseChildWindow<IBaseViewModel> dialog)
        {
            if (WindowState == WindowState.Normal && Visibility == Visibility.Visible)
            {
                dialog.Owner = this;
                dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }
            else
            {
                dialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }

            dialog.ShowInTaskbar = false;
            //dialog.Topmost = true;
            dialog.ResizeMode = ResizeMode.NoResize;

            if (!dialog.IsClosed)
            {
                return dialog.ShowDialog();
            }

            return dialog.DialogResult;
        }

        public void ShowErrorMessage(string message)
        {
            MessageBox.Show(this, message, "Error", MessageBoxButton.OK,
                                    MessageBoxImage.Error);
        }

        public void Show(IBaseChildWindow<IBaseViewModel> dialog)
        {
            dialog.Owner = this;
            dialog.ShowInTaskbar = false;
            //dialog.Topmost = true;
            dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            dialog.ResizeMode = ResizeMode.NoResize;

            if (!dialog.IsClosed)
            {
                dialog.Show();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            IsClosed = true;
            ViewModel.Cleanup();
            base.OnClosed(e);
            _logger.Info("Closed");
        }

        private void OnCancel(object sender, EventArgs eventArgs)
        {
            if (IsModal() && !IsClosed)
            {
                Dispatcher.Invoke(() =>
                                  {
                                      DialogResult = false;
                                  });
            }
            else
            {
                Close();
            }
        }

        private void OnClose(object sender, EventArgs<bool> eventArgs)
        {
            if (IsModal() && Visibility == Visibility.Visible)
            {
                Dispatcher.Invoke(() =>
                                  {
                                      DialogResult = eventArgs.Value;
                                  });
            }
            else
            {
                Close();
            }
        }

        private bool IsModal()
        {
            return (bool)typeof(Window).GetField("_showingAsDialog", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(this);
        }
    }
}
