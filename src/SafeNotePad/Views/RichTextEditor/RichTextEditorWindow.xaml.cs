using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using WpfRichText.Ex.AttachedProperties;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace SafeNotePad.Views.RichTextEditor
{
    /// <summary>
    /// Interaction logic for RichTextEditorWindow.xaml
    /// </summary>
    partial class RichTextEditorWindow
    {
        public static RoutedCommand MyCommand = new RoutedCommand();

        public override bool AutoInitialization
        {
            get { return false; }
        }

        static RichTextEditorWindow()
        {
            MyCommand.InputGestures.Add( new KeyGesture( Key.S , ModifierKeys.Control ));
        }

        public RichTextEditorWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            ViewModel.Loaded();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!DialogResult.GetValueOrDefault() && !ViewModel.CloseToExit && !ViewModel.ProcessClose())
            {
                e.Cancel = true;
            }

            base.OnClosing(e);
        }

        protected override void OnKeyDown(KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.Key == Key.Enter)
            {
                return;
            }

            base.OnKeyDown(keyEventArgs);
        }

        private void MyCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            RichTextboxAssistant.UpdateDocumentBound(Editor.mainRTB);
            ViewModel.OkCommand.Execute(null);
            e.Handled = true;
        }
    }

    class RichTextEditorWindowBase : BaseChildWindow<RichTextEditorViewModel>
    {
        
    }
}
