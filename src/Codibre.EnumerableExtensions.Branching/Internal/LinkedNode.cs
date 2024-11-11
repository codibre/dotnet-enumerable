namespace Codibre.EnumerableExtensions.Branching.Internal;

internal sealed record LinkedNode<T>(T Value, BranchContext<T> Context)
{
    public Lazy<ValueTask<LinkedNode<T>?>> Next { get; } = new(Context.FillNext, LazyThreadSafetyMode.ExecutionAndPublication);
}