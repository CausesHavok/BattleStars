using BattleStars.Shapes;

namespace BattleStars.Core;

public class BattleStar
{

    private readonly IShape _shape;
    private readonly IShapeDrawer _shapeDrawer;
    private readonly IMovable _movable;

    public BattleStar(IShape shape, IShapeDrawer shapeDrawer, IMovable movable)
    {
        ArgumentNullException.ThrowIfNull(shape, nameof(shape));
        ArgumentNullException.ThrowIfNull(shapeDrawer, nameof(shapeDrawer));
        ArgumentNullException.ThrowIfNull(movable, nameof(movable));
        
        _shape = shape;
        _shapeDrawer = shapeDrawer;
        _movable = movable;
    }

    public void Draw()
    {
        _shape.Draw(_movable.Position, _shapeDrawer);
    }

    public BoundingBox GetBoundingBox()
    {
        return _shape.BoundingBox;
    }

    public void Move(IContext context)
    {
        _movable.Move(context);
    }

}