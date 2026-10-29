using System;

namespace SafeNotePad
{
    class EventArgs<T> : EventArgs
    {
        public T Value { get; set; }

        public EventArgs(T value)
        {
            Value = value;
        }
    }
}
