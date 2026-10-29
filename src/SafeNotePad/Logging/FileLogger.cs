using System;
using System.Diagnostics;
using System.IO;
using SafeNotePad.Utils;

namespace SafeNotePad.Logging
{
    public class FileLogger : ILogger
    {
        private static readonly string PathToLogFile = Settings.PathToLogFile;
        private string _className;
        private static TextWriterTraceListener _listener;

        static FileLogger()
        {
            try
            {
                var directory = Path.GetDirectoryName(PathToLogFile);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var t = File.AppendText(PathToLogFile);
                _listener = new TextWriterTraceListener(t);
                Trace.Listeners.Add(_listener);
                Trace.AutoFlush = true;
                Trace.Indent();
                Trace.WriteLine("Start logging SafeNotePad v. " + Settings.Version);
            }
            catch (Exception)
            {
                if (_listener != null && Trace.Listeners.Contains(_listener))
                {
                    Trace.Listeners.Remove(_listener);
                }
            }
        }

        public FileLogger(string className)
        {
            _className = className;
        }

        public void Info(string message)
        {
            Trace.WriteLine("[" + DateTime.Now + "] " + _className + ": " + message);
        }

        public void Error(string message)
        {
            Trace.WriteLine("[" + DateTime.Now + "] " + _className + ": " + message);
        }

        public void Error(Exception ex)
        {
            var message = ex.Message + " : " + ex.StackTrace;
            Trace.WriteLine("[" + DateTime.Now + "] " + _className + ": " + message);
        }
    }
}
