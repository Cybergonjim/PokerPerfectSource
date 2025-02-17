using PokerPerfect.UseCases.Interfaces.Chips;
using PokerPerfect.UseCases.PluginInterfaces;

namespace PokerPerfect.UseCases.UseCases.Chips
{
    public class RebuyChipUseCase : IRebuyChipUseCase
    {
        private readonly IChipRepository chipRepository;

        public RebuyChipUseCase(IChipRepository chipRepository)
        {
            this.chipRepository = chipRepository;
        }

        public async Task ExecuteAsync(int chipId)
        {
            await chipRepository.RebuyChipAsync(chipId);
        }
    }
}
