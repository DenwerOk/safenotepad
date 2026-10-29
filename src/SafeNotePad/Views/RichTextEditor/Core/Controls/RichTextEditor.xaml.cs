using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SafeNotePad.ViewModels;

namespace WpfRichText.Ex.Controls
{
    /// <summary>
    /// Interaction logic for BindableRichTextbox.xaml
    /// </summary>
    public partial class RichTextEditor : UserControl
    {
        public static readonly DependencyProperty TextProperty =
          DependencyProperty.Register("Text", typeof(string), typeof(RichTextEditor),
          new PropertyMetadata(string.Empty));

        public event EventHandler Save;
        
        public RichTextEditor()
        {
            InitializeComponent();
        }

        public string Text
        {
            get { return GetValue(TextProperty) as string; }
            set { 
                SetValue(TextProperty, value);
            }
        }

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (Save != null)
            {
                Save(this, EventArgs.Empty);
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
            }
        }

        public EditorPanelSessionSelection GetCurrentSelection()
        {
            var contentStart = mainRTB.Document.ContentStart;
            return new EditorPanelSessionSelection { OffsetStart = contentStart.GetOffsetToPosition(mainRTB.Selection.Start), OffsetEnd = contentStart.GetOffsetToPosition(mainRTB.Selection.End) };
        }

        public EditorPanelSessionSelection GetDefaultSelection()
        {
            return new EditorPanelSessionSelection { OffsetStart = 0, OffsetEnd = 0 };
        }

        public void SetSelection(EditorPanelSessionSelection selection)
        {
            if (mainRTB.Document != null)
            {
                TextPointer contentStart = mainRTB.Document.ContentStart;

                mainRTB.Focus();
                // Use previously determined offsets to create corresponding TextPointers, 
                // and use these to restore the selection.
                mainRTB.Selection.Select(
                   contentStart.GetPositionAtOffset(selection.OffsetStart),
                   contentStart.GetPositionAtOffset(selection.OffsetEnd)
                );
            }
        }

        private void FrameworkElement_OnLoaded(object sender, RoutedEventArgs e)
        {
            ToolBar toolBar = sender as ToolBar;
            var overflowGrid = toolBar.Template.FindName("OverflowGrid", toolBar) as FrameworkElement;
            if (overflowGrid != null)
            {
                overflowGrid.Visibility = Visibility.Collapsed;
            }
        }
    }
}
