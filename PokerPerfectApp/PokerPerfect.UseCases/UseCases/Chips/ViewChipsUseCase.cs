using PokerPerfect.UseCases.PluginInterfaces;
using PokerPerfect.CoreBusiness;
using PokerPerfect.UseCases.Interfaces.Chips;

namespace PokerPerfect.UseCases.UseCases.Chips
{
    // All the code in this file is included in all platforms.
    public class ViewChipsUseCase : IViewChipsUseCase
    {
        private readonly IChipRepository chipRepository;

        public ViewChipsUseCase(IChipRepository chipRepository)
        {
            this.chipRepository = chipRepository;
        }

        public async Task<List<Chip>> ExecuteAsync(string filterText)
        {
            return await chipRepository.GetChipsAsync(filterText);
        }

    }
}