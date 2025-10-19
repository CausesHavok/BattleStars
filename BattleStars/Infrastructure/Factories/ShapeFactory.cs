using BattleStars.Domain.Entities.Shapes;
using BattleStars.Domain.Interfaces;
using BattleStars.Domain.ValueObjects;
using BattleStars.Core.Guards;
using BattleStars.Presentation.Drawers;

namespace BattleStars.Infrastructure.Factories;

/// <summary>
/// Factory class for creating shapes based on descriptors.
/// </summary>
/// <remarks>
/// This factory utilizes the IShapeDescriptor interface to create various shapes such as Circle, Square,
/// Triangle, and Hexagon. It ensures that the shapes are created with valid parameters and delegates
/// drawing responsibilities to the provided IShapeDrawer implementation.
/// </remarks>
public static class ShapeFactory
{
    private static readonly float _defaultSize = 1.0f; // Default size for shapes

    /// <summary>
    /// Creates a shape based on the provided descriptor and drawer.
    /// </summary>
    /// <param name="shapeDescriptor">The descriptor defining the shape properties.</param>
    /// <param name="drawer">The drawer responsible for rendering the shape.</param>
    /// <returns>An instance of IShape as defined by the descriptor.</returns>
    /// <exception cref="ArgumentNullException">Thrown if shapeDescriptor or drawer is null.</exception>
    /// <exception cref="ArgumentException">Thrown if shape type is invalid or scale is non-positive.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if scale is NaN or Infinity.</exception>
    /// <remarks>
    /// The method validates the input parameters and uses a switch expression to create the appropriate shape.
    /// Each shape is scaled according to the descriptor's scale property and colored as specified.
    /// </remarks>
    internal static IShape CreateShape(IShapeDescriptor shapeDescriptor, IShapeDrawer drawer)
    {
        Guard.NotNull(shapeDescriptor, nameof(shapeDescriptor));
        Guard.NotNull(drawer, nameof(drawer));
        Guard.RequireValid(shapeDescriptor.Scale, nameof(shapeDescriptor.Scale));
        Guard.RequirePositive(shapeDescriptor.Scale, nameof(shapeDescriptor.Scale));

        return shapeDescriptor.ShapeType switch
        {
            ShapeType.Circle => CreateCircle(shapeDescriptor, drawer),
            ShapeType.Square => CreateSquare(shapeDescriptor, drawer),
            ShapeType.Triangle => CreateTriangle(shapeDescriptor, drawer),
            ShapeType.Hexagon => CreateHexagon(shapeDescriptor, drawer),
            _ => throw new ArgumentException("Invalid shape type"),
        };
    }

    /// <summary>
    /// Creates a circle shape based on the provided descriptor and drawer.
    /// </summary>
    /// <param name="shapeDescriptor">The descriptor defining the shape properties.</param>
    /// <param name="drawer">The drawer responsible for rendering the shape.</param>
    /// <returns>An instance of Circle.</returns>
    private static Circle CreateCircle(IShapeDescriptor shapeDescriptor, IShapeDrawer drawer)
    {
        return new Circle(shapeDescriptor.Scale * _defaultSize, shapeDescriptor.Color, drawer);
    }

    /// <summary>
    /// Creates a square shape based on the provided descriptor and drawer.
    /// </summary>
    /// <param name="shapeDescriptor">The descriptor defining the shape properties.</param>
    /// <param name="drawer">The drawer responsible for rendering the shape.</param>
    /// <returns>An instance of Rectangle representing a square.</returns>
    private static Rectangle CreateSquare(IShapeDescriptor shapeDescriptor, IShapeDrawer drawer)
    {
        float halfSize = shapeDescriptor.Scale * _defaultSize / 2f;
        PositionalVector2 topLeft = new(-halfSize, -halfSize);
        PositionalVector2 bottomRight = new(halfSize, halfSize);
        return new Rectangle(topLeft, bottomRight, shapeDescriptor.Color, drawer);
    }

    /// <summary>
    /// Creates a triangle shape based on the provided descriptor and drawer.
    /// </summary>
    /// <param name="shapeDescriptor">The descriptor defining the shape properties.</param>
    /// <param name="drawer">The drawer responsible for rendering the shape.</param>
    /// <returns>An instance of Triangle.</returns>
    private static Triangle CreateTriangle(IShapeDescriptor shapeDescriptor, IShapeDrawer drawer)
    {
        float halfSize = shapeDescriptor.Scale * _defaultSize / 2f;
        float height = (float)(Math.Sqrt(3) * halfSize);

        // Center the centroid at (0,0)
        PositionalVector2 point1 = new(-halfSize, height / 3f);
        PositionalVector2 point2 = new(halfSize, height / 3f);
        PositionalVector2 point3 = new(0, -2f * height / 3f);

        return new Triangle(point1, point2, point3, shapeDescriptor.Color, drawer);
    }

    /// <summary>
    /// Creates a hexagon shape based on the provided descriptor and drawer.
    /// </summary>
    /// <param name="shapeDescriptor">The descriptor defining the shape properties.</param>
    /// <param name="drawer">The drawer responsible for rendering the shape.</param>
    /// <returns>An instance of PolyShape representing a hexagon.</returns>
    private static PolyShape CreateHexagon(IShapeDescriptor shapeDescriptor, IShapeDrawer drawer)
    {
        float radius = shapeDescriptor.Scale * _defaultSize / 2f;
        PositionalVector2[] outerPoints = new PositionalVector2[6];
        for (int i = 0; i < 6; i++)
        {
            float angle = MathF.PI / 3f * i; // 60 degrees in radians
            outerPoints[i] = new PositionalVector2(
                radius * MathF.Cos(angle),
                radius * MathF.Sin(angle)
            );
        }

        // Create 6 triangles, each with one vertex at the origin
        Triangle[] triangles = new Triangle[6];
        for (int i = 0; i < 6; i++)
        {
            PositionalVector2 p1 = PositionalVector2.Zero;
            PositionalVector2 p2 = outerPoints[i];
            PositionalVector2 p3 = outerPoints[(i + 1) % 6];
            triangles[i] = new Triangle(p1, p2, p3, shapeDescriptor.Color, drawer);
        }

        return new PolyShape(triangles);
    }
}