namespace PokerPerfect.UseCases.Interfaces.Chips
{
    public interface IViewChipsUseCase
    {
        Task<List<CoreBusiness.Chip>> ExecuteAsync(string filterText);
    }
}