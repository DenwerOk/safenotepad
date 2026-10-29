using System.Windows.Controls;
using System.Windows.Input;

namespace SafeNotePad.Views.Controls
{
    public class NoteDataGrid : DataGrid
    {
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                CommitEdit();
                e.Handled = true;
                return;
            }

            base.OnKeyDown(e);
        }
    }
}
