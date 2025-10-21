using BattleStars.Domain.ValueObjects;

public interface IGameController
{
    FrameSnapshot GetFrameSnapshot();
    bool RunFrame();
}