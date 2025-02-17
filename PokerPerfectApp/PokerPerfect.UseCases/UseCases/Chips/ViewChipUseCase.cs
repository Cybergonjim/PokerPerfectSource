using PokerPerfect.UseCases.PluginInterfaces;
using PokerPerfect.CoreBusiness;
using PokerPerfect.UseCases.Interfaces.Chips;

namespace PokerPerfect.UseCases.UseCases.Chips
{
    public class ViewChipUseCase : IViewChipUseCase
    {
        private readonly IChipRepository chipRepository;

        public ViewChipUseCase(IChipRepository chipRepository)
        {
            this.chipRepository = chipRepository;
        }

        public async Task<Chip> ExecuteAsync(int chipId)
        {
            return await chipRepository.GetChipByIdAsync(chipId);
        }
    }
}
