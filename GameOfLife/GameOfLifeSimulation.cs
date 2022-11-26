namespace Timmoth.GameOfLife;

public class GameOfLifeSimulation
{
    public int Width { get; }
    public int Height { get; }
    public float Density { get; set; }
    public int[,] Cells { get; private set; }

    private readonly Random _rnd = new();
    public GameOfLifeSimulation(int width, int height, float density)
    {
        Width = width;
        Height = height;
        Density = density;

        Cells = new int[Width, Height];
        Reset();
    }

    public void Reset()
    {
        for (var i = 0; i < Width; i++)
        {
            for (var j = 0; j < Height; j++)
            {
                if (_rnd.NextSingle() <= Density)
                    Cells[i, j] = 0;
                else
                    Cells[i, j] = 1;
            }
        }
    }

    private int LiveNeighbours(int x, int y)
    {
        var liveNeighbours = 0;
        for (var i = -1; i <= 1; i++)
        {
            for (var j = -1; j <= 1; j++)
            {
                if (x + i < 0 || x + i >= Width)
                    continue;
                if (y + j < 0 || y + j >= Height)
                    continue;
                if (x + i == x && y + j == y)
                    continue;

                liveNeighbours += Cells[x + i, y + j] >= 1 ? 1 : 0;
            }
        }

        return liveNeighbours;
    }

    public void Next()
    {
        var cells = new int[Width, Height];

        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                var liveNeighbours = LiveNeighbours(x, y);
                var age = Cells[x, y];

                if (liveNeighbours == 3)
                {
                    cells[x, y] = age + 1;
                }else if (liveNeighbours == 2)
                {
                    cells[x, y] = age;
                }
                else
                {
                    cells[x, y] = 0;
                }
            }
        }

        Cells = cells;
    }
}