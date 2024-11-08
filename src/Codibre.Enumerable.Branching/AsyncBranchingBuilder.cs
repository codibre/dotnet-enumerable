using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Codibre.Enumerable.Branching.Internal;

namespace Codibre.Enumerable.Branching;
public class AsyncBranchingBuilder<T>(IAsyncEnumerable<T> source)
{
    private readonly List<Func<IAsyncEnumerable<T>, Task>> _branches = [];
    public AsyncBranchingBuilder<T> Add<R>(Func<IAsyncEnumerable<T>, ValueTask<R>> branch, out BranchResult<R> result)
    {
        var refResult = result = new();
        _branches.Add(async (x) => refResult.Result = await branch(x));
        return this;
    }

    public async Task Run()
    {
        var enumerator = source.GetAsyncEnumerator();
        LinkedNode<T>? node = await enumerator.MoveNextAsync() ? new(enumerator.Current) : null;
        var context = new BranchContext();
        await Task.WhenAll(_branches.Select(x => x(node.GetBranchedAsyncIterable(enumerator, context))));
    }
}