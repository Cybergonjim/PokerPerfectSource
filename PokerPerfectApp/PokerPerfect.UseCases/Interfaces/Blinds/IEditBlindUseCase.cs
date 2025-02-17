namespace PokerPerfect.UseCases.Interfaces.Blinds
{
    public interface IEditBlindUseCase
    {
        Task ExecuteAsync(int blindId, CoreBusiness.Blind blind);
    }
}