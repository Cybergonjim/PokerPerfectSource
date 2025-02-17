namespace PokerPerfect.UseCases.Interfaces.Chipsets
{
    public interface IViewChipsetUseCase
    {
        Task<CoreBusiness.Chipset> ExecuteAsync(int chipsetId);
    }
}