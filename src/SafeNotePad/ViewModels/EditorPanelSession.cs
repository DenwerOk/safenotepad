using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SafeNotePad.Models;

namespace SafeNotePad.ViewModels
{
    class EditorPanelSession : INotifyPropertyChanged
    {
        private bool _isAuthorized;
        private string _text;
        private string _initialText;
        private Note _note;

        public Note Note
        {
            get
            {
                return _note;
            }
            set
            {
                _note = value;
                OnPropertyChanged();
            }
        }

        public string InitialText
        {
            get
            {
                return _initialText;
            }
            set
            {
                _initialText = value;
                Note.IsModified = InitialText != Text;
            }
        }

        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                ResetAccessTime();
                OnPropertyChanged();
                Note.IsModified = InitialText != Text;
            }
        }

        public bool IsAuthorized
        {
            get
            {
                return _isAuthorized;
            }
            set
            {
                _isAuthorized = value;
                ResetAccessTime();
                OnPropertyChanged();
            }
        }

        public EditorPanelSessionSelection EditorPanelSessionSelection {get; set; }

        public DateTime AccessTime { get; set; }

        public EditorPanelSession()
        {
            ResetAccessTime();
        }

        public void ResetAccessTime()
        {
            AccessTime = DateTime.Now;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
