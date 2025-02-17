using PokerPerfect.UseCases.PluginInterfaces;
using PokerPerfect.CoreBusiness;
using PokerPerfect.UseCases.Interfaces.Blinds;

namespace PokerPerfect.UseCases.UseCases.Blinds
{
    public class AddBlindUseCase : IAddBlindUseCase
    {
        private readonly IBlindRepository blindRepository;

        public AddBlindUseCase(IBlindRepository blindRepository)
        {
            this.blindRepository = blindRepository;
        }

        public async Task ExecuteAsync(Blind blind)
        {
            await blindRepository.AddBlindAsync(blind);
        }
    }
}
