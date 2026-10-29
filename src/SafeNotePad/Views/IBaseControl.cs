using System.Windows;

namespace SafeNotePad.Views
{
    interface IBaseControl
    {
        T FindPage<T>() where T : DependencyObject;
    }
}
