using PokerPerfect.UseCases.PluginInterfaces;
using PokerPerfect.CoreBusiness;
using PokerPerfect.UseCases.Interfaces.Tables;

namespace PokerPerfect.UseCases.UseCases.Tables
{
    public class ViewTableUseCase : IViewTableUseCase
    {
        private readonly ITableRepository tableRepository;

        public ViewTableUseCase(ITableRepository tableRepository)
        {
            this.tableRepository = tableRepository;
        }

        public async Task<Table> ExecuteAsync(int tableId)
        {
            return await tableRepository.GetTableByIdAsync(tableId);
        }
    }
}
