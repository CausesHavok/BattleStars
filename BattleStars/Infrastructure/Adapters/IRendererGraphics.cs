using System.Drawing;

namespace BattleStars.Infrastructure.Adapters;

public interface IRendererGraphics
{
    void BeginDrawing();
    void EndDrawing();
    void ClearBackground(Color color);
    void DrawText(string text, int x, int y, int fontSize, Color color);
}