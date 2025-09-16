using System.Drawing;
using System.Numerics;
using BattleStars.Utility;
namespace BattleStars.Shapes;

public class ShapeFactory
{
    private readonly float _defaultSize = 1.0f; // Default size for shapes
    private Color _color;
    private float _scale;

    public IShape CreateShape(ShapeDescriptor shapeDescriptor)
    {
        ArgumentNullException.ThrowIfNull(shapeDescriptor, nameof(shapeDescriptor));
        FloatValidator.ThrowIfNaNOrInfinity(shapeDescriptor.Scale, nameof(shapeDescriptor.Scale));
        FloatValidator.ThrowIfNegative(shapeDescriptor.Scale, nameof(shapeDescriptor.Scale));
        FloatValidator.ThrowIfZero(shapeDescriptor.Scale, nameof(shapeDescriptor.Scale));

        _color = shapeDescriptor.Color;
        _scale = shapeDescriptor.Scale;

        return shapeDescriptor.ShapeType switch
        {
            ShapeType.Circle => CreateCircle(),
            ShapeType.Square => CreateSquare(),
            ShapeType.Triangle => CreateTriangle(),
            ShapeType.Hexagon => CreateHexagon(),
            _ => throw new ArgumentException("Invalid shape type"),
        };
    }


    private Circle CreateCircle()
    {
        return new Circle(_scale * _defaultSize, _color);
    }

    private Rectangle CreateSquare()
    {
        float halfSize = _scale * _defaultSize / 2;
        PositionalVector2 topLeft = new(-halfSize, -halfSize);
        PositionalVector2 bottomRight = new(halfSize, halfSize);
        return new Rectangle(topLeft, bottomRight, _color);
    }

    private Triangle CreateTriangle()
    {
        float halfSize = _scale * _defaultSize / 2f;
        float height = (float)(Math.Sqrt(3) * halfSize);

        // Center the centroid at (0,0)
        PositionalVector2 point1 = new(-halfSize, height / 3f);
        PositionalVector2 point2 = new(halfSize, height / 3f);
        PositionalVector2 point3 = new(0, -2f * height / 3f);

        return new Triangle(point1, point2, point3, _color);
    }

    private PolyShape CreateHexagon()
    {
        float radius = _scale * _defaultSize / 2f;
        PositionalVector2[] outerPoints = new PositionalVector2[6];
        for (int i = 0; i < 6; i++)
        {
            float angle = MathF.PI / 3 * i; // 60 degrees in radians
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
            triangles[i] = new Triangle(p1, p2, p3, _color);
        }

        return new PolyShape(triangles);
    }
}