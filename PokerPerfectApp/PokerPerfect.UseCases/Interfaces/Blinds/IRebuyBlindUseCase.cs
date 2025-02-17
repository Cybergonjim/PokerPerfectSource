namespace PokerPerfect.UseCases.Interfaces.Blinds
{
    public interface IRebuyBlindUseCase
    {
        Task ExecuteAsync(int blindId);
    }
}