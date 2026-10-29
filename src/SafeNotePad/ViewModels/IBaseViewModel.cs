using SafeNotePad.Facade;
using SafeNotePad.Views;

namespace SafeNotePad.ViewModels
{
    interface IBaseViewModel
    {
        bool CloseToExit { get; set; }

        ServiceContext ServiceContext { get; }

        IBaseChildWindow<BaseViewModel> ChildWindowView { get; }

        IBaseUserControl<BaseViewModel> UserControlView { get; }

        IBaseControl View { get; set; }
    }
}
