using PokerPerfect.UseCases.Interfaces.Blinds;
using PokerPerfect.UseCases.PluginInterfaces;

namespace PokerPerfect.UseCases.UseCases.Blinds
{
    public class RebuyBlindUseCase : IRebuyBlindUseCase
    {
        private readonly IBlindRepository blindRepository;

        public RebuyBlindUseCase(IBlindRepository blindRepository)
        {
            this.blindRepository = blindRepository;
        }

        public async Task ExecuteAsync(int blindId)
        {
            await blindRepository.RebuyBlindAsync(blindId);
        }
    }
}
