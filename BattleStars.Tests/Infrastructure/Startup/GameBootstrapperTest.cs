using FluentAssertions;
using Moq;
using BattleStars.Infrastructure.Startup;
using BattleStars.Domain.Interfaces;
using BattleStars.Presentation.Drawers;

namespace BattleStars.Tests.Infrastructure.Startup;

public class GameBootstrapperTest
{
    // BDD: Given / When / Then style tests for GameBootstrapper.Initialize()

    [Fact]
    public void GivenValidWindowSizeAndShapeDrawer_WhenInitialize_ThenReturnsBootstrapResultAndCallsAllCreationMethods()
    {
        // Given
        var shapeDrawerMock = new Mock<IShapeDrawer>().Object;
        var sut = new TestGameBootstrapper(800, 600, shapeDrawerMock);

        // When
        var result = sut.Initialize();

        // Then
        result.Should().NotBeNull("Initialize should return a BootstrapResult instance even when implementations are overridden");
        sut.WasCreatePlayerBattleStarCalled.Should().BeTrue();
        sut.WasCreateEnemyBattleStarsCalled.Should().BeTrue();
        sut.WasCreateBasicContextCalled.Should().BeTrue();
        sut.WasCreateInputHandlerCalled.Should().BeTrue();
        sut.WasCreateBoundaryCheckerCalled.Should().BeTrue();
        sut.WasCreateCollisionCheckerCalled.Should().BeTrue();
        sut.WasCreateInitialGameStateCalled.Should().BeTrue();
        sut.WasCreateGameControllerCalled.Should().BeTrue();

        result.GameState.Player.Should().BeSameAs(sut.CapturedPlayerBattleStar);
        result.GameState.Enemies.Should().BeSameAs(sut.CapturedEnemyBattleStars);
        result.Context.Should().BeSameAs(sut.CapturedContext);
        result.InputHandler.Should().BeSameAs(sut.CapturedInputHandler);
        result.BoundaryChecker.Should().BeSameAs(sut.CapturedBoundaryChecker);
        result.CollisionChecker.Should().BeSameAs(sut.CapturedCollisionChecker);
        result.GameState.Should().BeSameAs(sut.CapturedInitialGameState.Object);
        result.GameController.Should().BeSameAs(sut.CapturedGameController);
        result.ShapeDrawer.Should().BeSameAs(shapeDrawerMock);
    }

    [Fact]
    public void GivenShapeDrawerPassedIntoConstructor_WhenInitialize_ThenPlayerCreationReceivesSameShapeDrawer()
    {
        // Given
        var shapeDrawerMock = new Mock<IShapeDrawer>().Object;
        var sut = new TestGameBootstrapper(10, 20, shapeDrawerMock);

        // When
        _ = sut.Initialize();

        // Then
        sut.CapturedShapeDrawer.Should().BeSameAs(shapeDrawerMock);
    }

    [Fact]
    public void GameBootstrapper_Initialize_ShouldReturnFullyWiredBootstrapResult()
    {
        // Arrange
        var drawer = new Mock<IShapeDrawer>().Object;
        var bootstrapper = new GameBootstrapper(600, 800, drawer);

        // Act
        var result = bootstrapper.Initialize();

        // Assert
        result.Should().NotBeNull();
        result.GameController.Should().NotBeNull();
        result.GameState.Should().NotBeNull();
        result.InputHandler.Should().NotBeNull();
        result.BoundaryChecker.Should().NotBeNull();
        result.CollisionChecker.Should().NotBeNull();
        result.Context.Should().NotBeNull();
        result.ShapeDrawer.Should().BeSameAs(drawer);

        result.GameState.Player.Should().NotBeNull("the player should be initialized");
        result.GameState.Enemies.Should().NotBeNull("enemies list should be initialized");
        result.GameState.Enemies.Should().NotBeEmpty("there should be at least one enemy");
    }


    // Testable subclass that overrides protected factory methods to capture calls and inputs
    private class TestGameBootstrapper : GameBootstrapper
    {
        public bool WasCreatePlayerBattleStarCalled;
        public bool WasCreateEnemyBattleStarsCalled;
        public bool WasCreateBasicContextCalled;
        public bool WasCreateInputHandlerCalled;
        public bool WasCreateBoundaryCheckerCalled;
        public bool WasCreateCollisionCheckerCalled;
        public bool WasCreateInitialGameStateCalled;
        public bool WasCreateGameControllerCalled;

        public IShapeDrawer CapturedShapeDrawer = new Mock<IShapeDrawer>().Object;
        public List<IBattleStar> CapturedEnemyBattleStars = new List<IBattleStar>();
        public IBattleStar CapturedPlayerBattleStar = new Mock<IBattleStar>().Object;
        public IContext CapturedContext = new Mock<IContext>().Object;
        public IInputHandler CapturedInputHandler = new Mock<IInputHandler>().Object;
        public IBoundaryChecker CapturedBoundaryChecker = new Mock<IBoundaryChecker>().Object;
        public ICollisionChecker CapturedCollisionChecker = new Mock<ICollisionChecker>().Object;
        public Mock<IGameState> CapturedInitialGameState = new Mock<IGameState>();
        public IGameController CapturedGameController = new Mock<IGameController>().Object;

        public TestGameBootstrapper(int windowHeight, int windowWidth, IShapeDrawer shapeDrawer)
            : base(windowHeight, windowWidth, shapeDrawer)
        {
        }

        protected override IBattleStar CreatePlayerBattleStar(IShapeDrawer drawer)
        {
            WasCreatePlayerBattleStarCalled = true;
            CapturedShapeDrawer = drawer;
            return CapturedPlayerBattleStar;
        }

        protected override List<IBattleStar> CreateEnemyBattleStars(IShapeDrawer drawer)
        {
            WasCreateEnemyBattleStarsCalled = true;
            CapturedShapeDrawer = drawer;
            return CapturedEnemyBattleStars;
        }

        protected override IContext CreateBasicContext()
        {
            WasCreateBasicContextCalled = true;
            return CapturedContext;
        }

        protected override IInputHandler CreateInputHandler()
        {
            WasCreateInputHandlerCalled = true;
            return CapturedInputHandler;
        }

        protected override IBoundaryChecker CreateBoundaryChecker(int width, int height)
        {
            WasCreateBoundaryCheckerCalled = true;
            
            return CapturedBoundaryChecker;
        }

        protected override ICollisionChecker CreateCollisionChecker()
        {
            WasCreateCollisionCheckerCalled = true;
            return CapturedCollisionChecker;
        }

        protected override IGameState CreateInitialGameState(IContext context, IBattleStar playerBattleStar, List<IBattleStar> enemies)
        {
            WasCreateInitialGameStateCalled = true;
            // setup mocked gamestate to return the passed in player and enemies
            var gameStateMock = CapturedInitialGameState;
            gameStateMock.Setup(gs => gs.Player).Returns(playerBattleStar);
            gameStateMock.Setup(gs => gs.Enemies).Returns(enemies);
            return CapturedInitialGameState.Object;
        }

        protected override IGameController CreateGameController(IGameState gameState, IBoundaryChecker boundaryChecker, ICollisionChecker collisionChecker, IInputHandler inputHandler, IContext context)
        {
            WasCreateGameControllerCalled = true;
            return CapturedGameController;
        }
    }
}