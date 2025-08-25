namespace BattleStars.Core;

public class StandardMoveable(float initialX, float initialY) : IMoveable
{
    private float _x = initialX;
    private float _y = initialY;

    public void Move(float x, float y)
    {
        _x += x;
        _y += y;
    }
}
