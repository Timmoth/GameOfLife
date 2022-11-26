using Aptacode.BlazorCanvas;
using Microsoft.AspNetCore.Components;

namespace Timmoth.GameOfLife
{
    public class GameOfLifeBase : ComponentBase, IDisposable
    {
        protected BlazorCanvas Canvas { get; set; } = default!;

        [Parameter, EditorRequired]
        public int Width { get; set; }

        [Parameter, EditorRequired]
        public int Height { get; set; }
        [Parameter, EditorRequired]
        public RenderFragment ChildContent { get; set; } = default!;

        [Parameter] public int CellSize { get; set; } = 10;
        [Parameter] public float Density { get; set; } = 0.8f;
        [Parameter] public TimeSpan Delay { get; set; } = TimeSpan.FromMilliseconds(20);

        private GameOfLifeSimulation gameOfLife = default!;
        private readonly CancellationTokenSource _cancellationTokenSource = new();
        private bool _reset = false;
        protected override async Task OnInitializedAsync()
        {
            gameOfLife = new GameOfLifeSimulation(Width / CellSize, Height / CellSize, Density);
            using var timer = new PeriodicTimer(Delay);
            while (!_cancellationTokenSource.IsCancellationRequested && await timer.WaitForNextTickAsync(_cancellationTokenSource.Token))
            {
                if (!Canvas.Ready)
                {
                    continue;
                }

                if (_reset)
                {
                    _reset = false;
                    gameOfLife.Reset();
                }

                gameOfLife.Next();
                Draw(Canvas);
            }
        }
        private void Draw(BlazorCanvas canvas)
        {
            Canvas.ClearRect(0, 0, Width, Height);

            // clear
            canvas.LineWidth(1);
            canvas.StrokeStyle("white");

            // Update Scene
            for (var i = 0; i < gameOfLife.Width; i++)
            {
                for (var j = 0; j < gameOfLife.Height; j++)
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

                    canvas.FillRect(CellSize * i, CellSize * j, CellSize, CellSize);
                    canvas.StrokeRect(CellSize * i, CellSize * j, CellSize, CellSize);
                }
            }
        }

        public void Dispose()
        {
            _cancellationTokenSource.Dispose();
        }

        public void Reset()
        {
            _reset = true;
        }
    }
}
