using System;
using System.IO;
using System.Reflection;

namespace SafeNotePad.Utils
{
    class Settings
    {
        public static string ApplicationAssemblyName { get; set; }

        public const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss.FFFF";

        public const string DefaultNoteText = "<html></html>";

        public static string SettingsFilePath
        {
            get
            {
                return AppFolder + "\\Settings.xml";
            }
        }

        public static string AppFolder
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppFolderName);
            }
        }

        public static string AppFolderName
        {
            get
            {
                return !IsDebug ? "SafeNotePad" : "SafeNotePad_Debug";
            }
        }

        public static string PathToLogFile
        {
            get
            {
                return Path.Combine(AppFolder, IsContextMenu ? "context_menu_log.txt" : "log.txt");
            }
        }

        public static string NoteExtension = "sfnote";

        public const string Version = "v 1.76";

        private static bool? _isContextMenu;

        public static bool IsContextMenu
        {
            get
            {
                if (!_isContextMenu.HasValue)
                {
                    _isContextMenu = Assembly.GetEntryAssembly() != Assembly.GetExecutingAssembly();
                }

                return _isContextMenu.Value;
            }
        }

#if DEBUG
        public static bool IsDebug = true;
#else
        public static bool IsDebug = false;
#endif
    }
}
