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


    #region CreatePlayerBattleStar Tests
    [Fact]
    public void GivenNullDrawer_WhenCreatePlayerBattleStar_ThenThrowsArgumentNullException()
    {
        var boundaryChecker = new BoundaryChecker(0, 100, 0, 100);
        Action act = () => SceneFactory.CreatePlayerBattleStar(null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void WhenCreatePlayerBattleStar_ThenReturnsNonNullBattleStar()
    {
        var drawer = new MockShapeDrawer();
        var battleStar = SceneFactory.CreatePlayerBattleStar(drawer);
        battleStar.Should().NotBeNull();
        battleStar.Should().BeOfType<BattleStar>();
    }

    [Fact]
    public void WhenCreatedBattleStar_DrawMoveShootTakeDamageWork()
    {
        var drawer = new MockShapeDrawer();
        var battleStar = SceneFactory.CreatePlayerBattleStar(drawer);

        // Draw should not throw
        battleStar.Invoking(bs => bs.Draw()).Should().NotThrow();

        // Move should not throw
        var context = new BasicContext();
        battleStar.Invoking(bs => bs.Move(context)).Should().NotThrow();

        // Shoot should return non-null and non-empty
        var shots = battleStar.Shoot(context);
        shots.Should().NotBeNull();

        // Take enough damage to destroy
        battleStar.IsDestroyed.Should().BeFalse();
        battleStar.TakeDamage(1000f);
        battleStar.IsDestroyed.Should().BeTrue();
    }
    #endregion

    #region CreateEnemyBattleStars Tests
    [Fact]
    public void GivenNullDrawer_WhenCreateEnemyBattleStars_ThenThrowsArgumentNullException()
    {
        Action act = () => SceneFactory.CreateEnemyBattleStars(null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void WhenCreateEnemyBattleStars_ThenReturnsListOfBattleStars()
    {
        var drawer = new MockShapeDrawer();
        var enemies = SceneFactory.CreateEnemyBattleStars(drawer);
        enemies.Should().NotBeNull();
        enemies.Should().BeOfType<List<BattleStar>>();
        enemies.Count.Should().BeGreaterThan(0);
    }

    [Fact]
    public void WhenCreatedEnemyBattleStars_DrawMoveShootTakeDamageWork()
    {
        var drawer = new MockShapeDrawer();
        var enemies = SceneFactory.CreateEnemyBattleStars(drawer);

        foreach (var enemy in enemies)
        {
            // Draw should not throw
            enemy.Invoking(e => e.Draw()).Should().NotThrow();

            // Move should not throw
            var context = new BasicContext();
            enemy.Invoking(e => e.Move(context)).Should().NotThrow();

            // Shoot should return non-null (may be empty)
            var shots = enemy.Shoot(context);
            shots.Should().NotBeNull();

            // Take enough damage to destroy
            enemy.IsDestroyed.Should().BeFalse();
            enemy.TakeDamage(1000f);
            enemy.IsDestroyed.Should().BeTrue();
        }
    }



    #endregion

    #region CreateBasicContext Tests
    [Fact]
    public void WhenCreateBasicContext_ThenReturnsNonNullBasicContext()
    {
        var context = SceneFactory.CreateBasicContext();
        context.Should().NotBeNull();
        context.Should().BeOfType<BasicContext>();
    }
    #endregion

    #region CreateShapeDrawer Tests
    [Fact]
    public void WhenCreateShapeDrawer_ThenReturnsNonNullIShapeDrawer()
    {
        var drawer = SceneFactory.CreateShapeDrawer();
        drawer.Should().NotBeNull();
        drawer.Should().BeOfType<RaylibShapeDrawer>();
    }

    #endregion
}