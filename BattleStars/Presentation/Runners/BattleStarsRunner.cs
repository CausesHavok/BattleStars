using BattleStars.Infrastructure.Startup;
using BattleStars.Infrastructure.Adapters;
using BattleStars.Presentation.Renderers;
using BattleStars.Core.Guards;

namespace BattleStars.Presentation.Runners;

public class BattleStarsRunner(IFrameRenderer frameRenderer, IWindowConfiguration windowConfiguration, IGameBootstrapper gameBootstrapper)
{
    private readonly IFrameRenderer _frameRenderer = Guard.NotNull(frameRenderer, nameof(frameRenderer));
    private readonly IWindowConfiguration _windowConfiguration = Guard.NotNull(windowConfiguration, nameof(windowConfiguration));
    private readonly IGameBootstrapper _gameBootstrapper = Guard.NotNull(gameBootstrapper, nameof(gameBootstrapper));
    private const string Title = "BattleStars";
    private const int TargetFps = 60;
    private const int WindowWidth = 800;
    private const int WindowHeight = 600;

    public void Run()
    {
        _windowConfiguration.InitWindow(WindowWidth, WindowHeight, Title);
        _windowConfiguration.SetTargetFPS(TargetFps);

        var gameController = _gameBootstrapper.Initialize().GameController;
        var shouldContinue = true;

        while (!_windowConfiguration.WindowShouldClose())
        {
            if (shouldContinue) shouldContinue = gameController.RunFrame();
            _frameRenderer.RenderFrame(gameController.GetFrameSnapshot());
        }
        _windowConfiguration.CloseWindow();
    }
}