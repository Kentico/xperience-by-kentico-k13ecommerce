namespace DancingGoat.Extensions;

public static class EnumerableExtensions
{
    public static async IAsyncEnumerable<T> AsAsyncEnumerable<T>(this IEnumerable<Task<T>> itemTasks)
    {
        foreach (var task in itemTasks)
        {
            yield return await task;
        }
    }
}
