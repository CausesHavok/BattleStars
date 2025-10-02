using System.Numerics;
using FluentAssertions;
using BattleStars.Domain.ValueObjects;
using BattleStars.Infrastructure.Factories;

namespace BattleStars.Tests.Infrastructure.Factories;

public class ShotFactoryTest
{
    [Fact]
    public void GivenPositionAndDirection_WhenCreateScatterShot_ThenReturnsShotWithScatterProperties()
    {
        // Given
        var position = new PositionalVector2(1, 2);
        var direction = DirectionalVector2.UnitX;

        // When
        var shot = ShotFactory.CreateScatterShot(position, direction);

        // Then
        shot.Position.Should().Be(position);
        shot.Direction.Should().Be(direction);
        shot.Speed.Should().Be(3f);
        shot.Damage.Should().Be(3f);
        shot.IsActive.Should().BeTrue();
    }

    [Fact]
    public void GivenPositionAndDirection_WhenCreateSniperShot_ThenReturnsShotWithSniperProperties()
    {
        // Given
        var position = new PositionalVector2(3, 4);
        var direction = DirectionalVector2.UnitX;

        // When
        var shot = ShotFactory.CreateSniperShot(position, direction);

        // Then
        shot.Position.Should().Be(position);
        shot.Direction.Should().Be(direction);
        shot.Speed.Should().Be(50f);
        shot.Damage.Should().Be(15f);
        shot.IsActive.Should().BeTrue();
    }

    [Fact]
    public void GivenPositionAndDirection_WhenCreateCannonShot_ThenReturnsShotWithCannonProperties()
    {
        // Given
        var position = new PositionalVector2(5, 6);
        var direction = DirectionalVector2.UnitY;

        // When
        var shot = ShotFactory.CreateCannonShot(position, direction);

        // Then
        shot.Position.Should().Be(position);
        shot.Direction.Should().Be(direction);
        shot.Speed.Should().Be(2f);
        shot.Damage.Should().Be(20f);
        shot.IsActive.Should().BeTrue();
    }

    [Fact]
    public void GivenPositionAndDirection_WhenCreateLaserShot_ThenReturnsShotWithLaserProperties()
    {
        // Given
        var position = new PositionalVector2(7, 8);
        var direction = DirectionalVector2.UnitY;

        // When
        var shot = ShotFactory.CreateLaserShot(position, direction);

        // Then
        shot.Position.Should().Be(position);
        shot.Direction.Should().Be(direction);
        shot.Speed.Should().Be(10f);
        shot.Damage.Should().Be(3f);
        shot.IsActive.Should().BeTrue();
    }

    [Fact]
    public void GivenCustomParameters_WhenCustomShot_ThenReturnsShotWithGivenProperties()
    {
        // Given
        var position = new PositionalVector2(9, 10);
        var direction = new DirectionalVector2(Vector2.Normalize(new Vector2(1, 1)));
        var speed = 4f;
        var damage = 5f;

        // When
        var shot = ShotFactory.CustomShot(position, direction, speed, damage);

        // Then
        shot.Position.Should().Be(position);
        shot.Direction.X.Should().BeApproximately(direction.X, 0.001f);
        shot.Direction.Y.Should().BeApproximately(direction.Y, 0.001f);
        shot.Speed.Should().Be(speed);
        shot.Damage.Should().Be(damage);
        shot.IsActive.Should().BeTrue();
    }

    [Fact]
    public void GivenNoParameters_WhenCreateNoOpShot_ThenReturnsInactiveShotWithZeroSpeedAndDamage()
    {
        // When
        var shot = ShotFactory.CreateNoOpShot();

        // Then
        shot.Position.Should().Be(new PositionalVector2(0, 0));
        shot.Direction.Should().Be(new DirectionalVector2(0, 0));
        shot.Speed.Should().Be(0f);
        shot.Damage.Should().Be(0f);
        shot.IsActive.Should().BeTrue();
    }

    [Fact]
    public void GivenPosition_WhenCreateNoOpShotWithPosition_ThenReturnsInactiveShotWithGivenPosition()
    {
        // Given
        var position = new PositionalVector2(42, 24);

        // When
        var shot = ShotFactory.CreateNoOpShot(position);

        // Then
        shot.Position.Should().Be(position);
        shot.Direction.Should().Be(new DirectionalVector2(0, 0));
        shot.Speed.Should().Be(0f);
        shot.Damage.Should().Be(0f);
        shot.IsActive.Should().BeTrue();
    }

    [Fact]
    public void GivenInvalidSpeedOrDamage_WhenCustomShot_ThenThrowsArgumentException()
    {
        // Given
        var position = new PositionalVector2(1, 1);
        var direction = DirectionalVector2.UnitX;

        // When/Then
        FluentActions.Invoking(() => ShotFactory.CustomShot(position, direction, float.NaN, 1f))
            .Should().Throw<ArgumentException>();
        FluentActions.Invoking(() => ShotFactory.CustomShot(position, direction, 1f, float.NaN))
            .Should().Throw<ArgumentException>();
        FluentActions.Invoking(() => ShotFactory.CustomShot(position, direction, float.PositiveInfinity, 1f))
            .Should().Throw<ArgumentException>();
        FluentActions.Invoking(() => ShotFactory.CustomShot(position, direction, 1f, float.NegativeInfinity))
            .Should().Throw<ArgumentException>();
        FluentActions.Invoking(() => ShotFactory.CustomShot(position, direction, -1f, 1f))
            .Should().Throw<ArgumentException>();
        FluentActions.Invoking(() => ShotFactory.CustomShot(position, direction, 1f, -1f))
            .Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GivenNoParameters_WhenCreateEmptyShotList_ThenReturnsEmptyList()
    {
        // When
        var shots = ShotFactory.CreateEmptyShotList();

        // Then
        shots.Should().BeEmpty();
    }
}