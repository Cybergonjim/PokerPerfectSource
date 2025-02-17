namespace PokerPerfect.UseCases.Interfaces.Blinds
{
    public interface IAddBlindUseCase
    {
        Task ExecuteAsync(CoreBusiness.Blind blind);
    }
}