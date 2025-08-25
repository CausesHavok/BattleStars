using System.Numerics;
using BattleStars.Shapes;
using BattleStars.Shots;
namespace BattleStars;

public class Enemy : Entity
{
    public Enemy(
        Vector2 position,
        float health,
        Func<Vector2, Vector2, IShot> shotFactory,
        IShape shape) : base(position, health, shotFactory, shape) { }
}
