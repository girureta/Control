using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Control
{
    /// <summary>
    /// Used to run tasks on the main thread
    /// </summary>
    public class ThreadHelper
    {
        private readonly TaskScheduler taskScheduler;

        public ThreadHelper(TaskScheduler taskScheduler)
        {
            this.taskScheduler = taskScheduler;
        }

        public Task<T> GetMainThreadTask<T>(Func<T> function)
        {
            var t = Task.Factory.StartNew<T>(
                () => CallFunctionWithTryCatch(function),
            CancellationToken.None,
            TaskCreationOptions.None,
            taskScheduler);
            return t;
        }

        public Task GetMainThreadTask(Action function)
        {
            var t = Task.Factory.StartNew(
                () => CallFunctionWithTryCatch(function),
            CancellationToken.None,
            TaskCreationOptions.None,
            taskScheduler);
            return t;
        }

        protected T CallFunctionWithTryCatch<T>(Func<T> function)
        {
            T response = default;

            try
            {
                response = function.Invoke();
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
            return response;
        }

        protected void CallFunctionWithTryCatch(Action function)
        {
            try
            {
                function.Invoke();
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }
    }
}

