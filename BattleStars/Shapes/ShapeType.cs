namespace BattleStars.Shapes;

/// <summary>
/// Enumeration of supported factory shape types.
/// </summary>
/// <remarks>
/// This enum defines the various shape types that can be created by the ShapeFactory.
/// It is used in conjunction with IShapeDescriptor to specify the desired shape.
/// </remarks>
public enum ShapeType
{
    Circle,
    Square,
    Triangle,
    Hexagon
}
