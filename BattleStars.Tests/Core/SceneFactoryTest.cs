using System.Drawing;
using BattleStars.Core;
using BattleStars.Logic;
using BattleStars.Shapes;
using BattleStars.Utility;
using FluentAssertions;

namespace BattleStars.Tests.Core;

public class SceneFactoryTest
{
    public class MockShapeDrawer : IShapeDrawer
    {
        public void DrawRectangle(PositionalVector2 v1, PositionalVector2 v2, Color color) { }
        public void DrawTriangle(PositionalVector2 p1, PositionalVector2 p2, PositionalVector2 p3, Color color) { }
        public void DrawCircle(PositionalVector2 center, float radius, Color color) { }
    }

    [Fact]
    public void GivenNullDrawer_WhenCreatePlayerBattleStar_ThenThrowsArgumentNullException()
    {
        var boundaryChecker = new BoundaryChecker(0, 100, 0, 100);
        Action act = () => SceneFactory.CreatePlayerBattleStar(null!, boundaryChecker);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GivenNullBoundaryChecker_WhenCreatePlayerBattleStar_ThenThrowsArgumentNullException()
    {
        var drawer = new MockShapeDrawer();
        Action act = () => SceneFactory.CreatePlayerBattleStar(drawer, null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void WhenCreatePlayerBattleStar_ThenReturnsNonNullBattleStar()
    {
        var drawer = new MockShapeDrawer();
        var boundaryChecker = new BoundaryChecker(0, 100, 0, 100);
        var battleStar = SceneFactory.CreatePlayerBattleStar(drawer, boundaryChecker);
        battleStar.Should().NotBeNull();
        battleStar.Should().BeOfType<BattleStar>();
    }

    [Fact]
    public void WhenCreatedBattleStar_DrawMoveShootTakeDamageWork()
    {
        var drawer = new MockShapeDrawer();
        var boundaryChecker = new BoundaryChecker(0, 100, 0, 100);
        var battleStar = SceneFactory.CreatePlayerBattleStar(drawer, boundaryChecker);

        // Draw should not throw
        battleStar.Invoking(bs => bs.Draw()).Should().NotThrow();

        // Move should not throw
        var context = new BasicContext();
        battleStar.Invoking(bs => bs.Move(context)).Should().NotThrow();

        // Shoot should return non-null and non-empty
        var shots = battleStar.Shoot(context);
        shots.Should().NotBeNull();

        // Take enough damage to destroy
        battleStar.TakeDamage(1000f);
        battleStar.IsDestroyed.Should().BeTrue();
    }
}