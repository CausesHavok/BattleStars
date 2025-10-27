using System.Drawing;
using BattleStars.Domain.Interfaces;
using BattleStars.Core.Guards;
namespace BattleStars.Domain.ValueObjects;

/// <summary>
/// Descriptor class for defining shape properties.
/// </summary>
/// <remarks>
/// This class implements the IShapeDescriptor interface and provides a concrete way to define shape properties
/// such as type, scale, and color. It includes validation to ensure that the scale is positive and not NaN or Infinity.
/// </remarks>
internal class ShapeDescriptor : IShapeDescriptor
{
    public ShapeType ShapeType { get; }
    public float Scale { get; }
    public Color Color { get; }

    /// <summary>
    /// Initializes a new instance of the ShapeDescriptor class with specified properties.
    /// </summary>
    /// <param name="shapeType">The type of shape to be created.</param>
    /// <param name="scale">The scale factor for the shape. Must be positive and not NaN or Infinity.</param>
    /// <param name="color">The color of the shape.</param>
    /// <exception cref="ArgumentException">Thrown if scale is non-positive.</exception>
    public ShapeDescriptor(ShapeType shapeType, float scale, Color color)
    {
        ShapeType = shapeType;
        Scale = Guard.RequirePositive(scale);
        Color = color;
    }
}