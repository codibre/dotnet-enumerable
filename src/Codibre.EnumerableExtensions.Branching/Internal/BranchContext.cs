using System.Runtime.CompilerServices;
using System.Xml.XPath;

namespace Codibre.EnumerableExtensions.Branching.Internal;

internal sealed record BranchContext<T>(Func<BranchContext<T>, ValueTask<LinkedNode<T>?>> GetNext, int _branchCount)
{
    private ushort _count = 0;
    private readonly ushort _limit = 1024;

    internal ValueTask<LinkedNode<T>?> FillNext()
        => ++_count <= _limit ? GetNext(this) : GetYielded();

    private async ValueTask<LinkedNode<T>?> GetYielded()
    {
        _count = 0;
        await Task.Yield();
        return await GetNext(this);
    }
}