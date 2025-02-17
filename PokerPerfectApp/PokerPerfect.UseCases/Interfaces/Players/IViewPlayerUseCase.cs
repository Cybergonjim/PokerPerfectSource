namespace PokerPerfect.UseCases.Interfaces.Players
{
    public interface IViewPlayerUseCase
    {
        Task<CoreBusiness.Player> ExecuteAsync(int playerId);
    }
}