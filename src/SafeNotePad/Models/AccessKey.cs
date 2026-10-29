using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Collections.Generic;
using SafeNotePad.Annotations;
using SafeNotePad.Models.Crypto;
using SafeNotePad.Models.Enum;


namespace SafeNotePad.Models {

    public class AccessKey : INotifyPropertyChanged
    {
        private int _encryptionTypeID;
        public virtual int AccessKeyID { get; set; }
        public virtual string Created { get; set; }
        public virtual bool IsLogin { get; set; }
        public virtual bool IsBasic { get; set; }
        public virtual string Name { get; set; }
        public virtual string PublicKey { get; set; }
        public virtual string PrivateKeyEncrypted { get; set; }
        public virtual string PrivateKeyEncryptedBackup { get; set; }
        public virtual string PrivateKeyBackupPasswordHash { get; set; }
        public virtual string Tip { get; set; }

        public virtual int EncryptionTypeID
        {
            get
            {
                return _encryptionTypeID;
            }
            set
            {
                _encryptionTypeID = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("EncryptionTypeID"));
                }
            }
        }

        public virtual bool IsTipExists
        {
            get
            {
                return Tip != null;
            }
        }

        public virtual string ProtectionType
        {
            get
            {
                if (IsBasic)
                {
                    return "Default";
                }

                if (EncryptionTypeID == (int) EncryptionType.Password)
                {
                    return "Password";
                }

                if (EncryptionTypeID == (int)EncryptionType.SecurityFile)
                {
                    return "Security File";
                }

                return null;
            }
        }

        public virtual IList<Note> Notes { get; set; }

        public virtual IEncryptionData EncryptionData { get; set; }

        public virtual event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
