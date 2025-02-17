namespace PokerPerfect.UseCases.Interfaces.Tables
{
    public interface IEditTableUseCase
    {
        Task ExecuteAsync(int tableId, CoreBusiness.Table table);
    }
}