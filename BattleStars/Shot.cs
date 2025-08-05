using System.Numerics;

public class Shot
{
    public Vector2 Position { get; private set; }
    public Vector2 Direction { get; private set; }
    public float Speed { get; private set; }
    public float Damage { get; private set; }
    public bool IsActive { get; private set; } = true;

    public Shot(Vector2 position, Vector2 direction, float speed, float damage)
    {

    }

    public void Update()
    {

    }

    public void Deactivate()
    {

    }
}