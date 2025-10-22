using System.Drawing;
using BattleStars.Domain.ValueObjects;
using BattleStars.Infrastructure.Adapters;
using BattleStars.Core.Guards;
namespace BattleStars.Presentation.Drawers
{
    public class RaylibShapeDrawer : IShapeDrawer
    {
        private readonly IRaylibGraphics _graphics;

        public RaylibShapeDrawer(IRaylibGraphics graphics) => _graphics = Guard.NotNull(graphics, nameof(graphics));

        public void DrawRectangle(PositionalVector2 topleft, PositionalVector2 bottomright, Color color)
        {
            PositionalVector2 size = bottomright - topleft;
            _graphics.DrawRectangle(topleft, size, color);
        }

        public void DrawTriangle(PositionalVector2 p1, PositionalVector2 p2, PositionalVector2 p3, Color color) =>
            _graphics.DrawTriangle(p1, p2, p3, color);

        public void DrawCircle(PositionalVector2 center, float radius, Color color) =>
            _graphics.DrawCircle(center, radius, color);
    }
}
