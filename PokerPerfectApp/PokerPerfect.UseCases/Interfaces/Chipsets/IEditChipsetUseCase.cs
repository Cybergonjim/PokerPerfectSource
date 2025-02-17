namespace PokerPerfect.UseCases.Interfaces.Chipsets
{
    public interface IEditChipsetUseCase
    {
        Task ExecuteAsync(int chipsetId, CoreBusiness.Chipset chipset);
    }
}