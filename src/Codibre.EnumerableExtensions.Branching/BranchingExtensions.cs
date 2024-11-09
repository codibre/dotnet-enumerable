using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Codibre.EnumerableExtensions.Branching.Internal;

namespace Codibre.EnumerableExtensions.Branching;

public static class BranchingExtensions
{
    public static BaseBranchingBuilder<T> Add<T, R>(this BaseBranchingBuilder<T> builder, Func<IAsyncEnumerable<T>, ValueTask<R>> branch, out BranchResult<R> result)
    {
        var refResult = result = new();
        return builder.Add(async (x) => refResult.Result = await branch(x).ConfigureAwait(false));
    }

    public static BaseBranchingBuilder<T> Add<T, R>(this BaseBranchingBuilder<T> builder, Func<IAsyncEnumerable<T>, ValueTask<R>> branch)
        => builder.Add(async (x) => await branch(x).AsTask().ConfigureAwait(false));
    public static BaseBranchingBuilder<T> Add<T, R>(this BaseBranchingBuilder<T> builder, Func<IAsyncEnumerable<T>, Task<R>> branch, out BranchResult<R> result)
    {
        var refResult = result = new();
        return builder.Add(async (x) => refResult.Result = await branch(x).ConfigureAwait(false));
    }

    public static BaseBranchingBuilder<T> Add<T, R>(this BaseBranchingBuilder<T> builder, Func<IAsyncEnumerable<T>, Task<R>> branch)
        => builder.Add(async (x) => await branch(x).ConfigureAwait(false));
}