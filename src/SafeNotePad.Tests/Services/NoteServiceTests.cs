using System;
using Moq;
using NUnit.Framework;
using SafeNotePad.Interfaces.Services;
using SafeNotePad.Models;
using SafeNotePad.Models.Crypto;
using SafeNotePad.Services;
using SafeNotePad.Utils;

namespace SafeNotePad.Tests.Services
{
    [TestFixture]
    class NoteServiceTests
    {
        private Mock<IDomainService> _domainServiceMock;

        private Mock<IAccessKeyService> _accessKeyServiceMock;

        private NoteService GetNoteService()
        {
            Helper.IsTests = true;

            _domainServiceMock = new Mock<IDomainService>();
            _accessKeyServiceMock = new Mock<IAccessKeyService>();
            return new NoteService(_domainServiceMock.Object, _accessKeyServiceMock.Object);
        }

        [Test]
        public void EncryptDecryptNote()
        {
            var noteService = GetNoteService();

            _accessKeyServiceMock.Setup(
                accessKeyService => accessKeyService.Encrypt(Helper.TestKeyStr, It.IsAny<AccessKey>()))
                .Returns("UhhnFHRMp2hWp24frWRJ2kDeekPUQ6JVxXp4W8PAZj98o7SpnIE/gPFN6clqvJx/fRb6K7lQhSSn1QED5itAppbiihrtQLVuiikAFVQGLtLzArVzJZSNCFpvfXQTQJ+6GMUZ8rTp49mBTuuITnHfnXPs8gV1iKzUvWQm+qwB9/1vQ3BJVQXghya9dm7RBcltrd0HhHyFfUzy6d26prF6Z8XNqb1i6JCrim0wIAamED/4yZJhPzSZseHI4idoPohZfpvgNsNYfdtN6hhT19VelcNvtXG2F0juv3DwpyHK/reWt9TEYgXs2USrMshhw7sk6wydfnZ1UPc0a8K7aprBBA==");

            _accessKeyServiceMock.Setup(
                accessKeyService => accessKeyService.Decrypt("UhhnFHRMp2hWp24frWRJ2kDeekPUQ6JVxXp4W8PAZj98o7SpnIE/gPFN6clqvJx/fRb6K7lQhSSn1QED5itAppbiihrtQLVuiikAFVQGLtLzArVzJZSNCFpvfXQTQJ+6GMUZ8rTp49mBTuuITnHfnXPs8gV1iKzUvWQm+qwB9/1vQ3BJVQXghya9dm7RBcltrd0HhHyFfUzy6d26prF6Z8XNqb1i6JCrim0wIAamED/4yZJhPzSZseHI4idoPohZfpvgNsNYfdtN6hhT19VelcNvtXG2F0juv3DwpyHK/reWt9TEYgXs2USrMshhw7sk6wydfnZ1UPc0a8K7aprBBA==", It.IsAny<AccessKey>(), It.Is<PasswordDecryptionData>(pdd => pdd.Password == "x")))
                .Returns(Helper.TestKeyStr);


            const string content = @"k|tghjOO\_PPzi_XbJi`MAo";

            var note = new Note
                           {
                               AccessKey = new AccessKey()
                           };
            noteService.EncryptNoteAndSave(note, content);

            Assert.AreNotEqual(note.KeyEncrypted, Helper.TestKeyStr, "Key must be encrypted!");

            Assert.IsNotEmpty(note.KeyEncrypted, "Note must have a key after encryption.");
            Assert.IsNotEmpty(note.ValueEncrypted, "Note must have a value after encryption.");

            Assert.IsFalse(note.KeyEncrypted.Contains("RSAKeyValue"), "Note key should be encrypted!");
            Assert.IsFalse(note.ValueEncrypted.Contains("RSAKeyValue"), "Note value should be encrypted!");

            var decryptedContent = noteService.DecryptNote(note,
                new PasswordDecryptionData {Password = "x"});

            Assert.AreEqual(content, decryptedContent, "Note encryption process failed. Decryption doesn't match");
        }
    }
}
