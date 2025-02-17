using PokerPerfect.UseCases.PluginInterfaces;
using PokerPerfect.CoreBusiness;
using PokerPerfect.UseCases.Interfaces.Players;

namespace PokerPerfect.UseCases.UseCases.Players
{
    public class EditPlayerUseCase : IEditPlayerUseCase
    {
        private readonly IPlayerRepository playerRepository;

        public EditPlayerUseCase(IPlayerRepository playerRepository)
        {
            this.playerRepository = playerRepository;
        }

        public async Task ExecuteAsync(int playerId, Player player)
        {
            await playerRepository.UpdatePlayerAsync(playerId, player);
        }
    }
}
