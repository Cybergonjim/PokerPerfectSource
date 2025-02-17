namespace PokerPerfect.UseCases.Interfaces.Blinds
{
    public interface IDeleteBlindUseCase
    {
        Task ExecuteAsync(int blindId);
    }
}