using System;

namespace SafeNotePad.Facade
{
    class ExecuteResult : ResultBase
    {
        public ExecuteResult OnSuccess(Action action)
        {
            if (!HasError)
            {
                action();
            }

            return this;
        }
    }
}
