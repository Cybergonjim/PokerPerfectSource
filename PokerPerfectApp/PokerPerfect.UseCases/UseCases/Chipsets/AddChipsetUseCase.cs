using PokerPerfect.UseCases.PluginInterfaces;
using PokerPerfect.CoreBusiness;
using PokerPerfect.UseCases.Interfaces.Chipsets;

namespace PokerPerfect.UseCases.UseCases.Chipsets
{
    public class AddChipsetUseCase : IAddChipsetUseCase
    {
        private readonly IChipsetRepository chipsetRepository;

        public AddChipsetUseCase(IChipsetRepository chipsetRepository)
        {
            this.chipsetRepository = chipsetRepository;
        }

        public async Task ExecuteAsync(Chipset chipset)
        {
            await chipsetRepository.AddChipsetAsync(chipset);
        }
    }
}
