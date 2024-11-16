using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Codibre.EnumerableExtensions.Branching.Internal;

namespace Codibre.EnumerableExtensions.Branching;

public sealed class BranchingBuilder<T>(IEnumerable<T> source) : BaseBranchingBuilder<T>
{
    internal override LinkedNode<T> Iterate(BranchRunOptions options)
    {
        var enumerator = source.GetEnumerator();
        var getNext = (IBranchContext<T> c) => ValueTask.FromResult(LinkedNode<T>.New(enumerator, options, c));
        BranchContext<T> context = new(getNext, BranchRunOptions.Yielder);
        return LinkedNode<T>.Root(enumerator, options, context);
    }
}