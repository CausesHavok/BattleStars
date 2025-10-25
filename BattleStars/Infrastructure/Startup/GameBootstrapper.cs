using BattleStars.Application.Checkers;
using BattleStars.Application.Services;
using BattleStars.Domain.Entities;
using BattleStars.Domain.Interfaces;
using BattleStars.Infrastructure.Adapters;
using BattleStars.Infrastructure.Factories;
using BattleStars.Presentation.Drawers;

namespace BattleStars.Infrastructure.Startup;

public class GameBootstrapper : IGameBootstrapper
{
    private readonly int _windowWidth;
    private readonly int _windowHeight;
    private readonly IShapeDrawer _shapeDrawer;

    public GameBootstrapper(int windowWidth, int windowHeight, IShapeDrawer shapeDrawer)
    {
        _windowWidth = windowWidth;
        _windowHeight = windowHeight;
        _shapeDrawer = shapeDrawer;
    }

    public BootstrapResult Initialize()
    {
        // Create player BattleStar
        var playerBattleStar = CreatePlayerBattleStar(_shapeDrawer);

        // Create some enemies
        var enemies = CreateEnemyBattleStars(_shapeDrawer);

        // Create context
        var context = CreateBasicContext();

        // Create input handler
        var inputHandler = CreateInputHandler();

        // Create boundary checker
        var boundaryChecker = CreateBoundaryChecker(_windowWidth, _windowHeight);

        // Create collision checker
        var collisionChecker = CreateCollisionChecker();

        // Create initial game state
        var gameState = CreateInitialGameState(
            context,
            playerBattleStar,
            enemies
        );

        // Create game controller
        var gameController = CreateGameController(
            gameState,
            boundaryChecker,
            collisionChecker,
            inputHandler
        );

        return new BootstrapResult(
            gameController,
            gameState,
            inputHandler,
            boundaryChecker,
            collisionChecker,
            _shapeDrawer
        );
    }

    protected virtual IBattleStar CreatePlayerBattleStar(IShapeDrawer drawer) => SceneFactory.CreatePlayerBattleStar(drawer);
    protected virtual List<IBattleStar> CreateEnemyBattleStars(IShapeDrawer drawer) => SceneFactory.CreateEnemyBattleStars(drawer);
    protected virtual IContext CreateBasicContext() => SceneFactory.CreateBasicContext();
    protected virtual IInputHandler CreateInputHandler() => new InputHandler(new RaylibKeyBoardProvider());
    protected virtual IBoundaryChecker CreateBoundaryChecker(int width, int height) => new BoundaryChecker(0, width, 0, height);
    protected virtual ICollisionChecker CreateCollisionChecker() => new CollisionChecker();
    protected virtual IGameState CreateInitialGameState(
        IContext context,
        IBattleStar playerBattleStar,
        List<IBattleStar> enemies) => new GameState(
            context,
            playerBattleStar,
            ShotFactory.CreateEmptyShotList(),
            enemies,
            ShotFactory.CreateEmptyShotList()
        );

    protected virtual IGameController CreateGameController(
        IGameState gameState,
        IBoundaryChecker boundaryChecker,
        ICollisionChecker collisionChecker,
        IInputHandler inputHandler
        ) => ControllerFactory.CreateGameController(
            gameState,
            boundaryChecker,
            collisionChecker,
            inputHandler
        );

}