namespace Codibre.EnumerableExtensions.Branching.Internal;

internal struct LinkedNode<T>(T value) : ILinkedNode<T>
{
    public T Value { get; } = value;
    public ILinkedNode<T>? Next { get; set; } = null;
    public bool End { get; set; } = false;
}