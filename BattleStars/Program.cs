using Raylib_cs;
using System.Numerics;
using BattleStars;
using BattleStars.Shapes;
using BattleStars.Logic;

Raylib.InitWindow(800, 600, "BattleStars - Square Test");
Raylib.SetTargetFPS(60);
var drawer = new RaylibShapeDrawer();

// Create boundary checker for the window (player stays fully inside)
var boundaryChecker = new BoundaryChecker(0, 800 - 50, 0, 600 - 50); // 50 is the player size

// Create a shape factory
var shapeFactory = new ShapeFactory();

// Create player at starting position
var player = new Player(
    new Vector2(100, 100),
    100f,
    boundaryChecker,
    ShotFactory.CreateLaserShot,
    shapeFactory.CreateShape(new ShapeDescriptor
    {
        ShapeType = ShapeType.Square,
        Scale = 10.0f,
        Color = System.Drawing.Color.Blue
    })
    ); // Initial shot parameters

// Create enemies at different positions
var enemies = new List<Enemy>
{
    new Enemy(
        new Vector2(400, 100), 
        50f, 
        ShotFactory.CreateLaserShot,
        shapeFactory.CreateShape(new ShapeDescriptor
        {
            ShapeType = ShapeType.Triangle,
            Scale = 20.0f,
            Color = System.Drawing.Color.Red
        })
    ),
    new Enemy(
        new Vector2(200, 300), 
        50f, 
        ShotFactory.CreateLaserShot, 
        shapeFactory.CreateShape(new ShapeDescriptor
        {
            ShapeType = ShapeType.Triangle,
            Scale = 20.0f,
            Color = System.Drawing.Color.Red
        })
    ),
    new Enemy(
        new Vector2(600, 400), 
        50f, 
        ShotFactory.CreateLaserShot, 
        shapeFactory.CreateShape(new ShapeDescriptor
        {
            ShapeType = ShapeType.Triangle,
            Scale = 20.0f,
            Color = System.Drawing.Color.Red
        })
    )
};

var shots = new List<IShot>();

while (!Raylib.WindowShouldClose())
{
    // Movement input
    Vector2 move = Vector2.Zero;
    if (Raylib.IsKeyDown(KeyboardKey.Right)) move.X += 5;
    if (Raylib.IsKeyDown(KeyboardKey.Left)) move.X -= 5;
    if (Raylib.IsKeyDown(KeyboardKey.Up)) move.Y -= 5;
    if (Raylib.IsKeyDown(KeyboardKey.Down)) move.Y += 5;
    if (Raylib.IsKeyDown(KeyboardKey.Escape)) break;
    if (Raylib.IsKeyPressed(KeyboardKey.Space))
    {
        // Player shoots a shot in the current direction
        var shot = player.Shoot(new Vector2(1, 0)); // Default direction, can be overridden
        shots.Add(shot);
    }

    player.Move(move);

    // Draw
    Raylib.BeginDrawing();
    Raylib.ClearBackground(Raylib_cs.Color.Black);
    Raylib.DrawText("Arrow keys move the square", 10, 10, 20, Raylib_cs.Color.White);

    // Draw player
    player.Shape.Draw(player.Position, drawer);

    // Draw enemies
    foreach (var enemy in enemies)
    {
        enemy.Shape.Draw(enemy.Position, drawer);
    }

    // Update and draw shots
    foreach (var shot in shots)
    {
        shot.Update();
        drawer.DrawRectangle(shot.Position, new Vector2(10 + shot.Position.X, 5 + shot.Position.Y), System.Drawing.Color.Yellow);
    }

    // Check for collisions
    foreach (var enemy in enemies)
    {
        foreach (var shot in shots)
        {
            if (CollisionChecker.CheckEntityShotCollision(enemy, shot))
            {
                // Handle collision (e.g., remove enemy and shot)
                enemy.TakeDamage(shot.Damage);
                shots.Remove(shot);
                break; // Exit inner loop to avoid modifying collection while iterating
            }
        }
    }

    // Remove dead enemies
    enemies.RemoveAll(e => e.IsDead);

    Raylib.EndDrawing();
}

Raylib.CloseWindow();
