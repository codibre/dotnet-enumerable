namespace Codibre.EnumerableExtensions.Branching.Internal;

internal static class TaskExtensions
{
    public static ValueTask<T> ResolveAsync<T>(this ValueTask<T> valueTask)
        => valueTask.IsCompletedSuccessfully
            ? new(Task.Run(() => valueTask!.Result))
            : valueTask;
}