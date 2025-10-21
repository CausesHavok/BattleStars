using BattleStars.Application.Controllers;
using BattleStars.Application.Services;
using BattleStars.Domain.Interfaces;
using BattleStars.Core.Guards;
using BattleStars.Domain.Entities;

namespace BattleStars.Infrastructure.Factories;

public static class ControllerFactory
{
    public static GameController CreateGameController(
        IGameState gameState,
        IBoundaryChecker boundaryChecker,
        ICollisionChecker collisionChecker,
        IInputHandler inputHandler,
        IContext context)
    {
        Guard.NotNull(gameState, nameof(gameState));
        Guard.NotNull(inputHandler, nameof(inputHandler));
        Guard.NotNull(boundaryChecker, nameof(boundaryChecker));
        Guard.NotNull(collisionChecker, nameof(collisionChecker));
        Guard.NotNull(context, nameof(context));
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
            inputHandler,
            context);
    }
}