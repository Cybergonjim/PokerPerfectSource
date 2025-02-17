using PokerPerfect.UseCases.PluginInterfaces;
using PokerPerfect.CoreBusiness;
using PokerPerfect.UseCases.Interfaces.Games;

namespace PokerPerfect.UseCases.UseCases.Games
{
    public class EditGameUseCase : IEditGameUseCase
    {
        private readonly IGameRepository gameRepository;

        public EditGameUseCase(IGameRepository gameRepository)
        {
            this.gameRepository = gameRepository;
        }

        public async Task ExecuteAsync(int gameId, Game game)
        {
            await gameRepository.UpdateGameAsync(gameId, game);
        }
    }
}
