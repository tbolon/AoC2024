namespace AoC;

[Flags]
public enum Grid8Direction : byte
{
    Undefined = 0,
    Up = 1,
    Left = 2,
    Down = 4,
    Right = 8,
    LeftUp = Left | Up,
    LeftDown = Left | Down,
    RightDown = Right | Down,
    RightUp = Right | Up
}

public static class Grid8DirectionExtensions
{
    public static Grid8Direction Invert(this Grid8Direction @this) => @this switch
    {
        Grid8Direction.Up => Grid8Direction.Down,
        Grid8Direction.Left => Grid8Direction.Right,
        Grid8Direction.Down => Grid8Direction.Up,
        Grid8Direction.Right => Grid8Direction.Left,
        Grid8Direction.LeftUp => Grid8Direction.RightDown,
        Grid8Direction.LeftDown => Grid8Direction.RightUp,
        Grid8Direction.RightDown => Grid8Direction.LeftUp,
        Grid8Direction.RightUp => Grid8Direction.LeftDown,
        _ => throw new ArgumentOutOfRangeException(nameof(@this), @this, "Not supported")
    };

    public static Grid8Direction Rotate90(this Grid8Direction @this, int count = 1)
    {
        if (count > 0)
        {
            while (count-- != 0)
            {
                @this = @this switch
                {
                    Grid8Direction.Up => Grid8Direction.Right,
                    Grid8Direction.Right => Grid8Direction.Down,
                    Grid8Direction.Down => Grid8Direction.Left,
                    Grid8Direction.Left => Grid8Direction.Up,
                    _ => throw new NotImplementedException($"Valeur {@this} non supportée")
                };
            }
        }
        else if (count < 0)
        {
            while (count++ != 0)
            {
                @this = @this switch
                {
                    Grid8Direction.Up => Grid8Direction.Left,
                    Grid8Direction.Left => Grid8Direction.Down,
                    Grid8Direction.Down => Grid8Direction.Right,
                    Grid8Direction.Right => Grid8Direction.Up,
                    _ => throw new NotImplementedException($"Valeur {@this} non supportée")
                };
            }
        }

        return @this;

    }
}