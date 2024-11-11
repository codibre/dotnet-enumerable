using System.Runtime.CompilerServices;
using System.Xml.XPath;

namespace Codibre.EnumerableExtensions.Branching.Internal;

internal sealed record BranchContext<T>(Func<BranchContext<T>, ValueTask<LinkedNode<T>?>> GetNext, int _branchCount)
{
    private ushort _count = 0;
    private readonly ushort _limit = (ushort)int.Min(_branchCount << 14, ushort.MaxValue / 2);

    internal async ValueTask<LinkedNode<T>?> FillNext()
    {
        if (++_count > _limit)
        {
            _count = 0;
            await Task.Yield();
        }
        return await GetNext(this);
    }
}