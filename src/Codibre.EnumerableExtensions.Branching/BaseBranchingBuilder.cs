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

    public async Task Run()
    {
        var node = Iterate(_branches.Count);
        await Task.WhenAll(_branches.Select(x => x(node.GetBranchedIterable())));
    }

    internal abstract LinkedNode<T> Iterate(int branchCount);
}