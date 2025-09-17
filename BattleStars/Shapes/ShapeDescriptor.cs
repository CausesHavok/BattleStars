using System.Drawing;
using BattleStars.Utility;
namespace BattleStars.Shapes;

public enum ShapeType
{
    Circle,
    Square,
    Triangle,
    Hexagon
}

public class ShapeDescriptor
{
    public ShapeType ShapeType { get; }
    public float Scale { get; }
    public Color Color { get; }

    public ShapeDescriptor(ShapeType shapeType, float scale, Color color)
    {
        FloatValidator.ThrowIfNaNOrInfinity(scale, nameof(scale));
        FloatValidator.ThrowIfNegativeOrZero(scale, nameof(scale));
        ShapeType = shapeType;
        Scale = scale;
        Color = color;
    }
}