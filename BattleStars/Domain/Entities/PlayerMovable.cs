using BattleStars.Domain.Interfaces;
using BattleStars.Domain.ValueObjects;
using BattleStars.Core.Guards;
using System.Numerics;
namespace BattleStars.Domain.Entities;

internal class PlayerMovable(PositionalVector2 initialPosition, float speed, IBoundaryChecker boundaryChecker) : IMovable
{
    private PositionalVector2 _position = initialPosition;
    public PositionalVector2 Position => _position;
    private readonly float _speed = Guard.RequirePositive(speed);
    private readonly IBoundaryChecker _boundaryChecker = Guard.NotNull(boundaryChecker);

    public void Move(IContext context)
    {
        Guard.NotNull(context);

        var direction = context.PlayerDirection;
        if (direction == Vector2.Zero)
            return;

        Guard.RequireNormalized(direction);

        var newPosition = _position + direction * _speed;

        if (_boundaryChecker.IsOutsideYBounds(newPosition.Y))
        {
            newPosition.Y -=
                _boundaryChecker.YDistanceToBoundary(newPosition.Y)
                * Math.Sign(direction.Y);
        }
        if (_boundaryChecker.IsOutsideXBounds(newPosition.X))
        {
            newPosition.X -=
                _boundaryChecker.XDistanceToBoundary(newPosition.X)
                * Math.Sign(direction.X);
        }

        _position = newPosition;
    }
}