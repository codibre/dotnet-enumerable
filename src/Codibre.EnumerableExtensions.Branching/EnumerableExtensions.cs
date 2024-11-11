﻿using Codibre.EnumerableExtensions.Branching.Internal;

namespace Codibre.EnumerableExtensions.Branching;

public static class EnumerableExtensions
{
    public static AsyncBranchingBuilder<T> Branch<T>(this IAsyncEnumerable<T> enumerable) => new(enumerable);
    public static BranchingBuilder<T> Branch<T>(this IEnumerable<T> enumerable) => new(enumerable);
}