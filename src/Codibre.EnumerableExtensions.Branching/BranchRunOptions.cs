
namespace Codibre.EnumerableExtensions.Branching;

public readonly struct BranchRunOptions(int limit)
{
    public static readonly BranchRunOptions Default = new(ushort.MaxValue / 4);
    public int Limit => limit;
}