using PokerPerfect.UseCases.PluginInterfaces;
using PokerPerfect.CoreBusiness;
using PokerPerfect.UseCases.Interfaces.Blinds;

namespace PokerPerfect.UseCases.UseCases.Blinds
{
    public class EditBlindUseCase : IEditBlindUseCase
    {
        private readonly IBlindRepository blindRepository;

        public EditBlindUseCase(IBlindRepository blindRepository)
        {
            this.blindRepository = blindRepository;
        }

        public async Task ExecuteAsync(int blindId, Blind blind)
        {
            await blindRepository.UpdateBlindAsync(blindId, blind);
        }
    }
}
