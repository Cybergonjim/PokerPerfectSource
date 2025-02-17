using PokerPerfect.UseCases.PluginInterfaces;
using PokerPerfect.CoreBusiness;
using PokerPerfect.UseCases.Interfaces.Games;

namespace PokerPerfect.UseCases.UseCases.Games
{
    // All the code in this file is included in all platforms.
    public class ViewGamesUseCase : IViewGamesUseCase
    {
        private readonly IGameRepository gameRepository;

        public ViewGamesUseCase(IGameRepository gameRepository)
        {
            this.gameRepository = gameRepository;
        }

        public async Task<List<Game>> ExecuteAsync(string filterText)
        {
            return await gameRepository.GetGamesAsync(filterText);
        }

    }
}