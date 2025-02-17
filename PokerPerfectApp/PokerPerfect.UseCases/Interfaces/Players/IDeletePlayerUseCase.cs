namespace PokerPerfect.UseCases.Interfaces.Players
{
    public interface IDeletePlayerUseCase
    {
        Task ExecuteAsync(int playerId);
    }
}