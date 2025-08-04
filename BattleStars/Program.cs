using Raylib_cs;
using System.Numerics;
using BattleStars;

Raylib.InitWindow(800, 600, "BattleStars - Square Test");
Raylib.SetTargetFPS(60);

// Create boundary checker for the window (player stays fully inside)
var boundaryChecker = new BoundaryChecker(0, 800 - 50, 0, 600 - 50); // 50 is the player size

// Create player at starting position
var player = new Player(new Vector2(100, 100), 100f, boundaryChecker);

while (!Raylib.WindowShouldClose())
{
    // Movement input
    Vector2 move = Vector2.Zero;
    if (Raylib.IsKeyDown(KeyboardKey.Right)) move.X += 5;
    if (Raylib.IsKeyDown(KeyboardKey.Left))  move.X -= 5;
    if (Raylib.IsKeyDown(KeyboardKey.Up))    move.Y -= 5;
    if (Raylib.IsKeyDown(KeyboardKey.Down))  move.Y += 5;
    if (Raylib.IsKeyDown(KeyboardKey.Escape)) break;

    player.Move(move);

    // Draw
    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.Black);
    Raylib.DrawText("Arrow keys move the square", 10, 10, 20, Color.White);
    Raylib.DrawRectangle((int)player.Position.X, (int)player.Position.Y, 50, 50, Color.Red);
    Raylib.EndDrawing();
}

Raylib.CloseWindow();
