using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Codibre.EnumerableExtensions.Branching.Internal;

namespace Codibre.EnumerableExtensions.Branching;

public sealed class BranchingBuilder<T>(IEnumerable<T> source) : BaseBranchingBuilder<T>
{
    internal override LinkedNode<T> Iterate(int branchCount)
    {
        var enumerator = source.GetEnumerator();
        return new LinkedNode<T>(default!, new(
            (c) => ValueTask.FromResult<LinkedNode<T>?>(enumerator.MoveNext() ? new(enumerator.Current, c) : null),
            branchCount
        ));
    }
}