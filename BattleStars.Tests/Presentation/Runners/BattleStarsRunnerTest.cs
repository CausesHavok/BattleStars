using System.Reflection;
using FluentAssertions;
using Moq;
using BattleStars.Presentation.Runners;
using BattleStars.Presentation.Renderers;
using BattleStars.Infrastructure.Startup;
using BattleStars.Infrastructure.Adapters;
using BattleStars.Domain.Interfaces;
using BattleStars.Domain.ValueObjects;
using BattleStars.Infrastructure.Factories;
using BattleStars.Presentation.Drawers;

namespace BattleStars.Tests.Presentation.Runners;

public class BattleStarsRunnerTests
{
    private static FrameSnapshot CreateDummyFrameSnapshot()
    {
        return new FrameSnapshot(
            new Mock<IBattleStar>().Object,
            [new Mock<IBattleStar>().Object],
            [ShotFactory.CreateNoOpShot()],
            [ShotFactory.CreateNoOpShot()],
            true
        );
    }

    private static Mock<IGameController> CreateGameControllerMock()
    {
        var gameControllerMock = new Mock<IGameController>();
        gameControllerMock.Setup(g => g.RunFrame()).Returns(true);
        gameControllerMock.Setup(g => g.GetFrameSnapshot()).Returns(CreateDummyFrameSnapshot());
        return gameControllerMock;
    }

    private static Mock<IGameBootstrapper> CreateBootstrapperMock(Mock<IGameController> gameControllerMock)
    {
        var bootstrapperMock = new Mock<IGameBootstrapper>();
        bootstrapperMock.Setup(b => b.Initialize()).Returns(
            new BootstrapResult(
                gameControllerMock.Object,
                new Mock<IGameState>().Object,
                new Mock<IInputHandler>().Object,
                new Mock<IBoundaryChecker>().Object,
                new Mock<ICollisionChecker>().Object,
                new Mock<IShapeDrawer>().Object
            )
        );
        return bootstrapperMock;
    }

    [Fact]
    public void GivenNullGameController_WhenRunOnce_InitializesAndRunsFrame()
    {
        // Given
        var frameRendererMock = new Mock<IFrameRenderer>();
        var windowConfigMock = new Mock<IWindowConfiguration>();
        var gameControllerMock = CreateGameControllerMock();
        var bootstrapperMock = CreateBootstrapperMock(gameControllerMock);

        var sut = new BattleStarsRunner(frameRendererMock.Object, windowConfigMock.Object, bootstrapperMock.Object);

        // When
        var result = sut.RunOnce();

        // Then
        result.Should().BeTrue();
        bootstrapperMock.Verify(b => b.Initialize(), Times.Once);
        frameRendererMock.Verify(r => r.RenderFrame(It.IsAny<FrameSnapshot>()), Times.Once);
    }

    [Fact]
    public void GivenAlreadyInitialized_WhenRunOnce_UsesExistingGameController()
    {
        // Given
        var frameRendererMock = new Mock<IFrameRenderer>();
        var windowConfigMock = new Mock<IWindowConfiguration>();
        var gameControllerMock = CreateGameControllerMock();
        var bootstrapperMock = CreateBootstrapperMock(gameControllerMock);

        var sut = new BattleStarsRunner(frameRendererMock.Object, windowConfigMock.Object, bootstrapperMock.Object);

        // First call to initialize
        sut.RunOnce();

        // Reset invocation counts
        bootstrapperMock.Invocations.Clear();
        frameRendererMock.Invocations.Clear();
        gameControllerMock.Invocations.Clear();

        // When
        var result = sut.RunOnce();

        // Then
        result.Should().BeTrue();
        bootstrapperMock.Verify(b => b.Initialize(), Times.Never);
        gameControllerMock.Verify(g => g.RunFrame(), Times.Once);
        frameRendererMock.Verify(r => r.RenderFrame(It.IsAny<FrameSnapshot>()), Times.Once);
    }

    [Fact]
    public void GivenGameController_RunOnce_InvokesRunFrameAndRendersAndReturnsFlag()
    {
        // Given
        var frameRendererMock = new Mock<IFrameRenderer>();
        var windowConfigMock = new Mock<IWindowConfiguration>();
        var gameControllerMock = CreateGameControllerMock();
        var bootstrapperMock = CreateBootstrapperMock(gameControllerMock);

        var sut = new BattleStarsRunner(frameRendererMock.Object, windowConfigMock.Object, bootstrapperMock.Object);

        // When
        var result = sut.RunOnce();

        // Then
        result.Should().BeTrue();
        gameControllerMock.Verify(g => g.RunFrame(), Times.Once);
        frameRendererMock.Verify(r => r.RenderFrame(It.IsAny<FrameSnapshot>()), Times.Once);
    }

    [Fact]
    public void GivenWindowAlreadyClosed_Run_DisposesWindow()
    {
        // Given
        var frameRendererMock = new Mock<IFrameRenderer>();
        var windowConfigMock = new Mock<IWindowConfiguration>();
        var gameControllerMock = CreateGameControllerMock();
        var bootstrapperMock = CreateBootstrapperMock(gameControllerMock);

        windowConfigMock.Setup(w => w.WindowShouldClose()).Returns(true);

        var sut = new BattleStarsRunner(frameRendererMock.Object, windowConfigMock.Object, bootstrapperMock.Object);

        // When
        sut.Run(CancellationToken.None);

        // Then
        windowConfigMock.Verify(w => w.CloseWindow(), Times.Once);
        frameRendererMock.Verify(r => r.RenderFrame(It.IsAny<FrameSnapshot>()), Times.Never);
    }

    [Fact]
    public void GivenGameSignalsStop_Run_RendersFinalFrameThenExitsAndDisposes()
    {
        // Given
        var frameRendererMock = new Mock<IFrameRenderer>();
        var windowConfigMock = new Mock<IWindowConfiguration>();
        var gameControllerMock = CreateGameControllerMock();
        gameControllerMock.Setup(g => g.RunFrame()).Returns(false);
        var bootstrapperMock = CreateBootstrapperMock(gameControllerMock);

        // WindowShouldClose: first call -> false (enter loop), second call -> true (exit)
        windowConfigMock.SetupSequence(w => w.WindowShouldClose()).Returns(false).Returns(true);

        var sut = new BattleStarsRunner(frameRendererMock.Object, windowConfigMock.Object, bootstrapperMock.Object);

        // When
        sut.Run(CancellationToken.None);

        // Then
        gameControllerMock.Verify(g => g.RunFrame(), Times.Once);
        frameRendererMock.Verify(r => r.RenderFrame(It.IsAny<FrameSnapshot>()), Times.Once);
        windowConfigMock.Verify(w => w.CloseWindow(), Times.Once);
    }

    [Fact]
    public void GivenCancellationRequested_Run_ExitsAndDisposes()
    {
        // Given
        var frameRendererMock = new Mock<IFrameRenderer>();
        var windowConfigMock = new Mock<IWindowConfiguration>();
        var gameControllerMock = CreateGameControllerMock();
        var bootstrapperMock = CreateBootstrapperMock(gameControllerMock);

        // WindowShouldClose would be false but token cancels immediately
        windowConfigMock.Setup(w => w.WindowShouldClose()).Returns(false);

        var sut = new BattleStarsRunner(frameRendererMock.Object, windowConfigMock.Object, bootstrapperMock.Object);

        using var cts = new CancellationTokenSource();
        cts.Cancel(); // cancel immediately

        // When
        sut.Run(cts.Token);

        // Then
        windowConfigMock.Verify(w => w.CloseWindow(), Times.Once);
    }
}