namespace BattleStars.Infrastructure.Adapters;

public interface IWindowConfiguration
{
    void InitWindow(int width, int height, string title);
    bool WindowShouldClose();
    void SetTargetFPS(int fps);
    void CloseWindow();
}