namespace Codibre.EnumerableExtensions.Branching.Internal;

internal sealed record LinkedNode<T>
{
    public T Value { get; }
    public Lazy<ValueTask<LinkedNode<T>?>>? Next { get; private set; }

    public LinkedNode(T value, IBranchContext<T> context)
    {
        Value = value;
        Next = new(context.FillNext, LazyThreadSafetyMode.ExecutionAndPublication);
    }

    private LinkedNode(T value) => Value = value;

    public static LinkedNode<T>? New(IEnumerator<T> source, BranchRunOptions options, IBranchContext<T> context)
    {
        if (!source.MoveNext()) return null;
        var preload = options.Limit;
        LinkedNode<T> root = new(source.Current);
        var node = root;
        while (preload-- > 0 && source.MoveNext()) node = (node.Next = new(ValueTask.FromResult<LinkedNode<T>?>(new(source.Current)))).Value.Result!;
        node.Next = new(context.FillNext, LazyThreadSafetyMode.ExecutionAndPublication);
        return root;
    }

    public static LinkedNode<T> Root(IEnumerator<T> source, BranchRunOptions options, IBranchContext<T> context)
        => new(default(T)!)
        {
            Next = new(ValueTask.FromResult(New(source, options, context)))
        };

    public static LinkedNode<T> Root(Func<IBranchContext<T>, ValueTask<LinkedNode<T>?>> getNext, BranchRunOptions options)
    {
        IBranchContext<T> context = options.Limit <= 1
            ? new AsyncBranchContext<T>(getNext)
            : new BranchContext<T>(getNext, options);
        return new(default!, context);
    }
}