namespace PokerPerfect.UseCases.Interfaces.Chips
{
    public interface IEditChipUseCase
    {
        Task ExecuteAsync(int chipId, CoreBusiness.Chip chip);
    }
}