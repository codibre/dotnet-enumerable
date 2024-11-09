using System.Collections;

namespace Codibre.EnumerableExtensions.Branching.Internal;

internal record BranchedEnumerable<T>(LinkedNode<T> node) : IAsyncEnumerable<T>
{
    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        => new BranchedEnumerator<T>(node);
}