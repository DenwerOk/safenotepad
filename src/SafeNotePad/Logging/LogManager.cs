namespace SafeNotePad.Logging
{
    class LogManager
    {
        public static ILogger GetLogger(string className)
        {
            return new FileLogger(className);
        }

        public static ILogger GetLogger(object o)
        {
            return GetLogger(o.GetType().Name);
        }
    }
}
