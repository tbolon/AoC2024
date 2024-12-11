namespace AoC;

public class ExtensionTests
{
    [Fact]
    public void Grid8Direction_Rotate90_Baseline()
    {
        Xunit.Assert.Equal(Grid8Direction.Left, Grid8Direction.Left.Rotate90(0));
        Xunit.Assert.Equal(Grid8Direction.Down, Grid8Direction.Down.Rotate90(0));
        Xunit.Assert.Equal(Grid8Direction.Right, Grid8Direction.Right.Rotate90(0));
        Xunit.Assert.Equal(Grid8Direction.Up, Grid8Direction.Up.Rotate90(0));
    }

    [Fact]
    public void Grid8Direction_Rotate90_Basic()
    {
        Xunit.Assert.Equal(Grid8Direction.Up, Grid8Direction.Left.Rotate90(1));
        Xunit.Assert.Equal(Grid8Direction.Left, Grid8Direction.Down.Rotate90(1));
        Xunit.Assert.Equal(Grid8Direction.Down, Grid8Direction.Right.Rotate90(1));
        Xunit.Assert.Equal(Grid8Direction.Right, Grid8Direction.Up.Rotate90(1));

        Xunit.Assert.Equal(Grid8Direction.Up, Grid8Direction.Left.Rotate90(5));
        Xunit.Assert.Equal(Grid8Direction.Left, Grid8Direction.Down.Rotate90(5));
        Xunit.Assert.Equal(Grid8Direction.Down, Grid8Direction.Right.Rotate90(5));
        Xunit.Assert.Equal(Grid8Direction.Right, Grid8Direction.Up.Rotate90(5));

        Xunit.Assert.Equal(Grid8Direction.Up, Grid8Direction.Left.Rotate90(-3));
        Xunit.Assert.Equal(Grid8Direction.Left, Grid8Direction.Down.Rotate90(-3));
        Xunit.Assert.Equal(Grid8Direction.Down, Grid8Direction.Right.Rotate90(-3));
        Xunit.Assert.Equal(Grid8Direction.Right, Grid8Direction.Up.Rotate90(-3));
    }
}