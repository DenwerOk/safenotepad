using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SafeNotePad.Models {
    
    public class Note : INotifyPropertyChanged {
        private bool _isModified;
        private AccessKey _accessKey;
        private string _path;

        public virtual string Created { get; set; }

        public virtual string Name
        {
            get
            {
                if (Path != null)
                {
                    return System.IO.Path.GetFileNameWithoutExtension(Path);
                }

                return "New Note";
            }
        }

        public virtual string KeyEncrypted { get; set; }
        public virtual string ValueEncrypted { get; set; }
        public virtual bool IsNew { get; set; }

        public bool IsAccessKeyExists
        {
            get { return AccessKey != null; }
        }

        public virtual string Path
        {
            get
            {
                return _path;
            }
            set
            {
                _path = value;
                OnPropertyChanged("Name");
            }
        }

        public virtual AccessKey AccessKey
        {
            get
            {
                return _accessKey;
            }
            set
            {
                _accessKey = value;
                OnPropertyChanged();
                OnPropertyChanged("IsAccessKeyExists");
            }
        }

        public virtual bool IsModified
        {
            get
            {
                return _isModified;
            }
            set
            {
                if (_isModified != value)
                {
                    _isModified = value;
                    OnPropertyChanged();
                }
            }
        }

        public virtual event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
