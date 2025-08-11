using System.Numerics;

namespace BattleStars.Shapes;

public interface IShape
{
    bool Contains(Vector2 point, Vector2 entityPosition);

    void Draw(Vector2 position, IShapeDrawer drawer);
}