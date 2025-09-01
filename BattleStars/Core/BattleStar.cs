using BattleStars.Shapes;

namespace BattleStars.Core;

public class BattleStar
{

    private readonly IShape _shape;
    private readonly IShapeDrawer _shapeDrawer;
    private readonly IMovable _movable;
    private readonly IDestructable _destructable;

    public BattleStar(IShape shape, IShapeDrawer shapeDrawer, IMovable movable, IDestructable destructable)
    {
        ArgumentNullException.ThrowIfNull(shape, nameof(shape));
        ArgumentNullException.ThrowIfNull(shapeDrawer, nameof(shapeDrawer));
        ArgumentNullException.ThrowIfNull(movable, nameof(movable));
        ArgumentNullException.ThrowIfNull(destructable, nameof(destructable));

        _shape = shape;
        _shapeDrawer = shapeDrawer;
        _movable = movable;
        _destructable = destructable;
    }

    public void Draw() => _shape.Draw(_movable.Position, _shapeDrawer);

    public BoundingBox GetBoundingBox() => _shape.BoundingBox;

    public void Move(IContext context) => _movable.Move(context);

    public void TakeDamage(float amount) => _destructable.TakeDamage(amount);

    public bool IsDestroyed => _destructable.IsDestroyed;

}