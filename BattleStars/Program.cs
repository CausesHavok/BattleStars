using Raylib_cs;
using System.Numerics;
using BattleStars.Logic;
using BattleStars.Shots;
using BattleStars.Core;
using BattleStars.Utility;

int windowWidth = 800;
int windowHeight = 600;

Raylib.InitWindow(windowWidth, windowHeight, "BattleStars - Square Test");
Raylib.SetTargetFPS(60);

// Create drawer
var drawer = SceneFactory.CreateShapeDrawer();

// Create player BattleStar
var playerBattleStar = SceneFactory.CreatePlayerBattleStar(drawer);

// Create some enemies
var enemies = SceneFactory.CreateEnemyBattleStars(drawer);

// Create context
var context = SceneFactory.CreateBasicContext();

// Create input handler
var inputHandler = new InputHandler(new RaylibKeyBoardProvider());

// Create boundary checker
var boundaryChecker = new BoundaryChecker(0, windowWidth, 0, windowHeight);

// Create collision checker
var collisionChecker = new CollisionChecker();

// Create initial game state
var gameState = new GameState(
    context,
    playerBattleStar,
    new List<IShot>(),
    enemies,
    new List<IShot>()
);

// Create controllers
var playerController = new PlayerController();
var enemyController = new EnemyController();
var shotController = new ShotController();
var boundaryController = new BoundaryController(boundaryChecker);
var collisionController = new CollisionController(collisionChecker);

// Create game controller
var gameController = new GameController(
    gameState,
    playerController,
    enemyController,
    shotController,
    boundaryController,
    collisionController,
    inputHandler
);
var shouldContinue = true;

while (!Raylib.WindowShouldClose())
{

    // Update game state
    if (shouldContinue)
    {
        shouldContinue = gameController.RunFrame(context);
    }
    var frameSnapshot = gameController.GetFrameSnapshot();

    // Draw
    Raylib.BeginDrawing();
    Raylib.ClearBackground(Raylib_cs.Color.Black);
    Raylib.DrawText("Arrow keys move the square", 10, 10, 20, Raylib_cs.Color.White);

    // Draw player
    frameSnapshot.Player.Draw();

    // Draw enemies
    foreach (var enemy in frameSnapshot.Enemies)
    {
        enemy.Draw();
    }


    // Update and draw shots
    foreach (var shot in frameSnapshot.PlayerShots)
    {
        drawer.DrawRectangle(shot.Position, new PositionalVector2(10 + shot.Position.X, 5 + shot.Position.Y), System.Drawing.Color.Yellow);
    }
    foreach (var shot in frameSnapshot.EnemyShots)
    {
        drawer.DrawRectangle(shot.Position, new PositionalVector2(10 + shot.Position.X, 5 + shot.Position.Y), System.Drawing.Color.Orange);
    }

    // Remove player
    if (frameSnapshot.Player.IsDestroyed)
    {
        Raylib.DrawText("Game Over!", 350, 280, 40, Color.Red);
        Raylib.DrawText("Press ESC to exit", 320, 330, 20, Color.White);
    }

    Raylib.EndDrawing();
}

Raylib.CloseWindow();
