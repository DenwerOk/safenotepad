using System;
using SafeNotePad.BusinessLogic.CryptoProviders;
using SafeNotePad.BusinessLogic.CryptoProviders.Engine;
using SafeNotePad.Interfaces.Services;
using SafeNotePad.Models;
using SafeNotePad.Models.Crypto;

namespace SafeNotePad.Services
{
    class NoteService
    {
        private readonly IDomainService _domainService;

        private readonly IAccessKeyService _accessKeyService;

        public NoteService(IDomainService domainService, IAccessKeyService accessKeyService)
        {
            _domainService = domainService;
            _accessKeyService = accessKeyService;
        }

        public void EncryptNoteAndSave(Note note, string newNoteText)
        {
            EncryptNote(note, newNoteText);
            _domainService.SaveNote(note);
        }

        public void EncryptNote(Note note, string newNoteText)
        {
            var aes = new AESCryptoProvider();
            var key = Convert.ToBase64String(AESThenHMAC.NewKey());
            note.KeyEncrypted = _accessKeyService.Encrypt(key, note.AccessKey);
            note.ValueEncrypted = aes.Encrypt(newNoteText, new PasswordEncryptionData { Password = key });
        }

        public string DecryptNote(Note note, IDecryptionData data)
        {
            var key = _accessKeyService.Decrypt(note.KeyEncrypted, note.AccessKey, data);
            var aes = new AESCryptoProvider();
            return aes.Decrypt(note.ValueEncrypted, new PasswordDecryptionData { Password = key });
        }
    }
}
