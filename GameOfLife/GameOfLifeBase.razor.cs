using Aptacode.BlazorCanvas;
using Microsoft.AspNetCore.Components;

namespace Timmoth.GameOfLife
{
    public class GameOfLifeBase : ComponentBase
    {
        protected BlazorCanvas Canvas { get; set; } = default!;

        [Parameter, EditorRequired]
        public int Width { get; set; }

        [Parameter, EditorRequired]
        public int Height { get; set; }

        private GameOfLifeSimulation gameOfLife = default!;
        public readonly int _cellSize = 10;
        protected override async Task OnInitializedAsync()
        {
            gameOfLife = new GameOfLifeSimulation(Width / _cellSize, Height / _cellSize);
            using var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(20));
            while (await timer.WaitForNextTickAsync())
            {
                if (!Canvas.Ready)
                {
                    continue;
                }

                Draw(Canvas);
                gameOfLife.Next();
            }
        }
        private void Draw(BlazorCanvas canvas)
        {
            Canvas.ClearRect(0, 0, Width, Height);

            // clear
            canvas.LineWidth(1);
            canvas.StrokeStyle("white");

            // Update Scene
            for (int i = 0; i < gameOfLife.Width; i++)
            {
                for (int j = 0; j < gameOfLife.Height; j++)
                {
                    var age = gameOfLife.Cells[i, j];
                    switch (age)
                    {
                        case 0:
                            continue;
                        case 1:
                            canvas.FillStyle("#d41515");
                            break;
                        case 2:
                            canvas.FillStyle("#d46515");
                            break;
                        case 3:
                            canvas.FillStyle("#d4c415");
                            break;
                        case 4:
                            canvas.FillStyle("#68d415");
                            break;
                        case 5:
                            canvas.FillStyle("#15d4a4");
                            break;
                        default:
                            canvas.FillStyle("#1558d4");
                            break;
                    }

                    canvas.FillRect(_cellSize * i, _cellSize * j, _cellSize, _cellSize);
                    canvas.StrokeRect(_cellSize * i, _cellSize * j, _cellSize, _cellSize);
                }
            }
        }
    }
}
