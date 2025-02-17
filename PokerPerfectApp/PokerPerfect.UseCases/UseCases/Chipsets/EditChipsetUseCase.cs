
using PokerPerfect.UseCases.PluginInterfaces;
using PokerPerfect.CoreBusiness;
using PokerPerfect.UseCases.Interfaces.Chipsets;

namespace PokerPerfect.UseCases.UseCases.Chipsets
{
    public class EditChipsetUseCase : IEditChipsetUseCase
    {
        private readonly IChipsetRepository chipsetRepository;

        public EditChipsetUseCase(IChipsetRepository chipsetRepository)
        {
            this.chipsetRepository = chipsetRepository;
        }

        public async Task ExecuteAsync(int chipsetId, Chipset chipset)
        {
            await chipsetRepository.UpdateChipsetAsync(chipsetId, chipset);
        }
    }
}
