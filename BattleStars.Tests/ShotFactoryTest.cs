using System.Numerics;
using FluentAssertions;
using BattleStars.Shots;

namespace BattleStars.Tests;

public class ShotFactoryTest
{
    [Fact]
    public void CreateScatterShot_ShouldReturnShotWithCorrectProperties()
    {
        var position = new Vector2(1, 2);
        var direction = Vector2.Normalize(new Vector2(1, 0));
        var shot = ShotFactory.CreateScatterShot(position, direction);

        shot.Position.Should().Be(position);
        shot.Direction.Should().Be(direction);
        shot.Speed.Should().Be(3f);
        shot.Damage.Should().Be(3f);
    }

    [Fact]
    public void CreateSniperShot_ShouldReturnShotWithCorrectProperties()
    {
        var position = new Vector2(3, 4);
        var direction = Vector2.Normalize(new Vector2(0, 1));
        var shot = ShotFactory.CreateSniperShot(position, direction);

        shot.Position.Should().Be(position);
        shot.Direction.Should().Be(direction);
        shot.Speed.Should().Be(50f);
        shot.Damage.Should().Be(15f);
    }

    [Fact]
    public void CreateCannonShot_ShouldReturnShotWithCorrectProperties()
    {
        var position = new Vector2(5, 6);
        var direction = Vector2.Normalize(new Vector2(-1, 0));
        var shot = ShotFactory.CreateCannonShot(position, direction);

        shot.Position.Should().Be(position);
        shot.Direction.Should().Be(direction);
        shot.Speed.Should().Be(2f);
        shot.Damage.Should().Be(20f);
    }

    [Fact]
    public void CreateLaserShot_ShouldReturnShotWithCorrectProperties()
    {
        var position = new Vector2(7, 8);
        var direction = Vector2.Normalize(new Vector2(0, -1));
        var shot = ShotFactory.CreateLaserShot(position, direction);

        shot.Position.Should().Be(position);
        shot.Direction.Should().Be(direction);
        shot.Speed.Should().Be(10f);
        shot.Damage.Should().Be(3f);
    }

    [Fact]
    public void CustomShot_ShouldReturnShotWithCorrectProperties()
    {
        var position = new Vector2(9, 10);
        var direction = Vector2.Normalize(new Vector2(1, 1));
        var speed = 4f;
        var damage = 5f;
        var shot = ShotFactory.CustomShot(position, direction, speed, damage);

        shot.Position.Should().Be(position);
        shot.Direction.X.Should().BeApproximately(direction.X, 0.001f);
        shot.Direction.Y.Should().BeApproximately(direction.Y, 0.001f);
        shot.Speed.Should().Be(speed);
        shot.Damage.Should().Be(damage);
    }
}