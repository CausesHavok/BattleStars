using Raylib_cs;
using System.Numerics;
using BattleStars.Shapes;
using BattleStars.Logic;
using BattleStars.Shots;
using BattleStars.Core;
using BattleStars.Utility;

Raylib.InitWindow(800, 600, "BattleStars - Square Test");
Raylib.SetTargetFPS(60);
var graphics = new RaylibGraphicsAdapter();
var drawer = new RaylibShapeDrawer(graphics);


// Create boundary checker for the window (player stays fully inside)
var boundaryChecker = new BoundaryChecker(0 + 25, 800 - 25, 0 + 25, 600 - 25); // 50 is the player size

// Create player BattleStar
var playerBattleStar = SceneFactory.CreatePlayerBattleStar(drawer, boundaryChecker);


// Create some enemies
var enemies = SceneFactory.CreateEnemyBattleStars(drawer);

var playerShots = new List<IShot>();
var enemyShots = new List<IShot>();

var context = new BasicContext();

var rnd = new Random();

while (!Raylib.WindowShouldClose())
{
    // Movement input
    var move = Vector2.Zero;
    if (Raylib.IsKeyDown(KeyboardKey.Right)) move.X += 1;
    if (Raylib.IsKeyDown(KeyboardKey.Left)) move.X -= 1;
    if (Raylib.IsKeyDown(KeyboardKey.Up)) move.Y -= 1;
    if (Raylib.IsKeyDown(KeyboardKey.Down)) move.Y += 1;

    if (move != Vector2.Zero)
    {
        move = Vector2.Normalize(move);
    }

    context.PlayerDirection = (move == Vector2.Zero)
        ? DirectionalVector2.Zero
        : new DirectionalVector2(move);

    if (Raylib.IsKeyDown(KeyboardKey.Escape)) break;
    if (Raylib.IsKeyPressed(KeyboardKey.Space))
    {
        // Guard against shoot returning null
        var shots = playerBattleStar.Shoot(context);
        if (shots != null)
        {
            playerShots.AddRange(shots);
        }
    }

    playerBattleStar.Move(context);


    // Enemy actions
    foreach (var enemy in enemies)
    {
        enemy.Move(context);
        if (rnd.NextDouble() < 0.01) // 1% chance to shoot each frame
        {
            // Guard against shoot returning null
            var shots = enemy.Shoot(context);
            if (shots != null)
            {
                enemyShots.AddRange(shots);
            }
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
        drawer.DrawRectangle(shot.Position, new PositionalVector2(10 + shot.Position.X, 5 + shot.Position.Y), System.Drawing.Color.Yellow);
    }
    foreach (var shot in enemyShots)
    {
        shot.Update();
        drawer.DrawRectangle(shot.Position, new PositionalVector2(10 + shot.Position.X, 5 + shot.Position.Y), System.Drawing.Color.Orange);
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
