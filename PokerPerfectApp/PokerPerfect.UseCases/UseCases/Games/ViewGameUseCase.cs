using PokerPerfect.UseCases.PluginInterfaces;
using PokerPerfect.CoreBusiness;
using PokerPerfect.UseCases.Interfaces.Games;

namespace PokerPerfect.UseCases.UseCases.Games
{
    public class ViewGameUseCase : IViewGameUseCase
    {
        private readonly IGameRepository gameRepository;

        public ViewGameUseCase(IGameRepository gameRepository)
        {
            this.gameRepository = gameRepository;
        }

        public async Task<Game> ExecuteAsync(int gameId)
        {
            return await gameRepository.GetGameByIdAsync(gameId);
        }
    }
}
