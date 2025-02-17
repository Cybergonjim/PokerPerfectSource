using PokerPerfect.UseCases.PluginInterfaces;
using PokerPerfect.CoreBusiness;
using PokerPerfect.UseCases.Interfaces.Players;

namespace PokerPerfect.UseCases.UseCases.Players
{
    public class ViewPlayerUseCase : IViewPlayerUseCase
    {
        private readonly IPlayerRepository playerRepository;

        public ViewPlayerUseCase(IPlayerRepository playerRepository)
        {
            this.playerRepository = playerRepository;
        }

        public async Task<Player> ExecuteAsync(int playerId)
        {
            return await playerRepository.GetPlayerByIdAsync(playerId);
        }
    }
}
