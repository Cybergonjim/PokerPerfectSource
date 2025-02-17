namespace PokerPerfect.UseCases.Interfaces.Players
{
    public interface IRebuyPlayerUseCase
    {
        Task ExecuteAsync(int playerId);
    }
}