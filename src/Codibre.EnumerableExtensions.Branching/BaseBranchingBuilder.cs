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
        var (node, iterate) = await Iterate().ConfigureAwait(false);
        await Task.WhenAll(
            _branches
                .Select((x, index) => x(node.GetBranchedIterable()))
                .Append(iterate)
        ).ConfigureAwait(false);
    }

    internal abstract ValueTask<(LinkedNode<T>?, Task)> Iterate();
}