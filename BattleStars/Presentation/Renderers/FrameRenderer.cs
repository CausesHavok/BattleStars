using BattleStars.Domain.ValueObjects;
using BattleStars.Domain.Interfaces;
using BattleStars.Infrastructure.Adapters;
using BattleStars.Presentation.Drawers;
using System.Drawing;
using BattleStars.Core.Guards;

namespace BattleStars.Presentation.Renderers;

public sealed class FrameRenderer : IFrameRenderer
{
    private readonly IRendererGraphics _graphics;
    private readonly IShapeDrawer _shapeDrawer;

    private const string MoveHint = "Arrow keys move the square";
    private const string GameOverText = "Game Over!";
    private const string ExitHint = "Press ESC to exit";

    public FrameRenderer(IRendererGraphics graphics, IShapeDrawer shapeDrawer)
    {
        _graphics = Guard.NotNull(graphics);
        _shapeDrawer = Guard.NotNull(shapeDrawer);
    }

    public FrameRenderer()
    {
        var adapter = new RaylibGraphicsAdapter();
        _graphics = adapter;
        _shapeDrawer = new RaylibShapeDrawer(adapter);
    }


    public void RenderFrame(FrameSnapshot frameSnapshot)
    {
        // Draw
        _graphics.BeginDrawing();
        _graphics.ClearBackground(Color.Black);
        _graphics.DrawText(MoveHint, 10, 10, 20, Color.White);

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
        _graphics.DrawText(GameOverText, 350, 280, 40, Color.Red);
        _graphics.DrawText(ExitHint, 320, 330, 20, Color.White);
    }
}