using BattleStars.Domain.Interfaces;
using BattleStars.Domain.ValueObjects;
using BattleStars.Core.Guards;
namespace BattleStars.Domain.Entities;

internal class BasicMovable : IMovable
{
    private PositionalVector2 _position;
    public PositionalVector2 Position
    {
        get => _position;
        private set
        {
            Guard.RequireValid(value, nameof(value));
            _position = value;
        }
    }

    private readonly DirectionalVector2 _direction;
    private readonly float _speed;

    public BasicMovable(PositionalVector2 initialPosition, DirectionalVector2 direction, float speed)
    {
        Guard.RequireValid(speed, nameof(speed));
        Guard.RequirePositive(speed, nameof(speed));

        Position = initialPosition;
        _direction = direction;
        _speed = speed;
    }

    public void Move(IContext context)
    {
        Guard.NotNull(context, nameof(context));

        Position += _direction * _speed;
    }
}