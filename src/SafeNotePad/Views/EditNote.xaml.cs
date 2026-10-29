using System;
using System.ComponentModel;
using System.Windows;
using SafeNotePad.Utils;
using SafeNotePad.ViewModels;

namespace SafeNotePad.Views
{
    /// <summary>
    /// Interaction logic for EditNote.xaml
    /// </summary>
    partial class EditNote
    {
        public EditNote()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            if (txtExistingPassword.Visibility == Visibility.Visible)
            {
                txtExistingPassword.Focus();
            }
            else
            {
                txtPassword.Focus();
            }
        }
    }

    class EditNoteBase : BaseChildWindow<EditNoteViewModel>
    {
        
    }
}
