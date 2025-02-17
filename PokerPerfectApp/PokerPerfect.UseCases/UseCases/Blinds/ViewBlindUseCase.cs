using PokerPerfect.UseCases.PluginInterfaces;
using PokerPerfect.CoreBusiness;
using PokerPerfect.UseCases.Interfaces.Blinds;

namespace PokerPerfect.UseCases.UseCases.Blinds
{
    public class ViewBlindUseCase : IViewBlindUseCase
    {
        private readonly IBlindRepository blindRepository;

        public ViewBlindUseCase(IBlindRepository blindRepository)
        {
            this.blindRepository = blindRepository;
        }

        public async Task<Blind> ExecuteAsync(int blindId)
        {
            return await blindRepository.GetBlindByIdAsync(blindId);
        }
    }
}
