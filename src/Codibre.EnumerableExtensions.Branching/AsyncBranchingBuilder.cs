using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Codibre.EnumerableExtensions.Branching.Internal;

namespace Codibre.EnumerableExtensions.Branching;
public class AsyncBranchingBuilder<T>(IAsyncEnumerable<T> source) : BaseBranchingBuilder<T>
{
    internal override async ValueTask<(LinkedNode<T>?, Task)> Iterate()
    {
        var enumerator = source.GetAsyncEnumerator();
        var node = await enumerator.MoveLoose() ? new LinkedNode<T>(enumerator.Current) : null;
        return (node, node is null ? Task.CompletedTask : Task.Run(async () =>
        {
            try
            {
                while (await enumerator.MoveLoose()) node = node.Next = new LinkedNode<T>(enumerator.Current);
            }
            finally
            {
                node.End = true;
            }
        }));
    }
}