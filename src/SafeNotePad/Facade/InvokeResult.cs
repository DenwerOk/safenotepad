using System;

namespace SafeNotePad.Facade
{
    class InvokeResult<T> : ResultBase
    {
        public T Value { get; set; }

        public InvokeResult<T> OnSuccess(Action<T> action)
        {
            if (!HasError)
            {
                action(Value);
            }

            return this;
        }
    }
}
