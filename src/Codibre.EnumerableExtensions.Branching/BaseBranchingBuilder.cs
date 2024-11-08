using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Codibre.EnumerableExtensions.Branching.Internal;

namespace Codibre.EnumerableExtensions.Branching;

public abstract class BaseBranchingBuilder<T>
{
    private readonly List<Func<IAsyncEnumerable<T>, Task>> _branches = [];

    protected abstract IAsyncEnumerable<T> Source { get; }

    internal BaseBranchingBuilder<T> Add(Func<IAsyncEnumerable<T>, Task> branch)
    {
        _branches.Add(branch);
        return this;
    }

    public async Task Run()
    {
        var enumerator = Source.GetAsyncEnumerator();
        LinkedNode<T>? node = await enumerator.MoveNextAsync() ? new(enumerator.Current) : null;
        BranchContext context = new();
        await Task.WhenAll(_branches.Select((x, index) => Task.Run(() => x(node.GetBranchedAsyncIterable(enumerator, context)))).ToArray());
    }
}