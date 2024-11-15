using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Codibre.EnumerableExtensions.Branching.Internal;

namespace Codibre.EnumerableExtensions.Branching;
public sealed class AsyncBranchingBuilder<T>(IAsyncEnumerable<T> source) : BaseBranchingBuilder<T>
{
    private static readonly LinkedNode<T>? _null = null;
    internal override LinkedNode<T> Iterate(BranchRunOptions options)
    {
        var enumerator = source.GetAsyncEnumerator();
        return LinkedNode<T>.Root(
            async (c) => await enumerator.MoveNextAsync() ? new(enumerator.Current, c) : _null,
            options
        );
    }
}