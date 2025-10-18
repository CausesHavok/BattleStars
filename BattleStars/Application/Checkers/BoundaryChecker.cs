using BattleStars.Core.Guards;
using BattleStars.Domain.Interfaces;

namespace BattleStars.Application.Checkers;

public class BoundaryChecker : IBoundaryChecker
{
    private readonly float minX, maxX, minY, maxY;

    public BoundaryChecker(float minX, float maxX, float minY, float maxY)
    {
        FloatGuard.ThrowIfNaNOrInfinity(minX, nameof(minX));
        FloatGuard.ThrowIfNaNOrInfinity(maxX, nameof(maxX));
        FloatGuard.ThrowIfNaNOrInfinity(minY, nameof(minY));
        FloatGuard.ThrowIfNaNOrInfinity(maxY, nameof(maxY));
        if (minX >= maxX)
            throw new ArgumentException("minX must be less than maxX.");
        if (minY >= maxY)
            throw new ArgumentException("minY must be less than maxY.");

        this.minX = minX;
        this.maxX = maxX;
        this.minY = minY;
        this.maxY = maxY;
    }

    public bool IsOutsideXBounds(float x)
    {
        FloatGuard.ThrowIfNaNOrInfinity(x, nameof(x));
        return x < minX || x > maxX;
    }

    public float XDistanceToBoundary(float x)
    {
        FloatGuard.ThrowIfNaNOrInfinity(x, nameof(x));
        return Math.Min(Math.Abs(x - minX), Math.Abs(x - maxX));
    }

    public bool IsOutsideYBounds(float y)
    {
        FloatGuard.ThrowIfNaNOrInfinity(y, nameof(y));
        return y < minY || y > maxY;
    }

    public float YDistanceToBoundary(float y)
    {
        FloatGuard.ThrowIfNaNOrInfinity(y, nameof(y));
        return Math.Min(Math.Abs(y - minY), Math.Abs(y - maxY));
    }
}