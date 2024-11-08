namespace Codibre.EnumerableExtensions.Branching.Internal;

internal interface ILinkedNode<T>
{
    public T Value { get; }
    public ILinkedNode<T>? Next { get; set; }
    public bool End { get; set; }
}