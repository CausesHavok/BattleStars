using System.Drawing;

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
    public ShapeType ShapeType { get; set; }
    public float Scale { get; set; }
    public Color Color { get; set; }
}