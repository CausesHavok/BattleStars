using BattleStars.Application.Controllers;
using BattleStars.Domain.Interfaces;
using BattleStars.Presentation.Drawers;

namespace BattleStars.Infrastructure.Startup;

public record BootstrapResult(
    IGameController GameController,
    IGameState GameState,
    IInputHandler InputHandler,
    IBoundaryChecker BoundaryChecker,
    ICollisionChecker CollisionChecker,
    IShapeDrawer ShapeDrawer,
    IContext Context
);