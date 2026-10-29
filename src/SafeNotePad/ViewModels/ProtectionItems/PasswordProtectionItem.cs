using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using SafeNotePad.Models;
using SafeNotePad.Models.Crypto;
using SafeNotePad.Models.Enum;
using SafeNotePad.Utils;

namespace SafeNotePad.ViewModels.ProtectionItems
{
    class PasswordProtectionItem : ProtectionItem
    {
        private string _newPassword;

        private string _confirmPassword;
        private string _tip;

        [Required(ErrorMessage = "Password value is required")]
        public string NewPassword
        {
            get
            {
                return _newPassword;
            }
            set
            {
                _newPassword = value;
                RaisePropertyChanged(() => NewPassword);
            }
        }

        [Required(ErrorMessage = "Confirm password value is required")]
        public string ConfirmPassword
        {
            get
            {
                return _confirmPassword;
            }
            set
            {
                _confirmPassword = value;
                RaisePropertyChanged(() => ConfirmPassword);
            }
        }

        public string Tip
        {
            get
            {
                return _tip;
            }
            set
            {
                _tip = value;
                RaisePropertyChanged(() => Tip);
            }
        }

        protected override string GetError(PropertyInfo info)
        {
            if (info.Name == Helper.GetPropertyName(() => ConfirmPassword) && NewPassword != ConfirmPassword)
            {
                return "Confirm password doesn't match";
            }

            return base.GetError(info);
        }

        public override AccessKey GetAccessKey()
        {
            var accessKey = new AccessKey
                            {
                                Name = "New Password",
                                Created = DateTime.Now.ToString(Settings.DateTimeFormat),
                                EncryptionTypeID = (int)EncryptionType.Password,
                                Tip = Tip
                            };

            ServiceContext.EncryptKey(
                accessKey, 
                new PasswordEncryptionData
                {
                    Password = NewPassword
                });

            return accessKey;
        }
    }
}
