namespace PokerPerfect.UseCases.Interfaces.Players
{
    public interface IEditPlayerUseCase
    {
        Task ExecuteAsync(int playerId, CoreBusiness.Player player);
    }
}