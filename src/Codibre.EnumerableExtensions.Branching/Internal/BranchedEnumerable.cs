using System.Collections;

namespace Codibre.EnumerableExtensions.Branching.Internal;

internal record BranchedEnumerable<T>(ILinkedNode<T> node) : IAsyncEnumerable<T>
{
    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        => new BranchedEnumerator<T>(node);
}