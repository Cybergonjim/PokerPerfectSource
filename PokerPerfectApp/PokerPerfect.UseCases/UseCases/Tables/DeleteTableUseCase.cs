using PokerPerfect.UseCases.Interfaces.Tables;
using PokerPerfect.UseCases.PluginInterfaces;

namespace PokerPerfect.UseCases.UseCases.Tables
{
    public class DeleteTableUseCase : IDeleteTableUseCase
    {
        private readonly ITableRepository tableRepository;

        public DeleteTableUseCase(ITableRepository tableRepository)
        {
            this.tableRepository = tableRepository;
        }

        public async Task ExecuteAsync(int tableId)
        {
            await tableRepository.DeleteTableAsync(tableId);
        }
    }
}
