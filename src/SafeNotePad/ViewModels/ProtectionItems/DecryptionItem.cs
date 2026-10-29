using SafeNotePad.Models.Crypto;

namespace SafeNotePad.ViewModels.ProtectionItems
{
    abstract class DecryptionItem : BaseViewModel
    {
        public abstract DecryptionType DecryptionType
        {
            get;
        }

        public abstract IDecryptionData GetDecryptionData();

        protected DecryptionItem()
        {
            EnableValidationHandlers();
        }

        public virtual void SetError(string error)
        {
            
        }
    }
}
