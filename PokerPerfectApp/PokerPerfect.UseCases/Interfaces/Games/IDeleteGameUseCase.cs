namespace PokerPerfect.UseCases.Interfaces.Games
{
    public interface IDeleteGameUseCase
    {
        Task ExecuteAsync(int gameId);
    }
}