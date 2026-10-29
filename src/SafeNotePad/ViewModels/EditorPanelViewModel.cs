using System.Windows;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using SafeNotePad.Facade;
using SafeNotePad.Models;
using SafeNotePad.Models.Crypto;
using SafeNotePad.Models.Enum;
using SafeNotePad.ViewModels.ProtectionItems;
using SafeNotePad.Views;
using Settings = SafeNotePad.Utils.Settings;

namespace SafeNotePad.ViewModels
{
    class EditorPanelViewModel : BaseViewModel
    {
        public RelayCommand SaveCommand { get; set; }

        public RelayCommand SaveAsCommand { get; set; }

        public RelayCommand AuthorizeCommand { get; set; }

        public RelayCommand<Note> SecureCommand
        {
            get
            {
                return _secureCommand;
            }
            set
            {
                _secureCommand = value;
                RaisePropertyChanged(() => SecureCommand);
            }
        }

        private EditorPanelSession _session;

        private readonly EditorPanelSessionManager _editorPanelSessionManager;
        private DecryptionItem _currentDecryptionItem;
        private RelayCommand<Note> _secureCommand;

        public DecryptionItem CurrentDecryptionItem
        {
            get
            {
                return _currentDecryptionItem;
            }
            set
            {
                _currentDecryptionItem = value;
                RaisePropertyChanged(() => CurrentDecryptionItem);
                RaisePropertyChanged(() => IsSecurityFileVisible);
                RaisePropertyChanged(() => IsPasswordVisible);
            }
        }

        public EditorPanelSession Session
        {
            get
            {
                return _session;
            }

            private set
            {
                _session = value;
                RaisePropertyChanged(() => Session);

                CurrentDecryptionItem = null;

                if (_session != null)
                {
                    if (_session.IsAuthorized)
                    {
                        CurrentDecryptionItem = null;
                    }
                    else if(_session.Note.AccessKey.EncryptionTypeID == (int)EncryptionType.Password)
                    {
                        CurrentDecryptionItem = new PasswordDecryptionItem();
                    }
                }
            }
        }

        public bool IsSecurityFileVisible
        {
            get
            {
                return CurrentDecryptionItem != null &&
                       CurrentDecryptionItem.DecryptionType == DecryptionType.SecurityFile;
            }
        }

        public bool IsPasswordVisible
        {
            get
            {
                return CurrentDecryptionItem != null &&
                       CurrentDecryptionItem.DecryptionType == DecryptionType.Password;
            }
        }

        public override bool IsValid()
        {
            if (CurrentDecryptionItem != null)
            {
                return base.IsValid() & CurrentDecryptionItem.IsValid();
            }

            return base.IsValid();
        }

        public EditorPanelViewModel()
        {
            SaveCommand = new RelayCommand(() => OnSave(false), CanSave);
            SaveAsCommand = new RelayCommand(OnSaveAs);
            AuthorizeCommand = new RelayCommand(OnAuthorize, CanAuthorize);
            _editorPanelSessionManager = new EditorPanelSessionManager();
        }

        private bool CanAuthorize()
        {
            return Session != null && !Session.IsAuthorized && CurrentDecryptionItem != null;
        }

        protected override ServiceContext CreateServiceContext()
        {
            return View.FindPage<MainWindow>().ViewModel.ServiceContext;
        }

        protected bool CanSave()
        {
            return _session != null && _session.InitialText != _session.Text;
        }

        private IDecryptionData GetDecryption()
        {
            if (CurrentDecryptionItem != null)
            {
                return CurrentDecryptionItem.GetDecryptionData();
            }

            return null;
        }

        private void OnAuthorize()
        {
            if (!IsValid())
            {
                return;
            }

            var decrypted = ServiceContext.DecryptNote(_session.Note, GetDecryption());
            if (decrypted.HasError)
            {
                SetValidationSummaryError(decrypted.Error.Message);
            }
            else
            {
                _session.IsAuthorized = true;
                _session.Text = decrypted.Value;
                _session.InitialText = decrypted.Value;
                CurrentDecryptionItem = null;
            }
        }

        public void Initialize(Note note)
        {
            ClearValidationSummaryErrors();

            var session = _editorPanelSessionManager.GetSession(note) ?? _editorPanelSessionManager.CreateSession(note);

            Session = session;

            SaveCommand.RaiseCanExecuteChanged();
        }

        protected void OnSave(bool saveAs)
        {
            if (_session.Note.AccessKey == null)
            {
                var noteEditor = new EditNote();

                noteEditor.ViewModel.Initialize();
                var result = ((MainWindow)Application.Current.MainWindow).ShowDialog(noteEditor);
                if (result.GetValueOrDefault())
                {
                    _session.Note.AccessKey = noteEditor.ViewModel.NewAccessKey;
                }
                else
                {
                    return;
                }
            }

            if (string.IsNullOrEmpty(_session.Note.Path) || saveAs)
            {
                var saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = string.Format("Safe Note Files (*.{0})|*.{0}", Settings.NoteExtension);
                var dialgResult = saveFileDialog.ShowDialog();
                if (dialgResult.GetValueOrDefault())
                {
                    _session.Note.Path = saveFileDialog.FileName;
                }
                else
                {
                    return;
                }
            }

            SaveNote();
        }

        public void SaveNote()
        {
            ServiceContext.EncryptNoteAndSave(_session.Note, _session.Text);

            _session.ResetAccessTime();

            _session.InitialText = _session.Text;
            SaveCommand.RaiseCanExecuteChanged();
        }

        private void OnSaveAs()
        {
            OnSave(true);
        }
    }
}
