using Raylib_cs;
using BattleStars.Infrastructure.Startup;
using BattleStars.Infrastructure.Factories;
using BattleStars.Presentation.Renderers;

int windowWidth  = 800;
int windowHeight = 600;

Raylib.InitWindow(windowWidth, windowHeight, "BattleStars - Square Test");
Raylib.SetTargetFPS(60);

var drawer = SceneFactory.CreateShapeDrawer();
var bootstrapper = new GameBootstrapper(windowWidth, windowHeight, drawer);
var gameController = bootstrapper.Initialize().GameController;

var shouldContinue = true;
var frameRenderer = new FrameRenderer();

while (!Raylib.WindowShouldClose())
{
    // Update game state
    if (shouldContinue)
    {
        shouldContinue = gameController.RunFrame();
    }
    var frameSnapshot = gameController.GetFrameSnapshot();

    // Render frame
    frameRenderer.RenderFrame(frameSnapshot, drawer);
}

Raylib.CloseWindow();
