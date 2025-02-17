namespace PokerPerfect.UseCases.Interfaces.Players
{
    public interface IViewPlayersUseCase
    {
        Task<List<CoreBusiness.Player>> ExecuteAsync(string filterText);
    }
}