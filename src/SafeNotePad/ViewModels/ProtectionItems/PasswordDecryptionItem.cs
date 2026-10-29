using System.ComponentModel.DataAnnotations;
using System.Reflection;
using SafeNotePad.Models.Crypto;
using SafeNotePad.Utils;

namespace SafeNotePad.ViewModels.ProtectionItems
{
    class PasswordDecryptionItem : DecryptionItem
    {
        private string _password;
        private string _passwordValidationMessage;

        [Required(ErrorMessage = "Password is required")]
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                _passwordValidationMessage = null;
                RaisePropertyChanged(() => Password);
            }
        }

        public override DecryptionType DecryptionType
        {
            get { return DecryptionType.Password; }
        }

        public override IDecryptionData GetDecryptionData()
        {
            return new PasswordDecryptionData { Password = Password };
        }

        protected override string GetError(PropertyInfo info)
        {
            if (info.Name == Helper.GetPropertyName(() => Password) && _passwordValidationMessage != null)
            {
                return _passwordValidationMessage;
            }

            return base.GetError(info);
        }

        public override void SetError(string error)
        {
            _passwordValidationMessage = error;
            RaisePropertyChanged(() => Password);
        }
    }
}
