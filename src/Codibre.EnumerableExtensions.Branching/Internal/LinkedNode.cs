namespace Codibre.EnumerableExtensions.Branching.Internal;

internal record LinkedNode<T>(T value)
{
    public T Value { get; } = value;
    public LinkedNode<T>? Next { get; set; } = null;
    public bool End { get; set; } = false;
}