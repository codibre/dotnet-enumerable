using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Codibre.EnumerableExtensions.Branching.Internal;

internal static class BranchingHelper
{
    internal static IAsyncEnumerable<T> GetBranchedIterable<T>(this ILinkedNode<T>? node)
        => node is null ? new List<T>().ToAsyncEnumerable() : new BranchedEnumerable<T>(node);
}