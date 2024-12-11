namespace AoC2024;

public static class Day09
{
    private const string charMap = @"0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

    public static long Solve()
    {
        var sample = false;
        var map = Input.GetInput(sample).Trim().ToArray();

        // file table
        var ft = new SortedList<int, File>();
        int x = 0;
        for (var i = 0; i < map.Length; i++)
        {
            var size = map[i] - '0';
            if (i % 2 == 0)
            {
                ft[x] = new File(i / 2, size);
            }

            x += size;
        }

        if (sample) WriteDisk_Part2(ft);

        // defrag files
        foreach ((int index, File file) in ft.Reverse().ToArray())
        {
            var spaceIndex = 0;
            foreach ((int locIndex, File locFile) in ft)
            {
                if (locIndex > index) break; // only move nearer

                var availableSpace = locIndex - spaceIndex;
                if (availableSpace >= file.Size)
                {
                    // found enough space, move file
                    ft.Remove(index);
                    ft.Add(spaceIndex, file);
                    break;
                }

                spaceIndex = locIndex + locFile.Size;
            }
        }

        if (sample) WriteDisk_Part2(ft);

        long score = 0;
        foreach ((int index, File file) in ft)
        {
            for (int i = 0; i < file.Size; i++)
            {
                score += (index + i) * file.Id;
            }
        }

        return score;
    }

    record File(int Id, int Size);

    private static void WriteDisk_Part2(SortedList<int, File> ft)
    {
        var fi = 0;
        var i = 0;
        foreach ((int index, (int fileId, int size)) in ft)
        {
            while (i < index)
            {
                SysConsole.Write('.');
                i++;
            }

            while (i < index + size)
            {
                SysConsole.Write(fileId % charMap.Length);
                i++;
            }

            fi++;
        }

        SysConsole.WriteLine();
    }

    public static long Solve_Part1()
    {
        var map = Input.GetInput(false).Trim().ToArray();

        // init disk. -1=free
        var disk = new int[200000];
        for (int i = 0; i < disk.Length; i++)
            disk[i] = -1;

        // file table => disk
        var freeBlocks = new List<int>();
        int x = 0;
        for (var i = 0; i < map.Length; i++)
        {
            var size = map[i] - '0';
            if (i % 2 == 0)
            {
                for (var j = 0; j < size; j++)
                {
                    disk[x + j] = i / 2;
                }
            }
            else
            {
                foreach (var fs in Enumerable.Range(x, size))
                {
                    freeBlocks.Add(fs);
                }
            }

            x += size;
        }

        Array.Resize(ref disk, x);

        // reorg
        var fb = 0;
        for (int i = disk.Length - 1; i >= 0; i--)
        {
            var fi = disk[i];
            if (fi != -1)
            {
                var fbi = freeBlocks[fb++];
                if (fbi > i)
                    break;
                Assert(disk[fbi] == -1);
                disk[fbi] = disk[i];
                disk[i] = -1;

                freeBlocks.Add(i);
                freeBlocks.Sort();
                //WriteDisk(disk);
            }
        }

        // compute CRC
        long score = 0;
        for (int i = 0; i < disk.Length; i++)
        {
            if (disk[i] > 0)
            {
                score += i * disk[i];
            }
        }

        return score;
    }

    private static void WriteDisk_Part1(int[] disk)
    {
        for (int i = 0; i < disk.Length; i++)
        {
            var x = disk[i];
            if (x == -1)
                SysConsole.Write('.');
            else
                SysConsole.Write(x % charMap.Length);
        }
        SysConsole.WriteLine();
    }
}
