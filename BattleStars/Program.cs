using Raylib_cs;
using System.Numerics;
using BattleStars;
using BattleStars.Shapes;
using BattleStars.Logic;
using BattleStars.Shots;
using BattleStars.Core;

Raylib.InitWindow(800, 600, "BattleStars - Square Test");
Raylib.SetTargetFPS(60);
var drawer = new RaylibShapeDrawer();

// Create boundary checker for the window (player stays fully inside)
var boundaryChecker = new BoundaryChecker(0 + 25, 800 - 25, 0 + 25, 600 - 25); // 50 is the player size

// Create a shape factory
var shapeFactory = new ShapeFactory();


// Create player BattleStar
var playershape = shapeFactory.CreateShape(new ShapeDescriptor
{
    ShapeType = ShapeType.Square,
    Scale = 50.0f,
    Color = System.Drawing.Color.Blue
});

var playerMovable = new PlayerMovable(new Vector2(100, 100), 5f, boundaryChecker);

var playerDestructable = new BasicDestructable(100f);

var playerShooter = new BasicShooter(ShotFactory.CreateLaserShot, new Vector2(1, 0)); // Default direction to the right

var playerBattleStar = new BattleStar(
    playershape,
    drawer,
    playerMovable,
    playerDestructable,
    playerShooter
);


// Create some enemies
var enemies = new List<BattleStar>();
int enemyCount = 5;

for (int i = 0; i < enemyCount; i++)
{
    var enemyShape = shapeFactory.CreateShape(new ShapeDescriptor
    {
        ShapeType = ShapeType.Circle,
        Scale = 30.0f,
        Color = System.Drawing.Color.Red
    });

    var enemyPosition = new Vector2(700, i * 100 + 50);
    var enemyDirection = Vector2.Normalize(new Vector2(-1, 0)); // Move left
    var enemySpeed = 1f;

    var enemyMovable = new BasicMovable(enemyPosition, enemyDirection, enemySpeed);
    var enemyDestructable = new BasicDestructable(1f);
    var enemyShooter = new BasicShooter(ShotFactory.CreateCannonShot, new Vector2(-1, 0)); // Shoot left

    var enemyBattleStar = new BattleStar(
        enemyShape,
        drawer,
        enemyMovable,
        enemyDestructable,
        enemyShooter
    );

    enemies.Add(enemyBattleStar);
}



var playerShots = new List<IShot>();
var enemyShots = new List<IShot>();

var context = new BasicContext();

var rnd = new Random();

while (!Raylib.WindowShouldClose())
{
    // Movement input
    Vector2 move = Vector2.Zero;
    if (Raylib.IsKeyDown(KeyboardKey.Right)) move.X += 1;
    if (Raylib.IsKeyDown(KeyboardKey.Left)) move.X -= 1;
    if (Raylib.IsKeyDown(KeyboardKey.Up)) move.Y -= 1;
    if (Raylib.IsKeyDown(KeyboardKey.Down)) move.Y += 1;

    if (move != Vector2.Zero)
    {
        move = Vector2.Normalize(move);
    }

    context.PlayerDirection = move;

    if (Raylib.IsKeyDown(KeyboardKey.Escape)) break;
    if (Raylib.IsKeyPressed(KeyboardKey.Space))
    {
        playerShots.AddRange(playerBattleStar.Shoot(context));
    }

    playerBattleStar.Move(context);


    // Enemy actions
    foreach (var enemy in enemies)
    {
        enemy.Move(context);
        if (rnd.NextDouble() < 0.01) // 10% chance to shoot each frame
        {
            enemyShots.AddRange(enemy.Shoot(context));
        }
    }

    // Draw
    Raylib.BeginDrawing();
    Raylib.ClearBackground(Raylib_cs.Color.Black);
    Raylib.DrawText("Arrow keys move the square", 10, 10, 20, Raylib_cs.Color.White);

    // Draw player
    playerBattleStar.Draw();

    // Draw enemies
    foreach (var enemy in enemies)
    {
        enemy.Draw();
    }


    // Update and draw shots
    foreach (var shot in playerShots)
    {
        shot.Update();
        drawer.DrawRectangle(shot.Position, new Vector2(10 + shot.Position.X, 5 + shot.Position.Y), System.Drawing.Color.Yellow);
    }
    foreach (var shot in enemyShots)
    {
        shot.Update();
        drawer.DrawRectangle(shot.Position, new Vector2(10 + shot.Position.X, 5 + shot.Position.Y), System.Drawing.Color.Orange);
    }

    // Check for collisions
    foreach (var enemy in enemies)
    {
        foreach (var shot in playerShots)
        {
            if (CollisionChecker.CheckBattleStarShotCollision(enemy, shot))
            {
                // Handle collision (e.g., remove enemy and shot)
                enemy.TakeDamage(shot.Damage);
                playerShots.Remove(shot);
                break; // Exit inner loop to avoid modifying collection while iterating
            }
        }
    }

    foreach (var shot in enemyShots)
    {
        if (CollisionChecker.CheckBattleStarShotCollision(playerBattleStar, shot))
        {
            // Handle collision (e.g., damage player and remove shot)
            playerBattleStar.TakeDamage(shot.Damage);
            enemyShots.Remove(shot);
            break; // Exit loop to avoid modifying collection while iterating
        }
    }

    // Remove destroyed enemies
    enemies.RemoveAll(e => e.IsDestroyed);

    // Remove player
    if (playerBattleStar.IsDestroyed)
    {
        Raylib.DrawText("Game Over!", 350, 280, 40, Raylib_cs.Color.Red);
    }

    Raylib.EndDrawing();
}

Raylib.CloseWindow();
