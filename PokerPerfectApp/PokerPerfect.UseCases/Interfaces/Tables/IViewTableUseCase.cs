namespace PokerPerfect.UseCases.Interfaces.Tables
{
    public interface IViewTableUseCase
    {
        Task<CoreBusiness.Table> ExecuteAsync(int tableId);
    }
}