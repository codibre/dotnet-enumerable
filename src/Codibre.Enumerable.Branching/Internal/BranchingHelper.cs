using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Codibre.Enumerable.Branching.Internal;

internal static class BranchingHelper
{
    internal static async IAsyncEnumerable<T> GetBranchedIterable<T>(this LinkedNode<T>? node, IEnumerator<T> enumerator)
    {
        while (node is not null)
        {
            yield return await Task.FromResult(node.Value);
            if (node.Next is null)
            {
                lock (enumerator)
                {
                    if (node.Next is null && enumerator.MoveNext())
                    {
                        node.Next = new(enumerator.Current);
                    }
                }
            }
            node = node.Next;
        }
    }

    internal static async IAsyncEnumerable<T> GetBranchedAsyncIterable<T>(this LinkedNode<T>? node, IAsyncEnumerator<T> enumerator, BranchContext context)
    {
        while (node is not null)
        {
            yield return node.Value;
            if (node.Next is null)
            {
                lock (enumerator) context.Next = GetNext(enumerator, node, context);
                await context.Next;
            }
            node = node.Next;
        }
    }

    internal static async Task GetNext<T>(IAsyncEnumerator<T> enumerator, LinkedNode<T> node, BranchContext context)
    {
        try
        {
            if (node.Next is null && await enumerator.MoveNextAsync()) node.Next = new(enumerator.Current);
        }
        finally
        {
            context.Next = null;
        }
    }
}