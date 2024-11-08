using System.Collections;

namespace Codibre.EnumerableExtensions.Branching.Internal;

internal record BranchedEnumerator<T> : IAsyncEnumerator<T>
{
    private ILinkedNode<T> _node;
    public BranchedEnumerator(ILinkedNode<T> node) => _node = new LinkedNode<T>(default)
    {
        Next = node,
    };

    public T Current => _node.Value;
    public ValueTask DisposeAsync() => ValueTask.CompletedTask;

    public async ValueTask<bool> MoveNextAsync()
    {
        while (!_node.End && _node.Next is null) await Task.Yield();
        if (_node.Next is null) return false;
        _node = _node.Next;
        return true;
    }
}