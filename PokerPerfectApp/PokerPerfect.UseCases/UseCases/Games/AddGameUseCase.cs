using PokerPerfect.UseCases.PluginInterfaces;
using PokerPerfect.CoreBusiness;
using PokerPerfect.UseCases.Interfaces.Games;

namespace PokerPerfect.UseCases.UseCases.Games
{
    public class AddGameUseCase : IAddGameUseCase
    {
        private readonly IGameRepository gameRepository;

        public AddGameUseCase(IGameRepository gameRepository)
        {
            this.gameRepository = gameRepository;
        }

        public async Task ExecuteAsync(Game game)
        {
            await gameRepository.AddGameAsync(game);
        }
    }
}
