using System;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Command;
using SafeNotePad.Models;
using SafeNotePad.Models.Enum;
using SafeNotePad.ViewModels;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace SafeNotePad.Views
{
    /// <summary>
    /// Interaction logic for EditorPanel.xaml
    /// </summary>
    partial class EditorPanel
    {
        private static readonly DependencyProperty NoteProperty = DependencyProperty.Register(
            "Note",
            typeof (Note),
            typeof(EditorPanel),
            new FrameworkPropertyMetadata(OnNotePropertyChanged));

        private static readonly DependencyProperty SecureCommandProperty = DependencyProperty.Register(
            "SecureCommand",
            typeof(RelayCommand<Note>),
            typeof(EditorPanel),
            new FrameworkPropertyMetadata(OnSecureCommandPropertyChanged));

        public Note Note
        {
            get
            {
                return (Note)GetValue(NoteProperty);
            }

            set
            {
                SetValue(NoteProperty, value);
            }
        }

        public RelayCommand SecureCommand
        {
            get
            {
                return (RelayCommand)GetValue(SecureCommandProperty);
            }
            set
            {
                SetValue(SecureCommandProperty, value);
            }
        }

        public EditorPanel()
        {
            InitializeComponent();
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.Key == Key.Enter && ((FrameworkElement)e.OriginalSource).ParentOfName("pwdBox") != null)
            {
                ViewModel.AuthorizeCommand.Execute(null);
            }

            base.OnKeyUp(e);
        }

        private static void OnSecureCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var panel = (EditorPanel)d;
            panel.ViewModel.SecureCommand = (RelayCommand<Note>)e.NewValue;
        }

        private static void OnNotePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var panel = (EditorPanel) d;

            if (panel.ViewModel.Session != null)
            {
                panel.ViewModel.Session.EditorPanelSessionSelection = panel.Editor.GetCurrentSelection();
            }

            if (e.NewValue != null)
            {
                panel.ViewModel.Initialize((Note)e.NewValue);

                if (panel.ViewModel.Session != null)
                {
                    if (!panel.ViewModel.Session.IsAuthorized)
                    {
                        if (panel.ViewModel.Session.Note.AccessKey.EncryptionTypeID == (int)EncryptionType.Password)
                        {
                            panel.Dispatcher.BeginInvoke(
                                DispatcherPriority.ContextIdle,
                                new Action(delegate ()
                                           {
                                               panel.txtPassword.Focus();
                                           }));
                        }
                    }
                    else
                    {
                        EditorPanelSessionSelection selection = panel.ViewModel.Session.EditorPanelSessionSelection ?? panel.Editor.GetDefaultSelection();

                        panel.Dispatcher.BeginInvoke(
                            DispatcherPriority.ContextIdle,
                            new Action(delegate ()
                                       {
                                           panel.Editor.SetSelection(selection);
                                       }));
                    }
                }
            }
        }

        private void Editor_OnSave(object sender, EventArgs e)
        {
            ViewModel.SaveCommand.Execute(null);
        }

        private void UIElement_OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // Set Height manually since binding doesn't work sometimes
            if (PanelGrid != null && PanelGrid.RowDefinitions.Any())
            {
                var actualHeight = (bool)e.NewValue ? noteProtection.Height : 0;
                PanelGrid.RowDefinitions[0].Height = new GridLength(actualHeight);
            }
        }
    }

    class EditorPanelBase : BaseUserControl<EditorPanelViewModel>
    {
        
    }
}
