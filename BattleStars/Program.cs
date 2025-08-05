using Raylib_cs;
using System.Numerics;
using BattleStars;

Raylib.InitWindow(800, 600, "BattleStars - Square Test");
Raylib.SetTargetFPS(60);

// Create boundary checker for the window (player stays fully inside)
var boundaryChecker = new BoundaryChecker(0, 800 - 50, 0, 600 - 50); // 50 is the player size

// Create player at starting position
var player = new Player(new Vector2(100, 100), 100f, boundaryChecker, ShotFactory.CreateLaserShot); // Initial shot parameters

// Create enemies at different positions
var enemies = new List<Enemy>
{
    new Enemy(new Vector2(400, 100), 50f, ShotFactory.CreateLaserShot),
    new Enemy(new Vector2(200, 300), 50f, ShotFactory.CreateLaserShot),
    new Enemy(new Vector2(600, 400), 50f, ShotFactory.CreateLaserShot)
};

var shots = new List<IShot>();

while (!Raylib.WindowShouldClose())
{
    // Movement input
    Vector2 move = Vector2.Zero;
    if (Raylib.IsKeyDown(KeyboardKey.Right)) move.X += 5;
    if (Raylib.IsKeyDown(KeyboardKey.Left)) move.X -= 5;
    if (Raylib.IsKeyDown(KeyboardKey.Up)) move.Y -= 5;
    if (Raylib.IsKeyDown(KeyboardKey.Down)) move.Y += 5;
    if (Raylib.IsKeyDown(KeyboardKey.Escape)) break;
    if (Raylib.IsKeyPressed(KeyboardKey.Space))
    {
        // Player shoots a shot in the current direction
        var shot = player.Shoot(new Vector2(1, 0)); // Default direction, can be overridden
        shots.Add(shot);
    }

    player.Move(move);

    // Draw
    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.Black);
    Raylib.DrawText("Arrow keys move the square", 10, 10, 20, Color.White);

    // Draw player
    Raylib.DrawRectangle((int)player.Position.X, (int)player.Position.Y, 50, 50, Color.Red);

    // Draw enemies
    foreach (var enemy in enemies)
    {
        Raylib.DrawRectangle((int)enemy.Position.X, (int)enemy.Position.Y, 50, 50, Color.Green);
    }

    // Update and draw shots
    foreach (var shot in shots)
    {
        shot.Update();
        Raylib.DrawRectangle((int)shot.Position.X, (int)shot.Position.Y, 10, 5, Color.Yellow);
    }

    Raylib.EndDrawing();
}

Raylib.CloseWindow();
