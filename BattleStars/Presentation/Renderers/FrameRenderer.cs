using BattleStars.Domain.ValueObjects;
using BattleStars.Presentation.Drawers;
using Raylib_cs;

namespace BattleStars.Presentation.Renderers;

public class FrameRenderer
{
    public void RenderFrame(FrameSnapshot frameSnapshot, IShapeDrawer drawer)
    {
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
}