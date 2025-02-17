using PokerPerfect.UseCases.Interfaces.Games;
using PokerPerfect.UseCases.PluginInterfaces;

namespace PokerPerfect.UseCases.UseCases.Games
{
    public class DeleteGameUseCase : IDeleteGameUseCase
    {
        private readonly IGameRepository gameRepository;

        public DeleteGameUseCase(IGameRepository gameRepository)
        {
            this.gameRepository = gameRepository;
        }

        public async Task ExecuteAsync(int gameId)
        {
            await gameRepository.DeleteGameAsync(gameId);
        }
    }
}
