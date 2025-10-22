using BattleStars.Domain.Interfaces;
using BattleStars.Domain.ValueObjects;
using BattleStars.Core.Guards;
namespace BattleStars.Domain.Entities;

internal class BasicMovable(PositionalVector2 initialPosition, DirectionalVector2 direction, float speed) : IMovable
{
    private PositionalVector2 _position = initialPosition;
    public PositionalVector2 Position
    {
        get => _position;
        private set
        {
            Guard.RequireValid(value, nameof(value));
            _position = value;
        }
    }

    private readonly DirectionalVector2 _direction = direction;
    private readonly float _speed = Guard.RequirePositive(speed, nameof(speed));
    public void Move(IContext context) => Position += _direction * _speed;
}