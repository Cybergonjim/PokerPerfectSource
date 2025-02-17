using PokerPerfect.UseCases.PluginInterfaces;
using PokerPerfect.CoreBusiness;
using PokerPerfect.UseCases.Interfaces.Tables;

namespace PokerPerfect.UseCases.UseCases.Tables
{
    public class AddTableUseCase : IAddTableUseCase
    {
        private readonly ITableRepository tableRepository;

        public AddTableUseCase(ITableRepository tableRepository)
        {
            this.tableRepository = tableRepository;
        }

        public async Task ExecuteAsync(Table table)
        {
            await tableRepository.AddTableAsync(table);
        }
    }
}
