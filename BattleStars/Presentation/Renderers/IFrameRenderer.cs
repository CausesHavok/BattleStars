using BattleStars.Domain.ValueObjects;

namespace BattleStars.Presentation.Renderers;
public interface IFrameRenderer
{
    void RenderFrame(FrameSnapshot frameSnapshot);
}