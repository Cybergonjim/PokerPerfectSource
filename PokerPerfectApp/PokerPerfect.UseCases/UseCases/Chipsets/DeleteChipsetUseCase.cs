using PokerPerfect.UseCases.Interfaces.Chipsets;
using PokerPerfect.UseCases.PluginInterfaces;

namespace PokerPerfect.UseCases.UseCases.Chipsets
{
    public class DeleteChipsetUseCase : IDeleteChipsetUseCase
    {
        private readonly IChipsetRepository chipsetRepository;

        public DeleteChipsetUseCase(IChipsetRepository chipsetRepository)
        {
            this.chipsetRepository = chipsetRepository;
        }

        public async Task ExecuteAsync(int chipsetId)
        {
            await chipsetRepository.DeleteChipsetAsync(chipsetId);
        }
    }
}
