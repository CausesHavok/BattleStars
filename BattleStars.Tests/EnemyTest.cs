using System.Numerics;
using FluentAssertions;
using BattleStars.Shapes;

namespace BattleStars.Tests;

public class EnemyTest
{

    // Minimal concrete subclass for testing purposes
    private class TestEntity(Vector2 position, int health, Func<Vector2, Vector2, IShot> shotFactory, IShape shape) : Entity(position, health, shotFactory, shape)
    {
    }

    Func<Vector2, Vector2, IShot> testShotFactory = (pos, dir) => new Shot(pos, dir, 1f, 1f);
    private IShape _circle = new Circle(1f, System.Drawing.Color.Red);

    [Fact]
    public void GivenEnemy_WhenMoveCalled_DoesNotOverrideBaseMove()
    {
        // Arrange & Act
        var method = typeof(Enemy).GetMethod("Move");
        method.Should().NotBeNull();

        // Assert
        method.DeclaringType.Should().Be<Entity>();
    }

    [Fact]
    public void GivenEnemy_WhenTakeDamageCalled_DoesNotOverrideBaseTakeDamage()
    {
        // Arrange & Act
        var method = typeof(Enemy).GetMethod("TakeDamage");
        method.Should().NotBeNull();

        // Assert
        method.DeclaringType.Should().Be<Entity>();
    }

    [Fact]
    public void GivenEnemy_WhenShootCalled_DoesNotOverrideBaseShoot()
    {
        // Arrange & Act
        var method = typeof(Enemy).GetMethod("Shoot");
        method.Should().NotBeNull();

        // Assert
        method.DeclaringType.Should().Be<Entity>();
    }

    [Fact]
    public void GivenEnemy_WhenConstructed_IsIdenticalToEntity()
    {
        // Arrange & Act
        var enemy = new Enemy(new Vector2(1, 2), 100, testShotFactory, _circle);
        var testEntity = new TestEntity(new Vector2(1, 2), 100, testShotFactory, _circle);

        // Assert
        enemy.Position.Should().Be(testEntity.Position);
        enemy.Health.Should().Be(testEntity.Health);
        enemy.IsDead.Should().Be(testEntity.IsDead);
    }
}
