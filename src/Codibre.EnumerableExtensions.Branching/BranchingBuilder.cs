using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Codibre.EnumerableExtensions.Branching.Internal;

namespace Codibre.EnumerableExtensions.Branching;

public class BranchingBuilder<T>(IEnumerable<T> source) : BaseBranchingBuilder<T>
{
    internal override ValueTask<(LinkedNode<T>?, Task)> Iterate()
    {
        var enumerator = source.GetEnumerator();
        var node = enumerator.MoveNext() ? new LinkedNode<T>(enumerator.Current) : null;
        return ValueTask.FromResult((node, node is null ? Task.CompletedTask : Task.Run(async () =>
        {
            try
            {
                while (enumerator.MoveNext()) node = node.Next = new LinkedNode<T>(enumerator.Current);
            }
            finally
            {
                node.End = true;
            }
        })));
    }
}