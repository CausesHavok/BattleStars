using Moq;
using BattleStars.Presentation.Renderers;
using BattleStars.Domain.Interfaces;
using BattleStars.Domain.ValueObjects;
using BattleStars.Presentation.Drawers;
using System.Drawing;
using BattleStars.Infrastructure.Adapters;

namespace BattleStars.Tests.Presentation.Renderers;

public class FrameRendererTests
{
    [Fact]
    public void GivenPlayer_WhenRenderFrame_ThenPlayerDrawInvoked()
    {
        // Given
        var graphicsMock = new Mock<IRendererGraphics>();
        var shapeDrawerMock = new Mock<IShapeDrawer>();

        var playerMock = new Mock<IBattleStar>();
        playerMock.SetupGet(p => p.IsDestroyed).Returns(false);

        var sut = new FrameRenderer(graphicsMock.Object, shapeDrawerMock.Object);

        var frame = new FrameSnapshot(
            playerMock.Object,
            new IBattleStar[0],
            new IShot[0],
            new IShot[0],
            true);

        // When
        sut.RenderFrame(frame);

        // Then
        playerMock.Verify(p => p.Draw(), Times.Once);
        graphicsMock.Verify(g => g.DrawText("Game Over!", 350, 280, 40, Color.Red), Times.Never);
        graphicsMock.Verify(g => g.DrawText("Press ESC to exit", 320, 330, 20, Color.White), Times.Never);
    }

    [Fact]
    public void GivenPlayerShots_WhenRenderFrame_ThenShapeDrawerDrawRectangleCalledForEachShot()
    {
        // Given
        var graphicsMock = new Mock<IRendererGraphics>();
        var shapeDrawerMock = new Mock<IShapeDrawer>();

        var playerMock = new Mock<IBattleStar>();
        playerMock.SetupGet(p => p.IsDestroyed).Returns(false);

        var shot1 = new Mock<IShot>();
        shot1.SetupGet(s => s.Position).Returns(new PositionalVector2(1, 2));
        var shot2 = new Mock<IShot>();
        shot2.SetupGet(s => s.Position).Returns(new PositionalVector2(3, 4));

        var sut = new FrameRenderer(graphicsMock.Object, shapeDrawerMock.Object);

        var frame = new FrameSnapshot(
            playerMock.Object,
            new IBattleStar[0],
            new[] { shot1.Object, shot2.Object },
            new IShot[0],
            true);

        // When
        sut.RenderFrame(frame);

        // Then
        graphicsMock.Verify(g => g.BeginDrawing(), Times.Once);
        graphicsMock.Verify(g => g.EndDrawing(), Times.Once);

        shapeDrawerMock.Verify(s => s.DrawRectangle(
            It.IsAny<PositionalVector2>(),
            It.IsAny<PositionalVector2>(),
            It.IsAny<Color>()), Times.Exactly(2));
    }

    [Fact]
    public void GivenEnemies_WhenRenderFrame_ThenEachEnemyDrawInvoked()
    {
        // Given
        var graphicsMock = new Mock<IRendererGraphics>();
        var shapeDrawerMock = new Mock<IShapeDrawer>();

        var enemy1 = new Mock<IBattleStar>();
        var enemy2 = new Mock<IBattleStar>();

        var playerMock = new Mock<IBattleStar>();
        playerMock.SetupGet(p => p.IsDestroyed).Returns(false);

        var sut = new FrameRenderer(graphicsMock.Object, shapeDrawerMock.Object);

        var frame = new FrameSnapshot(
            playerMock.Object,
            new[] { enemy1.Object, enemy2.Object },
            new IShot[0],
            new IShot[0],
            true);

        // When
        sut.RenderFrame(frame);

        // Then
        enemy1.Verify(e => e.Draw(), Times.Once);
        enemy2.Verify(e => e.Draw(), Times.Once);
    }

    [Fact]
    public void GivenDestroyedPlayer_WhenRenderFrame_ThenGameOverTextIsDrawn()
    {
        // Given
        var graphicsMock = new Mock<IRendererGraphics>();
        var shapeDrawerMock = new Mock<IShapeDrawer>();

        var playerMock = new Mock<IBattleStar>();
        playerMock.SetupGet(p => p.IsDestroyed).Returns(true);

        var sut = new FrameRenderer(graphicsMock.Object, shapeDrawerMock.Object);

        var frame = new FrameSnapshot(
            playerMock.Object,
            new IBattleStar[0],
            new IShot[0],
            new IShot[0],
            true);

        // When
        sut.RenderFrame(frame);

        // Then
        graphicsMock.Verify(g => g.DrawText("Game Over!", 350, 280, 40, Color.Red), Times.Once);
        graphicsMock.Verify(g => g.DrawText("Press ESC to exit", 320, 330, 20, Color.White), Times.Once);
    }

    [Fact]
    public void GivenNoArguments_WhenConstructed_ThenDependenciesAreNotNull()
    {
        // When
        var sut = new FrameRenderer();

        // Then
        Assert.NotNull(sut);
    }
}