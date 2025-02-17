namespace PokerPerfect.UseCases.Interfaces.Chips
{
    public interface IAddChipUseCase
    {
        Task ExecuteAsync(CoreBusiness.Chip chip);
    }
}