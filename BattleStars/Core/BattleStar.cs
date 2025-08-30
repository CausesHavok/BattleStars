using System.Numerics;
using BattleStars.Shapes;

namespace BattleStars.Core;

public class BattleStar
{

    private readonly IShape _shape;
    private readonly Vector2 _internalPosition = Vector2.Zero;

    private readonly IShapeDrawer _shapeDrawer;

    public BattleStar(IShape shape, IShapeDrawer shapeDrawer)
    {
        ArgumentNullException.ThrowIfNull(shape, nameof(shape));
        ArgumentNullException.ThrowIfNull(shapeDrawer, nameof(shapeDrawer));
        
        _shape = shape;
        _shapeDrawer = shapeDrawer;
    }

    public void Draw()
    {
        _shape.Draw(_internalPosition, _shapeDrawer);
    }

    public BoundingBox GetBoundingBox()
    {
        return _shape.BoundingBox;
    }

}