using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace SafeNotePad.Views
{
    public static class DataGridRowExtensions
    {
        public static DataGridCell GetCell(this DataGridRow row, int columnIndex = 0)
        {
            var grid = row.ParentOfType<DataGrid>();
            if (row == null) return null;

            var presenter = row.GetChildOfType<DataGridCellsPresenter>();
            if (presenter == null) return null;

            var cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(columnIndex);
            if (cell != null) return cell;

            // now try to bring into view and retreive the cell
            grid.ScrollIntoView(row, grid.Columns[columnIndex]);
            cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(columnIndex);

            return cell;
        }
    }
}
