using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Codibre.EnumerableExtensions.Branching.Internal;

namespace Codibre.EnumerableExtensions.Branching;

public abstract class BaseBranchingBuilder<T>
{
    private readonly List<Func<IAsyncEnumerable<T>, Task>> _branches = [];

    internal BaseBranchingBuilder<T> Add(Func<IAsyncEnumerable<T>, Task> branch)
    {
        _branches.Add(branch);
        return this;
    }

    protected abstract IAsyncEnumerable<T> Source { get; }

    public async Task Run()
    {
        var enumerator = Source.GetAsyncEnumerator();
        ILinkedNode<T>? node = await enumerator.MoveNextAsync() ? new LinkedNode<T>(enumerator.Current) : null;
        var iterate = Iterate(node, enumerator);
        await Task.WhenAll(
            _branches
                .Select((x, index) => x(node.GetBranchedIterable()))
                .Append(iterate)
        );
    }

    private static async Task Iterate(ILinkedNode<T>? node, IAsyncEnumerator<T> enumerator)
    {
        if (node is null) return;
        try
        {
            while (await enumerator.MoveNextAsync()) node = node.Next = new LinkedNode<T>(enumerator.Current);
        }
        finally
        {
            node.End = true;
        }
    }
}