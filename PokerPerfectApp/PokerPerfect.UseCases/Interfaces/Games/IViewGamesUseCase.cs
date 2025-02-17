namespace PokerPerfect.UseCases.Interfaces.Games
{
    public interface IViewGamesUseCase
    {
        Task<List<CoreBusiness.Game>> ExecuteAsync(string filterText);
    }
}