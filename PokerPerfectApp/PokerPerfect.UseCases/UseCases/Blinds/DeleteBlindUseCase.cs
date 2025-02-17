using PokerPerfect.UseCases.Interfaces.Blinds;
using PokerPerfect.UseCases.PluginInterfaces;

namespace PokerPerfect.UseCases.UseCases.Blinds
{
    public class DeleteBlindUseCase : IDeleteBlindUseCase
    {
        private readonly IBlindRepository blindRepository;

        public DeleteBlindUseCase(IBlindRepository blindRepository)
        {
            this.blindRepository = blindRepository;
        }

        public async Task ExecuteAsync(int blindId)
        {
            await blindRepository.DeleteBlindAsync(blindId);
        }
    }
}
