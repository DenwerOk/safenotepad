using System;

namespace SafeNotePad.Facade
{
    abstract class ResultBase
    {
        public Exception Error { get; set; }

        public bool HasError
        {
            get
            {
                return Error != null;
            }
        }

        public void OnError(Action<Exception> action)
        {
            if (HasError)
            {
                action(Error);
            }
        }
    }
}
