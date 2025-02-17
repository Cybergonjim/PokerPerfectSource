namespace PokerPerfect.UseCases.Interfaces.Games
{
    public interface IViewGameUseCase
    {
        Task<CoreBusiness.Game> ExecuteAsync(int gameId);
    }
}