using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Codibre.EnumerableExtensions.Branching.Internal;

internal static class BranchingHelper
{
    internal static async IAsyncEnumerable<T> GetBranchedIterable<T>(this LinkedNode<T> root)
    {
        var node = await root.Next.Value;
        while (node is not null)
        {
            yield return node.Value;
            node = await node.Next.Value;
        }
    }
}