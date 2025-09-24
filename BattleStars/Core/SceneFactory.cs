using System.Drawing;
using BattleStars.Logic;
using BattleStars.Shapes;
using BattleStars.Shots;
using BattleStars.Utility;
namespace BattleStars.Core;

public static class SceneFactory
{
    public static BattleStar CreatePlayerBattleStar(
        IShapeDrawer drawer,
        BoundaryChecker boundaryChecker)
    {
        // Player configuration
        var size = 50.0f;
        var initialHealth = 100f;
        var speed = 5f;
        var initialPosition = new PositionalVector2(100, 100);
        var shapeDescriptor = new ShapeDescriptor(ShapeType.Square, size, Color.Blue);
        var shotDirection = DirectionalVector2.UnitX; // Default direction to the right
        var shot = ShotFactory.CreateLaserShot;

        // Create player components
        var playershape = ShapeFactory.CreateShape(shapeDescriptor, drawer);
        var playerMovable = new PlayerMovable(initialPosition, speed, boundaryChecker);
        var playerDestructable = new BasicDestructable(initialHealth);
        var playerShooter = new BasicShooter(shot, shotDirection);

        // Create and return the player BattleStar
        var playerBattleStar = new BattleStar(
            playershape,
            playerMovable,
            playerDestructable,
            playerShooter
        );

        return playerBattleStar;
    }
}