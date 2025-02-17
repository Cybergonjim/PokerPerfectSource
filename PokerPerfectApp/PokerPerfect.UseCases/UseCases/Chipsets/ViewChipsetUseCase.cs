using PokerPerfect.UseCases.PluginInterfaces;
using PokerPerfect.CoreBusiness;
using PokerPerfect.UseCases.Interfaces.Chipsets;

namespace PokerPerfect.UseCases.UseCases.Chipsets
{
    public class ViewChipsetUseCase : IViewChipsetUseCase
    {
        private readonly IChipsetRepository chipsetRepository;

        public ViewChipsetUseCase(IChipsetRepository chipsetRepository)
        {
            this.chipsetRepository = chipsetRepository;
        }

        public async Task<Chipset> ExecuteAsync(int chipsetId)
        {
            return await chipsetRepository.GetChipsetByIdAsync(chipsetId);
        }
    }
}
