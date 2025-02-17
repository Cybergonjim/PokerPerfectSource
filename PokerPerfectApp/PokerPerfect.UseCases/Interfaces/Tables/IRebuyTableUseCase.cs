namespace PokerPerfect.UseCases.Interfaces.Tables
{
    public interface IRebuyTableUseCase
    {
        Task ExecuteAsync(int tableId);
    }
}