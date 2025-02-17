using PokerPerfect.UseCases.Interfaces.Players;
using PokerPerfect.UseCases.PluginInterfaces;

namespace PokerPerfect.UseCases.UseCases.Players
{
    public class DeletePlayerUseCase : IDeletePlayerUseCase
    {
        private readonly IPlayerRepository playerRepository;

        public DeletePlayerUseCase(IPlayerRepository playerRepository)
        {
            this.playerRepository = playerRepository;
        }

        public async Task ExecuteAsync(int playerId)
        {
            await playerRepository.DeletePlayerAsync(playerId);
        }
    }
}
