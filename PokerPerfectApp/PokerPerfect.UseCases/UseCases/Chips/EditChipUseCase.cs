
using PokerPerfect.UseCases.PluginInterfaces;
using PokerPerfect.CoreBusiness;
using PokerPerfect.UseCases.Interfaces.Chips;

namespace PokerPerfect.UseCases.UseCases.Chips
{
    public class EditChipUseCase : IEditChipUseCase
    {
        private readonly IChipRepository chipRepository;

        public EditChipUseCase(IChipRepository chipRepository)
        {
            this.chipRepository = chipRepository;
        }

        public async Task ExecuteAsync(int chipId, Chip chip)
        {
            await chipRepository.UpdateChipAsync(chipId, chip);
        }
    }
}
