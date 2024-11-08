namespace Codibre.EnumerableExtensions.Branching.Internal;

internal record LinkedNode<T>(T Value)
{
    public LinkedNode<T>? Next { get; set; }
}
