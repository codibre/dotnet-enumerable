using System.Collections;

namespace Codibre.EnumerableExtensions.Branching.Internal;

internal record BranchedEnumerator<T> : IAsyncEnumerator<T>
{
    private LinkedNode<T> _node;
    private readonly int _altenateOn;
    private int _count;
    public BranchedEnumerator(LinkedNode<T> node, int altenateOn = 100)
    {
        _node = new LinkedNode<T>(default)
        {
            Next = node,
        };
        _altenateOn = altenateOn;
        _count = altenateOn;
    }

    public T Current => _node.Value;
    public ValueTask DisposeAsync() => ValueTask.CompletedTask;

    public async ValueTask<bool> MoveNextAsync()
    {
        while (!_node.End && _node.Next is null) await Task.Yield();
        if (_node.Next is null) return false;
        _node = _node.Next;
        _count--;
        if (_count <= 0)
        {
            _count = _altenateOn;
            await Task.Yield();
        }
        return true;
    }
}