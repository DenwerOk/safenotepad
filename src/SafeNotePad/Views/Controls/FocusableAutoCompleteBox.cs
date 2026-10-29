using System.Windows.Controls;
using System.Windows.Input;

namespace SafeNotePad.Views.Controls
{
    internal class FocusableAutoCompleteBox : AutoCompleteBox
    {
        public new void Focus()
        {
            var textbox = Template.FindName("Text", this) as TextBox;
            if (textbox != null) textbox.Focus();
        }

        // This still goes in your code behind (yuck!)
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (!IsDropDownOpen && SelectedItem != null && (e.Key == Key.Enter || e.Key == Key.Return))
            {
                // Drop down is closed so the item in the textbox should be submitted with a press of the Enter key
                base.OnKeyDown(e); // This has to happen before we mark Handled = false
                e.Handled = false; // Set Handled = false so the event bubbles up to your inputbindings
                return;
            }

            // Drop down is open so user must be selecting an AutoComplete list item
            base.OnKeyDown(e);
        }
    }
}