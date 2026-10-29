using System;
using System.Collections.Generic;
using SafeNotePad.Models;

namespace SafeNotePad.Interfaces.Services
{
    public interface IDomainService
    {
        void SaveNote(Note note);

        Note LoadNote(string path);
    }
}