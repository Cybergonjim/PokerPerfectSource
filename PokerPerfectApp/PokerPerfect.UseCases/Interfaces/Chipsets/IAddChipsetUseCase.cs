namespace PokerPerfect.UseCases.Interfaces.Chipsets
{
    public interface IAddChipsetUseCase
    {
        Task ExecuteAsync(CoreBusiness.Chipset chipset);
    }
}