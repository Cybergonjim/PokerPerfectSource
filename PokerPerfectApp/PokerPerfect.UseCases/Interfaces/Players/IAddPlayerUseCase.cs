namespace PokerPerfect.UseCases.Interfaces.Players
{
    public interface IAddPlayerUseCase
    {
        Task ExecuteAsync(CoreBusiness.Player player);
    }
}