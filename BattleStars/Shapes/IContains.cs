namespace BattleStars.Shapes;

public interface IContains<T>
{
    bool Contains(T item);
}