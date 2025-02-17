namespace PokerPerfect.UseCases.Interfaces.Chipsets
{
    public interface IRebuyChipsetUseCase
    {
        Task ExecuteAsync(int chipsetId);
    }
}