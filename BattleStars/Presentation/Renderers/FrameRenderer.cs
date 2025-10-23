using BattleStars.Domain.ValueObjects;
using BattleStars.Domain.Interfaces;
using BattleStars.Infrastructure.Adapters;
using BattleStars.Presentation.Drawers;
using System.Drawing;
using BattleStars.Core.Guards;

namespace BattleStars.Presentation.Renderers;

public class FrameRenderer
{
    private readonly IRendererGraphics _graphics;
    private readonly IShapeDrawer _shapeDrawer;

    public FrameRenderer(IRendererGraphics graphics, IShapeDrawer shapeDrawer)
    {
        _graphics = Guard.NotNull(graphics, nameof(graphics));
        _shapeDrawer = Guard.NotNull(shapeDrawer, nameof(shapeDrawer));
    }

    public FrameRenderer()
    {
        var adapter = new RaylibGraphicsAdapter();
        _graphics = Guard.NotNull(adapter, nameof(adapter));
        _shapeDrawer = new RaylibShapeDrawer(adapter);
    }


    public void RenderFrame(FrameSnapshot frameSnapshot)
    {
        // Draw
        _graphics.BeginDrawing();
        _graphics.ClearBackground(Color.Black);
        _graphics.DrawText("Arrow keys move the square", 10, 10, 20, Color.White);

        DrawPlayer(frameSnapshot.Player);
        DrawEnemies(frameSnapshot.Enemies);
        DrawShots(frameSnapshot.PlayerShots, Color.Yellow);
        DrawShots(frameSnapshot.EnemyShots, Color.Orange);

        // Game over message
        if (frameSnapshot.Player.IsDestroyed)
        {
            DrawEndMessage();
        }

        _graphics.EndDrawing();
    }


    private void DrawEnemies(IEnumerable<IBattleStar> enemies)
    {
        foreach (var enemy in enemies)
        {
            enemy.Draw();
        }
    }

    private void DrawShots(IEnumerable<IShot> shots, Color color)
    {
        foreach (var shot in shots)
        {
            _shapeDrawer.DrawRectangle(shot.Position, new PositionalVector2(10 + shot.Position.X, 5 + shot.Position.Y), color);
        }
    }

    private void DrawPlayer(IBattleStar player) => player.Draw();

    private void DrawEndMessage()
    {
        _graphics.DrawText("Game Over!", 350, 280, 40, Color.Red);
        _graphics.DrawText("Press ESC to exit", 320, 330, 20, Color.White);
    }
}