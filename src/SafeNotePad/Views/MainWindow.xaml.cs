using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SafeNotePad.ViewModels;

namespace SafeNotePad.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    partial class MainWindow
    {
        public static RoutedCommand MyCommand = new RoutedCommand();
        private string _filePath = null;

        public MainWindow(string filePath)
        {
            MyCommand.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));
            InitializeComponent();
            NoteListBox.SelectionChanged += PasswordGridOnSelectionChanged;
            _filePath = filePath;
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            if (_filePath != null)
            {
                ViewModel.LoadNote(_filePath);
            }
            else
            {
                ViewModel.NewNote();
            }
        }

        private void PasswordGridOnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            if (selectionChangedEventArgs.AddedItems.Count > 0)
            {
                NoteListBox.ScrollIntoView(selectionChangedEventArgs.AddedItems[0]);
            }
        }

        private void MyCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ViewModel.OkCommand.Execute(null);
            e.Handled = true;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (!ViewModel.EnsureClose())
            {
                e.Cancel = true;
            }
        }
    }

    class MainWindowBase : BaseChildWindow<MainWindowViewModel>
    {
        
    }
}
