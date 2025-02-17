namespace PokerPerfect.UseCases.Interfaces.Tables
{
    public interface IViewTablesUseCase
    {
        Task<List<CoreBusiness.Table>> ExecuteAsync(string filterText);
    }
}