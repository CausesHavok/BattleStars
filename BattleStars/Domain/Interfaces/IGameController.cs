using BattleStars.Domain.ValueObjects;

namespace BattleStars.Domain.Interfaces
{
    public interface IGameController
    {
        FrameSnapshot GetFrameSnapshot();
        bool RunFrame();
    }
}