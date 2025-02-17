namespace PokerPerfect.UseCases.Interfaces.Chipsets
{
    public interface IDeleteChipsetUseCase
    {
        Task ExecuteAsync(int chipsetId);
    }
}