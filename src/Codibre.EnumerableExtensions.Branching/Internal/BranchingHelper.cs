using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Codibre.EnumerableExtensions.Branching.Internal;

internal static class BranchingHelper
{
    internal static IAsyncEnumerable<T> GetBranchedIterable<T>(this LinkedNode<T>? node)
        => node is null ? Array.Empty<T>().ToAsyncEnumerable() : new BranchedEnumerable<T>(node);

    internal static ConfiguredValueTaskAwaitable<bool> MoveLoose<T>(this IAsyncEnumerator<T> enumerator)
        => enumerator.MoveNextAsync().ConfigureAwait(false);
}