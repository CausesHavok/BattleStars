using System.Numerics;
namespace BattleStars.Shapes;

public interface IShape : IContains<Vector2>
{
    BoundingBox BoundingBox { get; }

    void Draw(Vector2 position, IShapeDrawer drawer);
}