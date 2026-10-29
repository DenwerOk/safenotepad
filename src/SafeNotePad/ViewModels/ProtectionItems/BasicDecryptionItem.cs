using SafeNotePad.Models;
using SafeNotePad.Models.Crypto;

namespace SafeNotePad.ViewModels.ProtectionItems
{
    class BasicDecryptionItem : DecryptionItem
    {
        public override DecryptionType DecryptionType
        {
            get { return DecryptionType.None; }
        }

        public override IDecryptionData GetDecryptionData()
        {
            return new BasicDecryptionData {PrivateKey = Session.CurrentSession.BasicKey.Value};
        }
    }
}
