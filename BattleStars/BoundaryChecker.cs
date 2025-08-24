namespace BattleStars;
public class BoundaryChecker : IBoundaryChecker
{
    private readonly float minX, maxX, minY, maxY;

    public BoundaryChecker(float minX, float maxX, float minY, float maxY)
    {
        this.minX = minX;
        this.maxX = maxX;
        this.minY = minY;
        this.maxY = maxY;
    }

    public bool IsOutsideXBounds(float x)
    {
        return x < minX || x > maxX;
    }

    public float XDistanceToBoundary(float x)
    {
        return Math.Min(Math.Abs(x - minX), Math.Abs(x - maxX));
    }

    public bool IsOutsideYBounds(float y)
    {
        return y < minY || y > maxY;
    }

    public float YDistanceToBoundary(float y)
    {
        return Math.Min(Math.Abs(y - minY), Math.Abs(y - maxY));
    }
}