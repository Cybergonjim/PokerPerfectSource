namespace PokerPerfect.UseCases.Interfaces.Chips
{
    public interface IRebuyChipUseCase
    {
        Task ExecuteAsync(int chipId);
    }
}