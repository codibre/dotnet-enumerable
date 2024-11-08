namespace Codibre.Enumerable.Branching.Test;

public class UnitTest1
{
    private static IEnumerable<int> Op(int[] list) => list
            .Select(x => x * 2)
            .Select(x => x + 2)
            .Select(x => x / 2);
    [Fact]
    public async Task Should_Branch_Results()
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
}
