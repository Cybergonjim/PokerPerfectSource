using PokerPerfect.UseCases.PluginInterfaces;
using PokerPerfect.CoreBusiness;
using PokerPerfect.UseCases.Interfaces.Chips;

namespace PokerPerfect.UseCases.UseCases.Chips
{
    public class AddChipUseCase : IAddChipUseCase
    {
        private readonly IChipRepository chipRepository;

        public AddChipUseCase(IChipRepository chipRepository)
        {
            this.chipRepository = chipRepository;
        }

        public async Task ExecuteAsync(Chip chip)
        {
            await chipRepository.AddChipAsync(chip);
        }
    }
}
