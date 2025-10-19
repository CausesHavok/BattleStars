namespace BattleStars.Domain.Interfaces;

internal interface IContains<T>
{
    bool Contains(T item);
}