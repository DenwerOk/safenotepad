using System;
using System.Collections.Generic;
using SafeNotePad.Models;
using SafeNotePad.Models.Crypto;
using SafeNotePad.Services;

namespace SafeNotePad.Facade
{
    class ServiceContext
    {
        private readonly DomainService _domainService;
        private readonly NoteService _noteService;
        private readonly AccessKeyService _accessKeyService;

        public event EventHandler<EventArgs<Exception>> ErrorHandler;

        public ServiceContext()
        {
            _domainService = new DomainService();
            _accessKeyService = new AccessKeyService(_domainService);
            _noteService = new NoteService(_domainService, _accessKeyService);
        }

        public ServiceContext(DomainService domainService, AccessKeyService accessKeyService,
            NoteService noteService)
        {
            _domainService = domainService;
            _accessKeyService = accessKeyService;
            _noteService = noteService;
        }

        #region Core

        public ServiceContext Clone()
        {
            return new ServiceContext(_domainService, _accessKeyService, _noteService);
        }

        protected virtual ExecuteResult Execute(Action action)
        {
            var result = CommandWrapper.Execute(action);

            if (ErrorHandler != null && result.HasError)
            {
                ErrorHandler(this, new EventArgs<Exception>(result.Error));
            }

            return result;
        }

        protected virtual InvokeResult<T> Invoke<T>(Func<T> action)
        {
            var result = CommandWrapper.Invoke(action);

            if (ErrorHandler != null && result.HasError)
            {
                ErrorHandler(this, new EventArgs<Exception>(result.Error));
            }

            return result;
        }

        protected virtual InvokeResult<T> Invoke<TParam, T>(out TParam param, CommandWrapper.Func<TParam, T> action)
        {
            var result = CommandWrapper.Invoke(action, out param);

            if (ErrorHandler != null && result.HasError)
            {
                ErrorHandler(this, new EventArgs<Exception>(result.Error));
            }

            return result;
        }

        protected virtual LoadResult<T> Load<T>(Func<IList<T>> action)
        {
            var result = CommandWrapper.Load(action);

            if (ErrorHandler != null && result.HasError)
            {
                ErrorHandler(this, new EventArgs<Exception>(result.Error));
            }

            return result;
        }

        #endregion

        public ExecuteResult SaveNote(Note note)
        {
            return Execute(() => _domainService.SaveNote(note));
        }

        public InvokeResult<Note> LoadNote(string path)
        {
            return Invoke(() => _domainService.LoadNote(path));
        }

        public ExecuteResult EncryptNoteAndSave(Note note, string newNoteText)
        {
            return
                Execute(
                    () => _noteService.EncryptNoteAndSave(note, newNoteText));
        }

        public InvokeResult<string> DecryptNote(Note note, IDecryptionData data)
        {
            return Invoke(() => _noteService.DecryptNote(note, data));
        }

        public ExecuteResult EncryptKey(AccessKey key, IEncryptionData data)
        {
            return
                Execute(
                    () => _accessKeyService.EncryptKey(key, data));
        }

        public InvokeResult<bool> IsDecryptionValid(IDecryptionData data, AccessKey accessKey)
        {
            return Invoke(() => _accessKeyService.IsDecryptionValid(data, accessKey));
        }

        public InvokeResult<string> GenerateSecurityFile()
        {
            return Invoke(() => _accessKeyService.GenerateSecurityFile());
        }

        public InvokeResult<string> ValidateSecurityFile(string path)
        {
            return Invoke(() => _accessKeyService.ValidateSecurityFile(path));
        }
    }
}
