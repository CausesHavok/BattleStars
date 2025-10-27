using BattleStars.Core.Guards;
using BattleStars.Domain.Interfaces;

namespace BattleStars.Application.Checkers;

public class BoundaryChecker : IBoundaryChecker
{
    private readonly float _minX, _maxX, _minY, _maxY;

    public BoundaryChecker(float minX, float maxX, float minY, float maxY)
    {
        _minX = Guard.RequireValid(minX);
        _maxX = Guard.RequireValid(maxX);
        _minY = Guard.RequireValid(minY);
        _maxY = Guard.RequireValid(maxY);
        if (_minX >= _maxX)
            throw new ArgumentException("minX must be less than maxX.");
        if (_minY >= _maxY)
            throw new ArgumentException("minY must be less than maxY.");
    }

    public bool IsOutsideXBounds(float x)
    {
        Guard.RequireValid(x);
        return x < _minX || x > _maxX;
    }

    public float XDistanceToBoundary(float x)
    {
        Guard.RequireValid(x);
        return Math.Min(Math.Abs(x - _minX), Math.Abs(x - _maxX));
    }

    public bool IsOutsideYBounds(float y)
    {
        Guard.RequireValid(y);
        return y < _minY || y > _maxY;
    }

    public float YDistanceToBoundary(float y)
    {
        Guard.RequireValid(y);
        return Math.Min(Math.Abs(y - _minY), Math.Abs(y - _maxY));
    }
}