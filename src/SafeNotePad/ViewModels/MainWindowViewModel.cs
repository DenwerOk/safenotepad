using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using GalaSoft.MvvmLight.Command;
using SafeNotePad.Models;
using SafeNotePad.Services;
using SafeNotePad.Views;
using MessageBox = System.Windows.MessageBox;
using Settings = SafeNotePad.Utils.Settings;

namespace SafeNotePad.ViewModels
{
    class MainWindowViewModel : BaseViewModel
    {
       private IList<Note> _noteList = new List<Note>();
        private Note _selectedNote;
        private string _searchText;

        public RelayCommand NewNoteCommand { get; set; }
        public RelayCommand LoadNoteCommand { get; set; }
        public RelayCommand SaveNoteCommand { get; set; }
        public RelayCommand SaveNoteAsCommand { get; set; }
        public RelayCommand<Note> CloseNoteCommand { get; set; }
        public RelayCommand AboutCommand { get; set; }
        public RelayCommand<Note> SecureCommand { get; set; }

        public bool IsEditorPanelVisible
        {
            get
            {
                return SelectedNote != null;
            }
        }

        public Note SelectedNote
        {
            get
            {
                return _selectedNote;
            }
            set
            {
                _selectedNote = value;
                RaisePropertyChanged(() => SelectedNote);
                RaisePropertyChanged(() => IsEditorPanelVisible);
                SaveNoteCommand.RaiseCanExecuteChanged();
                SaveNoteAsCommand.RaiseCanExecuteChanged();
            }
        }

        public IList<Note> NoteList
        {
            get
            {
                var noteList = _noteList;
                if (noteList == null)
                {
                    return null;
                }

                if (!string.IsNullOrEmpty(SearchText))
                {
                    noteList = noteList.Where(
                        p =>
                            p.Name != null && p.Name.ToLowerInvariant().Contains(SearchText.ToLowerInvariant()))
                            .ToList();
                }

                return noteList.ToList();
            }
        }

        public string SearchText
        {
            get
            {
                return _searchText;
            }
            set
            {
                _searchText = value;
                RaisePropertyChanged(() => SearchText);
                RaisePropertyChanged(() => NoteList);
            }
        }

        public MainWindowViewModel()
        {
            NewNoteCommand = new RelayCommand(OnNewFile);
            LoadNoteCommand = new RelayCommand(OnLoadFile);
            SaveNoteCommand = new RelayCommand(OnSaveNote, CanSave);
            SaveNoteAsCommand = new RelayCommand(OnSaveNoteAs, CanSave);
            CloseNoteCommand = new RelayCommand<Note>(OnCloseNote);
            AboutCommand = new RelayCommand(OnAbout);
            SecureCommand = new RelayCommand<Note>(OnSecure);
            Caption = "Main Window";
        }

        private bool CanSave()
        {
            return SelectedNote != null;
        }

        private void OnSaveNoteAs()
        {
            ((MainWindow)View).Editor.ViewModel.SaveAsCommand.Execute(null);
        }

        private void OnSaveNote()
        {
            ((MainWindow)View).Editor.ViewModel.SaveCommand.Execute(null);
        }

        private void OnCloseNote(Note note)
        {
            if (note.IsModified)
            {
                var result = MessageBox.Show(string.Format("Save note {0}?", note.Name), "Save",
                    MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                {
                    var originallySelected = SelectedNote;
                    SelectedNote = note;
                    ((MainWindow) View).Editor.ViewModel.SaveCommand.Execute(null);
                    SelectedNote = originallySelected;

                    // Note was not saved
                    if (note.IsModified)
                    {
                        return;
                    }
                }

                if (result == MessageBoxResult.Cancel)
                {
                    return;
                }
            }

            _noteList.Remove(note);
            RaisePropertyChanged(() => NoteList);

            if (SelectedNote == null)
            {
                SelectedNote = _noteList.FirstOrDefault();
            }
        }

        private void OnNewFile()
        {
            NewNote();
        }

        public void NewNote()
        {
            var note = new Note
            {
                Created = DateTime.Now.ToString(Settings.DateTimeFormat),
                IsNew = true
            };

            AddNote(note);
        }

        private void OnLoadFile()
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = string.Format("Safe Note Files (*.{0})|*.{0}", Settings.NoteExtension);
            var dialogResult = dialog.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                LoadNote(dialog.FileName);
            }
        }

        public void LoadNote(string path)
        {
            var existentNote = _noteList.FirstOrDefault(note => note.Path == path);
            if (existentNote != null)
            {
                SelectedNote = existentNote;
            }
            else
            {
                var note = ServiceContext.LoadNote(path);
                if (note.HasError)
                {
                    ShowError("Couldn't load a file. File has invalid format.");
                }
                else
                {
                    AddNote(note.Value);
                }
            }
        }

        private void AddNote(Note note)
        {
            _noteList.Add(note);
            RaisePropertyChanged(() => NoteList);
            SelectedNote = note;
        }

        private void OnSecure(Note note)
        {
            var noteEditor = new EditNote
                             {
                                 ViewModel =
                                 {
                                     AccessKey = note.AccessKey
                                 }
                             };

            noteEditor.ViewModel.Initialize();
            var result = ShowDialog(noteEditor);
            if (result.GetValueOrDefault())
            {
                note.AccessKey = noteEditor.ViewModel.NewAccessKey;
                if (note.Path != null)
                {
                    ((MainWindow)View).Editor.ViewModel.SaveNote();
                }
            }
            
            // Rebind selected note to pick up the protection setup
            var selectedNote = SelectedNote;
            SelectedNote = null;
            SelectedNote = selectedNote;
        }

        private void OnAbout()
        {
            var about = new About();
            about.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            about.Owner = ChildWindowView as Window;
            about.ShowDialog();
        }

        public bool EnsureClose()
        {
            if (NoteList == null)
            {
                return true;
            }

            // Take selected note first and then follow other notes
            var noteList = NoteList.Where(note => note == SelectedNote).ToList();
            noteList.AddRange(NoteList.Where(note => note != SelectedNote));

            foreach (var note in noteList)
            {
                if (note.IsModified)
                {
                    SelectedNote = note;
                    var result = MessageBox.Show(string.Format("Save note {0}?", note.Name), "Save", MessageBoxButton.YesNoCancel);
                    if (result == MessageBoxResult.Yes)
                    {
                        ((MainWindow)View).Editor.ViewModel.SaveCommand.Execute(null);

                        // Note was not saved
                        if (note.IsModified)
                        {
                            return false;
                        }
                    }

                    if (result == MessageBoxResult.Cancel)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public void Initialize()
        {
            _noteList = new List<Note>();
            RaisePropertyChanged(() => NoteList);
        }
    }
}
