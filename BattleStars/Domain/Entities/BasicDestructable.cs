using BattleStars.Domain.Interfaces;
using BattleStars.Core.Guards;
namespace BattleStars.Domain.Entities
{
    internal class BasicDestructable(float health) : IDestructable
    {
        private float _health = Guard.RequirePositive(health);

        public float Health
        {
            get => _health;
            set => _health = value < 0 ? 0 : value;
        }

        public bool IsDestroyed => Health <= 0;
        public void TakeDamage(float amount) => Health -= Guard.RequireNonNegative(amount);
    }
}