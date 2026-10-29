using SafeNotePad.Models;

namespace SafeNotePad.ViewModels.ProtectionItems
{
    class ProtectionItem : BaseViewModel
    {
        public ProtectionType Type { get; set; }

        protected ProtectionItem()
        {
            EnableValidationHandlers();
        }

        public virtual AccessKey GetAccessKey()
        {
            return null;
        }
    }
}
