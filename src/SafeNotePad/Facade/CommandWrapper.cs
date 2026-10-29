using System;
using System.Collections.Generic;
using SafeNotePad.Logging;

namespace SafeNotePad.Facade
{
    static class CommandWrapper
    {
        private static ILogger _logger = LogManager.GetLogger("CommandWrapper");

        public delegate TResult Func<T, out TResult>(out T output);

        public static ExecuteResult Execute(Action action)
        {
            _logger.Info(action.Method.Name);

            var executeResult = new ExecuteResult();;

            try
            {
                action();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                executeResult.Error = ex;
            }

            return executeResult;
        }

        public static InvokeResult<T> Invoke<T>(Func<T> action)
        {
            _logger.Info(action.Method.Name);

            var invokeResult = new InvokeResult<T>();

            try
            {
                invokeResult.Value = action();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                invokeResult.Error = ex;
            }

            return invokeResult;
        }

        public static InvokeResult<T> Invoke<TParam, T>(Func<TParam, T> action, out TParam param)
        {
            _logger.Info(action.Method.Name);

            var invokeResult = new InvokeResult<T>();

            try
            {
                invokeResult.Value = action(out param);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                invokeResult.Error = ex;
                param = default(TParam);
            }

            return invokeResult;
        }

        public static LoadResult<T> Load<T>(Func<IList<T>> action)
        {
            _logger.Info(action.Method.Name);

            var loadResult = new LoadResult<T>();

            try
            {
                loadResult.Entities = action();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                loadResult.Error = ex;
            }

            return loadResult;
        }

        public static SubmitResult Submit(Action action)
        {
            _logger.Info(action.Method.Name);

            var submitResult = new SubmitResult();

            try
            {
                action();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                submitResult.Error = ex;
            }

            return submitResult;
        }
    }
}
