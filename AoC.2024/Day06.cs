namespace AoC2024;

public static class Day06
{
    [NoFancy]
    public static int Solve()
    {
        var grid = Input.GetLines(false).AsGridOfChars('\0');

        // initial position
        (Point start, char symbol) = grid.FirstOrDefault(c => c.Value == '^');

        // build path, get all points where the guard was
        BuildPath(grid);
        //ConsoleWriteGrid(grid);
        //SysConsole.ReadKey();

        var pathPoints = grid.Where(x => x.Value == 'X' && x.Point != start).Select(x => x.Point).ToList();
        WriteLine($"Points.Count = {pathPoints.Count}");

        var cellGrid = grid.Copy((x, c) => c switch { '#' => Cell.NewObstacle(), '.' => new Cell(), '^' => Cell.NewStart(), 'X' => new Cell(), _ => throw new NotImplementedException() });

        var count = 0;

        Point? previous = null;
        for (int i = 0; i < pathPoints.Count; i++)
        {
            // reset grid
            cellGrid.ForEach(c => c.Value.RemoveVisited());

            if (previous != null)
                cellGrid[previous.Value] = new Cell();
            cellGrid[start] = Cell.NewStart();

            // setup new obstacle
            var obstacle = pathPoints[i];
            cellGrid[obstacle] = Cell.NewObstacle();

            var isLoop = CheckLoop(cellGrid);

            count += isLoop ? 1 : 0;

            ConsoleWriteGrid(cellGrid);
            WriteLine(isLoop);

            previous = obstacle;

        }

        return count;
    }

    private static void ResetGrid(Grid<char> grid, Point start)
    {
        grid.ForEach(c => c.Value switch { 'X' or '|' or '-' or 'O' or '+' => '.', _ => c.Value });
        grid[start] = '^';
    }

    private static void ConsoleWriteGrid(Grid<char> grid)
    {
        Clear();
        WriteLine(new string('=', (int)grid.Width));
        grid.VisitConsole(c => c switch
        {
            'X' => ConsoleColor.Green,
            '.' => ConsoleColor.DarkGray,
            '^' => ConsoleColor.Red,
            '#' => ConsoleColor.Magenta,
            '*' => ConsoleColor.Red,
            _ => ConsoleColor.White
        });
        WriteLine(new string('=', (int)grid.Width));
    }

    private static void ConsoleWriteGrid(Grid<Cell> grid)
    {
        Clear();
        WriteLine(new string('=', (int)grid.Width));
        grid.VisitConsole(c =>
        {
            var color = c switch
            {
                { IsVisited: true } => ConsoleColor.Green,
                { IsEmpty: true } => ConsoleColor.DarkGray,
                { IsStart: true } => ConsoleColor.Red,
                { IsObstacle: true } => ConsoleColor.Magenta,
                _ => ConsoleColor.White
            };

            Write(c.ToString(), color);
        });        
        WriteLine(new string('=', (int)grid.Width));
    }

    /// <summary>
    /// Checks if the current grid is an infinite loop
    /// </summary>
    private static bool CheckLoop(Grid<Cell> grid)
    {
        // initial position
        (Point guard, Cell cell) = grid.FirstOrDefault(c => c.Value.IsStart);

        var dir = Grid8Direction.Up;
        do
        {
            // get and paint the leaving cell
            cell = grid[guard];
            cell.SetAsVisited(dir);

            // get next cell info
            var next = guard.Move(dir);
            if (!grid.Contains(next))
                return false;

            var nextCell = grid[next];

            if (nextCell.IsBlock)
            {
                dir = dir.Rotate90(1);
            }
            else if (nextCell.IsVisitedFrom(dir))
            {
                return true;
            }
            else
            {
                guard = next;
            }
        } while (grid.Contains(guard));

        return false;
    }

    [NoFancy]
    public static int Solve_Part1()
    {
        var grid = Input.GetLines(false).AsGridOfChars('\0');

        BuildPath(grid);

        grid.VisitConsole(c => c switch { 'X' => ConsoleColor.Green, '.' => ConsoleColor.DarkGray, '^' => ConsoleColor.Red, '#' => ConsoleColor.Magenta, _ => ConsoleColor.White });

        var count = grid.Count(x => x.Value == 'X');

        return count;
    }

    private static void BuildPath(Grid<char> grid)
    {
        (Point guard, char symbol) = grid.FirstOrDefault(c => c.Value == '^');
        var dir = Grid8Direction.Up;

        while (grid.Contains(guard))
        {
            var next = guard.Move(dir);
            var nextSymbol = grid[next];
            if (nextSymbol == '#')
            {
                dir = dir.Rotate90(1);
            }
            else if (nextSymbol == '.' || nextSymbol == '^')
            {
                grid[next] = 'X';
                guard = next;
            }
            else if (nextSymbol == 'X')
            {
                guard = next;
            }
            else if (nextSymbol == '\0')
            {
                guard = next;
            }
        }
    }

    private record Cell
    {
        public static Cell NewBlock() => new(16);
        public static Cell NewObstacle() => new(16 | 64);
        public static Cell NewStart() => new(32 | 1);

        private byte _value;


        public Cell()
        {
        }

        private Cell(byte value)
        {
            _value = value;
        }

        public Cell(Grid8Direction directions)
        {
            _value = AsFlag(directions);
        }

        public bool IsUp => HasFlag(1);
        public bool IsLeft => HasFlag(2);
        public bool IsDown => HasFlag(4);
        public bool IsRight => HasFlag(8);
        public bool IsBlock => HasFlag(16);
        public bool IsStart => HasFlag(32);
        public bool IsObstacle => HasFlag(64);

        public bool IsEmpty => _value == 0;

        public bool IsVisited => (_value & 15) != 0;

        public void RemoveVisited()
        {
            _value = (byte)(_value & 0b11110000);
        }

        public void RemoveObstacle() => SetFlag(64 | 16, false);

        public void SetAsVisited(Grid8Direction direction) => _value |= AsFlag(direction);

        public bool IsVisitedFrom(Grid8Direction direction) => (_value & AsFlag(direction)) != 0;

        private static byte AsFlag(Grid8Direction direction)
        {
            byte value = 0;
            if (direction.HasFlag(Grid8Direction.Up)) value |= 1;
            if (direction.HasFlag(Grid8Direction.Left)) value |= 2;
            if (direction.HasFlag(Grid8Direction.Down)) value |= 4;
            if (direction.HasFlag(Grid8Direction.Right)) value |= 8;
            return value;
        }


        private bool HasFlag(byte mask) => (_value & mask) != 0;


        private void SetFlag(byte mask, bool value = true)
        {
            if (value)
                _value |= mask;
            else
                _value &= (byte)~mask;
        }

        public override string ToString()
        {
            if (IsObstacle) return "●";

            if (IsUp)
            {
                if (IsLeft)
                {
                    if (IsDown)
                    {
                        if (IsRight)
                            return "╬";
                        else
                            return "■"; // ▲+◄+▼
                    }
                    else
                    {
                        if (IsRight)
                            return "■"; // ▲+◄+►
                        else
                            return "■"; // ▲+◄
                    }
                }
                else
                {
                    if (IsDown)
                    {
                        if (IsRight)
                            return "■"; // ▲+▼+►
                        else
                            return "║"; // ▲+▼
                    }
                    else
                    {
                        if (IsRight)
                            return "■"; // ▲+►
                        else
                            return "▲"; // ▲
                    }
                }
            }
            else
            {
                if (IsLeft)
                {
                    if (IsDown)
                    {
                        if (IsRight)
                            return "■"; // ◄+▼+►
                        else
                            return "■"; // ◄+▼
                    }
                    else
                    {
                        if (IsRight)
                            return "═"; // ◄+►
                        else
                            return "◄"; // ◄
                    }
                }
                else
                {
                    if (IsDown)
                    {
                        if (IsRight)
                        {
                            return "■"; // ▼+►
                        }
                        else
                        {
                            return "▼"; // ▼
                        }
                    }
                    else
                    {
                        if (IsRight)
                        {
                            return "►"; // ►
                        }
                        else
                        {
                            return "."; // .
                        }
                    }
                }
            }
        }
    }
}
