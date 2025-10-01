using BattleStars.Domain.Interfaces;
using BattleStars.Infrastructure.Utilities;
namespace BattleStars.Domain.Entities
{
    public class BasicDestructable : IDestructable
    {
        private float _health;

        public BasicDestructable(float health)
        {
            FloatValidator.ThrowIfNaNOrInfinity(health, nameof(health));
            FloatValidator.ThrowIfNegativeOrZero(health, nameof(health));

            _health = health;
        }

        public float Health
        {
            get => _health;
            set => _health = value < 0 ? 0 : value;
        }

        public bool IsDestroyed => Health <= 0;
        public void TakeDamage(float amount)
        {
            FloatValidator.ThrowIfNaNOrInfinity(amount, nameof(amount));
            FloatValidator.ThrowIfNegative(amount, nameof(amount));

            Health -= amount;
        }
    }
}