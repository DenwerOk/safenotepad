using System;

namespace SafeNotePad.Logging
{
    interface ILogger
    {
        void Info(string message);
        void Error(string message);
        void Error(Exception ex);
    }
}
