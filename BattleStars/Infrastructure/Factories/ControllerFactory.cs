using BattleStars.Application.Controllers;
using BattleStars.Domain.Interfaces;
using BattleStars.Core.Guards;

namespace BattleStars.Infrastructure.Factories;

public static class ControllerFactory
{
    public static GameController CreateGameController(
        IGameState gameState,
        IBoundaryChecker boundaryChecker,
        ICollisionChecker collisionChecker,
        IInputHandler inputHandler
    )
    {
        Guard.NotNull(gameState);
        Guard.NotNull(inputHandler);
        Guard.NotNull(boundaryChecker);
        Guard.NotNull(collisionChecker);
        gameState.Validate();
        var playerController    = new PlayerController();
        var enemyController     = new EnemyController();
        var shotController      = new ShotController();
        var boundaryController  = new BoundaryController(boundaryChecker);
        var collisionController = new CollisionController(collisionChecker);

        return new GameController(
            gameState,
            playerController,
            enemyController,
            shotController,
            boundaryController,
            collisionController,
            inputHandler
        );
    }
}