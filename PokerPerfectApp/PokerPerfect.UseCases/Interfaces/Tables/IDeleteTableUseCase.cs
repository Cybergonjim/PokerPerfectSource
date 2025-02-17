namespace PokerPerfect.UseCases.Interfaces.Tables
{
    public interface IDeleteTableUseCase
    {
        Task ExecuteAsync(int tableId);
    }
}