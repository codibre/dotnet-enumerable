using System.Runtime.CompilerServices;
using System.Xml.XPath;

namespace Codibre.EnumerableExtensions.Branching.Internal;

internal sealed record AsyncBranchContext<T>(Func<IBranchContext<T>, ValueTask<LinkedNode<T>?>> GetNext)
    : IBranchContext<T>
{
    public ValueTask<LinkedNode<T>?> FillNext() => GetNext(this).ResolveAsync();
}