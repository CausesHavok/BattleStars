namespace BattleStars.Infrastructure.Startup;

public interface IGameBootstrapper
{
    BootstrapResult Initialize();
}