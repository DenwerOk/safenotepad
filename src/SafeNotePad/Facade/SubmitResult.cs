using System;

namespace SafeNotePad.Facade
{
    class SubmitResult : ResultBase
    {
        public SubmitResult OnSuccess(Action action)
        {
            if (!HasError)
            {
                action();
            }

            return this;
        }
    }
}
