using System;
using System.Collections.Generic;

namespace SafeNotePad.Facade
{
    class LoadResult<T> : ResultBase
    {
        public IList<T> Entities { get; set; }

        public LoadResult<T> OnSuccess(Action<IList<T>> action)
        {
            if (!HasError)
            {
                action(Entities);
            }

            return this;
        }
    }
}
