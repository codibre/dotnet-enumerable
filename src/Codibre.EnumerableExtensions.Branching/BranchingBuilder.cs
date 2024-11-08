using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Codibre.EnumerableExtensions.Branching.Internal;

namespace Codibre.EnumerableExtensions.Branching;

public class BranchingBuilder<T>(IEnumerable<T> source) : BaseBranchingBuilder<T>
{
    protected override IAsyncEnumerable<T> Source { get; } = source.ToAsyncEnumerable();
}