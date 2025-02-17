using PokerPerfect.UseCases.Interfaces.Tables;
using PokerPerfect.UseCases.PluginInterfaces;

namespace PokerPerfect.UseCases.UseCases.Tables
{
    public class RebuyTableUseCase : IRebuyTableUseCase
    {
        private readonly ITableRepository tableRepository;

        public RebuyTableUseCase(ITableRepository tableRepository)
        {
            this.tableRepository = tableRepository;
        }

        public async Task ExecuteAsync(int tableId)
        {
            await tableRepository.RebuyTableAsync(tableId);
        }
    }
}
