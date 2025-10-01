namespace BattleStars.Domain.Interfaces;

public interface IContains<T>
{
    bool Contains(T item);
}