using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Codibre.Enumerable.Branching.Internal;

namespace Codibre.Enumerable.Branching;

public class BranchingBuilder<T>(IEnumerable<T> source)
{
    private readonly List<Func<IAsyncEnumerable<T>, Task>> _branches = [];
    public BranchingBuilder<T> Add<R>(Func<IAsyncEnumerable<T>, ValueTask<R>> branch, out BranchResult<R> result)
    {
        var refResult = result = new();
        _branches.Add(async (x) => refResult.Result = await branch(x));
        return this;
    }

    public Task Run()
    {
        var enumerator = source.GetEnumerator();
        LinkedNode<T>? node = enumerator.MoveNext() ? new(enumerator.Current) : null;
        return Task.WhenAll(_branches.Select(x => x(node.GetBranchedIterable(enumerator))));
    }
}