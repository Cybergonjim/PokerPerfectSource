
using PokerPerfect.UseCases.PluginInterfaces;
using PokerPerfect.CoreBusiness;
using PokerPerfect.UseCases.Interfaces.Tables;

namespace PokerPerfect.UseCases.UseCases.Tables
{
    public class EditTableUseCase : IEditTableUseCase
    {
        private readonly ITableRepository tableRepository;

        public EditTableUseCase(ITableRepository tableRepository)
        {
            this.tableRepository = tableRepository;
        }

        public async Task ExecuteAsync(int tableId, Table table)
        {
            await tableRepository.UpdateTableAsync(tableId, table);
        }
    }
}
