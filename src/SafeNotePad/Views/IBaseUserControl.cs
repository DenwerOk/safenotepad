using SafeNotePad.ViewModels;

namespace SafeNotePad.Views
{
    interface IBaseUserControl<out T> : IBaseControl where T : IBaseViewModel
    {
    }
}
