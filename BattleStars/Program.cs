using BattleStars.Infrastructure.Startup;
using BattleStars.Infrastructure.Factories;
using BattleStars.Presentation.Renderers;
using BattleStars.Infrastructure.Adapters;

int windowWidth  = 800;
int windowHeight = 600;
var raylibAdapter = new RaylibGraphicsAdapter();
raylibAdapter.InitWindow(windowWidth, windowHeight, "BattleStars - Square Test");
raylibAdapter.SetTargetFPS(60);

var drawer = SceneFactory.CreateShapeDrawer();
var bootstrapper = new GameBootstrapper(windowWidth, windowHeight, drawer);
var gameController = bootstrapper.Initialize().GameController;

var shouldContinue = true;
var frameRenderer = new FrameRenderer();

while (!raylibAdapter.WindowShouldClose())
{
    if (shouldContinue) shouldContinue = gameController.RunFrame();
    frameRenderer.RenderFrame(gameController.GetFrameSnapshot());
}

raylibAdapter.CloseWindow();