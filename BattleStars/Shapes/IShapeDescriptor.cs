using System.Drawing;
namespace BattleStars.Shapes;

/// <summary>
/// Descriptor interface for defining shape properties.
/// </summary>
/// <remarks>
/// This interface is used to abstract shape definitions, allowing for flexible shape creation and manipulation.
/// It includes properties for the type of shape, its scale, and color.
/// </remarks>
public interface IShapeDescriptor
{
    ShapeType ShapeType { get; }
    float Scale { get; }
    Color Color { get; }
}