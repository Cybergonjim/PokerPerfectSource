using PokerPerfect.UseCases.Interfaces.Chips;
using PokerPerfect.UseCases.PluginInterfaces;

namespace PokerPerfect.UseCases.UseCases.Chips
{
    public class DeleteChipUseCase : IDeleteChipUseCase
    {
        private readonly IChipRepository chipRepository;

        public DeleteChipUseCase(IChipRepository chipRepository)
        {
            this.chipRepository = chipRepository;
        }

        public async Task ExecuteAsync(int chipId)
        {
            await chipRepository.DeleteChipAsync(chipId);
        }
    }
}
