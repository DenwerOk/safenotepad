using SafeNotePad.Interfaces.Services;
using SafeNotePad.Logging;
using SafeNotePad.Models;

namespace SafeNotePad.Services
{
    class DomainService : IDomainService
    {
        private static ILogger _logger = LogManager.GetLogger("DomainService");

        public void SaveNote(Note note)
        {
            var noteEntity = MapNoteToEntity(note);
            noteEntity.SaveToFile(note.Path);
        }

        public Note LoadNote(string path)
        {
            _logger.Info(string.Format("Loading note: {0}", path));
            var noteEntity = NoteEntity.LoadFromFile(path);
            var note = MapEntityToNote(noteEntity);
            note.Path = path;
            return note;
        }

        private NoteEntity MapNoteToEntity(Note note)
        {
            return new NoteEntity
                   {
                       KeyEncrypted = note.KeyEncrypted,
                       ValueEncrypted = note.ValueEncrypted,
                       AccessKeyEntity = new NoteEntityAccessKeyEntity()
                                         {
                                             PrivateKeyEncrypted = note.AccessKey.PrivateKeyEncrypted,
                                             PublicKey = note.AccessKey.PublicKey,
                                             EncryptionTypeID = note.AccessKey.EncryptionTypeID,
                                             Tip = note.AccessKey.Tip
                                         }
                   };
        }

        private Note MapEntityToNote(NoteEntity noteEntity)
        {
            return new Note
            {
                KeyEncrypted = noteEntity.KeyEncrypted,
                ValueEncrypted = noteEntity.ValueEncrypted,
                AccessKey = new AccessKey
                            {
                                PrivateKeyEncrypted = noteEntity.AccessKeyEntity.PrivateKeyEncrypted,
                                PublicKey = noteEntity.AccessKeyEntity.PublicKey,
                                EncryptionTypeID = noteEntity.AccessKeyEntity.EncryptionTypeID,
                                Tip = noteEntity.AccessKeyEntity.Tip
                            }
            };
        }
    }
}
