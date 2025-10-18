using BattleStars.Domain.Interfaces;
using BattleStars.Core.Guards;
namespace BattleStars.Domain.Entities
{
    public class BasicDestructable : IDestructable
    {
        private float _health;

        public BasicDestructable(float health)
        {
            FloatGuard.RequireValid(health, nameof(health));
            FloatGuard.RequirePositive(health, nameof(health));

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
            FloatGuard.RequireValid(amount, nameof(amount));
            FloatGuard.RequireNonNegative(amount, nameof(amount));

            Health -= amount;
        }
    }
}