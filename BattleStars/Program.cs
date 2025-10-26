
using BattleStars.Infrastructure.Adapters;
using BattleStars.Infrastructure.Factories;
using BattleStars.Infrastructure.Startup;
using BattleStars.Presentation.Renderers;
using BattleStars.Presentation.Runners;

int windowWidth  = 800;
int windowHeight = 600;

var drawer = SceneFactory.CreateShapeDrawer();
var bootstrapper = new GameBootstrapper(windowWidth, windowHeight, drawer);
var raylibAdapter = new RaylibGraphicsAdapter();
var frameRenderer = new FrameRenderer(raylibAdapter, drawer);
var app = new BattleStarsRunner(frameRenderer, raylibAdapter, bootstrapper, windowWidth, windowHeight);

app.Run();