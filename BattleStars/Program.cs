using Raylib_cs;

Raylib.InitWindow(800, 600, "BattleStars - Square Test");
Raylib.SetTargetFPS(60);

int x = 100;
int y = 100;

while (!Raylib.WindowShouldClose())
{
    // Movement
    if (Raylib.IsKeyDown(KeyboardKey.Right)) x += 5;
    if (Raylib.IsKeyDown(KeyboardKey.Left)) x -= 5;
    if (Raylib.IsKeyDown(KeyboardKey.Up)) y -= 5;
    if (Raylib.IsKeyDown(KeyboardKey.Down)) y += 5;

    // Draw
    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.Black);
    Raylib.DrawText("Arrow keys move the square", 10, 10, 20, Color.White);
    Raylib.DrawRectangle(x, y, 50, 50, Color.Red);
    Raylib.EndDrawing();
}

Raylib.CloseWindow();
