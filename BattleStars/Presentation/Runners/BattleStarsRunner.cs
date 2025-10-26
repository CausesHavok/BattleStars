using BattleStars.Infrastructure.Startup;
using BattleStars.Infrastructure.Adapters;
using BattleStars.Presentation.Renderers;
using BattleStars.Core.Guards;
using BattleStars.Domain.Interfaces;

namespace BattleStars.Presentation.Runners;

public class BattleStarsRunner(
    IFrameRenderer frameRenderer,
    IWindowConfiguration windowConfiguration,
    IGameBootstrapper gameBootstrapper,
    int windowWidth,
    int windowHeight
)
{
    private readonly IFrameRenderer _frameRenderer = Guard.NotNull(frameRenderer, nameof(frameRenderer));
    private readonly IWindowConfiguration _windowConfiguration = Guard.NotNull(windowConfiguration, nameof(windowConfiguration));
    private readonly IGameBootstrapper _gameBootstrapper = Guard.NotNull(gameBootstrapper, nameof(gameBootstrapper));
    private readonly int _windowWidth = windowWidth;
    private readonly int _windowHeight = windowHeight;

    private IGameController? _gameController;
    private bool _initialized;

    private const string Title = "BattleStars";
    private const int TargetFps = 60;

    private IGameController EnsureInitialized()
    {
        if (_initialized) return Guard.NotNull(_gameController, nameof(_gameController));

        _windowConfiguration.InitWindow(_windowWidth, _windowHeight, Title);
        _windowConfiguration.SetTargetFPS(TargetFps);

        var initResult = _gameBootstrapper.Initialize();
        _gameController = initResult.GameController;
        _initialized = true;

        return _gameController!;
    }

    /// <summary>
    /// Run a single frame. Returns true when the game signals it should continue; false if the game is finished.
    /// Does NOT close the window (caller controls lifecycle).
    /// </summary>
    public bool RunSingleFrame()
    {
        var gameController = EnsureInitialized();

        var shouldContinue = gameController.RunFrame();
        _frameRenderer.RenderFrame(gameController.GetFrameSnapshot());

        return shouldContinue;
    }

    /// <summary>
    /// Continuous run loop. Observes cancellationToken to exit gracefully.
    /// </summary>
    public void Run(CancellationToken cancellationToken = default)
    {
        var gameController = EnsureInitialized();
        bool shouldContinue = true;
        try
        {
            while (!_windowConfiguration.WindowShouldClose() && !cancellationToken.IsCancellationRequested)
            {
                if (shouldContinue)
                {
                    shouldContinue = gameController.RunFrame();
                }
                _frameRenderer.RenderFrame(gameController.GetFrameSnapshot());
            }
        }
        finally 
        {
            Cleanup();
        }
    }
    
    private void Cleanup()
    {
        if (_initialized)
        {
            _windowConfiguration.CloseWindow();
        }
    }

}