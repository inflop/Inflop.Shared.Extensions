namespace Inflop.Shared.Extensions;

internal static class TaskExtensions
{
    /// <summary>
    /// Start the task and forget about it. Optionally you can pass an error handler to the method.
    /// This error handler will be called when the task throws an exception.
    /// </summary>
    /// <param name="task"></param>
    /// <param name="errorHandler"></param>
    /// <example>
    /// SendEmailAsync().FireAndForget(errorHandler => Console.WriteLine(errorHandler.Message));
    /// </example>
    public static void FireAndForget(this Task task, Action<Exception> errorHandler = null)
    {
        task.ContinueWith(t =>
        {
            if (t.IsFaulted && errorHandler != null)
                errorHandler(t.Exception);
        }, TaskContinuationOptions.OnlyOnFaulted);
    }

    /// <summary>
    /// Retry the task until it succeeds or the maximum number of retries is reached.
    /// You can pass a delay between retries. This delay will be used between each retry.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="taskFactory"></param>
    /// <param name="maxRetries"></param>
    /// <param name="delay"></param>
    /// <returns></returns>
    /// <example>
    /// var result = await (() => GetResultAsync()).Retry(3, TimeSpan.FromSeconds(1));
    /// </example>
    public static async Task<TResult> Retry<TResult>(this Func<Task<TResult>> taskFactory, int maxRetries, TimeSpan delay)
    {
        for (int i = 0; i < maxRetries; i++)
        {
            try
            {
                return await taskFactory().ConfigureAwait(false);
            }
            catch
            {
                if (i == maxRetries - 1)
                    throw;
                await Task.Delay(delay).ConfigureAwait(false);
            }
        }

        return default;
    }

    /// <summary>
    /// Executes a callback function when a Task encounters an exception.
    /// </summary>
    /// <param name="task"></param>
    /// <param name="onFailure"></param>
    /// <returns></returns>
    /// <example>
    /// await GetResultAsync().OnFailure(ex => Console.WriteLine(ex.Message));
    /// </example>
    public static async Task OnFailure(this Task task, Action<Exception> onFailure)
    {
        try
        {
            await task.ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            onFailure(ex);
        }
    }

    /// <summary>
    /// Method to set a timeout for a task.
    /// If the task takes longer than the timeout the task will be cancelled.
    /// </summary>
    /// <param name="task"></param>
    /// <param name="timeout"></param>
    /// <returns></returns>
    /// <exception cref="TimeoutException"></exception>
    /// <example>
    /// await GetResultAsync().WithTimeout(TimeSpan.FromSeconds(1));
    /// </example>
    public static async Task WithTimeout(this Task task, TimeSpan timeout)
        => await task.WaitAsync(timeout);

    /// <summary>
    /// Use this extension method to use a fallback value when a task fails.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="task"></param>
    /// <param name="fallbackValue"></param>
    /// <returns></returns>
    /// <example>
    /// var result = await GetResultAsync().Fallback("fallback");
    /// </example>
    public static async Task<TResult> Fallback<TResult>(this Task<TResult> task, TResult fallbackValue)
    {
        try
        {
            return await task.ConfigureAwait(false);
        }
        catch
        {
            return fallbackValue;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="task"></param>
    /// <returns></returns>
    public static T Await<T>(this Task<T> task)
        => task.GetAwaiter().GetResult();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="task"></param>
    public static void Await(this Task task)
        => task.GetAwaiter().GetResult();
}
