// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Eawv.Service.Utils;

public static class AsyncUtils
{
    private static readonly TaskFactory _taskFactory = new
        TaskFactory(
            CancellationToken.None,
            TaskCreationOptions.None,
            TaskContinuationOptions.None,
            TaskScheduler.Default);

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Reliability",
        "CA2008:Do not create tasks without passing a TaskScheduler",
        Justification = "Default task scheduler will be used, which is fine in this case.")]
    public static TResult RunSync<TResult>(Func<Task<TResult>> func)
        => _taskFactory
            .StartNew(func)
            .Unwrap()
            .GetAwaiter()
            .GetResult();

    public static async Task<IEnumerable<T>> WhereAsync<T>(this IEnumerable<T> items, Func<T, Task<bool>> predicate)
    {
        var itemTaskList = items.Select(item => new { Item = item, PredTask = predicate.Invoke(item) }).ToList();
        await Task.WhenAll(itemTaskList.Select(x => x.PredTask));
        return itemTaskList.Where(x => x.PredTask.Result).Select(x => x.Item);
    }
}
