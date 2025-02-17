using PokerPerfect.UseCases.PluginInterfaces;
using PokerPerfect.CoreBusiness;
using PokerPerfect.UseCases.Interfaces.Players;

namespace PokerPerfect.UseCases.UseCases.Players
{
    public class AddPlayerUseCase : IAddPlayerUseCase
    {
        private readonly IPlayerRepository playerRepository;

        public AddPlayerUseCase(IPlayerRepository playerRepository)
        {
            this.playerRepository = playerRepository;
        }

        public async Task ExecuteAsync(Player player)
        {
            await playerRepository.AddPlayerAsync(player);
        }
    }
}
