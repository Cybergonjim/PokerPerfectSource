namespace PokerPerfect.UseCases.Interfaces.Tables
{
    public interface IAddTableUseCase
    {
        Task ExecuteAsync(CoreBusiness.Table table);
    }
}