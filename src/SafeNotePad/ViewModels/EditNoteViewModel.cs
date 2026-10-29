using GalaSoft.MvvmLight.Command;
using SafeNotePad.Models;
using SafeNotePad.ViewModels.ProtectionItems;

namespace SafeNotePad.ViewModels
{
    class EditNoteViewModel : BaseViewModel
    {
        private ProtectionItem _passwordProtectionItem;
        private ProtectionItem _selectedProtectionItem;
        private DecryptionItem _selectedDecryptionItem;
        private AccessKey _accessKey;

        public ProtectionItem PasswordProtectionItem
        {
            get
            {
                return _passwordProtectionItem;
            }
            set
            {
                _passwordProtectionItem = value;
                RaisePropertyChanged(() => PasswordProtectionItem);
            }
        }

        public DecryptionItem SelectedDecryptionItem
        {
            get
            {
                return _selectedDecryptionItem;
            }
            set
            {
                _selectedDecryptionItem = value;
                RaisePropertyChanged(() => SelectedDecryptionItem);
            }
        }

        public RelayCommand<string> SelectProtectionItem { get; set; }

        public ProtectionItem SelectedProtectionItem
        {
            get
            {
                return _selectedProtectionItem;
            }
            set
            {
                _selectedProtectionItem = value;
                RaisePropertyChanged(() => SelectedProtectionItem);
                RaisePropertyChanged(() => IsTipEnabled);
            }
        }

        public AccessKey AccessKey
        {
            get
            {
                return _accessKey;
            }

            set
            {
                _accessKey = value;
                RaisePropertyChanged(() => AccessKey);
            }
        }

        public AccessKey NewAccessKey { get; set; }

        public bool IsTipEnabled
        {
            get
            {
                return SelectedProtectionItem != null;
            }
        }

        public EditNoteViewModel()
        {
            SelectProtectionItem = new RelayCommand<string>(OnSelectProtectionItem);
        }

        private void OnSelectProtectionItem(string type)
        {
            switch (type)
            {
                case "Password":
                {
                    SelectedProtectionItem = PasswordProtectionItem;
                    break;
                }
                default:
                {
                    break;
                }
            }
        }

        public override bool IsValid()
        {
            return SelectedProtectionItem.IsValid() & SelectedDecryptionItem.IsValid() & base.IsValid();
        }

        public void Initialize()
        {
            LoadProtectionItems();

            if (AccessKey != null)
            {
                SelectedDecryptionItem = new PasswordDecryptionItem();
            }
            else
            {
                SelectedDecryptionItem = new BasicDecryptionItem();
            }

            SelectedProtectionItem = PasswordProtectionItem;
        }

        private void LoadProtectionItems()
        {
            PasswordProtectionItem = new PasswordProtectionItem {Type = ProtectionType.Password};
        }

        protected override void OnOk()
        {
            if (AccessKey != null && !ServiceContext.IsDecryptionValid(SelectedDecryptionItem.GetDecryptionData(), AccessKey).Value)
            {
                SelectedDecryptionItem.SetError("Invalid value");
                return;
            }

            NewAccessKey = SelectedProtectionItem.GetAccessKey();

            CloseOk();
        }
    }
}
