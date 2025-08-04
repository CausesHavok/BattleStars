using System.Numerics;
using FluentAssertions;

namespace BattleStars.Tests;

public class EnemyTest
{

    // Minimal concrete subclass for testing purposes
    private class TestEntity(Vector2 position, int health) : Entity(position, health)
    {
    }

    [Fact]
    public void GivenEnemy_WhenMoveCalled_DoesNotOverrideBaseMove()
    {
        var method = typeof(Enemy).GetMethod("Move");
        Assert.NotNull(method);
        Assert.True(method.DeclaringType == typeof(Entity));
    }

    [Fact]
    public void GivenEnemy_WhenTakeDamageCalled_DoesNotOverrideBaseTakeDamage()
    {
        var method = typeof(Enemy).GetMethod("TakeDamage");
        Assert.NotNull(method);
        Assert.True(method.DeclaringType == typeof(Entity));
    }

    [Fact]
    public void GivenEnemy_WhenConstructed_IsIdenticalToEntity()
    {
        var enemy = new Enemy(new Vector2(1, 2), 100);
        var testEntity = new TestEntity(new Vector2(1, 2), 100);
        enemy.Position.Should().Be(testEntity.Position);
        enemy.Health.Should().Be(testEntity.Health);
        enemy.IsDead.Should().Be(testEntity.IsDead);
    }
}
