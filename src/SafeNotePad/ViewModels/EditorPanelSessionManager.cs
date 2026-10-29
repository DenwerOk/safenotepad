using System.Collections.Generic;
using System.Linq;
using SafeNotePad.Models;

namespace SafeNotePad.ViewModels
{
    class EditorPanelSessionManager
    {
        private readonly List<EditorPanelSession> _sessions = new List<EditorPanelSession>();

        public EditorPanelSession CreateSession(Note note)
        {
            var s = new EditorPanelSession
            {
                Note = note,
                IsAuthorized = note.IsNew
            };

            _sessions.Add(s);

            return s;
        }

        public EditorPanelSession GetSession(Note note)
        {
            var existingSession = _sessions.FirstOrDefault(session => session.Note == note);
            if (existingSession != null)
            {
                return existingSession;
            }

            return null;
        }
    }
}
