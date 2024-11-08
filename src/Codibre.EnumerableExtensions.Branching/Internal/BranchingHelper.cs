using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Codibre.EnumerableExtensions.Branching.Internal;

internal static class BranchingHelper
{
    internal static async IAsyncEnumerable<T> GetBranchedAsyncIterable<T>(this LinkedNode<T>? node, IAsyncEnumerator<T> enumerator, BranchContext context)
    {
        while (node is not null)
        {
            yield return node.Value;
            if (node.Next is null)
            {
                lock (enumerator)
                {
                    if (node.Next is null) context.Next ??= GetNext(enumerator, node, context);
                }
                if (context.Next is not null) await context.Next;
            }
            node = node.Next;
            await Task.Yield();
        }
    }

    internal static Task GetNext<T>(IAsyncEnumerator<T> enumerator, LinkedNode<T> node, BranchContext context)
        => Task.Run(async () =>
        {
            try
            {
                if (node.Next is null && await enumerator.MoveNextAsync()) node.Next = new(enumerator.Current);
                context.Next = null;
            }
            finally
            {
                context.Next = null;
            }
        });
}