namespace Codibre.EnumerableExtensions.Branching.Internal;

internal sealed record LinkedNode<T>(T Value, IBranchContext<T> Context)
{
    public Lazy<ValueTask<LinkedNode<T>?>> Next { get; } = new(Context.FillNext, LazyThreadSafetyMode.ExecutionAndPublication);

    public static LinkedNode<T> Root(Func<IBranchContext<T>, ValueTask<LinkedNode<T>?>> getNext, BranchRunOptions options)
    {
        IBranchContext<T> context = options.Limit <= 1
            ? new AsyncBranchContext<T>(getNext)
            : new BranchContext<T>(getNext, options);
        return new(default!, context);
    }
}