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

    public void Run()
    {
        _windowConfiguration.InitWindow(800, 600, "BattleStars - Square Test");
        _windowConfiguration.SetTargetFPS(60);

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