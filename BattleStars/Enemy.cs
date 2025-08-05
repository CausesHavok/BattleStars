using System.Numerics;

namespace BattleStars;

public class Enemy : Entity
{
    public Enemy(Vector2 position, float health, Func<Vector2, Vector2, IShot> shotFactory) : base(position, health, shotFactory){ }
}
