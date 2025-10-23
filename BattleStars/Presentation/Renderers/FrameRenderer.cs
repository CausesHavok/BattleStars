using BattleStars.Domain.ValueObjects;
using BattleStars.Domain.Interfaces;
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

        DrawPlayer(frameSnapshot.Player);
        DrawEnemies(frameSnapshot.Enemies);
        DrawShots(frameSnapshot.PlayerShots, System.Drawing.Color.Yellow, drawer);
        DrawShots(frameSnapshot.EnemyShots, System.Drawing.Color.Orange, drawer);

        // Game over message
        if (frameSnapshot.Player.IsDestroyed)
        {
            DrawEndMessage();
        }

        Raylib.EndDrawing();
    }


    private void DrawEnemies(IEnumerable<IBattleStar> enemies)
    {
        foreach (var enemy in enemies)
        {
            enemy.Draw();
        }
    }

    private void DrawShots(IEnumerable<IShot> shots, System.Drawing.Color color, IShapeDrawer drawer)
    {
        foreach (var shot in shots)
        {
            drawer.DrawRectangle(shot.Position, new PositionalVector2(10 + shot.Position.X, 5 + shot.Position.Y), color);
        }
    }

    private void DrawPlayer(IBattleStar player) => player.Draw();

    private void DrawEndMessage()
    {
        Raylib.DrawText("Game Over!", 350, 280, 40, Color.Red);
        Raylib.DrawText("Press ESC to exit", 320, 330, 20, Color.White);
    }
}