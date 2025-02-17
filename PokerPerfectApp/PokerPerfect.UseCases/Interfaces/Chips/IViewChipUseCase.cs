namespace PokerPerfect.UseCases.Interfaces.Chips
{
    public interface IViewChipUseCase
    {
        Task<CoreBusiness.Chip> ExecuteAsync(int chipId);
    }
}