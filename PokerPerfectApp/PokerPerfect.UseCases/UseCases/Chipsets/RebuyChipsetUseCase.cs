using PokerPerfect.UseCases.Interfaces.Chipsets;
using PokerPerfect.UseCases.PluginInterfaces;

namespace PokerPerfect.UseCases.UseCases.Chipsets
{
    public class RebuyChipsetUseCase : IRebuyChipsetUseCase
    {
        private readonly IChipsetRepository chipsetRepository;

        public RebuyChipsetUseCase(IChipsetRepository chipsetRepository)
        {
            this.chipsetRepository = chipsetRepository;
        }

        public async Task ExecuteAsync(int chipsetId)
        {
            await chipsetRepository.RebuyChipsetAsync(chipsetId);
        }
    }
}
