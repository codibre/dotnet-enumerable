﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Codibre.EnumerableExtensions.Branching.Internal;

namespace Codibre.EnumerableExtensions.Branching;

public sealed class BranchingBuilder<T>(IEnumerable<T> source) : BaseBranchingBuilder<T>
{
    internal override LinkedNode<T> Iterate(BranchRunOptions options)
    {
        var enumerator = source.GetEnumerator();
        return LinkedNode<T>.Root(
            (c) => new(enumerator.MoveNext() ? new LinkedNode<T>(enumerator.Current, c) : null),
            options
        );
    }
}