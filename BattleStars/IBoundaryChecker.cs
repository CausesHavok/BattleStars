namespace BattleStars;
public interface IBoundaryChecker
{
    bool IsOutsideXBounds(float x);
    float XDistanceToBoundary(float x);
    bool IsOutsideYBounds(float y);
    float YDistanceToBoundary(float y);
}