using System.Windows;
using GalaSoft.MvvmLight.Command;
using SafeNotePad.ViewModels;

namespace SafeNotePad.Views.RichTextEditor
{
    class RichTextEditorViewModel : BaseViewModel
    {
        private string _text;
        private string _initialText;
        private bool _isNew;

        public RelayCommand CloseCommand { get; set; }

        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                RaisePropertyChanged(() => Text);
                OkCommand.RaiseCanExecuteChanged();
            }
        }

        public bool IsNew
        {
            get
            {
                return _isNew;
            }
            set
            {
                _isNew = value;
                RaisePropertyChanged(() => IsNew);
                RaisePropertyChanged(() => IsExistent);
                OkCommand.RaiseCanExecuteChanged();
            }
        }

        public bool IsExistent
        {
            get
            {
                return !IsNew;
            }
        }

        public RichTextEditorViewModel()
        {
            CloseCommand = new RelayCommand(OnCloseCommand);
            IsNew = true;
        }

        protected override bool CanOk()
        {
            return IsNew || (!string.IsNullOrEmpty(_initialText) && _initialText != Text);
        }

        protected override void OnOk()
        {
            if (IsExistent)
            {
                Save(Text);
            }
            else
            {
                Create(Text);
            }

            _initialText = Text;
        }

        private void OnCloseCommand()
        {
            Close();
        }

        public bool ProcessClose()
        {
            if (IsNew)
            {
                var result = MessageBox.Show((Window) ChildWindowView, "Do you really want to cancel and discard all changes?",
                    "Cancel", MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.OK);

                if (result == MessageBoxResult.Yes)
                {
                    return true;
                }
            }
            else if (_initialText != Text)
            {
                var result = MessageBox.Show((Window) ChildWindowView, "Do you want to save changes?",
                    "Close", MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.OK);

                if (result == MessageBoxResult.Yes)
                {
                    Save(Text);
                    return true;
                }
                else if (result == MessageBoxResult.No)
                {
                    return true;
                }
            }
            else
            {
                return true;
            }

            return false;
        }

        public void Loaded()
        {
            _initialText = Text;
        }

        public virtual void Save(string text)
        {
            
        }

        public virtual void Create(string text)
        {
            
        }
    }
}
