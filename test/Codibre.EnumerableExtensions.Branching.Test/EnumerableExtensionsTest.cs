﻿using System.Linq;

namespace Codibre.EnumerableExtensions.Branching.Test;

[Collection("Tests")]
public class BranchingBuilderTest()
{
    private static IEnumerable<int> Op(IEnumerable<int> list) => list
            .Select(x => x * 2)
            .Select(x => x + 2)
            .Select(x => x / 2);

    [Fact]
    public async Task Should_Work_With_Perfectly_Balanced_Operations()
    {
        // Arrange
        int[] list = [1, 2, 3];
        var enumerable = Op(list);

        // Act
        await enumerable.Branch()
            .Add(x => x.ToArrayAsync(), out var a)
            .Add(x => x.ToArrayAsync(), out var b)
            .Add(x => x.ToArrayAsync(), out var c)
            .Run();

        // Assert
        a.Result.Should().BeEquivalentTo(enumerable.ToArray());
        b.Result.Should().BeEquivalentTo(enumerable.ToArray());
        c.Result.Should().BeEquivalentTo(enumerable.ToArray());
    }

    [Fact]
    public async Task Should_Work_With_Empty_Collections()
    {
        // Arrange
        int[] list = [];
        var enumerable = Op(list);

        // Act
        await enumerable.Branch()
            .Add(x => x.ToArrayAsync(), out var a)
            .Add(x => x.FirstOrDefaultAsync(), out var b)
            .Add(x => x.LastOrDefaultAsync(), out var c)
            .Run();

        // Assert
        a.Result.Should().BeEquivalentTo(enumerable.ToArray());
        b.Result.Should().Be(enumerable.FirstOrDefault());
        c.Result.Should().Be(enumerable.LastOrDefault());
    }

    [Fact]
    public async Task Should_Work_With_NonEmpty_Collections()
    {
        // Arrange
        int[] list = [1, 2, 2, 3, 3, 3];
        var enumerable = Op(list);

        // Act
        await enumerable.Branch()
            .Add(x => x.Distinct().ToArrayAsync(), out var a)
            .Add(x => x.MinAsync(), out var b)
            .Add(x => x.MaxAsync(), out var c)
            .Run();

        // Assert
        a.Result.Should().BeEquivalentTo(enumerable.Distinct().ToArray());
        b.Result.Should().Be(enumerable.Min());
        c.Result.Should().Be(enumerable.Max());
    }

    [Fact]
    public async Task Should_Intercalate_The_Steps_Between_Every_Branch()
    {
        // Arrange
        var total = 100000;
        var list = Enumerable.Range(0, total);
        List<int> steps = [];
        var enumerable = Op(list);

        // Act
        await enumerable.Branch()
            .Add(x => x.Select((x) =>
                {
                    steps.Add(1);
                    return x;
                }).ToArrayAsync(), out var a)
            .Add(x => x.Select((x) =>
                {
                    steps.Add(2);
                    return x;
                }).ToArrayAsync(), out var b)
            .Add(x => x.Select((x) =>
                {
                    steps.Add(3);
                    return x;
                }).ToArrayAsync(), out var c)
            .Run();

        // Assert
        var refValue = steps[0];
        steps.TakeWhile((x) => x == refValue).Count().Should().BeLessThan(total);
    }
}