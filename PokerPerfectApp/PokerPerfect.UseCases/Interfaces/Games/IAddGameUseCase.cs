namespace PokerPerfect.UseCases.Interfaces.Games
{
    public interface IAddGameUseCase
    {
        Task ExecuteAsync(CoreBusiness.Game game);
    }
}