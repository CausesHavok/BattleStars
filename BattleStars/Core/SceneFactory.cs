using System.Drawing;
using BattleStars.Logic;
using BattleStars.Shapes;
using BattleStars.Shots;
using BattleStars.Utility;
namespace BattleStars.Core;

public static class SceneFactory
{
    public static BattleStar CreatePlayerBattleStar(IShapeDrawer drawer)
    {
        // Player configuration
        var size = 50.0f;
        var initialHealth = 100f;
        var speed = 5f;
        var initialPosition = new PositionalVector2(100, 100);
        var shapeDescriptor = new ShapeDescriptor(ShapeType.Square, size, Color.Blue);
        var shotDirection = DirectionalVector2.UnitX; // Default direction to the right
        var shot = ShotFactory.CreateLaserShot;
        var boundaryChecker = new BoundaryChecker(0 + 25, 800 - 25, 0 + 25, 600 - 25);

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

    public static List<BattleStar> CreateEnemyBattleStars(
        IShapeDrawer drawer)
    {
        var enemies = new List<BattleStar>();
        var rnd = new Random();
        var enemyCount = 10;

        for (int i = 0; i < enemyCount; i++)
        {
            // Enemy configuration
            var shapeDescriptor = new ShapeDescriptor(ShapeType.Circle, 30.0f, Color.Red);
            var shot = ShotFactory.CreateCannonShot;
            var initialHealth = 1f;
            var speed = 1f;
            var position = new PositionalVector2(rnd.Next(0, 800), rnd.Next(0, 600));
            var direction = -DirectionalVector2.UnitX; // Move left
            var shotDirection = -DirectionalVector2.UnitX; // Shoot left

            // Create enemy components
            var enemyShape = ShapeFactory.CreateShape(shapeDescriptor, drawer);
            var enemyMovable = new BasicMovable(position, direction, speed);
            var enemyDestructable = new BasicDestructable(initialHealth);
            var enemyShooter = new BasicShooter(shot, shotDirection);

            // Create and add the enemy BattleStar to the list
            var enemyBattleStar = new BattleStar(
                enemyShape,
                enemyMovable,
                enemyDestructable,
                enemyShooter
            );

            enemies.Add(enemyBattleStar);
        }

        return enemies;
    }

    public static BasicContext CreateBasicContext() => new();
}