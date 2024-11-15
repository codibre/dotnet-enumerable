using System.Runtime.CompilerServices;
using System.Xml.XPath;

namespace Codibre.EnumerableExtensions.Branching.Internal;

internal interface IBranchContext<T>
{
    ValueTask<LinkedNode<T>?> FillNext();
}