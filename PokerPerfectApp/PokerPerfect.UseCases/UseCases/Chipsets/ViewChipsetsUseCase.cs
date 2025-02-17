using PokerPerfect.UseCases.PluginInterfaces;
using PokerPerfect.CoreBusiness;
using PokerPerfect.UseCases.Interfaces.Chipsets;

namespace PokerPerfect.UseCases.UseCases.Chipsets
{
    // All the code in this file is included in all platforms.
    public class ViewChipsetsUseCase : IViewChipsetsUseCase
    {
        private readonly IChipsetRepository chipsetRepository;

        public ViewChipsetsUseCase(IChipsetRepository chipsetRepository)
        {
            this.chipsetRepository = chipsetRepository;
        }

        public async Task<List<Chipset>> ExecuteAsync(string filterText)
        {
            return await chipsetRepository.GetChipsetsAsync(filterText);
        }

    }
}