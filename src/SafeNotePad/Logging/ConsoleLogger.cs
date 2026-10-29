using System;

namespace SafeNotePad.Logging
{
    class ConsoleLogger : ILogger
    {
        private string _className;

        public ConsoleLogger(string className)
        {
            _className = className;
        }

        public void Info(string message)
        {
            Console.WriteLine("[" + DateTime.Now + "] " + _className + ": " + message);
        }

        public void Error(string message)
        {
            Console.WriteLine("[" + DateTime.Now + "] " + _className + ": " + message);
        }

        public void Error(Exception ex)
        {
            var message = ex.Message + " : " + ex.StackTrace;
            Console.WriteLine("[" + DateTime.Now + "] " + _className + ": " + message);
        }
    }
}
