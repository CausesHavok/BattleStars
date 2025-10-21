using Raylib_cs;
using BattleStars.Domain.ValueObjects;
using BattleStars.Infrastructure.Startup;
using BattleStars.Infrastructure.Factories;

int windowWidth  = 800;
int windowHeight = 600;

Raylib.InitWindow(windowWidth, windowHeight, "BattleStars - Square Test");
Raylib.SetTargetFPS(60);

var drawer = SceneFactory.CreateShapeDrawer();
var bootstrapper = new GameBootstrapper(windowHeight, windowWidth, drawer);
var gameController = bootstrapper.Initialize().GameController;

var shouldContinue = true;

while (!Raylib.WindowShouldClose())
{

    // Update game state
    if (shouldContinue)
    {
        shouldContinue = gameController.RunFrame();
    }
    var frameSnapshot = gameController.GetFrameSnapshot();

    // Draw
    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.Black);
    Raylib.DrawText("Arrow keys move the square", 10, 10, 20, Color.White);

    // Draw player
    frameSnapshot.Player.Draw();

    // Draw enemies
    foreach (var enemy in frameSnapshot.Enemies)
    {
        enemy.Draw();
    }

    // Update and draw shots
    foreach (var shot in frameSnapshot.PlayerShots)
    {
        drawer.DrawRectangle(shot.Position, new PositionalVector2(10 + shot.Position.X, 5 + shot.Position.Y), System.Drawing.Color.Yellow);
    }
    foreach (var shot in frameSnapshot.EnemyShots)
    {
        drawer.DrawRectangle(shot.Position, new PositionalVector2(10 + shot.Position.X, 5 + shot.Position.Y), System.Drawing.Color.Orange);
    }

    // Remove player
    if (frameSnapshot.Player.IsDestroyed)
    {
        Raylib.DrawText("Game Over!", 350, 280, 40, Color.Red);
        Raylib.DrawText("Press ESC to exit", 320, 330, 20, Color.White);
    }

    Raylib.EndDrawing();
}

Raylib.CloseWindow();
