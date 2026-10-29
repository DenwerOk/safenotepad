using System;
using System.Collections.Generic;
using SafeNotePad.Models;
using SafeNotePad.Utils;

namespace SafeNotePad.Views.DummyViewModels
{
    public class DummyMainViewModel
    {
        private List<Models.Note> _noteList;

        public List<Models.Note> NoteList
        {
            get
            {
                return _noteList;
            }
            set
            {
                _noteList = value;
            }
        }

        public DummyMainViewModel()
        {
            NoteList = new List<Models.Note>
                           {
                               new Models.Note
                               {
                                   Path = @"C:\Note1.sfnote",
                                   Created = DateTime.Now.ToString(Settings.DateTimeFormat),
                                   AccessKey = new AccessKey
                                               {
                                                   Name = "Login Key"
                                               }
                               },
                               new Models.Note
                               {
                                   Path = @"C:\Note2.sfnote",
                                   Created = DateTime.Now.ToString(Settings.DateTimeFormat),
                                   AccessKey = new AccessKey
                                               {
                                                   Name = "Basic Key"
                                               }
                               },
                               new Models.Note
                               {
                                   Path = @"C:\Note45.sfnote",
                                   Created = DateTime.Now.ToString(Settings.DateTimeFormat),
                                   AccessKey = new AccessKey
                                               {
                                                   Name = "Custom Key"
                                               }
                               },
                               new Models.Note
                               {
                                   Path = @"C:\Noteaaa.sfnote",
                                   Created = DateTime.Now.ToString(Settings.DateTimeFormat),
                                   AccessKey = new AccessKey
                                               {
                                                   Name = "Custom Key"
                                               }
                               },
                               new Models.Note
                               {
                                   Path = @"C:\xsd.sfnote",
                                   Created = DateTime.Now.ToString(Settings.DateTimeFormat),
                                   AccessKey = new AccessKey
                                               {
                                                   Name = "Custom Key"
                                               }
                               },
                               new Models.Note
                               {
                                   Path = @"C:\new.sfnote",
                                   Created = DateTime.Now.ToString(Settings.DateTimeFormat),
                                   AccessKey = new AccessKey
                                               {
                                                   Name = "Custom Key"
                                               }
                               },
                               new Models.Note
                               {
                                   Path = @"C:\ret.sfnote",
                                   Created = DateTime.Now.ToString(Settings.DateTimeFormat),
                                   AccessKey = new AccessKey
                                               {
                                                   Name = "Custom Key"
                                               }
                               }
                           };
        }
    }
}