using SafeNotePad.Models;

namespace SafeNotePad.ViewModels.ProtectionItems
{
    class BasicProtectionItem : ProtectionItem
    {
        public AccessKey AccessKey { get; set; }

        public override AccessKey GetAccessKey()
        {
            return AccessKey;
        }
    }
}
